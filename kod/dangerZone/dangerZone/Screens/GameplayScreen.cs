using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Gameplay.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using dangerZone.Particles;
using dangerZone.Gameplay;
using dangerZone.Gameplay.Lights;
using dangerZone.Gameplay.GameObjects.Enemies;
using dangerZone.Gameplay.Animations;
using Microsoft.Xna.Framework.Input;
using dangerZone.Gameplay.LevelItems;
using dangerZone.Gameplay.Editor;
using dangerZone.Gameplay.Configuration;

namespace dangerZone.Screens
{
    class GameplayScreen : Screen
    {
        Camera m_pCamera;
        Player[] m_rgPlayers = new Player[2];
        BulletManager m_pBulletManager;
        ParticleEngine m_pParticleEngine;
        LightEngine m_pLightEngine;
        EnemyManager m_pEnemyManager;
        AnimationManager m_pAnimationManager;

        GameConfiguration m_pGameConfiguration;
        GameEditor m_pGameEditor;

        GameObject[] m_rgLevelItems = new GameObject[1];

        ScrollerDisplay.ScrollerDisplay m_pScrollerDisplay;

        RenderTarget2D m_pGameRenderTarget;
        RenderTarget2D m_pLightRenderTarget;
        Effect m_pLightEffect;
        Effect m_pBlurEffect;

        SpriteFont m_pFont;
        Level m_pLevel;
        
        int m_nFrameRate = 0;
        int m_nFrameCounter = 0;
        TimeSpan m_pElapsedTime = TimeSpan.Zero;

        KeyboardState m_pPrevKBState;

        bool m_bDisplayDebug = false;

        float m_fGameOverTime = 0;
        float m_fMaxGameOverTime = 10000;
        


        public GameplayScreen(DangerZone pParent)
            : base(pParent)
        {

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {

            m_pGameConfiguration = new GameConfiguration();
            m_pGameConfiguration.Load();

            m_pCamera = new Camera(this.Parent.GraphicsDevice);
            m_pCamera.Zoom = 3;

            m_pParticleEngine = new ParticleEngine(this);
            m_pParticleEngine.LoadContent(pContent);

            /*
            m_pParticleEngine.Emitter(new Vector2(0, 0), new Vector2(0, 0), Color.Blue, 10000, 500);
            m_pParticleEngine.ExplosionEmitter(new Vector2(0, -4), Color.Red, Color.Yellow, 500, 50);
            */
            m_pLevel = new Level(this);
            m_pLevel.LoadContent(pContent);


            m_pBulletManager = new BulletManager(this);
            m_pBulletManager.LoadContent(pContent);

            m_rgPlayers[0] = new Player(this, 1);
            if(this.Parent.Settings.TwoPlayers)
                m_rgPlayers[1] = new Player(this, 2);
            else
                m_rgPlayers[1] = new Player(this, 0);
            
            m_rgPlayers[0].LoadContent(pContent);
            m_rgPlayers[1].LoadContent(pContent);

            

            m_rgPlayers[0].PlayerIndex = Parent.Settings.PlayerOne;
            m_rgPlayers[1].PlayerIndex = Parent.Settings.PlayerTwo;
            m_rgPlayers[0].Speed = m_pGameConfiguration.PlayerSpeed;
            m_rgPlayers[1].Speed = m_pGameConfiguration.PlayerSpeed;

            m_pEnemyManager = new EnemyManager(this);
            m_pEnemyManager.LoadContent(pContent);
            //m_pEnemyManager.InitRandom();

            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\debugFont");

            m_pLightEffect = pContent.Load<Effect>("Effects\\lightEffect");
            m_pBlurEffect = pContent.Load<Effect>("Effects\\blurEffect");

            m_pLightEngine = new LightEngine(this);
            m_pLightEngine.LoadContent(pContent);

            m_pAnimationManager = new AnimationManager(this);
            m_pAnimationManager.LoadContent(pContent);

            m_pScrollerDisplay = new ScrollerDisplay.ScrollerDisplay();
            m_pScrollerDisplay.LoadContent(pContent);

            m_pGameRenderTarget = new RenderTarget2D(this.Parent.GraphicsDevice, this.Parent.GraphicsDevice.PresentationParameters.BackBufferWidth, this.Parent.GraphicsDevice.PresentationParameters.BackBufferHeight);
            m_pLightRenderTarget = new RenderTarget2D(this.Parent.GraphicsDevice, this.Parent.GraphicsDevice.PresentationParameters.BackBufferWidth, this.Parent.GraphicsDevice.PresentationParameters.BackBufferHeight);

            m_rgLevelItems[0] = new FlickeringLight(this);
            m_rgLevelItems[0].LoadContent(pContent);
            //m_rgLevelItems[1] = new FlickeringLight(this);

            m_pGameEditor = new GameEditor(this);
            m_pGameEditor.LoadContent(pContent);

            base.LoadContent(pContent);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime pGameTime)
        {
            m_pElapsedTime += pGameTime.ElapsedGameTime;
            if (m_pElapsedTime > TimeSpan.FromSeconds(1))
            {
                m_pElapsedTime -= TimeSpan.FromSeconds(1);
                m_nFrameRate = m_nFrameCounter;
                m_nFrameCounter = 0;
            }

            if (m_rgPlayers[0].Dead() && m_rgPlayers[1].Dead())
            {
                // Då ska vi räkna ner timern.
                m_fGameOverTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
                if (m_fGameOverTime > m_fMaxGameOverTime)
                {
                    /*
                     * Då ska vi ställa in tiden från båda spelarna
                     */

                    this.Parent.Settings.PlayerOnePlayTime = m_rgPlayers[0].PlayTime;
                    this.Parent.Settings.PlayerTwoPlayTime = m_rgPlayers[1].PlayTime;

                    this.Parent.LoadScreen(AvailableScreens.GAMEOVER);
                    return;
                }
            }

            if (m_pGameEditor.Visible())
            {
                m_pGameEditor.Update(pGameTime);
                return;
            }

            m_rgPlayers[0].Update(pGameTime);
            
            m_rgPlayers[1].Update(pGameTime);
            
            m_pBulletManager.Update(pGameTime);
            
            m_pParticleEngine.Update(pGameTime);
            
            m_pEnemyManager.Update(pGameTime);

            m_pLightEngine.Update(pGameTime);

            m_pAnimationManager.Update(pGameTime);

            m_pScrollerDisplay.Update(pGameTime);

            if (this.Parent.Settings.TwoPlayers)
            {
                Vector2 pCameraPosition = (m_rgPlayers[0].Animation.Position + m_rgPlayers[1].Animation.Position) / 2;
                m_pCamera.Position = pCameraPosition;
            }
            else
            {
                m_pCamera.Position = m_rgPlayers[0].Animation.Position;
            }

            m_pCamera.Update(pGameTime);

            m_pLevel.Update(pGameTime);

            KeyboardState pKBState = Keyboard.GetState();
            if (m_pPrevKBState == null)
            {
                m_pPrevKBState = pKBState;
                return;
            }
            if (m_pPrevKBState.IsKeyUp(Keys.F) && pKBState.IsKeyDown(Keys.F))
            {
                m_pParticleEngine.ExplosionEmitter(new Vector2(0, -4), Color.Red, Color.Yellow, 500, 50);
            }
            if (m_pPrevKBState.IsKeyUp(Keys.D) && pKBState.IsKeyDown(Keys.D))
            {
                m_bDisplayDebug = (m_bDisplayDebug) ? false : true;
            }

            if (m_pPrevKBState.IsKeyUp(Keys.E) && pKBState.IsKeyDown(Keys.E))
            {
                m_pGameEditor.Display();
            }


            m_rgLevelItems[0].Update(pGameTime);

            m_pPrevKBState = pKBState;

            

            base.Update(pGameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch)
        {
            m_nFrameCounter++;
            //pSpriteBatch.Begin();

            this.Parent.GraphicsDevice.SetRenderTarget(m_pGameRenderTarget);
            this.Parent.GraphicsDevice.Clear(Color.Black);

            pSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, m_pCamera.View);
            
            // Måla ut level
            m_pLevel.Draw(pSpriteBatch);

            m_rgLevelItems[0].Draw(pSpriteBatch);

            m_pAnimationManager.Draw(pSpriteBatch);

            // Fiender
            m_pEnemyManager.Draw(pSpriteBatch);
            //pSpriteBatch.End();


           // pSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, m_pCamera.View);
            // Bullets
            //m_pBlurEffect.CurrentTechnique.Passes[0].Apply();
            m_pBulletManager.Draw(pSpriteBatch);
            //pSpriteBatch.End();

            //pSpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, m_pCamera.View);
            // Spelare
            m_rgPlayers[0].Draw(pSpriteBatch);
           
            m_rgPlayers[1].Draw(pSpriteBatch);

            // Partiklar
            m_pParticleEngine.Draw(pSpriteBatch, true);

            

            
            pSpriteBatch.End();

            this.Parent.GraphicsDevice.SetRenderTarget(null);

            this.Parent.GraphicsDevice.SetRenderTarget(m_pLightRenderTarget);
            this.Parent.GraphicsDevice.Clear(Color.Black);
            m_pLightEngine.Draw(m_pCamera, pSpriteBatch);

            this.Parent.GraphicsDevice.SetRenderTarget(null);

            this.Parent.GraphicsDevice.Clear(Color.Black);

            pSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            m_pLightEffect.Parameters["pLightRenderTarget"].SetValue(m_pLightRenderTarget);
            m_pLightEffect.CurrentTechnique.Passes[0].Apply();
            
            pSpriteBatch.Draw(m_pGameRenderTarget, new Vector2(0, 0), Color.White);
            pSpriteBatch.End();

            // Kör drawhud det sista vi gör. Utanför kameran och ovanför allting.
            this.DrawHUD(pSpriteBatch);

            base.Draw(pSpriteBatch);
        }

        public void DrawHUD(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Begin();

            m_pEnemyManager.DrawHUD(pSpriteBatch);


            m_rgPlayers[0].DrawHUD(pSpriteBatch);
            m_rgPlayers[1].DrawHUD(pSpriteBatch);

            m_pScrollerDisplay.Draw(pSpriteBatch);

            if (m_bDisplayDebug)
            {
                pSpriteBatch.DrawString(m_pFont, "Active bullets: " + m_pBulletManager.ActiveBullets.ToString() + " / " + Constants.MAXBULLETS, new Vector2(600, 10), Color.White);

                pSpriteBatch.DrawString(m_pFont, "Active particles: " + m_pParticleEngine.ActiveParticles.ToString() + " / " + Constants.MAXPARTICLES, new Vector2(600, 20), Color.White);

                pSpriteBatch.DrawString(m_pFont, "Active particle emitters: " + m_pParticleEngine.ActiveEmitters.ToString() + " / " + Constants.MAXEMITTERS, new Vector2(600, 30), Color.White);

                pSpriteBatch.DrawString(m_pFont, "Active enemies: " + m_pEnemyManager.ActiveEnemies + " / " + Constants.MAXENEMIES, new Vector2(600, 40), Color.White);

                pSpriteBatch.DrawString(m_pFont, "FPS: " + m_nFrameRate.ToString(), new Vector2(600, 690), Color.White);
            }

            pSpriteBatch.End();

            m_pGameEditor.Draw(pSpriteBatch);

        }

        public BulletManager BulletManager
        {
            get { return m_pBulletManager; }
            set { m_pBulletManager = value; }
        }

        public ParticleEngine ParticleEngine
        {
            get { return m_pParticleEngine; }
            set { m_pParticleEngine = value; }
        }

        public Level Level
        {
            get { return m_pLevel; }
            set { m_pLevel = value; }
        }

        public LightEngine LightEngine
        {
            get { return m_pLightEngine; }
            set { m_pLightEngine = value; }
        }

        public Effect LightEffect
        {
            get { return m_pLightEffect; }
            set { m_pLightEffect = value; }
        }

        public Player[] Players
        {
            get { return m_rgPlayers; }
            set { m_rgPlayers = value; }
        }

        public SpriteFont Font
        {
            get { return m_pFont; }
        }

        public EnemyManager EnemyManager
        {
            get { return m_pEnemyManager; }
            set { m_pEnemyManager = value; }
        }

        public AnimationManager AnimationManager
        {
            get { return m_pAnimationManager; }
            set { m_pAnimationManager = value; }
        }

        public ScrollerDisplay.ScrollerDisplay ScrollerDisplay
        {
            get { return m_pScrollerDisplay; }
            set { m_pScrollerDisplay = value; }
        }

        public GameConfiguration GameConfiguration
        {
            get { return m_pGameConfiguration; }
            set { m_pGameConfiguration = value; }
        }

    }
}
