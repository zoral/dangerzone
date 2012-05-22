using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay
{
    class BoundingBox2D
    {

        Vector2 m_pPosition;
        Vector2 m_pSize;

        public bool Intersects(BoundingBox2D pOther)
        {
            /*
             * Kolla ifall vi intersektar med den!
             */
            /*
            if (this.Bottom < pBoundingBox.Top) return false;
            if (this.Top > pBoundingBox.Bottom) return false;
            if (this.Right < pBoundingBox.Left) return false;
            if (this.Left > pBoundingBox.Right) return false;

            return true;*/
            return !(this.Left > pOther.Right || this.Right < pOther.Left ||
                this.Top > pOther.Bottom || this.Bottom < pOther.Top);
        }

        public override string ToString()
        {
            string szTemp = m_pPosition.ToString();
            szTemp += m_pSize.ToString();

            return szTemp;
        }

        public BoundingBox2D(float fX, float fY, float fWidth, float fHeight)
        {
            m_pPosition = Vector2.Zero;
            m_pPosition.X = fX;// +(fWidth / 2);
            m_pPosition.Y = fY;// +(fHeight / 2);

            m_pSize = Vector2.Zero;
            m_pSize.X = fWidth;
            m_pSize.Y = fHeight;
        }

        public void Update(float fX, float fY)
        {
            m_pPosition.X = fX;// +(m_pSize.X / 2);
            m_pPosition.Y = fY;// +(m_pSize.Y / 2);
        }

        public void UpdateSize(float fWidth, float fHeight)
        {
            m_pSize.X = fWidth;
            m_pSize.Y = fHeight;
        }

        public float Bottom
        {
            get { return m_pPosition.Y + (m_pSize.Y / 2); }
        }

        public float Top
        {
            get { return m_pPosition.Y - (m_pSize.Y / 2); }
        }
        public float Left
        {
            get { return m_pPosition.X - (m_pSize.X / 2); }
        }
        public float Right
        {
            get { return m_pPosition.X + (m_pSize.X / 2); }
        }

    }
}
