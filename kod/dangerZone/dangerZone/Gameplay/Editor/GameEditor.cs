using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace dangerZone.Gameplay.Editor
{
    class GameEditor
    {

        GameplayScreen m_pParent;

        SpriteFont m_pFont;
        bool m_bVisible = false;

        KeyboardState m_pPrevKBState;
        KeyboardState m_pKBState;
        MouseState m_pPrevMState;
        MouseState m_pMouseState;
        Rectangle m_pMouseBoundingBox;

        static int m_nMaxTabs = 8;
        GameEditorMenuTab[] m_rgItemTabs = new GameEditorMenuTab[m_nMaxTabs];

        Texture2D m_pTexture;

        int m_nSelectedItem = 0;

        public GameEditor(GameplayScreen pParent)
        {
            m_pParent = pParent;
        }

        public void LoadContent(ContentManager pContent)
        {
            m_pMouseBoundingBox = new Rectangle(0, 0, 10, 10);
            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\debugFont");

            addPlayerData();

            addWeaponData();

            addEnemyData();
    
            generateBackground();

        }

        private void addPlayerData()
        {
            GameEditorMenuTab pPlayerTab = new GameEditorMenuTab(this, "Player", 0);
            pPlayerTab.addItems(3);

            GameEditorMenuItem pPlayerSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.PLAYERSPEED, "Player speed:");
            pPlayerSpeedItem.SetValue(ref m_pParent.GameConfiguration.PlayerSpeed);
            pPlayerTab.addItem(0, pPlayerSpeedItem);

            GameEditorMenuItem pPlayerLivesItem = new GameEditorMenuItem(this, GameEditorMenuItems.PLAYERLIVES, "Player lives:");
            pPlayerLivesItem.SetValue(ref m_pParent.GameConfiguration.PlayerLives);
            pPlayerTab.addItem(1, pPlayerLivesItem);

            pPlayerTab.addItem(2, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pPlayerTab.finishAddItems();
            m_rgItemTabs[0] = pPlayerTab;
        }

        private void addWeaponData()
        {

            GameEditorMenuTab pWeaponTab = new GameEditorMenuTab(this, "W:Standard gun", 1);
            pWeaponTab.addItems(5);
            GameEditorMenuItem pWeapon0AnimationSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON0ANIMATIONSPEED, "Animation Speed (ms/frame):");
            pWeapon0AnimationSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponAnimationSpeeds[0]);
            pWeaponTab.addItem(0, pWeapon0AnimationSpeedItem);

            GameEditorMenuItem pWeapon0BulletDamageItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON0BULLETDAMAGE, "Bullet damage(per hit):");
            pWeapon0BulletDamageItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletDamage[0]);
            pWeaponTab.addItem(1, pWeapon0BulletDamageItem);

            GameEditorMenuItem pWeapon0BulletSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON0BULLETSPEED, "Bullet speed (units/s):");
            pWeapon0BulletSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletSpeed[0]);
            pWeaponTab.addItem(2, pWeapon0BulletSpeedItem);

            GameEditorMenuItem pWeapon0ReloadTimeItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON0RELOADTIME, "Reload time(ms/frame):");
            pWeapon0ReloadTimeItem.SetValue(ref m_pParent.GameConfiguration.WeaponReloadTime[0]);
            pWeaponTab.addItem(3, pWeapon0ReloadTimeItem);

            pWeaponTab.addItem(4, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pWeaponTab.finishAddItems();

            m_rgItemTabs[1] = pWeaponTab;

            // MINIGUN

            GameEditorMenuTab pWeaponTab2 = new GameEditorMenuTab(this, "W:Minigun", 2);
            pWeaponTab2.addItems(5);
            GameEditorMenuItem pWeapon1AnimationSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON1ANIMATIONSPEED, "Animation Speed (ms/frame):");
            pWeapon1AnimationSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponAnimationSpeeds[1]);
            pWeaponTab2.addItem(0, pWeapon1AnimationSpeedItem);

            GameEditorMenuItem pWeapon1BulletDamageItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON1BULLETDAMAGE, "Bullet damage(per hit):");
            pWeapon1BulletDamageItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletDamage[1]);
            pWeaponTab2.addItem(1, pWeapon1BulletDamageItem);

            GameEditorMenuItem pWeapon1BulletSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON1BULLETSPEED, "Bullet speed (units/s):");
            pWeapon1BulletSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletSpeed[1]);
            pWeaponTab2.addItem(2, pWeapon1BulletSpeedItem);

            GameEditorMenuItem pWeapon1ReloadTimeItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON1RELOADTIME, "Reload time(ms/frame):");
            pWeapon1ReloadTimeItem.SetValue(ref m_pParent.GameConfiguration.WeaponReloadTime[1]);
            pWeaponTab2.addItem(3, pWeapon1ReloadTimeItem);

            pWeaponTab2.addItem(4, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pWeaponTab2.finishAddItems();

            m_rgItemTabs[2] = pWeaponTab2;

            // Rail gun

            GameEditorMenuTab pWeaponTab3 = new GameEditorMenuTab(this, "W:Railgun", 3);
            pWeaponTab3.addItems(5);
            GameEditorMenuItem pWeapon2AnimationSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON2ANIMATIONSPEED, "Animation Speed (ms/frame):");
            pWeapon2AnimationSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponAnimationSpeeds[2]);
            pWeaponTab3.addItem(0, pWeapon2AnimationSpeedItem);

            GameEditorMenuItem pWeapon2BulletDamageItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON2BULLETDAMAGE, "Bullet damage(per hit):");
            pWeapon2BulletDamageItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletDamage[2]);
            pWeaponTab3.addItem(1, pWeapon2BulletDamageItem);

            GameEditorMenuItem pWeapon2BulletSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON2BULLETSPEED, "Bullet speed (units/s):");
            pWeapon2BulletSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletSpeed[2]);
            pWeaponTab3.addItem(2, pWeapon2BulletSpeedItem);

            GameEditorMenuItem pWeapon2ReloadTimeItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON2RELOADTIME, "Reload time(ms/frame):");
            pWeapon2ReloadTimeItem.SetValue(ref m_pParent.GameConfiguration.WeaponReloadTime[2]);
            pWeaponTab3.addItem(3, pWeapon2ReloadTimeItem);

            pWeaponTab3.addItem(4, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pWeaponTab3.finishAddItems();

            m_rgItemTabs[3] = pWeaponTab3;

            // SHOTGUN

            GameEditorMenuTab pWeaponTab4 = new GameEditorMenuTab(this, "W:Shotgun", 4);
            pWeaponTab4.addItems(5);
            GameEditorMenuItem pWeapon3AnimationSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON3ANIMATIONSPEED, "Animation Speed (ms/frame):");
            pWeapon3AnimationSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponAnimationSpeeds[3]);
            pWeaponTab4.addItem(0, pWeapon3AnimationSpeedItem);

            GameEditorMenuItem pWeapon3BulletDamageItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON3BULLETDAMAGE, "Bullet damage(per hit):");
            pWeapon3BulletDamageItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletDamage[3]);
            pWeaponTab4.addItem(1, pWeapon3BulletDamageItem);

            GameEditorMenuItem pWeapon3BulletSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON3BULLETSPEED, "Bullet speed (units/s):");
            pWeapon3BulletSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletSpeed[3]);
            pWeaponTab4.addItem(2, pWeapon3BulletSpeedItem);

            GameEditorMenuItem pWeapon3ReloadTimeItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON3RELOADTIME, "Reload time(ms/frame):");
            pWeapon3ReloadTimeItem.SetValue(ref m_pParent.GameConfiguration.WeaponReloadTime[3]);
            pWeaponTab4.addItem(3, pWeapon3ReloadTimeItem);

            pWeaponTab4.addItem(4, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pWeaponTab4.finishAddItems();

            m_rgItemTabs[4] = pWeaponTab4;

        
            // ROCKET LAUNCHER

            GameEditorMenuTab pWeaponTab5 = new GameEditorMenuTab(this, "W:ROCKER LAUNCHER", 5);
            pWeaponTab5.addItems(5);
            GameEditorMenuItem pWeapon4AnimationSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON4ANIMATIONSPEED, "Animation Speed (ms/frame):");
            pWeapon4AnimationSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponAnimationSpeeds[4]);
            pWeaponTab5.addItem(0, pWeapon4AnimationSpeedItem);

            GameEditorMenuItem pWeapon4BulletDamageItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON4BULLETDAMAGE, "Bullet damage(per hit):");
            pWeapon4BulletDamageItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletDamage[4]);
            pWeaponTab5.addItem(1, pWeapon4BulletDamageItem);

            GameEditorMenuItem pWeapon4BulletSpeedItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON4BULLETSPEED, "Bullet speed (units/s):");
            pWeapon4BulletSpeedItem.SetValue(ref m_pParent.GameConfiguration.WeaponBulletSpeed[4]);
            pWeaponTab5.addItem(2, pWeapon4BulletSpeedItem);

            GameEditorMenuItem pWeapon4ReloadTimeItem = new GameEditorMenuItem(this, GameEditorMenuItems.WEAPON4RELOADTIME, "Reload time(ms/frame):");
            pWeapon4ReloadTimeItem.SetValue(ref m_pParent.GameConfiguration.WeaponReloadTime[4]);
            pWeaponTab5.addItem(3, pWeapon4ReloadTimeItem);

            pWeaponTab5.addItem(4, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pWeaponTab5.finishAddItems();

            m_rgItemTabs[5] = pWeaponTab5;



        }

        private void addEnemyData()
        {
            GameEditorMenuTab pEnemy0Tab = new GameEditorMenuTab(this, "E:Blood maggot", 6);
            pEnemy0Tab.addItems(6);

            GameEditorMenuItem pAnimationSpeedItem0 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY0ANIMATIONSPEED, "Animation speed:");
            pAnimationSpeedItem0.SetValue(ref m_pParent.GameConfiguration.EnemyAnimationSpeed[0]);
            pEnemy0Tab.addItem(0, pAnimationSpeedItem0);

            GameEditorMenuItem pSpeedItem0 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY0SPEED, "Speed:");
            pSpeedItem0.SetValue(ref m_pParent.GameConfiguration.EnemySpeed[0]);
            pEnemy0Tab.addItem(1, pSpeedItem0);

            GameEditorMenuItem pScoreItem0 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY0SCORE, "Score:");
            pScoreItem0.SetValue(ref m_pParent.GameConfiguration.EnemyScore[0]);
            pEnemy0Tab.addItem(2, pScoreItem0);

            GameEditorMenuItem pDamageItem0 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY0DAMAGE, "Damage:");
            pDamageItem0.SetValue(ref m_pParent.GameConfiguration.EnemyDamage[0]);
            pEnemy0Tab.addItem(3, pDamageItem0);

            GameEditorMenuItem pHealthItem0 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY0HEALTH, "Health:");
            pHealthItem0.SetValue(ref m_pParent.GameConfiguration.EnemyHealth[0]);
            pEnemy0Tab.addItem(4, pHealthItem0);

            pEnemy0Tab.addItem(5, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pEnemy0Tab.finishAddItems();
            m_rgItemTabs[6] = pEnemy0Tab;


            GameEditorMenuTab pEnemy1Tab = new GameEditorMenuTab(this, "E:Temp enemy", 7);
            pEnemy1Tab.addItems(6);

            GameEditorMenuItem pAnimationSpeedItem1 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY1ANIMATIONSPEED, "Animation speed:");
            pAnimationSpeedItem1.SetValue(ref m_pParent.GameConfiguration.EnemyAnimationSpeed[1]);
            pEnemy1Tab.addItem(0, pAnimationSpeedItem1);

            GameEditorMenuItem pSpeedItem1 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY1SPEED, "Speed:");
            pSpeedItem1.SetValue(ref m_pParent.GameConfiguration.EnemySpeed[1]);
            pEnemy1Tab.addItem(1, pSpeedItem1);

            GameEditorMenuItem pScoreItem1 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY1SCORE, "Score:");
            pScoreItem1.SetValue(ref m_pParent.GameConfiguration.EnemyScore[1]);
            pEnemy1Tab.addItem(2, pScoreItem1);

            GameEditorMenuItem pDamageItem1 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY1DAMAGE, "Damage:");
            pDamageItem1.SetValue(ref m_pParent.GameConfiguration.EnemyDamage[1]);
            pEnemy1Tab.addItem(3, pDamageItem1);

            GameEditorMenuItem pHealthItem1 = new GameEditorMenuItem(this, GameEditorMenuItems.ENEMY1HEALTH, "Health:");
            pHealthItem1.SetValue(ref m_pParent.GameConfiguration.EnemyHealth[1]);
            pEnemy1Tab.addItem(4, pHealthItem1);

            pEnemy1Tab.addItem(5, new GameEditorMenuItem(this, GameEditorMenuItems.SAVE, "Save settings"));
            pEnemy1Tab.finishAddItems();
            m_rgItemTabs[7] = pEnemy1Tab;


        }

        private void generateBackground()
        {
            int nYSize = 50;
            
            int nXSize = 0;

            for (int i = 0; i < m_nMaxTabs; i++)
            {
                nXSize += (i * 30) + (int)m_rgItemTabs[i].getSize().X;
            }

            Color[] rgColors = new Color[nXSize * nYSize];
            for (int x = 0; x < nXSize; x++)
            {
                for (int y = 0; y < nYSize; y++)
                {
                    rgColors[x + y * nXSize] = Color.Black;
                }
            }
            m_pTexture = new Texture2D(m_pParent.Parent.GraphicsDevice, nXSize, nYSize, false, SurfaceFormat.Color);
            m_pTexture.SetData(rgColors);

        }

        private void updateMouseBoundingBox(MouseState pMouseState)
        {
            m_pMouseBoundingBox = new Rectangle(pMouseState.X, pMouseState.Y, 5, 5);

        }

        public void Update(GameTime pGameTime)
        {
            if (!m_bVisible)
                return;

            m_pKBState = Keyboard.GetState();
            m_pMouseState = Mouse.GetState();
            updateMouseBoundingBox(m_pMouseState);

            bool bOverruleIntersect = false;
            bool bIntersects = false;
            for (int i = 0; i < m_nMaxTabs; i++)
            {
                m_rgItemTabs[i].Update(pGameTime);

                if (m_pMouseBoundingBox.Intersects(m_rgItemTabs[i].BoundingBox))
                {
                    m_nSelectedItem = i;
                    bIntersects = true;
                    m_rgItemTabs[i].MouseOver();
                }
                else
                    m_rgItemTabs[i].MouseOut();

                

            }

            if (bIntersects && !bOverruleIntersect)
            {
                if (m_pPrevMState.LeftButton == ButtonState.Released && m_pMouseState.LeftButton == ButtonState.Pressed)
                {
                    for (int i = 0; i < m_nMaxTabs; i++)
                        m_rgItemTabs[i].Hide();
                    m_rgItemTabs[m_nSelectedItem].Display();
                }
            }
            


            m_pPrevKBState = m_pKBState;
            m_pPrevMState = m_pMouseState;
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            if (!m_bVisible)
                return;

            pSpriteBatch.Begin();
            pSpriteBatch.Draw(m_pTexture, new Vector2(100, 50), Color.White);
            for (int i = 0; i < m_nMaxTabs; i++)
            {
                m_rgItemTabs[i].Draw(pSpriteBatch);
            }

            pSpriteBatch.End();
        }

        public void Display()
        {

            m_bVisible = true;
            m_pParent.Parent.IsMouseVisible = true;
        }

        public void Hide()
        {
            m_bVisible = false;
            m_pParent.Parent.IsMouseVisible = false;
        }

        public GameplayScreen Parent
        {
            get { return m_pParent; }
            set { m_pParent = value; }
        }

        public SpriteFont Font
        {
            get { return m_pFont; }
            set { m_pFont = value; }
        }

        public KeyboardState PrevKBState
        {
            get { return m_pPrevKBState; }
        }
        public KeyboardState KBState
        {
            get { return m_pKBState; }
        }

        public bool Visible()
        {
            return m_bVisible;
        }

        public Rectangle MouseBoundingBox
        {
            get { return m_pMouseBoundingBox; }
            set { m_pMouseBoundingBox = value; }
        }

        public MouseState PrevMouseState
        {
            get { return m_pPrevMState; }
            set { m_pPrevMState = value; }
        }

        public MouseState MouseState
        {
            get { return m_pMouseState; }
            set { m_pMouseState = value; }
        }

        public GameEditorMenuTab[] Tabs
        {
            get { return m_rgItemTabs; }
            set { m_rgItemTabs = value; }
        }

    }
}
