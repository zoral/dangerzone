using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using dangerZone.Screens;
using System.Threading;
using dangerZone.Gameplay;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using Awesomium.Core;

namespace dangerZone
{

    public enum AvailableScreens
    {
        TITLE,
        MAINMENU,
        OPTIONS,
        CREDITS,
        GAMEPLAY,
        GAMEOVER,
        HIGHSCORE
    };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DangerZone : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch m_pSpriteBatch;

        Random m_pRandom;

        bool m_bAllLoaded = true;
        Screen m_pScreenToLoad = null;
        Screen m_pCurrentScreen = null;
        LoadingScreen m_pLoadingScreen;

        Settings m_pGameplaySettings;



        public DangerZone()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            // In med fullscreen här också kanske

            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
#if XBOX
            Components.Add(new GamerServicesComponent(this));
#endif



            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            base.Initialize();
            

#if XBOX
            Guide.ShowSignIn(1, false);
#endif 
            m_pRandom = new Random();

            m_pGameplaySettings = new Settings();
            /*
             * Läs in settings från en fil(om den finns sen innan!)
             */
           // 
           

            ReadSettingsStorage();

        }

        private void ReadSettingsStorage()
        {
            IAsyncResult pResult2 = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice pDevice = StorageDevice.EndShowSelector(pResult2);

            IAsyncResult pResult = pDevice.BeginOpenContainer("DangerZone", null, null);
            pResult.AsyncWaitHandle.WaitOne();
            StorageContainer pContainer = pDevice.EndOpenContainer(pResult);
            pResult.AsyncWaitHandle.Close();

            string szFilename = "dangerzonesettings.set";
            if (!pContainer.FileExists(szFilename))
            {
                pContainer.Dispose();
                return;
            }

            Stream pStream = pContainer.OpenFile(szFilename, System.IO.FileMode.Open);

            XmlSerializer pSerializer = new XmlSerializer(typeof(Settings));
            Settings pSettings = (Settings)pSerializer.Deserialize(pStream);
            pStream.Close();
            pContainer.Dispose();

            m_pGameplaySettings.PlayerOneName = pSettings.PlayerOneName;
            m_pGameplaySettings.PlayerTwoName = pSettings.PlayerTwoName;
            m_pGameplaySettings.PlayMusic = pSettings.PlayMusic;
            m_pGameplaySettings.PlaySFX = pSettings.PlaySFX;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Constants.BUTTON_A = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerButtonA");
            Constants.BUTTON_B = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerButtonB");
            Constants.BUTTON_X = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerButtonX");
            Constants.BUTTON_Y = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerButtonY");
            Constants.BUTTON_START = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerStart");
            Constants.BUTTON_BACK = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerBack");
            Constants.BUTTON_DPAD = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerDPad");
            Constants.BUTTON_GUIDE = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerButtonGuide");
            Constants.BUTTON_LEFTSHOULDER = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerLeftShoulder");
            Constants.BUTTON_LEFTTHUMBSTICK = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerLeftThumbstick");
            Constants.BUTTON_LEFTTRIGGER = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerLeftTrigger");
            Constants.BUTTON_RIGHTSHOULDER = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerRightShoulder");
            Constants.BUTTON_RIGHTTHUMBSTICK = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerRightThumbstick");
            Constants.BUTTON_RIGHTTRIGGER = Content.Load<Texture2D>("GFX\\XBOXCONTROLLER\\xboxControllerRightTrigger");


            // Create a new SpriteBatch, which can be used to draw textures.
            m_pSpriteBatch = new SpriteBatch(GraphicsDevice);
            m_pLoadingScreen = new LoadingScreen(this);
            m_pLoadingScreen.LoadContent(Content);
            LoadScreen(AvailableScreens.TITLE);

            // TODO: use this.Content to load your game content here
        }

        public void LoadScreen(AvailableScreens pScreen)
        {
            if (m_bAllLoaded)
            {
                m_bAllLoaded = false;
                switch (pScreen)
                {
                    case AvailableScreens.TITLE:
                        m_pScreenToLoad = new TitleScreen(this);
                        break;
                    case AvailableScreens.HIGHSCORE:
                        m_pScreenToLoad = new HighScoreScreen(this);
                        break;
                    case AvailableScreens.GAMEOVER:
                        m_pScreenToLoad = new GameOverScreen(this);
                        break;
                    case AvailableScreens.GAMEPLAY:
                        m_pScreenToLoad = new GameplayScreen(this);
                        break;
                    case AvailableScreens.MAINMENU:
                        m_pScreenToLoad = new MainMenuScreen(this);
                        break;
                    case AvailableScreens.OPTIONS:
                        m_pScreenToLoad = new OptionsScreen(this);
                        break;
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadScreenContent));
            }
            else
            {
                Console.WriteLine("SCREENS IS ALREADY LOADING! PLEASE WAIT");
            }
            
        }

        private void LoadScreenContent(object pState)
        {
            if (m_pScreenToLoad != null)
            {
                m_pScreenToLoad.LoadContent(Content);
                m_pCurrentScreen = m_pScreenToLoad;
                m_pScreenToLoad = null;
            }

            m_bAllLoaded = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime pGameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (m_bAllLoaded)
                m_pCurrentScreen.Update(pGameTime);
            else
                m_pLoadingScreen.Update(pGameTime);

            base.Update(pGameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (m_bAllLoaded)
                m_pCurrentScreen.Draw(m_pSpriteBatch);
            else
                m_pLoadingScreen.Draw(m_pSpriteBatch);

            base.Draw(gameTime);
        }

        public Random Random
        {
            get { return m_pRandom; }
        }

        public Settings Settings
        {
            get { return m_pGameplaySettings; }
            set { m_pGameplaySettings = value; }
        }

    }
}
