using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay.GameObjects
{
    class SimpleAnimation
    {

        // Enkla animationer. Vanlig grid. Kunna göra några inställningar så de passar för player, enemy och weapons.

        Texture2D m_pTexture;
        Vector2 m_pPosition = Vector2.Zero;

        int m_nMaxRows = 0;
        int m_nMaxColumns = 0;
        float m_fMaxFrameTime = 200f;
        float m_fFrameTime = 0f;
        int m_nCurrentRow = 0;
        int m_nCurrentColumn = 0;

        int m_nXSpriteSize = 0;
        int m_nYSpriteSize = 0;

        Rectangle m_pSourceRectangle;
        Vector2 m_pOrigin = Vector2.Zero;

        float m_fRotation = 0f;
        float m_fScale = 1f;
        float m_fDepthLayer = 0.24f;

        bool m_bLoop = true;
        bool m_bPlay = true;

        public SimpleAnimation(Texture2D pTexture, Vector2 pPosition, int nMaxRows, int nMaxColumns, float fMaxFrameTime)
        {

            if (nMaxRows == 0)
                nMaxRows = 1;

            m_pTexture = pTexture;
            m_nMaxRows = nMaxRows;
            m_nMaxColumns = nMaxColumns;
            m_fMaxFrameTime = fMaxFrameTime;
            m_pPosition = pPosition;

            m_nXSpriteSize = pTexture.Width / nMaxColumns;
            
            m_nYSpriteSize = pTexture.Height / nMaxRows;
            m_pSourceRectangle = new Rectangle(0, 0, m_nXSpriteSize, m_nYSpriteSize);
            m_pOrigin = new Vector2(m_nXSpriteSize / 2, m_nYSpriteSize / 2);



        }

        public void Update(GameTime pGameTime)
        {
            if (!m_bPlay)
                return;

            m_fFrameTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_fFrameTime > m_fMaxFrameTime)
            {
                m_fFrameTime = 0;
                m_nCurrentColumn++;
                if (m_nCurrentColumn >= m_nMaxColumns)
                {
                    m_nCurrentColumn = 0;
                    if (!m_bLoop)
                    {
                        m_nCurrentColumn = 0;
                        m_bPlay = false;
                    }
                }
                m_pSourceRectangle.X = m_nXSpriteSize * m_nCurrentColumn;
                m_pSourceRectangle.Y = m_nYSpriteSize * m_nCurrentRow;
                
            }

        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(m_pTexture, ConvertUnits.ToDisplayUnits(m_pPosition), m_pSourceRectangle, Color.White, m_fRotation, m_pOrigin, m_fScale, SpriteEffects.None, m_fDepthLayer);
        }


        public Vector2 Position
        {
            get { return m_pPosition; }
            set { m_pPosition = value; }
        }

        public int Row
        {
            get { return m_nCurrentRow; }
            set { m_nCurrentRow = value; }
        }

        public Vector2 Origin
        {
            get { return m_pOrigin; }
            set { m_pOrigin = value; }
        }

        public Rectangle SourceRectangle
        {
            get { return m_pSourceRectangle; }
            set { m_pSourceRectangle = value; }
        }

        public float Scale
        {
            get { return m_fScale; }
            set { m_fScale = value; }
        }

        public bool Loop
        {
            get { return m_bLoop; }
            set { m_bLoop = value; }
        }

        public bool Play
        {
            get { return m_bPlay; }
            set { m_bPlay = value; }
        }

        public void Reset()
        {
            m_bPlay = true;
            m_nCurrentColumn = 0;
            m_nCurrentRow = 0;
            m_fFrameTime = 0;
            
        }

        public float Rotation
        {
            get { return m_fRotation; }
            set { m_fRotation = value; }
        }

        public float DepthLayer
        {
            get { return m_fDepthLayer; }
            set { m_fDepthLayer = value; }
        }

        public int Col
        {
            get { return m_nCurrentColumn; }
            set { m_nCurrentColumn = value; }
        }

        public void UpdateSource()
        {
            m_pSourceRectangle.X = m_nXSpriteSize * m_nCurrentColumn;
            m_pSourceRectangle.Y = m_nYSpriteSize * m_nCurrentRow;
        }
    }
}
