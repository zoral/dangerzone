using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using dangerZone.Particles;
using Microsoft.Xna.Framework.GamerServices;
using dangerZone.Gameplay;
using Awesomium.Core;
using AwesomiumSharpXna;
using System.IO;

namespace dangerZone.Screens
{
    class TitleScreen : Screen
    {

        SpriteFont m_pFont;
        ParticleEngine m_pParticleEngine;

        ScrollerDisplay.ScrollerDisplay m_pScrollerDisplay;

        bool m_bLoading = false;

        Texture2D m_pTexture;
#if WINDOWS
        WebView m_pWebView;

#endif

        public TitleScreen(DangerZone pParent)
            : base(pParent)
        {

#if WINDOWS
            WebCoreConfig pConfig = new WebCoreConfig();
            pConfig.EnableJavascript = true;
            //WebCore.Initialize(pConfig, true);
            
            this.Parent.IsMouseVisible = true;

            m_pWebView = WebCore.CreateWebView(800, 600);
            {
                m_pWebView.LoadURL("http://www.google.com"); //Content\\Editor\\index.html
                while (m_pWebView.IsLoadingPage)
                    WebCore.Update();

                m_pWebView.Render().SaveToPNG("result.png", true);

                /*
                 * Här kan vi då kanske ladda in filen?
                 */

            }
            //WebCore.Shutdown();
#endif

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {
            m_pParticleEngine = new ParticleEngine(this);
            m_pParticleEngine.LoadContent(pContent);

            m_pScrollerDisplay = new ScrollerDisplay.ScrollerDisplay();
            m_pScrollerDisplay.LoadContent(pContent);

            m_pScrollerDisplay.Display("PRESS START", 0, 0);

            for (int i = 0; i < 10; i++)
            {
                float fRandomY = (float)m_pParticleEngine.Random.Next(-10, 10) / 10;
                float fRandomX = (float)m_pParticleEngine.Random.Next(-10, 10) / 10;
                m_pParticleEngine.Emitter(new Vector2(i*100 + 100, 600), new Vector2(fRandomX, fRandomY), Color.Red, 10000, 1000);
            }
            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\defaultFont");

            
            FileStream pStream = File.OpenRead("result.png");
            m_pTexture = Texture2D.FromStream(Parent.GraphicsDevice, pStream);
            

            base.LoadContent(pContent);
        }

        private void renderToTexture()
        {
            

            /*
            m_pWebView.Render().SaveToPNG("result.png");
            FileStream pStream = File.OpenRead("result.png");
            m_pTexture = Texture2D.FromStream(Parent.GraphicsDevice, pStream);
            */
            

        }

        public override void Update(Microsoft.Xna.Framework.GameTime pGameTime)
        {
            MouseState pMState = Mouse.GetState();
            if (m_pWebView != null)
            {

                    m_pWebView.InjectMouseMove(pMState.X, pMState.Y);

                    if (pMState.LeftButton == ButtonState.Pressed)
                    {
                        Console.WriteLine(pMState.X.ToString() + ", " + pMState.Y.ToString());
                        renderToTexture();
                        m_pWebView.InjectMouseDown(MouseButton.Left);
                    }
                    if (pMState.LeftButton == ButtonState.Released)
                        m_pWebView.InjectMouseUp(MouseButton.Right);


                    
                

                //renderToTexture();
                

            }

            WebCore.Update();

            m_pParticleEngine.Update(pGameTime);
            m_pScrollerDisplay.Update(pGameTime);
            // Kolla vilken utav kontrollerna som trycker på start. Detta blir förstaplayern.
            for (int i = 0; i < 4; i++)
            {
                PlayerIndex pIndex = PlayerIndex.One;
                switch (i)
                {
                    case 0:
                        pIndex = PlayerIndex.One;
                        break;
                    case 1:
                        pIndex = PlayerIndex.Two;
                        break;
                    case 2:
                        pIndex = PlayerIndex.Three;
                        break;
                    case 3:
                        pIndex = PlayerIndex.Four;
                        break;
                }
                GamePadState m_pGamePadState = GamePad.GetState(pIndex);
                if (m_pGamePadState.Buttons.Start == ButtonState.Pressed)
                {
                    if (m_bLoading)
                        return;
                    m_bLoading = true;
                    // Då har vi player[0] på pIndex
                    Parent.Settings.PlayerOne = pIndex;
                    Parent.LoadScreen(AvailableScreens.MAINMENU);
                }

                KeyboardState pKS = Keyboard.GetState();
                if (pKS.IsKeyDown(Keys.Space))
                {
                    if (m_bLoading)
                        return;
                    m_bLoading = true;
                    Parent.Settings.PlayerOne = PlayerIndex.One;
                    Parent.LoadScreen(AvailableScreens.MAINMENU);
                }
                if (pKS.IsKeyDown(Keys.V))
                {
                    if(!Guide.IsVisible)
                        Guide.ShowPlayers(PlayerIndex.One);
                }
            }

            base.Update(pGameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Begin();

           // 

            if(m_pTexture != null)
                pSpriteBatch.Draw(m_pTexture, new Vector2(0, 0), Color.White);


            m_pParticleEngine.Draw(pSpriteBatch, false);
            m_pScrollerDisplay.Draw(pSpriteBatch);
            pSpriteBatch.DrawString(m_pFont, "Press", new Vector2(100, 100), Color.White);
            pSpriteBatch.Draw(Constants.BUTTON_START, new Vector2(140, 100), Color.White);
            pSpriteBatch.End();
            base.Draw(pSpriteBatch);
        }


    }
}
