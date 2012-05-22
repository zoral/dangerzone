using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using dangerZone.Gameplay;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;

namespace dangerZone.Screens
{
    class OptionsScreen : Screen
    {

        DangerZone m_pParent;

        SpriteFont m_pFont;

        int m_nSelected = 0;
        OptionsItem[] m_rgItems = new OptionsItem[7];

        GamePadState m_pPrevState;
        KeyboardState m_pPrevKBState;
        

        bool m_bFirstRun = true;

        public OptionsScreen(DangerZone pParent)
            : base(pParent)
        {
            m_pParent = pParent;
            
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {
            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\defaultFont");
            m_rgItems[0] = new PlayerName(this, PlayerIndex.One, "Player one", m_pParent.Settings.PlayerOneName, new Vector2(100, 100));
            m_rgItems[0].Selected = true;
            m_rgItems[1] = new PlayerName(this, PlayerIndex.One, "Player two", m_pParent.Settings.PlayerTwoName, new Vector2(100, 200));
            m_rgItems[2] = new BoolCheckbox(this, "Music on/off:", m_pParent.Settings.PlayMusic, new Vector2(100, 300));
            m_rgItems[3] = new BoolCheckbox(this, "SFX on/off:", m_pParent.Settings.PlaySFX, new Vector2(100, 400));
            m_rgItems[4] = new ActionItem(this, "Reset high scores & configuration", new Vector2(100, 425), OptionItemActions.RESETHIGHSCORE);
            m_rgItems[5] = new ActionItem(this, "Return without saving", new Vector2(100, 450), OptionItemActions.RETURN);
            m_rgItems[6] = new ActionItem(this, "Save & return", new Vector2(400, 450), OptionItemActions.SAVE);
            base.LoadContent(pContent);
        }

        public OptionsItem[] Items
        {
            get { return m_rgItems; }
            set { m_rgItems = value; }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime pGameTime)
        {


            KeyboardState pKBState = Keyboard.GetState();
            GamePadState pGPState = GamePad.GetState(m_pParent.Settings.PlayerOne);

            if (m_bFirstRun)
            {
                m_pPrevKBState = pKBState;
                m_pPrevState = pGPState;
                m_bFirstRun = false;
                return;
            }

            if (m_pPrevKBState == null)
            {
                m_pPrevKBState = pKBState;
                m_pPrevState = pGPState;
                return;
            }

            bool bInUse = false;
            for (int i = 0; i < 7; i++)
            {
                m_rgItems[i].Update(pGameTime);
                if (m_rgItems[i].InUse)
                    bInUse = true;
            }

            if (!bInUse)
            {
                if (m_pPrevKBState.IsKeyUp(Keys.Down) && pKBState.IsKeyDown(Keys.Down))
                {
                    NextItem();
                }
                if (m_pPrevKBState.IsKeyUp(Keys.Up) && pKBState.IsKeyDown(Keys.Up))
                {
                    PreviousItem();
                }

                if (m_pPrevKBState.IsKeyUp(Keys.Enter) && pKBState.IsKeyDown(Keys.Enter))
                {
                    m_rgItems[m_nSelected].Use();
                    
                }


                if ((m_pPrevState.ThumbSticks.Left.Y == 0 && pGPState.ThumbSticks.Left.Y > 0) ||
                    (m_pPrevState.DPad.Down == ButtonState.Released && pGPState.DPad.Down == ButtonState.Pressed))
                {
                    // neråt.
                    NextItem();
                }
                if ((m_pPrevState.ThumbSticks.Left.Y == 0 && pGPState.ThumbSticks.Left.Y < 0) ||
                    (m_pPrevState.DPad.Up == ButtonState.Released && pGPState.DPad.Up == ButtonState.Pressed))
                {
                    PreviousItem();
                }
                if ((m_pPrevState.Buttons.A == ButtonState.Released && pGPState.Buttons.A == ButtonState.Pressed) ||
                    (m_pPrevState.Buttons.Start == ButtonState.Released && pGPState.Buttons.Start == ButtonState.Pressed))
                {
                    m_rgItems[m_nSelected].Use();
                    
                }

               

            }
            
            m_pPrevKBState = pKBState;
            m_pPrevState = pGPState;           

            base.Update(pGameTime);
        }

        private void NextItem()
        {
            m_rgItems[m_nSelected].Selected = false;
            m_nSelected++;
            if (m_nSelected >= 7)
                m_nSelected = 0;
            m_rgItems[m_nSelected].Selected = true;
        }

        private void PreviousItem()
        {
            m_rgItems[m_nSelected].Selected = false;
            m_nSelected--;
            if (m_nSelected < 0)
                m_nSelected = 6;
            m_rgItems[m_nSelected].Selected = true;
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Begin();
            for (int i = 0; i < 7; i++)
                m_rgItems[i].Draw(pSpriteBatch);
            pSpriteBatch.End();
            base.Draw(pSpriteBatch);
        }

        public SpriteFont Font
        {
            get { return m_pFont; }
        }
    }


    class PlayerName : OptionsItem
    {
        PlayerIndex m_pIndex;
        string m_szTitle = "";
        string m_szInput = "";


        Vector2 m_pPosition = Vector2.Zero;
        OptionsScreen m_pParent;

        Color m_pColor = Color.White;


        public PlayerName(OptionsScreen pParent, PlayerIndex pIndex, string szTitle, string szInput, Vector2 pPosition) : base()
        {
            m_pParent = pParent;
            this.setParent(this);
            m_pIndex = pIndex;
            m_szTitle = szTitle;
            m_szInput = szInput;
            m_pPosition = pPosition;
            this.InUse = false;
        }



        public override void Use()
        {
            /*
            if (this.InUse)
                return;
            */
            
            Guide.BeginShowKeyboardInput(Microsoft.Xna.Framework.PlayerIndex.One, m_szTitle, "Type in your player name", m_szInput, GetTypedChars, null);
            this.InUse = true;
        }

        protected void GetTypedChars(IAsyncResult r)
        {
            m_szInput = Guide.EndShowKeyboardInput(r);
            this.InUse = false;
        }

        public override void Update(GameTime pGameTime)
        {
            if (this.Selected)
                m_pColor = Color.Red;
            else
                m_pColor = Color.White;
            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.DrawString(m_pParent.Font, m_szTitle + ": " + m_szInput, m_pPosition, m_pColor);
            base.Draw(pSpriteBatch);
        }

        public override string ValueString()
        {
            return m_szInput;
        }

    }

    enum OptionItemActions
    {
        RETURN,
        SAVE,
        RESETHIGHSCORE
    };

    class ActionItem : OptionsItem
    {
        string m_szTitle = "";
        Vector2 m_pPosition = Vector2.Zero;
        Color m_pColor = Color.White;
        OptionsScreen m_pParent;
        OptionItemActions m_pAction;
        public ActionItem(OptionsScreen pParent, string szTitle, Vector2 pPosition, OptionItemActions pAction)
            : base()
        {
            m_pParent = pParent;
            m_szTitle = szTitle;
            m_pPosition = pPosition;
            this.InUse = false;
            m_pAction = pAction;
            
        }

        public override void Use()
        {
            switch (m_pAction)
            {
                case OptionItemActions.RETURN:
                    m_pParent.Parent.LoadScreen(AvailableScreens.MAINMENU);
                    break;
                case OptionItemActions.SAVE:
                    SaveSettings();
                    break;
                case OptionItemActions.RESETHIGHSCORE:
                    File.Delete("highscores.lst");
                    File.Delete("gameConfiguration.cfg");
                    Console.WriteLine("Deleted high score list");
                    
                    break;
            }
            base.Use();
        }

        private void SaveSettings()
        {


            OptionsItem[] rgItems = new OptionsItem[6];
            rgItems = m_pParent.Items;

            string szPlayerOneName = rgItems[0].ValueString();
            string szPlayerTwoName = rgItems[1].ValueString();
            bool bPlayMusic = rgItems[2].ValueBool();
            bool bPlaySFX = rgItems[3].ValueBool();

            m_pParent.Parent.Settings.PlayerOneName = szPlayerOneName;
            m_pParent.Parent.Settings.PlayerTwoName = szPlayerTwoName;
            m_pParent.Parent.Settings.PlayMusic = bPlayMusic;
            m_pParent.Parent.Settings.PlaySFX = bPlaySFX;



            IAsyncResult pResult2 = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice pDevice = StorageDevice.EndShowSelector(pResult2);

            IAsyncResult pResult = pDevice.BeginOpenContainer("DangerZone", null, null);
            pResult.AsyncWaitHandle.WaitOne();
            StorageContainer pContainer = pDevice.EndOpenContainer(pResult);
            pResult.AsyncWaitHandle.Close();

            string szFilename = "dangerzonesettings.set";
            if (!pContainer.FileExists(szFilename))
            {
                pContainer.DeleteFile(szFilename);
                
            }

            Stream pStream = pContainer.CreateFile(szFilename);

            XmlSerializer pSerializer = new XmlSerializer(typeof(Settings));
            pSerializer.Serialize(pStream, m_pParent.Parent.Settings);

           
            
            
            pStream.Close();
            pContainer.Dispose();


            m_pParent.Parent.LoadScreen(AvailableScreens.MAINMENU);
        }

        public override void Update(GameTime pGameTime)
        {
            if (this.Selected)
                m_pColor = Color.Red;
            else
                m_pColor = Color.White;

            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.DrawString(m_pParent.Font, m_szTitle, m_pPosition, m_pColor);
            base.Draw(pSpriteBatch);
        }
    }

    class BoolCheckbox : OptionsItem
    {
        
        string m_szTitle = "";
        bool m_bValue = false;

        Vector2 m_pPosition = Vector2.Zero;
        OptionsScreen m_pParent;
        Color m_pColor = Color.White;
        public BoolCheckbox(OptionsScreen pParent, string szTitle, bool bValue, Vector2 pPosition) : base()
        {
            m_pParent = pParent;
            m_szTitle = szTitle;
            m_bValue = bValue;
            this.setParent(this);
            m_pPosition = pPosition;
            this.InUse = false;
        }

        public override void Use()
        {
            
            m_bValue = (m_bValue) ? false : true;
        }

        public override void Update(GameTime pGameTime)
        {
            if (this.Selected)
                m_pColor = Color.Red;
            else
                m_pColor = Color.White;
            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            string szValue = "[   ]";
            if (m_bValue)
                szValue = "[ x ]";
            
            pSpriteBatch.DrawString(m_pParent.Font, m_szTitle + " " + szValue, m_pPosition, m_pColor);

            base.Draw(pSpriteBatch);
        }

        public override bool ValueBool()
        {
            return m_bValue;
        }

    }

    class OptionsItem
    {
        BoolCheckbox m_pCheckboxParent = null;
        PlayerName m_pPlayerNameParent = null;

        bool m_bSelected = false;
        bool m_bInUse = false;

        public OptionsItem()
        {

        }
        public void setParent(BoolCheckbox pCheckboxParent)
        {
            m_pCheckboxParent = pCheckboxParent;
        }
        public void setParent(PlayerName pPlayerNameParent)
        {
            m_pPlayerNameParent = pPlayerNameParent;
        }

        public BoolCheckbox CheckboxParent
        {
            get { return m_pCheckboxParent; }
        }
        public PlayerName PlayerNameParent
        {
            get { return m_pPlayerNameParent; }
        }

        public virtual void Update(GameTime pGameTime)
        {

        }

        public virtual void Draw(SpriteBatch pSpriteBatch)
        {
        }

        public virtual void Use()
        {

        }

        public bool Selected
        {
            get { return m_bSelected; }
            set { m_bSelected = value; }
        }
        public bool InUse
        {
            get { return m_bInUse; }
            set { m_bInUse = value; }
        }

        public virtual string ValueString()
        {
            return "";
        }

        public virtual bool ValueBool()
        {
            return false;
        }


    }


}
