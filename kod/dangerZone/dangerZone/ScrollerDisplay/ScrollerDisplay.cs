using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using dangerZone.Gameplay;

namespace dangerZone.ScrollerDisplay
{

   

    class Letter
    {
        Vector2 m_pPosition = Vector2.Zero;
        Vector2 m_pVelocity = Vector2.Zero;
        string m_szLetter;
        int m_nYDirection = -1;
        bool m_bInUse = false;

        // Testar att committa med github ...

        public void Init(string szLetter, Vector2 pPosition, Vector2 pVelocity)
        {
            m_szLetter = szLetter;
            m_pPosition = pPosition;
            m_pVelocity = pVelocity;
            m_bInUse = true;
        }

        public void Update(GameTime pGameTime)
        {
            /*
             * Vi ska ändra på x och y-positionerna.
             * Dels för att flytta över skärmen
             * och dels för att göra det till en "våg".
             */
            m_pPosition.X += m_pVelocity.X * (float)pGameTime.ElapsedGameTime.TotalSeconds ;

            if (m_nYDirection == -1)
            {
                // Vi vill gå uppåt.
                if (m_pPosition.Y < Constants.YTOP)
                {
                    m_nYDirection = 1;
                }

                m_pPosition.Y -= Constants.YSPEED * (float)pGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (m_pPosition.Y > Constants.YBOTTOM)
                    m_nYDirection = -1;

                m_pPosition.Y += Constants.YSPEED * (float)pGameTime.ElapsedGameTime.TotalSeconds;
            }

            //Console.WriteLine("For letter: " + Char + ". Position: " + m_pPosition.ToString());

        }

        public Vector2 Position
        {
            get { return m_pPosition; }
        }
        public string Char
        {
            get { return m_szLetter; }
        }

        public bool InUse
        {
            get { return m_bInUse; }
            set { m_bInUse = value; }
        }

    }

    class Message
    {
        float m_fDisplayTime = 0;
        float m_fAliveTime = 0;
        string m_szMessage = "";

        Letter[] m_rgLetters = new Letter[Constants.MAXLETTERS];

        bool m_bDead = true;
        ScrollerDisplay m_pParent;
        public Message(ScrollerDisplay pParent)
        {
            m_pParent = pParent;
            for (int i = 0; i < Constants.MAXLETTERS; i++)
            {
                m_rgLetters[i] = new Letter();
            }
        }

        public void Init(string szMessage, float fDisplayTime, int nDirection)
        {
            m_szMessage = szMessage;
            m_fDisplayTime = fDisplayTime;

            m_bDead = false;
            m_fAliveTime = 0;

            Vector2 pMessageSize = m_pParent.Font.MeasureString(szMessage);

            Vector2 pPosition = Vector2.Zero;
            Vector2 pVelocity = Vector2.Zero;
            switch (nDirection)
            {
                case -1:
                    // Kommer från höger och går mot vänster
                    pPosition = new Vector2(1280, 360 - (pMessageSize.Y/2));
                    pVelocity = new Vector2(-Constants.XSPEED, 0);
                    break;
                case 0:
                    // Ska bara stå i mitten! Ej röra på sig.
                    pPosition = new Vector2(640- (pMessageSize.X/2), 360- (pMessageSize.Y/2));
                    break;

                case 1:
                    // Kommer från vänster och gå mot höger

                    pPosition = new Vector2(0 - pMessageSize.X, 360 - (pMessageSize.Y / 2));
                    pVelocity = new Vector2(Constants.XSPEED, 0);
                    break;
            }

            // Låt oss säga att det finns 4 st startpositioner på bokstäverna. Det gör det lättare.
            float fYMovement = 0;
            for (int i = 0; i < szMessage.Length; i++)
            {
                if (szMessage[i].ToString() != "")
                {
                    pPosition.Y += fYMovement;
                    fYMovement += 2;
                }


                if (pPosition.Y < Constants.YTOP)
                    fYMovement *= -1;
                if (pPosition.Y > Constants.YBOTTOM)
                    fYMovement *= -1;

                pPosition.X += (pMessageSize.X / m_szMessage.Length)+2;
                //Console.WriteLine("POSITION " + i + ": " + pPosition.ToString());

                m_rgLetters[i].Init(szMessage[i].ToString(), pPosition, pVelocity);

                
            }
            

        }



        public void Update(GameTime pGameTime)
        {
            if (m_bDead)
                return;

            if (m_fDisplayTime != 0)
            {
                m_fAliveTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
                if (m_fAliveTime > m_fDisplayTime)
                {
                    //Console.WriteLine("Message died");
                    m_bDead = true;
                    for (int i = 0; i < Constants.MAXLETTERS; i++)
                    {
                        m_rgLetters[i].InUse = false;

                    }
                }
            }

            if (!m_bDead)
            {
                //Console.WriteLine("Messge (" + m_szMessage + ") is not dead");
                for (int i = 0; i < Constants.MAXLETTERS; i++)
                {
                    if(m_rgLetters[i].InUse)
                        m_rgLetters[i].Update(pGameTime);
                }
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            for (int i = 0; i < Constants.MAXLETTERS; i++)
            {
                if (m_rgLetters[i].InUse)
                    pSpriteBatch.DrawString(m_pParent.Font, m_rgLetters[i].Char, m_rgLetters[i].Position, Color.Red);
            }
        }

        public bool Dead
        {
            get { return m_bDead; }
            set { m_bDead = value; }
        }
    }

    class ScrollerDisplay
    {

        SpriteFont m_pFont;
        Message[] m_rgMessages = new Message[Constants.MAXMESSAGES];

        public ScrollerDisplay()
        {

        }

        public void LoadContent(ContentManager pContent)
        {
            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\ScrollerFont");
            for (int i = 0; i < Constants.MAXMESSAGES; i++)
            {
                m_rgMessages[i] = new Message(this);   
            }
        }

        public bool Display(string szMessage, float fDisplayTime, int nDirection)
        {
            for (int i = 0; i < Constants.MAXMESSAGES; i++)
            {
                if (m_rgMessages[i].Dead)
                {
                    m_rgMessages[i].Init(szMessage, fDisplayTime, nDirection);
                    return true;
                    
                }
            }

            return false;
        }

        public void Update(GameTime pGameTime)
        {
            for (int i = 0; i < Constants.MAXMESSAGES; i++)
            {
                if (!m_rgMessages[i].Dead)
                {
                    m_rgMessages[i].Update(pGameTime);
                }
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            for (int i = 0; i < Constants.MAXMESSAGES; i++)
            {
                if (!m_rgMessages[i].Dead)
                {
                    m_rgMessages[i].Draw(pSpriteBatch);
                }
            }
        }

        public SpriteFont Font
        {
            get { return m_pFont; }
        }

    }
}
