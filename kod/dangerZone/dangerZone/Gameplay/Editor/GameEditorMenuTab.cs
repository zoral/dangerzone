using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace dangerZone.Gameplay.Editor
{
    class GameEditorMenuTab
    {

        GameEditor m_pParent;
        string m_szName;

        bool m_bVisible = false;

        Texture2D m_pBackground;

        Color m_pColor = Color.White;
        Rectangle m_pBoundingBox;

        int m_nMaxItems = 0;
        int m_nItemNumber = 0;
        GameEditorMenuItem[] m_rgItems; // = new GameEditorMenuItem[m_nMaxItems];
        int m_nSelectedItem = 0;

        Vector2 m_pPosition;
        public Vector2 getSize()
        {
            return m_pParent.Font.MeasureString(m_szName);
        }
        public GameEditorMenuTab(GameEditor pParent, string szName, int nItem)
        {
            m_nItemNumber = nItem;

            m_pParent = pParent;
            m_szName = szName;
            Vector2 pSize = m_pParent.Font.MeasureString(m_szName);

            int nPrevSize = 0;
            if(nItem != 0)
            {
                nPrevSize = (int)m_pParent.Tabs[nItem-1].getSize().X + 15;
            }

            int nY = 50;
            if (nItem >= 6)
            {
                nY = 80;
                nItem -= 6;
            }

            m_pPosition = new Vector2(100 + ( (nItem * 140)), nY);

            
            m_pBoundingBox = new Rectangle((int)m_pPosition.X, (int)m_pPosition.Y, (int)pSize.X, (int)pSize.Y);

        }

        public void addItems(int nNumberOfItems)
        {
            m_rgItems = new GameEditorMenuItem[nNumberOfItems];
            m_nMaxItems = nNumberOfItems;
        }

        public void addItem(int nId, GameEditorMenuItem pItem)
        {
            
            m_rgItems[nId] = pItem;
        }

        public void finishAddItems()
        {
            for(int i = 0; i < m_nMaxItems; i++)
                m_rgItems[i].recalcPosition(i);
            generateBackground();
        }

        private void generateBackground()
        {
            int nXSize = 350;

            int nYSize = 0;// m_nMaxItems * 30;

            for (int i = 0; i < m_nMaxItems; i++)
            {
                nYSize += (i * 30); // +(int)m_rgItems[i].getSize().X; // +30;
            }
            Color[] rgColors = new Color[nXSize * nYSize];
            for (int x = 0; x < nXSize; x++)
            {
                for (int y = 0; y < nYSize; y++)
                {
                    rgColors[x + y * nXSize] = Color.Black;
                }
            }

            m_pBackground = new Texture2D(m_pParent.Parent.Parent.GraphicsDevice, nXSize, nYSize, false, SurfaceFormat.Color);
            m_pBackground.SetData(rgColors);

        }

        public void Display()
        {
            m_bVisible = true;
            m_pColor = Color.Red;

        }

        public void Hide()
        {
            m_bVisible = false;
            m_pColor = Color.White;
        }

        public void MouseOver()
        {
            m_pColor = Color.Pink;
        }
        public void MouseOut()
        {
            m_pColor = Color.White;
        }

        public Rectangle BoundingBox
        {
            get { return m_pBoundingBox; }
            set { m_pBoundingBox = value; }
        }

        public void Update(GameTime pGameTime)
        {
            if (m_bVisible)
            {
                bool bOverruleIntersect = false;
                bool bIntersects = false;
                for (int i = 0; i < m_nMaxItems; i++)
                {
                    m_rgItems[i].Update(pGameTime);

                    if (m_pParent.MouseBoundingBox.Intersects(m_rgItems[i].BoundingBox))
                    {
                        m_nSelectedItem = i;
                        bIntersects = true;
                        m_rgItems[i].MouseOver();
                    }
                    else
                        m_rgItems[i].MouseOut();

                    if (m_rgItems[i].IsEditing())
                        bOverruleIntersect = true;

                }

                if (bIntersects && !bOverruleIntersect)
                {
                    if (m_pParent.PrevMouseState.LeftButton == ButtonState.Released && m_pParent.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        m_rgItems[m_nSelectedItem].Edit();
                    }
                }
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.DrawString(m_pParent.Font, m_szName, m_pPosition, m_pColor);


            if (m_bVisible)
            {
                pSpriteBatch.Draw(m_pBackground, new Vector2(100, 120), Color.White);
                for (int i = 0; i < m_nMaxItems; i++)
                {
                    m_rgItems[i].Draw(pSpriteBatch);
                }
            }
        }

    }
}
