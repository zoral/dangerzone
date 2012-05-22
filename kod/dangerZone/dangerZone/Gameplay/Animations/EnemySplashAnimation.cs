using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay.Animations
{
    class EnemySplashAnimation
    {

        AnimationManager m_pParent;
        Texture2D m_pTexture;
        Vector2 m_pPosition;
        Vector2 m_pOrigin;
        Rectangle m_pSourceRectangle;
        float m_fDepthLayer = 0.245f;
        float m_fRotation = 0f;
        float m_fScale = 1f;

        bool m_bDead = true;
        float m_fAliveTime = 0f;
        float m_fMaxAliveTime = 20000f; // 20 sek

        public EnemySplashAnimation(AnimationManager pParent, Texture2D pTexture)
        {
            m_pParent = pParent;
            m_pTexture = pTexture;
            m_pOrigin = new Vector2(pTexture.Width / 2, pTexture.Height / 2);
            m_pSourceRectangle = new Rectangle(0, 0, pTexture.Width, pTexture.Height);
        }


        public void Init(Vector2 pPosition)
        {
            m_bDead = false;
            m_fAliveTime = 0f;
            m_pPosition = pPosition;
            m_fRotation = (float)m_pParent.Parent.Random.Next(0, 626) / 100;
            
        }

        public void Update(GameTime pGameTime)
        {
            if (!m_bDead)
            {
                m_fAliveTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
                if (m_fAliveTime > m_fMaxAliveTime)
                {
                    m_fAliveTime = 0;
                    m_bDead = true;
                }
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            if (!m_bDead)
            {
                pSpriteBatch.Draw(m_pTexture, ConvertUnits.ToDisplayUnits(m_pPosition), m_pSourceRectangle, Color.White, m_fRotation, m_pOrigin, m_fScale, SpriteEffects.None, m_fDepthLayer);
            }
        }

        public bool Dead
        {
            get { return m_bDead; }
            set { m_bDead = value; }
        }

        public float AliveTime
        {
            get { return m_fAliveTime; }
            set { m_fAliveTime = value; }
        }


    }
}
