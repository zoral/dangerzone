using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace dangerZone.Screens
{
    class Screen
    {

        DangerZone m_pParent;

        public Screen(DangerZone pParent)
        {
            m_pParent = pParent;
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


        public DangerZone Parent
        {
            get { return m_pParent; }
            set { m_pParent = value; }
        }

        public Random Random
        {
            get { return m_pParent.Random; }
        }


        
    }
}
