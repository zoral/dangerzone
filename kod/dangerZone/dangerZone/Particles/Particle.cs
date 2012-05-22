using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using dangerZone.Gameplay;

namespace dangerZone.Particles
{
    class Particle
    {

        bool m_bDead = true;
        ParticleEngine m_pParent;

        Vector2 m_pPosition;
        Vector2 m_pVelocity;
        Rectangle m_pSourceRectangle;
        Vector2 m_pOrigin;
        Color m_pColor = Color.White;

        float m_fTTL = 100;
        float m_fAliveTime = 0;

        public Particle(ParticleEngine pParent)
        {
            m_pParent = pParent;
            m_pSourceRectangle = new Rectangle(0, 0, 4, 4);
            m_pOrigin = new Vector2(0.5f, 0.5f);
        }

        public void Update(GameTime pGameTime)
        {

            m_fAliveTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_fAliveTime > m_fTTL)
                m_bDead = true;
            
            m_pPosition += m_pVelocity;
        }

        public void Draw(SpriteBatch pSpriteBatch, Texture2D pTexture, bool bConvert)
        {
            if (bConvert)
            {
                pSpriteBatch.Draw(pTexture, ConvertUnits.ToDisplayUnits(m_pPosition), m_pSourceRectangle, m_pColor, 0f, m_pOrigin, 0.5f, SpriteEffects.None, 0.24f);
                //pSpriteBatch.Draw(pTexture, ConvertUnits.ToDisplayUnits(m_pPosition), m_pColor);
            }
            else
                pSpriteBatch.Draw(pTexture, m_pPosition, m_pColor);
        }

        public bool Dead
        {
            get { return m_bDead; }
            set { m_bDead = value; }
        }
        
        public void Init(Vector2 pPosition, Vector2 pVelocity, Color pColor, float fTTL)
        {
            pVelocity /= 10;
            m_bDead = false;
            m_fTTL = fTTL;
            m_fAliveTime = 0;
            m_pPosition = pPosition;
            m_pVelocity = pVelocity;
            m_pColor = pColor;
            
        }
    }
}
