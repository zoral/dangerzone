using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using dangerZone.HighScore;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace dangerZone.Screens
{
    class GameOverScreen : Screen
    {

        SpriteFont m_pFont;
        DangerZone m_pParent;
        HighScoreManager m_pHighscore;

        KeyboardState m_pPrevKBState;
        GamePadState m_pPrevGPState;

        bool m_bPlayerOneHighscore = false;
        bool m_bPlayerTwoHighscore = false;
        int m_nPlayerOnePosition = -1;
        int m_nPlayerTwoPosition = -1;

        public GameOverScreen(DangerZone pParent)
            : base(pParent)
        {
            m_pParent = pParent;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {

            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\defaultFont");
            m_pHighscore = new HighScoreManager();
            m_pHighscore.LoadContent(pContent);

            /*
             * Vi ska väl se ifall det var något highscore kanske? 
             */
            if (this.Parent.Settings.PlayerOneScore > this.Parent.Settings.PlayerTwoScore)
            {
                m_nPlayerOnePosition = (m_pHighscore.AddHighscore(this.Parent.Settings.PlayerOneName, this.Parent.Settings.PlayerOneWave, this.Parent.Settings.PlayerOneScore, this.Parent.Settings.PlayerOnePlayTime));
                m_nPlayerTwoPosition = (m_pHighscore.AddHighscore(this.Parent.Settings.PlayerTwoName, this.Parent.Settings.PlayerTwoWave, this.Parent.Settings.PlayerTwoScore, this.Parent.Settings.PlayerTwoPlayTime));
            }
            else
            {
                m_nPlayerTwoPosition = (m_pHighscore.AddHighscore(this.Parent.Settings.PlayerTwoName, this.Parent.Settings.PlayerTwoWave, this.Parent.Settings.PlayerTwoScore, this.Parent.Settings.PlayerTwoPlayTime));
                m_nPlayerOnePosition = (m_pHighscore.AddHighscore(this.Parent.Settings.PlayerOneName, this.Parent.Settings.PlayerOneWave, this.Parent.Settings.PlayerOneScore, this.Parent.Settings.PlayerOnePlayTime));
            }


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

            if (m_pPrevKBState.IsKeyUp(Keys.Enter) && pKBState.IsKeyDown(Keys.Enter))
            {
                this.Parent.LoadScreen(AvailableScreens.MAINMENU);
            }
            if (m_pPrevGPState.Buttons.Start == ButtonState.Released && pGPState.Buttons.Start == ButtonState.Pressed)
            {
                this.Parent.LoadScreen(AvailableScreens.MAINMENU);
            }



            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {

            /*
             * Skriv ut namnen på spelarna.
             * Vilka vågor de kom till, vilken poäng de fick
             * och i rött under: om de fick highscore(och vilken plats!)
             */

            pSpriteBatch.Begin();

            pSpriteBatch.DrawString(m_pFont, "Player one", new Vector2(100, 100), Color.White);
            pSpriteBatch.DrawString(m_pFont, this.Parent.Settings.PlayerOneName, new Vector2(100, 120), Color.White);
            pSpriteBatch.DrawString(m_pFont, "Wave: " + this.Parent.Settings.PlayerOneWave, new Vector2(100, 140), Color.White);
            pSpriteBatch.DrawString(m_pFont, "Score: " + this.Parent.Settings.PlayerOneScore, new Vector2(100, 160), Color.White);
            if (m_nPlayerOnePosition != -1)
            {
                pSpriteBatch.DrawString(m_pFont, "HIGHSCORE! Position: " + m_nPlayerOnePosition, new Vector2(100, 180), Color.Red);
            }

            if (this.Parent.Settings.TwoPlayers)
            {
                pSpriteBatch.DrawString(m_pFont, "Player two", new Vector2(500, 100), Color.White);
                pSpriteBatch.DrawString(m_pFont, this.Parent.Settings.PlayerTwoName, new Vector2(500, 120), Color.White);
                pSpriteBatch.DrawString(m_pFont, "Wave: " + this.Parent.Settings.PlayerTwoWave, new Vector2(500, 140), Color.White);
                pSpriteBatch.DrawString(m_pFont, "Score: " + this.Parent.Settings.PlayerTwoScore, new Vector2(500, 160), Color.White);
                if (m_nPlayerTwoPosition != -1)
                {
                    pSpriteBatch.DrawString(m_pFont, "HIGHSCORE! Position: " + m_nPlayerTwoPosition, new Vector2(500, 180), Color.Red);
                }
            }

            pSpriteBatch.End();

            base.Draw(pSpriteBatch);
        }

    }
}
