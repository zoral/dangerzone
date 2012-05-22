using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay.Animations
{
    class AnimationManager
    {

        EnemyExplosionAnimation[] m_rgEnemyKillAnimation = new EnemyExplosionAnimation[Constants.MAXANIMATIONS];
        EnemyExplosionAnimation[] m_rgEnemyCollisionAnimation = new EnemyExplosionAnimation[Constants.MAXANIMATIONS];

        EnemySplashAnimation[] m_rgEnemySplashAnimation = new EnemySplashAnimation[Constants.MAXSPLASHES];

        RocketExplosionAnimation[] m_rgRocketExplosionAnimation = new RocketExplosionAnimation[Constants.MAXANIMATIONS];

        GameplayScreen m_pParent;

        int m_nActiveKillAnimations = 0;
        int m_nActiveCollisionAnimations = 0;
        int m_nActiveSplashes = 0;
        int m_nActiveRocketExplosions = 0;

        public AnimationManager(GameplayScreen pParent)
        {
            m_pParent = pParent;
        }

        public void LoadContent(ContentManager pContent)
        {
            Texture2D pKillAnimationSprite = pContent.Load<Texture2D>("GFX\\Enemies\\Animations\\explosionAnimation_temp");
            
            Texture2D pCollisionAnimationSprite = pContent.Load<Texture2D>("GFX\\Enemies\\Animations\\explosionAnimation_temp");

            Texture2D pRocketExplosionSprite = pContent.Load<Texture2D>("GFX\\Weapons\\rocketExplosion_temp");

            for (int i = 0; i < Constants.MAXANIMATIONS; i++)
            {
                m_rgEnemyKillAnimation[i] = new EnemyExplosionAnimation(this, pKillAnimationSprite, 2, 500);
                m_rgEnemyCollisionAnimation[i] = new EnemyExplosionAnimation(this, pCollisionAnimationSprite, 2, 500);
                m_rgRocketExplosionAnimation[i] = new RocketExplosionAnimation(this, pRocketExplosionSprite, 2, 250);
            }

            Texture2D pSplash = pContent.Load<Texture2D>("GFX\\Enemies\\Animations\\splash1");
            for (int i = 0; i < Constants.MAXSPLASHES; i++)
            {
                m_rgEnemySplashAnimation[i] = new EnemySplashAnimation(this, pSplash);
            }


        }

        public void Update(GameTime pGameTime)
        {
            for (int i = 0; i < Constants.MAXANIMATIONS; i++)
            {
                m_rgEnemyKillAnimation[i].Update(pGameTime);
                m_rgEnemyCollisionAnimation[i].Update(pGameTime);
                m_rgRocketExplosionAnimation[i].Update(pGameTime);
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            m_nActiveCollisionAnimations = 0;
            m_nActiveKillAnimations = 0;
            m_nActiveSplashes = 0;
            m_nActiveRocketExplosions = 0;

            for (int i = 0; i < Constants.MAXSPLASHES; i++)
            {
                if (!m_rgEnemySplashAnimation[i].Dead)
                {
                    m_rgEnemySplashAnimation[i].Draw(pSpriteBatch);
                    m_nActiveSplashes++;
                }
            }

            for (int i = 0; i < Constants.MAXANIMATIONS; i++)
            {
                m_rgEnemyKillAnimation[i].Draw(pSpriteBatch);
                if (m_rgEnemyKillAnimation[i].Playing)
                    m_nActiveKillAnimations++;
                m_rgEnemyCollisionAnimation[i].Draw(pSpriteBatch);
                if (m_rgEnemyCollisionAnimation[i].Playing)
                    m_nActiveCollisionAnimations++;

                m_rgRocketExplosionAnimation[i].Draw(pSpriteBatch);
                if (m_rgRocketExplosionAnimation[i].Playing)
                    m_nActiveRocketExplosions++;
            }



        }

        public void InitKillAnimation(Vector2 pPosition)
        {
            for (int i = 0; i < Constants.MAXANIMATIONS; i++)
            {
                if (!m_rgEnemyKillAnimation[i].Playing)
                    m_rgEnemyKillAnimation[i].Init(pPosition);
            }
        }
        public void InitCollisionAnimation(Vector2 pPosition)
        {
            for (int i = 0; i < Constants.MAXANIMATIONS; i++)
            {
                if (!m_rgEnemyCollisionAnimation[i].Playing)
                    m_rgEnemyCollisionAnimation[i].Init(pPosition);
            }
        }
        public void InitRocketExplosion(Vector2 pPosition)
        {
            for (int i = 0; i < Constants.MAXANIMATIONS; i++)
            {
                if (!m_rgRocketExplosionAnimation[i].Playing)
                    m_rgRocketExplosionAnimation[i].Init(pPosition);
            }
        }


        public void InitSplash(Vector2 pPosition)
        {
            /*
             * Om alla är upptagna ska vi ta den som har levt längst. Annars tar vi första bästa!
             */
            float fMaxAliveTime = 0;
            int nPosition = -1;
            bool bFoundDead = false;
            for (int i = 0; i < Constants.MAXSPLASHES; i++)
            {
                if (m_rgEnemySplashAnimation[i].Dead)
                {
                    bFoundDead = true;
                    m_rgEnemySplashAnimation[i].Init(pPosition);
                    break;
                }

                if (fMaxAliveTime < m_rgEnemySplashAnimation[i].AliveTime)
                {
                    fMaxAliveTime = m_rgEnemySplashAnimation[i].AliveTime;
                    nPosition = i;
                }

            }

            if (!bFoundDead && nPosition != -1)
            {
                // Init den som vi har i nPosition
                m_rgEnemySplashAnimation[nPosition].Init(pPosition);
            }
        }


        public GameplayScreen Parent
        {
            get { return m_pParent; }
        }

    }
}
