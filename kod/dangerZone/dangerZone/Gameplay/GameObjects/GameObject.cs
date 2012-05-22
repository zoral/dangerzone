using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace dangerZone.Gameplay.GameObjects
{
    class GameObject
    {

        /*
         * Standardobjekt för player/fiender och kanske annat
         * Behöver ha textur, position, namn(?), boundingbox(kanske från farseer), pekare till sin ägare(?)
         */

        GameplayScreen m_pParent;
        Rectangle m_pBoundingBox = Rectangle.Empty;
        SimpleAnimation m_pAnimation;
        BoundingBox2D m_pBoundingBox2D;
        bool m_bCollidable = true;

        public GameObject(GameplayScreen pParent)
        {
            m_pParent = pParent;
            m_pBoundingBox2D = new BoundingBox2D(0, 0, 0, 0);
        }


        public virtual void LoadContent(ContentManager pContent)
        {

        }
        public virtual void Update(GameTime pGameTime)
        {

        }
        public virtual void Draw(SpriteBatch pSpriteBatch)
        {

        }
        public virtual void DrawHUD(SpriteBatch pSpriteBatch)
        {

        }


        public virtual SimpleAnimation Animation
        {
            get { return m_pAnimation; }
            set { m_pAnimation = value; }
        }
        public GameplayScreen Parent
        {
            get { return m_pParent; }
            set { m_pParent = value; }
        }

        public BoundingBox2D BoundingBox
        {
            get { return m_pBoundingBox2D; }
            set { m_pBoundingBox2D = value; }
        }

        public bool Collidiable
        {
            get { return m_bCollidable; }
            set { m_bCollidable = value; }
        }
    }
}
