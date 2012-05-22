using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.HighScore;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using dangerZone.Gameplay;

namespace dangerZone.Screens
{
    class HighScoreScreen : Screen
    {


        HighScoreManager m_pManager;
        SpriteFont m_pFont;

        KeyboardState m_pPrevKBState;
        GamePadState m_pPrevGPState;
        


        public HighScoreScreen(DangerZone pParent)
            : base(pParent)
        {
            

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {
            m_pManager = new HighScoreManager();
            m_pManager.LoadContent(pContent);

            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\defaultFont");

            base.LoadContent(pContent);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime pGameTime)
        {
            KeyboardState pKBState = Keyboard.GetState();
            GamePadState pGPState = GamePad.GetState(this.Parent.Settings.PlayerOne);

            if (m_pPrevKBState == null)
            {
                m_pPrevKBState = pKBState;
                m_pPrevGPState = pGPState;
                return;
            }

            if (m_pPrevKBState.IsKeyUp(Keys.A) && pKBState.IsKeyDown(Keys.A))
            {
                m_pManager.AddHighscore("thomas", this.Parent.Random.Next(0, 100), this.Parent.Random.Next(0, 1000000), 120);
            }

            if ( (m_pPrevKBState.IsKeyDown(Keys.Escape) && pKBState.IsKeyDown(Keys.Escape)) ||
                (m_pPrevGPState.Buttons.B == ButtonState.Released && pGPState.Buttons.B == ButtonState.Pressed))
            {
                this.Parent.LoadScreen(AvailableScreens.MAINMENU);
                
            }


            m_pPrevKBState = pKBState;
            base.Update(pGameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            pSpriteBatch.DrawString(m_pFont, "#", new Vector2(200, 50), Color.White);
            pSpriteBatch.DrawString(m_pFont, "Name", new Vector2(250, 50), Color.White);
            pSpriteBatch.DrawString(m_pFont, "Waves", new Vector2(500, 50), Color.White);
            pSpriteBatch.DrawString(m_pFont, "Play time", new Vector2(650, 50), Color.White);
            pSpriteBatch.DrawString(m_pFont, "Score", new Vector2(800, 50), Color.White);
            
            

            for (int i = 0; i < 10; i++)
            {
                pSpriteBatch.DrawString(m_pFont, (i + 1).ToString(), new Vector2(200, 100 + (i*30)), Color.White);
                pSpriteBatch.DrawString(m_pFont, m_pManager.List[i].PlayerName, new Vector2(250, 100 + (i * 30)), Color.White);
                pSpriteBatch.DrawString(m_pFont, m_pManager.List[i].Wave.ToString(), new Vector2(500, 100 + (i * 30)), Color.White);
                pSpriteBatch.DrawString(m_pFont, m_pManager.List[i].PlayTime.ToString()+" sec", new Vector2(650, 100 + (i * 30)), Color.White);
                pSpriteBatch.DrawString(m_pFont, m_pManager.List[i].Score.ToString(), new Vector2(800, 100 + (i * 30)), Color.White);
  

            }
            
            pSpriteBatch.Draw(Constants.BUTTON_B, new Vector2(1100, 600), Color.White);
            pSpriteBatch.DrawString(m_pFont, "Back", new Vector2(1050, 600), Color.White);

            pSpriteBatch.End();

            base.Draw(pSpriteBatch);
        }

        public HighScoreManager Manager
        {
            get { return m_pManager; }
            set { m_pManager = value; }
        }


    }
}
