using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using dangerZone.Gameplay.GameObjects;

namespace dangerZone.Gameplay.Lights
{
    class Light
    {

        Vector2 m_pPosition;
        Texture2D m_pTexture;
        Color m_pColor;
        bool m_bDead = true;
        Rectangle m_pSourceRectangle;
        float m_fRotation = 0;
        Vector2 m_pOrigin;
        float m_fScale = 1f;
        float m_fDepthLayer = 0.25f;

        bool m_bTurnedOn = true;

        public Light()
        {
            
        }

        public void Init(Vector2 pPosition, Texture2D pTexture, Color pColor)
        {
            m_pPosition = pPosition;
            m_pTexture = pTexture;
            m_pColor = pColor;
            m_pSourceRectangle = new Rectangle(0, 0, pTexture.Width, pTexture.Height);
            m_pOrigin = new Vector2(pTexture.Width / 2, pTexture.Height / 2);
            m_bDead = false;
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            /*
            m_pPosition.X -= m_pTexture.Width / 2;
            m_pPosition.Y -= m_pTexture.Height / 2;
            */
            //pSpriteBatch.Draw(m_pTexture, ConvertUnits.ToDisplayUnits(m_pPosition), m_pColor);
            if(m_bTurnedOn)
                pSpriteBatch.Draw(m_pTexture, ConvertUnits.ToDisplayUnits(m_pPosition), m_pSourceRectangle, Color.White, m_fRotation, m_pOrigin, m_fScale, SpriteEffects.None, m_fDepthLayer);
       
            
        }

        public bool Dead
        {
            get { return m_bDead; }
            set { m_bDead = value; }
        }

        public Vector2 Position
        {
            get { return m_pPosition; }
            set { m_pPosition = value; }
        }

        public bool TurnedOn
        {
            get { return m_bTurnedOn; }
            set { m_bTurnedOn = value; }
        }
    }
}
