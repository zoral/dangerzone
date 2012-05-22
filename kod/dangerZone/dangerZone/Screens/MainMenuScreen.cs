using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using dangerZone.Gameplay;
using dangerZone.Gameplay.GameObjects;

namespace dangerZone.Screens
{
    class MainMenuScreen : Screen
    {
        DangerZone m_pParent;
        MainMenu m_pMainMenu;

        GamePadState m_pPrevState;
        KeyboardState m_pPrevKBState;
        bool m_bFirstRun = true;

        _3DModel m_pModel;

        Camera3D m_pCamera3D;
        SpriteFont m_pScrollerFont;
        Vector2 m_pNamePosition = Vector2.Zero;

        public MainMenuScreen(DangerZone pParent)
            : base(pParent)
        {
            m_pParent = pParent;
        }

        public override void LoadContent(ContentManager pContent)
        {

            m_pCamera3D = new Camera3D(this.Parent);

            m_pMainMenu = new MainMenu();
            m_pMainMenu.LoadContent(pContent);

            m_pModel = new _3DModel("GFX\\Models\\p1_wedge");
            m_pModel.LoadContent(pContent);
            Vector3 pPosition = m_pModel.Position;
            pPosition.X = -1000;
            pPosition.Y = 2000;
            m_pModel.Position = pPosition;

            m_pScrollerFont = pContent.Load<SpriteFont>("GFX\\Fonts\\ScrollerFont");

            Vector2 pTemp = m_pScrollerFont.MeasureString("D A N G E R  Z O N E");
            m_pNamePosition = new Vector2(((1280 / 2) - (pTemp.X / 2)), 100);

            base.LoadContent(pContent);
        }

        public override void Update(GameTime pGameTime)
        {
            KeyboardState pKBState = Keyboard.GetState();
            GamePadState pGPState = GamePad.GetState(m_pParent.Settings.PlayerOne);

            
            if (m_bFirstRun)
            {
                m_bFirstRun = false;
                m_pPrevKBState = pKBState;
                m_pPrevState = pGPState;
                return;
            }

            if (m_pPrevKBState.IsKeyUp(Keys.Down) && pKBState.IsKeyDown(Keys.Down))
            {
                m_pMainMenu.NextItem();
            }
            if (m_pPrevKBState.IsKeyUp(Keys.Up) && pKBState.IsKeyDown(Keys.Up))
            {
                m_pMainMenu.PreviousItem();
            }

            if (m_pPrevKBState.IsKeyUp(Keys.Enter) && pKBState.IsKeyDown(Keys.Enter))
            {
                LoadScreen(m_pMainMenu.UseItem());
            }


            if ( (m_pPrevState.ThumbSticks.Left.Y == 0 && pGPState.ThumbSticks.Left.Y > 0) ||
                (m_pPrevState.DPad.Down == ButtonState.Released && pGPState.DPad.Down == ButtonState.Pressed))
            {
                // neråt.
                m_pMainMenu.NextItem();
            }
            if ((m_pPrevState.ThumbSticks.Left.Y == 0 && pGPState.ThumbSticks.Left.Y < 0) ||
                (m_pPrevState.DPad.Up == ButtonState.Released && pGPState.DPad.Up == ButtonState.Pressed))
            {
                m_pMainMenu.PreviousItem();
            }
            if( (m_pPrevState.Buttons.A == ButtonState.Released && pGPState.Buttons.A == ButtonState.Pressed) ||
                (m_pPrevState.Buttons.Start == ButtonState.Released && pGPState.Buttons.Start == ButtonState.Pressed))
            {
                LoadScreen(m_pMainMenu.UseItem());
            }


            if (!this.Parent.Settings.TwoPlayers)
            {
                for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
                {
                    if (i == this.Parent.Settings.PlayerOne)
                        continue;
                    

                    // Se ifall denna kontrollen trycker på Start. Isf är han player 2.
                    GamePadState pState = GamePad.GetState(i);

                    if (pState.IsConnected)
                    {
                        if (pState.Buttons.Start == ButtonState.Pressed)
                        {
                            this.Parent.Settings.TwoPlayers = true;
                            this.Parent.Settings.PlayerTwo = i;
                            return;
                        }
                    }
                }
            }

            Vector3 pTemp = Vector3.Zero;
            pTemp.Y = (pGPState.ThumbSticks.Right.X/10);
            pTemp.X = (pGPState.ThumbSticks.Right.Y/10);

            m_pModel.Rotation += pTemp;

            pTemp = Vector3.Zero;
            pTemp.X = pGPState.ThumbSticks.Left.X * 10;
            pTemp.Y = pGPState.ThumbSticks.Left.Y * 10;
            if (pTemp != Vector3.Zero)
                m_pModel.Position += pTemp;
            
            
            m_pModel.Update(pGameTime);

            m_pPrevKBState = pKBState;
            m_pPrevState = pGPState;

            

            m_pMainMenu.Update(pGameTime);
            base.Update(pGameTime);
        }

        private void LoadScreen(MainMenuItems pItem)
        {
            AvailableScreens pScreen = AvailableScreens.MAINMENU;
            switch (pItem)
            {
                case MainMenuItems.NEWGAME:
                    pScreen = AvailableScreens.GAMEPLAY;
                    break;
                case MainMenuItems.HIGHSCORE:
                    pScreen = AvailableScreens.HIGHSCORE;
                    break;
                case MainMenuItems.OPTIONS:
                    pScreen = AvailableScreens.OPTIONS;
                    break;
                case MainMenuItems.CREDITS:
                    pScreen = AvailableScreens.CREDITS;
                    break;
                case MainMenuItems.QUIT:
                    m_pParent.Exit();
                    break;
            }

            m_pParent.LoadScreen(pScreen);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {


            m_pModel.Draw(m_pCamera3D);


            pSpriteBatch.Begin();
            m_pMainMenu.Draw(pSpriteBatch);

            pSpriteBatch.DrawString(m_pScrollerFont, "D A N G E R  Z O N E", m_pNamePosition, Color.White);
            pSpriteBatch.DrawString(m_pMainMenu.Font, "krutet", new Vector2(50, 600), Color.Tomato);

            pSpriteBatch.DrawString(m_pMainMenu.Font, "Player one: " + this.Parent.Settings.PlayerOneName + "", new Vector2(900, 200), Color.White);
            pSpriteBatch.DrawString(m_pMainMenu.Font, "Controller: " + this.Parent.Settings.PlayerOne.ToString(), new Vector2(900, 220), Color.White);

            if (this.Parent.Settings.TwoPlayers)
            {
                pSpriteBatch.DrawString(m_pMainMenu.Font, "Player two: " + this.Parent.Settings.PlayerTwoName + "", new Vector2(900, 300), Color.White);
                pSpriteBatch.DrawString(m_pMainMenu.Font, "Controller: " + this.Parent.Settings.PlayerTwo.ToString(), new Vector2(900, 320), Color.White);
            }
            else
            {
                pSpriteBatch.DrawString(m_pMainMenu.Font, "Player two: PRESS START", new Vector2(900, 300), Color.White);
            }

            pSpriteBatch.End();
            
            base.Draw(pSpriteBatch);
        }
    }

    class MainMenu
    {
        MainMenuItems m_pSelectedItem;
        MainMenuItem[] m_rgItems = new MainMenuItem[5];

        SpriteFont m_pFont;

        public MainMenu()
        {

        }

        public void LoadContent(ContentManager pContent)
        {
            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\defaultFont");
            m_rgItems[0] = new MainMenuItem(MainMenuItems.NEWGAME, "New game", true, this);
            m_rgItems[1] = new MainMenuItem(MainMenuItems.HIGHSCORE, "High score", false, this);
            m_rgItems[2] = new MainMenuItem(MainMenuItems.OPTIONS, "Options", false, this);
            m_rgItems[3] = new MainMenuItem(MainMenuItems.CREDITS, "Credits", false, this);
            m_rgItems[4] = new MainMenuItem(MainMenuItems.QUIT, "Quit", false, this);

        }

        public void Update(GameTime pGameTime)
        {
            for (int i = 0; i < 5; i++)
                m_rgItems[i].Update(pGameTime);




        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            for (int i = 0; i < 5; i++)
                m_rgItems[i].Draw(pSpriteBatch);
        }

        public void NextItem()
        {
            m_rgItems[(int)m_pSelectedItem].Selected = false;
            m_pSelectedItem++;
            if ((int)m_pSelectedItem > 4)
                m_pSelectedItem = MainMenuItems.NEWGAME;
            m_rgItems[(int)m_pSelectedItem].Selected = true;
        }

        public void PreviousItem()
        {
            m_rgItems[(int)m_pSelectedItem].Selected = false;
            m_pSelectedItem--;
            if ((int)m_pSelectedItem < 0)
                m_pSelectedItem = MainMenuItems.QUIT;
            m_rgItems[(int)m_pSelectedItem].Selected = true;
        }

        public MainMenuItems UseItem()
        {
            return m_pSelectedItem;
        }

        public SpriteFont Font
        {
            get { return m_pFont; }
            set { m_pFont = value; }
        }
    }

    enum MainMenuItems
    {
        NEWGAME,
        HIGHSCORE,
        OPTIONS,
        CREDITS,
        QUIT,
        EMPTY
    };

    class MainMenuItem
    {

        MainMenuItems m_pItem = MainMenuItems.EMPTY;
        string m_szName = "";
        bool m_bSelected = false;
        Vector2 m_pPosition = Vector2.Zero;
        Color m_pColor = Color.White;
        MainMenu m_pParent;
        public MainMenuItem(MainMenuItems pItem, string szName, bool bSelected, MainMenu pParent)
        {
            m_pParent = pParent;
            m_pItem = pItem;
            m_szName = szName;
            m_bSelected = bSelected;
            Vector2 pStringSize = pParent.Font.MeasureString(m_szName);
            m_pPosition.X = (200); //1280 / 2  - (pStringSize.X / 2)
            m_pPosition.Y = 200 + ((int)pItem * 30);
        }

        public void Update(GameTime pGameTime)
        {
            if (m_bSelected)
                m_pColor = Color.Red;
            else
                m_pColor = Color.White;
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.DrawString(m_pParent.Font, m_szName, m_pPosition, m_pColor);
        }

        public bool Selected
        {
            get { return m_bSelected; }
            set { m_bSelected = value; }
        }
    }
}
