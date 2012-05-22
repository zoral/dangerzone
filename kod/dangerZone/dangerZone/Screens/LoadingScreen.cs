using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace dangerZone.Screens
{
    class LoadingScreen : Screen
    {

        SpriteFont m_pFont;

        public LoadingScreen(DangerZone pParent)
            : base(pParent)
        {

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {
            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\defaultFont");
            base.LoadContent(pContent);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime pGameTime)
        {
            base.Update(pGameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Begin();
            pSpriteBatch.DrawString(m_pFont, "LOADING", new Microsoft.Xna.Framework.Vector2(100, 100), Color.White);
            pSpriteBatch.End();
            base.Draw(pSpriteBatch);
        }

    }
}
