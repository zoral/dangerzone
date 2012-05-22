using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace dangerZone.Gameplay.Editor
{

    public enum GameEditorMenuItems
    {
        PLAYERSPEED,
        PLAYERLIVES,
        
        WEAPON0ANIMATIONSPEED,
        WEAPON1ANIMATIONSPEED,
        WEAPON2ANIMATIONSPEED,
        WEAPON3ANIMATIONSPEED,
        WEAPON4ANIMATIONSPEED,

        WEAPON0RELOADTIME,
        WEAPON1RELOADTIME,
        WEAPON2RELOADTIME,
        WEAPON3RELOADTIME,
        WEAPON4RELOADTIME,

        WEAPON0BULLETDAMAGE,
        WEAPON1BULLETDAMAGE,
        WEAPON2BULLETDAMAGE,
        WEAPON3BULLETDAMAGE,
        WEAPON4BULLETDAMAGE,

        WEAPON0BULLETSPEED,
        WEAPON1BULLETSPEED,
        WEAPON2BULLETSPEED,
        WEAPON3BULLETSPEED,
        WEAPON4BULLETSPEED,

        ENEMY0ANIMATIONSPEED,
        ENEMY1ANIMATIONSPEED,

        ENEMY0SPEED,
        ENEMY1SPEED,

        ENEMY0DAMAGE,
        ENEMY1DAMAGE,

        ENEMY0HEALTH,
        ENEMY1HEALTH,

        ENEMY0SCORE,
        ENEMY1SCORE,

        SAVE

    };

    class GameEditorMenuItem
    {

        GameEditor m_pParent;

        Vector2 m_pPosition = Vector2.Zero;
        string m_szName = "";
        string m_szValue;
        float m_fValue;
        int m_nValue;
        GameEditorMenuItems m_pItemType;

        Color m_pColor = Color.White;

        bool m_bEditing = false;
        Rectangle m_pBoundingBox;
        int m_nPadding = 2;

        Dictionary<Keys, string> m_pKeys = new Dictionary<Keys,string>();

        bool m_bDisplayEdit = false;

        public void recalcPosition(int nPos)
        {
            m_pPosition = new Vector2(110, 120 +  ((nPos) * 30) );
            updateBoundingBox();
        }

        public GameEditorMenuItem(GameEditor pParent, GameEditorMenuItems pItemType, string szName)
        {
            Vector2 pPosition = new Vector2(130, 110 + ((int)pItemType) * 30);
            m_pKeys[Keys.D0] = "0";
            m_pKeys[Keys.D1] = "1";
            m_pKeys[Keys.D2] = "2";
            m_pKeys[Keys.D3] = "3";
            m_pKeys[Keys.D4] = "4";
            m_pKeys[Keys.D5] = "5";
            m_pKeys[Keys.D6] = "6";
            m_pKeys[Keys.D7] = "7";
            m_pKeys[Keys.D8] = "8";
            m_pKeys[Keys.D9] = "9";

            m_pKeys[Keys.NumPad0] = "0";
            m_pKeys[Keys.NumPad1] = "1";
            m_pKeys[Keys.NumPad2] = "2";
            m_pKeys[Keys.NumPad3] = "3";
            m_pKeys[Keys.NumPad4] = "4";
            m_pKeys[Keys.NumPad5] = "5";
            m_pKeys[Keys.NumPad6] = "6";
            m_pKeys[Keys.NumPad7] = "7";
            m_pKeys[Keys.NumPad8] = "8";
            m_pKeys[Keys.NumPad9] = "9";

            m_pKeys[Keys.OemPeriod] = ".";



            m_pParent = pParent;
            m_pItemType = pItemType;
            m_szName = szName;
            m_pPosition = pPosition;

            

            updateBoundingBox();
        }
        private void updateBoundingBox()
        {
            Vector2 pSize = m_pParent.Font.MeasureString(m_szName + " " + m_szValue);
            m_pBoundingBox = new Rectangle( ((int)m_pPosition.X - m_nPadding), ((int)m_pPosition.Y - m_nPadding), ((int)pSize.X + m_nPadding), ((int)pSize.Y + m_nPadding));
        }

        public Vector2 getSize()
        {
            return m_pParent.Font.MeasureString(m_szName + " " + m_szValue);
        }

        

        public void SetValue(ref float fValue)
        {
            m_fValue = fValue;
            m_szValue = m_fValue.ToString();
            updateBoundingBox();
        }

        public void Edit()
        {
            m_pColor = Color.Red;
            m_bEditing = true;
            switch (m_pItemType)
            {

                case GameEditorMenuItems.SAVE:
                    // Spara och ladda om gameplayscreen
                    m_pParent.Parent.GameConfiguration.Save();
                    m_pParent.Parent.Parent.LoadScreen(AvailableScreens.GAMEPLAY);
                    break;
            }
        }

        private void saveValue()
        {
            switch (m_pItemType)
            {
                case GameEditorMenuItems.PLAYERSPEED:
                    m_pParent.Parent.GameConfiguration.PlayerSpeed = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.PLAYERLIVES:
                    m_pParent.Parent.GameConfiguration.PlayerLives = System.Convert.ToSingle(m_szValue);
                    break;


                case GameEditorMenuItems.WEAPON0ANIMATIONSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponAnimationSpeeds[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON1ANIMATIONSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponAnimationSpeeds[1] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON2ANIMATIONSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponAnimationSpeeds[2] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON3ANIMATIONSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponAnimationSpeeds[3] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON4ANIMATIONSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponAnimationSpeeds[4] = System.Convert.ToSingle(m_szValue);
                    break;
                
                // Bullet damage
                case GameEditorMenuItems.WEAPON0BULLETDAMAGE:
                    m_pParent.Parent.GameConfiguration.WeaponBulletDamage[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON1BULLETDAMAGE:
                    m_pParent.Parent.GameConfiguration.WeaponBulletDamage[1] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON2BULLETDAMAGE:
                    m_pParent.Parent.GameConfiguration.WeaponBulletDamage[2] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON3BULLETDAMAGE:
                    m_pParent.Parent.GameConfiguration.WeaponBulletDamage[3] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON4BULLETDAMAGE:
                    m_pParent.Parent.GameConfiguration.WeaponBulletDamage[4] = System.Convert.ToSingle(m_szValue);
                    break;

                // bullet speed
                case GameEditorMenuItems.WEAPON0BULLETSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponBulletSpeed[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON1BULLETSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponBulletSpeed[1] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON2BULLETSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponBulletSpeed[2] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON3BULLETSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponBulletSpeed[3] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON4BULLETSPEED:
                    m_pParent.Parent.GameConfiguration.WeaponBulletSpeed[4] = System.Convert.ToSingle(m_szValue);
                    break;

                // RELOAD TIME
                case GameEditorMenuItems.WEAPON0RELOADTIME:
                    m_pParent.Parent.GameConfiguration.WeaponReloadTime[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON1RELOADTIME:
                    m_pParent.Parent.GameConfiguration.WeaponReloadTime[1] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON2RELOADTIME:
                    m_pParent.Parent.GameConfiguration.WeaponReloadTime[2] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON3RELOADTIME:
                    m_pParent.Parent.GameConfiguration.WeaponReloadTime[3] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.WEAPON4RELOADTIME:
                    m_pParent.Parent.GameConfiguration.WeaponReloadTime[4] = System.Convert.ToSingle(m_szValue);
                    break;

                // Enemy animation speed
                case GameEditorMenuItems.ENEMY0ANIMATIONSPEED:
                    m_pParent.Parent.GameConfiguration.EnemyAnimationSpeed[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.ENEMY1ANIMATIONSPEED:
                    m_pParent.Parent.GameConfiguration.EnemyAnimationSpeed[1] = System.Convert.ToSingle(m_szValue);
                    break;

                case GameEditorMenuItems.ENEMY0DAMAGE:
                    m_pParent.Parent.GameConfiguration.EnemyDamage[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.ENEMY1DAMAGE:
                    m_pParent.Parent.GameConfiguration.EnemyDamage[1] = System.Convert.ToSingle(m_szValue);
                    break;

                case GameEditorMenuItems.ENEMY0HEALTH:
                    m_pParent.Parent.GameConfiguration.EnemyHealth[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.ENEMY1HEALTH:
                    m_pParent.Parent.GameConfiguration.EnemyHealth[1] = System.Convert.ToSingle(m_szValue);
                    break;

                case GameEditorMenuItems.ENEMY0SCORE:
                    m_pParent.Parent.GameConfiguration.EnemyScore[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.ENEMY1SCORE:
                    m_pParent.Parent.GameConfiguration.EnemyScore[1] = System.Convert.ToSingle(m_szValue);
                    break;

                case GameEditorMenuItems.ENEMY0SPEED:
                    m_pParent.Parent.GameConfiguration.EnemySpeed[0] = System.Convert.ToSingle(m_szValue);
                    break;
                case GameEditorMenuItems.ENEMY1SPEED:
                    m_pParent.Parent.GameConfiguration.EnemySpeed[1] = System.Convert.ToSingle(m_szValue);
                    break;
            }
        }

        public void StopEdit()
        {
            this.saveValue();
            m_pColor = Color.White;
            m_bEditing = false;
            m_bDisplayEdit = false;
        }


        public void Update(GameTime pGameTime)
        {
            if (m_bEditing)
            {
                m_bDisplayEdit = (m_bDisplayEdit) ? false : true;
                if (m_pParent.PrevKBState.IsKeyUp(Keys.Back) && m_pParent.KBState.IsKeyDown(Keys.Back))
                {
                    if (m_szValue.Length > 0)
                        m_szValue = m_szValue.Remove(m_szValue.Length - 1, 1);
                }
                else
                {
                    foreach (Keys pKey in m_pParent.KBState.GetPressedKeys())
                    {
                        if (!m_pParent.PrevKBState.IsKeyUp(pKey))
                            continue;

                        if (pKey == Keys.Enter)
                        {
                            this.StopEdit();
                            break;
                        }
                        Keys pKey_;

                        if (pKey == Keys.OemComma)
                            pKey_ = Keys.OemPeriod;
                        else
                            pKey_ = pKey;

                        if (m_pKeys.ContainsKey(pKey_))
                        {
                            // Ny knapptryckning
                            m_szValue += m_pKeys[pKey_].ToString();
                        }
                        
                    }
                }
                updateBoundingBox();
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            if(m_bDisplayEdit)
                pSpriteBatch.DrawString(m_pParent.Font, m_szName + " " + m_szValue+"_", m_pPosition, m_pColor);
            else
                pSpriteBatch.DrawString(m_pParent.Font, m_szName + " " + m_szValue, m_pPosition, m_pColor);
        }

        public Rectangle BoundingBox
        {
            get { return m_pBoundingBox; }
        }

        public bool IsEditing()
        {
            return m_bEditing;
        }

        public void MouseOver()
        {
            m_pColor = Color.Pink;
        }
        public void MouseOut()
        {
            m_pColor = Color.White;
        }

    }
}
