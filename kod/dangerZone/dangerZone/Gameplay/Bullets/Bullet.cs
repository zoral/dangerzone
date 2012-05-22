using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Gameplay.GameObjects;
using dangerZone.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dangerZone.Gameplay.Bullets
{
    class Bullet : GameObject
    {

        bool m_bDead = true;

        float m_fRotation = 0f;
        float m_fScale = 0.5f;
        float m_fDepthLayer = 0.24f;

        BulletManager m_pParent;
        Color m_pColor;
        Vector2 m_pVelocity = Vector2.Zero;
        float m_fDamage = 0;

        float m_fTTL = 10000;
        float m_fAliveTime = 0f;

        int m_nLightIndex = -1;

        int m_nPlayer = 0;

        bool m_bDieOnCollision = true;

        public Bullet(BulletManager pParent, GameplayScreen pGameplayScreen)
            : base(pGameplayScreen)
        {
            m_pParent = pParent;
            this.Animation = new SimpleAnimation(m_pParent.Texture, Vector2.Zero, 0, 1, 0);
        }

        public void Init(int nPlayer, Vector2 pPosition, Vector2 pVelocity, float fDamage, Color pColor, int nLightIndex, float fTTL)
        {
            //this.Animation = new SimpleAnimation(m_pParent.Texture, pPosition, 0, 1, 0);
            this.Animation.Position = pPosition;
            m_pColor = pColor;
            Dead = false;
            m_pVelocity = pVelocity;
            m_fDamage = fDamage;
            m_fAliveTime = 0;
            m_nLightIndex = nLightIndex;
            m_nPlayer = nPlayer;
            m_fTTL = fTTL;

            this.BoundingBox.UpdateSize(0.25f, 0.25f);
            this.BoundingBox.Update(this.Animation.Position.X, this.Animation.Position.Y);

        }

        public override void Update(GameTime pGameTime)
        {
            if (Dead)
                return;

            m_fRotation += 0.05f;
            this.Animation.Position += m_pVelocity;

            /*
             * Ta bort lite utav velocity hela tiden(som om den saknar ner).
             * När velocity = 0 så är den död.
             */
            m_pVelocity = Vector2.SmoothStep(m_pVelocity, Vector2.Zero, 0.02f);
            if (m_pVelocity == Vector2.Zero)
            {
                Dead = true;
            }

            if (m_nLightIndex != -1)
            {
                this.Parent.LightEngine.Lights[m_nLightIndex].Position = this.Animation.Position;
            }

            this.BoundingBox.Update(this.Animation.Position.X, this.Animation.Position.Y);

            m_fAliveTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_fAliveTime > m_fTTL)
            {
                Dead = true;
                
            }

        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            if(!Dead)
                pSpriteBatch.Draw(m_pParent.Texture, ConvertUnits.ToDisplayUnits(this.Animation.Position), this.Animation.SourceRectangle, m_pColor, m_fRotation, this.Animation.Origin, m_fScale, SpriteEffects.None, m_fDepthLayer);
                        
        }

        public bool Dead
        {
            get { return m_bDead; }
            set 
            {
                if (value)
                {
                    if(m_nLightIndex != -1)
                        m_pParent.Parent.LightEngine.Lights[m_nLightIndex].Dead = true;
                }
                m_bDead = value; 
            }
        }

        public int Player
        {
            get { return m_nPlayer; }
            set { m_nPlayer = value; }
        }
        public float Damage
        {
            get { return m_fDamage; }
            set { m_fDamage = value; }
        }

        public Vector2 Velocity
        {
            get { return m_pVelocity; }
            set { m_pVelocity = value; }
        }

        public float TTL
        {
            get { return m_fTTL; }
            set { m_fTTL = value; }
        }

        public bool DieOnCollision
        {
            get { return m_bDieOnCollision; }
            set { m_bDieOnCollision = value; }
        }

    }

}
