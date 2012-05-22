using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Gameplay;
using dangerZone.Gameplay.Bullets;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using dangerZone.Gameplay.GameObjects.Enemies;

namespace dangerZone.Screens
{
    class BulletManager
    {

        Bullet[] m_rgBullets = new Bullet[Constants.MAXBULLETS];
        Rocket[] m_rgRockets = new Rocket[Constants.MAXROCKETS];
        Rail[] m_rgRails = new Rail[Constants.MAXRAILS];

        Texture2D m_pTexture;
        Texture2D m_pRocketTexture;
        Texture2D m_pRailTexture;
        GameplayScreen m_pParent;

        int m_nActiveBullets = 0;

        public BulletManager(GameplayScreen pParent)
        {
            m_pParent = pParent;
        }

        public void LoadContent(ContentManager pContent)
        {
            m_pTexture = pContent.Load<Texture2D>("GFX\\bullet_sprite");
            m_pRocketTexture = pContent.Load<Texture2D>("GFX\\Weapons\\rocket_temp");
            m_pRailTexture = pContent.Load<Texture2D>("GFX\\Weapons\\railBullet_temp");

            for (int i = 0; i < Constants.MAXBULLETS; i++)
            {
                m_rgBullets[i] = new Bullet(this, m_pParent);
                
            }
            for(int i = 0; i < Constants.MAXROCKETS; i++)
            {
                m_rgRockets[i] = new Rocket(this, m_pParent);
            }
            for (int i = 0; i < Constants.MAXRAILS; i++)
            {
                m_rgRails[i] = new Rail(this, m_pParent);
            }

            

        }

        public void InitBullet(int nPlayer, Vector2 pPosition, Vector2 pVelocity, float fDamage, Color pColor, float fTTL)
        {
            for (int i = 0; i < Constants.MAXBULLETS; i++)
            {
                if (m_rgBullets[i].Dead)
                {
                    //int nLightIndex = m_pParent.LightEngine.CreateLight(pPosition, Gameplay.Lights.LightSizes.SMALL, Color.Yellow);
                    m_rgBullets[i].Init(nPlayer, pPosition, pVelocity, fDamage, pColor, -1, fTTL);
                    
                    break;
                }
            }
        }

        public void InitBullet(int nPlayer, Vector2 pPosition, Vector2 pVelocity, float fDamage, Color pColor, float fTTL, bool bDieOnCollision)
        {
            for (int i = 0; i < Constants.MAXBULLETS; i++)
            {
                if (m_rgBullets[i].Dead)
                {
                   // int nLightIndex = m_pParent.LightEngine.CreateLight(pPosition, Gameplay.Lights.LightSizes.SMALL, Color.Yellow);
                    m_rgBullets[i].Init(nPlayer, pPosition, pVelocity, fDamage, pColor, -1, fTTL);
                    m_rgBullets[i].DieOnCollision = bDieOnCollision;

                    break;
                }
            }
        }

        public void InitRail(int nPlayer, Vector2 pPosition, Vector2 pVelocity, float fDamage, float fRotation, float fTTL)
        {
            for (int i = 0; i < Constants.MAXRAILS; i++)
            {
                if (m_rgRails[i].Dead)
                {
                    // int nLightIndex = m_pParent.LightEngine.CreateLight(pPosition, Gameplay.Lights.LightSizes.SMALL, Color.Yellow);
                    m_rgRails[i].Init(nPlayer, pPosition, pVelocity, fDamage, fRotation, -1, fTTL);
                    m_rgRails[i].DieOnCollision = false;

                    break;
                }
            }
        }

        public void InitRocket(int nPlayer, Vector2 pPosition, Vector2 pVelocity, float fDamage, float fRotation, float fTTL)
        {
            for (int i = 0; i < Constants.MAXROCKETS; i++)
            {
                if (m_rgRockets[i].Dead)
                {
                   // int nLightIndex = m_pParent.LightEngine.CreateLight(pPosition, Gameplay.Lights.LightSizes.MEDIUM, Color.Yellow);
                    m_rgRockets[i].Init(nPlayer, pPosition, pVelocity, fDamage, fRotation, -1, fTTL);

                    break;
                }
            }
        }

        public void Update(GameTime pGameTime)
        {
            m_nActiveBullets = 0;

            UpdateBullets(pGameTime);
            UpdateRockets(pGameTime);
            UpdateRails(pGameTime);

        }

        private void UpdateBullets(GameTime pGameTime)
        {
            for (int i = 0; i < Constants.MAXBULLETS; i++)
            {
                if (!m_rgBullets[i].Dead)
                {
                    m_rgBullets[i].Update(pGameTime);
                    m_nActiveBullets++;

                    // Vi ska väl se ifall kulan träffade en fiende. Isf ska det spruta blod + fienden ska ta skada.
                    for (int x = 0; x < Constants.MAXENEMIES; x++)
                    {
                        if (!this.Parent.EnemyManager.Enemies[x].Dead)
                        {

                            BaseEnemy pEnemy = this.Parent.EnemyManager.Enemies[x];
                            if (m_rgBullets[i].BoundingBox.Intersects(pEnemy.BoundingBox))
                            {
                                if (pEnemy.dealDamage(m_rgBullets[i].Damage))
                                {
                                    this.Parent.Players[m_rgBullets[i].Player - 1].AddScore(pEnemy.Score);
                                }
                                Vector2 pDirection = m_rgBullets[i].Velocity;
                                pDirection.Normalize();
                                this.Parent.ParticleEngine.Emitter(m_rgBullets[i].Animation.Position, pDirection, Color.LimeGreen, 500, 100);
                                if (m_rgBullets[i].DieOnCollision)
                                    m_rgBullets[i].Dead = true;

                            }
                        }
                    }

                }

            }
        }

        private void UpdateRockets(GameTime pGameTime)
        {
            for (int i = 0; i < Constants.MAXROCKETS; i++)
            {
                if (!m_rgRockets[i].Dead)
                {
                    m_nActiveBullets++;
                    m_rgRockets[i].Update(pGameTime);
                    /*
                     * Se ifall vi har träffat något. Då ska vi ta alla som är "nära" och skada dem också.
                     */

                    for (int x = 0; x < Constants.MAXENEMIES; x++)
                    {
                        if (!this.Parent.EnemyManager.Enemies[x].Dead)
                        {

                            BaseEnemy pEnemy = this.Parent.EnemyManager.Enemies[x];
                            if (m_rgRockets[i].BoundingBox.Intersects(pEnemy.BoundingBox))
                            {
                                if (pEnemy.dealDamage(m_rgRockets[i].Damage))
                                {
                                    this.Parent.Players[m_rgRockets[i].Player - 1].AddScore(pEnemy.Score);
                                }




                                Vector2 pDirection = m_rgBullets[i].Velocity;
                                pDirection.Normalize();
                                this.Parent.ParticleEngine.Emitter(m_rgRockets[i].Animation.Position, pDirection, Color.LimeGreen, 500, 100);
                                m_rgRockets[i].Dead = true;

                                /*
                                 * Nu ska vi gå igenom alla fiender igen och se vilka som var nära nog att skadas utav explosionen.
                                 */
                                this.Parent.AnimationManager.InitRocketExplosion(m_rgRockets[i].Animation.Position);

                                for (int y = 0; y < Constants.MAXENEMIES; y++)
                                {
                                    if (!this.Parent.EnemyManager.Enemies[y].Dead)
                                    {
                                        float fDistance = Vector2.Distance(m_rgRockets[i].Animation.Position, this.Parent.EnemyManager.Enemies[y].Animation.Position);

                                        if (fDistance < 2)
                                        {

                                            if (this.Parent.EnemyManager.Enemies[y].dealDamage((m_rgRockets[i].Damage / 2)))
                                            {
                                                this.Parent.Players[m_rgRockets[i].Player - 1].AddScore(this.Parent.EnemyManager.Enemies[y].Score);
                                            }

                                        }

                                    }
                                }

                            }
                        }
                    }

                }
            }
        }

        private void UpdateRails(GameTime pGameTime)
        {
            for (int i = 0; i < Constants.MAXRAILS; i++)
            {
                if (!m_rgRails[i].Dead)
                {
                    m_rgRails[i].Update(pGameTime);
                    m_nActiveBullets++;

                    // Vi ska väl se ifall kulan träffade en fiende. Isf ska det spruta blod + fienden ska ta skada.
                    for (int x = 0; x < Constants.MAXENEMIES; x++)
                    {
                        if (!this.Parent.EnemyManager.Enemies[x].Dead)
                        {

                            BaseEnemy pEnemy = this.Parent.EnemyManager.Enemies[x];
                            if (m_rgRails[i].BoundingBox.Intersects(pEnemy.BoundingBox))
                            {
                                if (pEnemy.dealDamage(m_rgRails[i].Damage))
                                {
                                    this.Parent.Players[m_rgRails[i].Player - 1].AddScore(pEnemy.Score);
                                }
                                Vector2 pDirection = m_rgRails[i].Velocity;
                                pDirection.Normalize();
                                
                                this.Parent.ParticleEngine.Emitter(m_rgRails[i].Animation.Position, pDirection, Color.LimeGreen, 500, 100);
                                
                                if (m_rgRails[i].DieOnCollision)
                                    m_rgRails[i].Dead = true;

                            }
                        }
                    }

                }

            }
        }
        

        public void Draw(SpriteBatch pSpriteBatch)
        {
            for (int i = 0; i < Constants.MAXBULLETS; i++)
            {
                if (!m_rgBullets[i].Dead)
                    m_rgBullets[i].Draw(pSpriteBatch);
                
            }
            for (int i = 0; i < Constants.MAXROCKETS; i++)
            {
                if (!m_rgRockets[i].Dead)
                    m_rgRockets[i].Draw(pSpriteBatch);
            }
            for (int i = 0; i < Constants.MAXRAILS; i++)
            {
                if (!m_rgRails[i].Dead)
                    m_rgRails[i].Draw(pSpriteBatch);
            }
        }

        public Texture2D Texture
        {
            get { return m_pTexture; }
            set { m_pTexture = value; }
        }

        public Texture2D RocketTexture
        {
            get { return m_pRocketTexture; }
            set { m_pRocketTexture = value; }
        }

        public Texture2D RailTexture
        {
            get { return m_pRailTexture; }
            set { m_pRailTexture = value; }
        }

        public Rail[] Rails()
        {
            return m_rgRails;
        }

        public Rocket[] Rockets()
        {
            return m_rgRockets;
        }

        public Bullet[] Bullets()
        {
            return m_rgBullets;
        }

        public int ActiveBullets
        {
            get { return m_nActiveBullets; }
        }

        public GameplayScreen Parent
        {
            get { return m_pParent; }
        }
        
    }
}
