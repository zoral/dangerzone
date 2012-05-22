using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using dangerZone.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using dangerZone.Gameplay.GameObjects.Weapons;
using Microsoft.Xna.Framework.Audio;

namespace dangerZone.Gameplay.GameObjects
{

    public enum PlayerDirection
    {
        SOUTH,
        SOUTHEAST,
        SOUTHWEST,
        EAST,
        WEST,
        NORTH,
        NORTHEAST,
        NORTHWEST
    };

    enum TextPositions
    {
        PLAYERNAME,
        PLAYERHEALTH,
        PLAYERSCORE,
        PLAYERWEAPONNAME,
        PLAYERWEAPONICON
    };

    class Player : GameObject
    {

        PlayerIndex m_pPlayerIndex; // Vilken kontroll använder man för denna spelaren.
        int m_nPlayer = 0;

        GamePadState m_pPrevGamePadState;
        Vector2 m_pVelocity = Vector2.Zero;
        Vector2 m_pRightStick = Vector2.Zero;

        SpriteFont m_pFont;

        float m_fSpeed = 10f;

        float m_fHealth = 10;
        float m_fMaxHealth = 10;
        float m_fHitTimer = 0;
        float m_fMaxHitTimer = 1500;

        float m_fPlayTime = 0;

        Weapon[] m_rgWeapons = new Weapon[5];
        int m_nCurrentWeapon = 2;
        int m_nMaxWeapons = 5;

        int m_nScore = 0;

        Vector2[] m_rgPositions = new Vector2[5];
        BoundingBox2D m_pMovementBoundingBox;
        
        

        public Player(GameplayScreen pParent, int nPlayer)
            : base(pParent)
        {
            m_nPlayer = nPlayer;
            
            if (nPlayer == 1)
            {
                m_rgPositions[(int)TextPositions.PLAYERNAME] = new Vector2(10, 10);
                m_rgPositions[(int)TextPositions.PLAYERHEALTH] = new Vector2(10, 30);
                m_rgPositions[(int)TextPositions.PLAYERSCORE] = new Vector2(10, 50);
                m_rgPositions[(int)TextPositions.PLAYERWEAPONNAME] = new Vector2(10, 680);
                m_rgPositions[(int)TextPositions.PLAYERWEAPONICON] = new Vector2(10, 700);
            }
            else
            {
                m_rgPositions[(int)TextPositions.PLAYERNAME] = new Vector2(1000, 10);
                m_rgPositions[(int)TextPositions.PLAYERHEALTH] = new Vector2(1000, 30);
                m_rgPositions[(int)TextPositions.PLAYERSCORE] = new Vector2(1000, 50);
                m_rgPositions[(int)TextPositions.PLAYERWEAPONNAME] = new Vector2(1000, 680);
                m_rgPositions[(int)TextPositions.PLAYERWEAPONICON] = new Vector2(1000, 700);
            }

            m_fMaxHealth = pParent.GameConfiguration.PlayerLives;
            m_fHealth = m_fMaxHealth;

            

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {
            if (m_nPlayer == 0)
                return;

            Texture2D pTexture = null;
            try
            {
                pTexture = pContent.Load<Texture2D>("GFX\\Players\\player" + m_nPlayer + "");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.ToString());
            }
            this.Animation = new SimpleAnimation(pTexture, Vector2.Zero, 1, 4, 200);

            this.BoundingBox = new BoundingBox2D(this.Animation.Position.X, this.Animation.Position.Y, 2, 2);
            m_pMovementBoundingBox = new BoundingBox2D(this.Animation.Position.X, this.Animation.Position.Y, 2, 2);

            m_pFont = pContent.Load<SpriteFont>("GFX\\Fonts\\defaultFont");

            m_rgWeapons[0] = new Weapon();
            m_rgWeapons[0].Color = Color.Blue;
            m_rgWeapons[0].Damage = this.Parent.GameConfiguration.WeaponBulletDamage[0];//1.5f;
            m_rgWeapons[0].Name = "Assault carbine";
            m_rgWeapons[0].ReloadTime = this.Parent.GameConfiguration.WeaponReloadTime[0];
            m_rgWeapons[0].Speed = this.Parent.GameConfiguration.WeaponBulletSpeed[0];
            m_rgWeapons[0].Animation = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\Weapons\\standardgun"), this.Animation.Position, 1, 4, this.Parent.GameConfiguration.WeaponAnimationSpeeds[0]);
            m_rgWeapons[0].Animation.DepthLayer = 0.239f;
            m_rgWeapons[0].Animation.Loop = false;
            m_rgWeapons[0].Animation.Play = false;
            m_rgWeapons[0].Icon = pContent.Load<Texture2D>("GFX\\Weapons\\weapon1_icon");
            m_rgWeapons[0].Sound = pContent.Load<SoundEffect>("SFX\\weapon1");

            m_rgWeapons[1] = new Weapon();
            m_rgWeapons[1].Color = Color.Red;
            m_rgWeapons[1].Damage = this.Parent.GameConfiguration.WeaponBulletDamage[1]; 
            m_rgWeapons[1].Name = "Minigun";
            m_rgWeapons[1].ReloadTime = this.Parent.GameConfiguration.WeaponReloadTime[1];
            m_rgWeapons[1].Speed = this.Parent.GameConfiguration.WeaponBulletSpeed[1];
            m_rgWeapons[1].Animation = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\Weapons\\minigun"), this.Animation.Position, 1, 4, this.Parent.GameConfiguration.WeaponAnimationSpeeds[1]);
            m_rgWeapons[1].Animation.DepthLayer = 0.239f;
            m_rgWeapons[1].Animation.Loop = false;
            m_rgWeapons[1].Animation.Play = false;
            m_rgWeapons[1].Icon = pContent.Load<Texture2D>("GFX\\Weapons\\weapon1_icon");
            m_rgWeapons[1].Sound = pContent.Load<SoundEffect>("SFX\\weapon1");

            m_rgWeapons[2] = new Weapon();
            m_rgWeapons[2].Color = Color.Red;
            m_rgWeapons[2].Damage = this.Parent.GameConfiguration.WeaponBulletDamage[2]; 
            m_rgWeapons[2].Name = "Railgun";
            m_rgWeapons[2].ReloadTime = this.Parent.GameConfiguration.WeaponReloadTime[2];
            m_rgWeapons[2].Speed = this.Parent.GameConfiguration.WeaponBulletSpeed[2];
            m_rgWeapons[2].Animation = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\Weapons\\railgun"), this.Animation.Position, 1, 4, this.Parent.GameConfiguration.WeaponAnimationSpeeds[2]);
            m_rgWeapons[2].Animation.DepthLayer = 0.239f;
            m_rgWeapons[2].Animation.Loop = false;
            m_rgWeapons[2].Animation.Play = false;
            m_rgWeapons[2].Icon = pContent.Load<Texture2D>("GFX\\Weapons\\weapon1_icon");
            m_rgWeapons[2].Sound = pContent.Load<SoundEffect>("SFX\\weapon1");
            m_rgWeapons[2].GunType = GunTypes.RAILGUN;

            m_rgWeapons[3] = new Weapon();
            m_rgWeapons[3].Color = Color.Red;
            m_rgWeapons[3].Damage = this.Parent.GameConfiguration.WeaponBulletDamage[3];
            m_rgWeapons[3].Name = "Shotgun";
            m_rgWeapons[3].ReloadTime = this.Parent.GameConfiguration.WeaponReloadTime[3];
            m_rgWeapons[3].Speed = this.Parent.GameConfiguration.WeaponBulletSpeed[3];
            m_rgWeapons[3].Animation = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\Weapons\\shotgun"), this.Animation.Position, 1, 4, this.Parent.GameConfiguration.WeaponAnimationSpeeds[3]);
            m_rgWeapons[3].Animation.DepthLayer = 0.239f;
            m_rgWeapons[3].Animation.Loop = false;
            m_rgWeapons[3].Animation.Play = false;
            m_rgWeapons[3].Icon = pContent.Load<Texture2D>("GFX\\Weapons\\weapon1_icon");
            m_rgWeapons[3].Sound = pContent.Load<SoundEffect>("SFX\\weapon1");
            m_rgWeapons[3].GunType = GunTypes.SHOTGUN;

            m_rgWeapons[4] = new Weapon();
            m_rgWeapons[4].Color = Color.Red;
            m_rgWeapons[4].Damage = this.Parent.GameConfiguration.WeaponBulletDamage[4];
            m_rgWeapons[4].Name = "Rocket launcher";
            m_rgWeapons[4].ReloadTime = this.Parent.GameConfiguration.WeaponReloadTime[4];
            m_rgWeapons[4].Speed = this.Parent.GameConfiguration.WeaponBulletSpeed[4];
            m_rgWeapons[4].Animation = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\Weapons\\rocketlauncher_temp"), this.Animation.Position, 1, 4, this.Parent.GameConfiguration.WeaponAnimationSpeeds[4]);
            m_rgWeapons[4].Animation.DepthLayer = 0.239f;
            m_rgWeapons[4].Animation.Loop = false;
            m_rgWeapons[4].Animation.Play = false;
            m_rgWeapons[4].Icon = pContent.Load<Texture2D>("GFX\\Weapons\\weapon1_icon");
            m_rgWeapons[4].Sound = pContent.Load<SoundEffect>("SFX\\weapon1");
            m_rgWeapons[4].GunType = GunTypes.ROCKET;



            base.LoadContent(pContent);
        }

        public override void Update(GameTime pGameTime)
        {
            if (m_nPlayer == 0)
                return;
            
            if (m_fHealth > 0)
                m_fPlayTime += (float)pGameTime.ElapsedGameTime.TotalSeconds;


            if (m_fHitTimer < m_fMaxHitTimer)
            {
                m_fHitTimer += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                GamePad.SetVibration(this.PlayerIndex, 0.0f, 0.0f); // Nollställ vibration när vi kan träffas igen...
            }
            /*
             * Vi ska kontrollera hur man rör sig/vart man skjuter
             */
            GamePadState pState = GamePad.GetState(m_pPlayerIndex);
            KeyboardState pKBState = Keyboard.GetState();

            if (pKBState.IsKeyDown(Keys.Space))
            {
                Shoot(new Vector2(-1, 0));
            }

            

            Vector2 pLeftStick = pState.ThumbSticks.Left;
            Vector2 pRightStick = pState.ThumbSticks.Right;

            

            if (pState.Buttons.A == ButtonState.Pressed)
            {
                this.Parent.ParticleEngine.Emitter(this.Animation.Position, pLeftStick, Color.Blue, 10000, 1000);
            }

            // Vi måste ta reda på om rightstick är så "långt" ut den kan åt något håll. Isf ska vi skjuta ditåt.
            
            if(pRightStick != Vector2.Zero)
                Shoot(pRightStick);
 
            m_pRightStick = pRightStick;

            if (m_pRightStick != Vector2.Zero)
            {
                this.Animation.Rotation = (float)Math.Atan2(m_pRightStick.X, m_pRightStick.Y);
            }

            m_pVelocity = pLeftStick;
            m_pVelocity.X *= m_fSpeed * (float)pGameTime.ElapsedGameTime.TotalSeconds;
            m_pVelocity.Y *= (m_fSpeed * (float)pGameTime.ElapsedGameTime.TotalSeconds)*-1;


            Vector2 pTempPosition = this.Animation.Position + m_pVelocity;
            m_pMovementBoundingBox.Update(this.Animation.Position.X + m_pVelocity.X, this.Animation.Position.Y + m_pVelocity.Y);

            // Se till så att spelaren inte går för långt ifrån sin kompis. Om de finns någon.
            bool bMove = true;
            if (m_nPlayer == 1)
            {
                // Man är spelare 1 och ska kontrollera spelare 2.
                if (this.Parent.Parent.Settings.TwoPlayers)
                {
                    if (Vector2.Distance(this.Parent.Players[1].Animation.Position, pTempPosition) > 10)
                    {
                        bMove = false;
                        //Console.WriteLine("DONT MOVE PLAYER ONE!");

                    }
                }
            }

            if (m_nPlayer == 2)
            {
                // Man är spelare 1 och ska kontrollera spelare 2.
                if (this.Parent.Parent.Settings.TwoPlayers)
                {
                    if (Vector2.Distance(this.Parent.Players[0].Animation.Position, pTempPosition) > 10)
                    {
                        bMove = false;
                        //Console.WriteLine("DONT MOVE PLAYER TWO!");
                    }
                }
            }


            if (bMove)
            {
                if (this.Parent.Level.BoundingBox.Intersects(m_pMovementBoundingBox))
                {
                    this.Animation.Position += m_pVelocity;

                    //
                }
            }
            else
            {
                this.Animation.Position -= m_pVelocity;
               
            }
            updateWeaponAnimation();

            for (int i = 0; i < m_nMaxWeapons; i++)
            {
                m_rgWeapons[i].Update(pGameTime);
            }

            if (m_pVelocity != Vector2.Zero)
            {
                 this.Animation.Update(pGameTime);
                 
            }

            /*
             * Ändra vapen 
             */
            if (pState.Buttons.RightShoulder == ButtonState.Pressed && m_pPrevGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                m_nCurrentWeapon++;
                if (m_nCurrentWeapon >= m_nMaxWeapons)
                    m_nCurrentWeapon = 0;
            }

            m_rgWeapons[m_nCurrentWeapon].Animation.Update(pGameTime);
            this.BoundingBox.Update(this.Animation.Position.X, this.Animation.Position.Y);


            m_pPrevGamePadState = pState;
            base.Update(pGameTime);
        }

        public void updateWeaponAnimation()
        {
            
            for (int i = 0; i < m_nMaxWeapons; i++)
            {
                m_rgWeapons[i].Animation.Rotation = this.Animation.Rotation;
                m_rgWeapons[i].Animation.Position += m_pVelocity;
            }
        }

        public void Shoot(Vector2 pVelocity)
        {
            if (m_rgWeapons[m_nCurrentWeapon].CanFire())
                m_rgWeapons[m_nCurrentWeapon].Fire(); // Nollställer timern på vapnet.
            else
                return;


            /*
             * Spela upp vapenanimationen en gång.
             */
            m_rgWeapons[m_nCurrentWeapon].Animation.Play = true;
            

            
            // Vänd på y och normalisera vektorn
            pVelocity.Y *= -1;
            pVelocity.Normalize();

            Vector2 pDirection = pVelocity;

            // Lägg på hastigheten.
            pVelocity *= m_rgWeapons[m_nCurrentWeapon].Speed;
            
            if(this.Parent.Parent.Settings.PlaySFX)
                m_rgWeapons[m_nCurrentWeapon].Sound.Play();

            switch (m_rgWeapons[m_nCurrentWeapon].GunType)
            {
                case GunTypes.STANDARD:
                    
                    this.Parent.BulletManager.InitBullet(m_nPlayer, this.Animation.Position, pVelocity, m_rgWeapons[m_nCurrentWeapon].Damage, m_rgWeapons[m_nCurrentWeapon].Color, m_rgWeapons[m_nCurrentWeapon].TTL);
                    break;
                case GunTypes.RAILGUN:
                    this.Parent.BulletManager.InitRail(m_nPlayer, this.Animation.Position, pVelocity, m_rgWeapons[m_nCurrentWeapon].Damage, this.Animation.Rotation, m_rgWeapons[m_nCurrentWeapon].TTL);
                    break;
                case GunTypes.SHOTGUN:

                    
                    for (int i = 0; i < 10; i++)
                    {
                        Vector2 pTemp = pDirection;
                        
                        pTemp.X += (float)(this.Parent.Random.NextDouble() - 0.5);
                        pTemp.Y += (float)(this.Parent.Random.NextDouble() - 0.5);
                        
                        pTemp *= m_rgWeapons[m_nCurrentWeapon].Speed;

                        this.Parent.BulletManager.InitBullet(m_nPlayer, this.Animation.Position, pTemp, m_rgWeapons[m_nCurrentWeapon].Damage, m_rgWeapons[m_nCurrentWeapon].Color, m_rgWeapons[m_nCurrentWeapon].TTL);
                    
                    }

        
                    break;

                case GunTypes.ROCKET:
                    this.Parent.BulletManager.InitRocket(m_nPlayer, this.Animation.Position, pVelocity, m_rgWeapons[m_nCurrentWeapon].Damage, this.Animation.Rotation, m_rgWeapons[m_nCurrentWeapon].TTL);
                    break;

            }
        }
        

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            if (m_nPlayer == 0)
                return;

            m_rgWeapons[m_nCurrentWeapon].Animation.Draw(pSpriteBatch);
            this.Animation.Draw(pSpriteBatch);
            

            base.Draw(pSpriteBatch);
        }

        public override void DrawHUD(SpriteBatch pSpriteBatch)
        {
            if (m_nPlayer == 0)
                return;

         
                pSpriteBatch.DrawString(m_pFont, m_fHealth + "/" + m_fMaxHealth + "", m_rgPositions[(int)TextPositions.PLAYERHEALTH], Color.White);

                // Skriv ut ikonen för vapnet + namnet
                pSpriteBatch.DrawString(m_pFont, m_rgWeapons[m_nCurrentWeapon].Name, m_rgPositions[(int)TextPositions.PLAYERWEAPONNAME], Color.White);
                pSpriteBatch.Draw(m_rgWeapons[m_nCurrentWeapon].Icon, m_rgPositions[(int)TextPositions.PLAYERWEAPONICON], Color.White);

                if (m_nPlayer == 1)
                {
                    pSpriteBatch.DrawString(m_pFont, this.Parent.Parent.Settings.PlayerOneName, m_rgPositions[(int)TextPositions.PLAYERNAME], Color.White);
                    pSpriteBatch.DrawString(m_pFont, this.BoundingBox.ToString(), new Vector2(200, 10), Color.White);
                }
                else
                    pSpriteBatch.DrawString(m_pFont, this.Parent.Parent.Settings.PlayerTwoName, m_rgPositions[(int)TextPositions.PLAYERNAME], Color.White);
            
                pSpriteBatch.DrawString(m_pFont, this.Score.ToString(), m_rgPositions[(int)TextPositions.PLAYERSCORE], Color.White);
            
 

            base.DrawHUD(pSpriteBatch);
        }

        public void dealDamage(float fDamage)
        {
            if (m_nPlayer == 0)
                return;



            if (m_fHitTimer < m_fMaxHitTimer)
                return;

            m_fHitTimer = 0;
            
            GamePad.SetVibration(this.PlayerIndex, 1.0f, 1.0f);

            

            this.m_fHealth -= fDamage;
            
            if (this.m_fHealth <= 0)
            {
                // Då är man död. Kan inte längre röra sig.
                // Om båda spelarna är döda så är spelet slut. Gå då till gameoverbilden och spara deras poäng typ. :)

                if (this.m_nPlayer == 1) // Man är spelare 1
                {
                    this.Parent.Parent.Settings.PlayerOneScore = this.Score;
                    this.Parent.Parent.Settings.PlayerOneWave = this.Parent.EnemyManager.CurrentWave;

                    // Kolla om tvåan finns, och lever.
                    if (!this.Parent.Players[1].Dead())
                    {
                        // Då finns den ! OCH den lever.

                    }
                    else
                    {
                        // Player två finns inte/är död.
                        // Då ska vi ta och gå till gameoverskärmen och spara våra poäng osv..
                        this.Parent.ScrollerDisplay.Display("GAME OVER", 10000, 0);
                    }
                }
                else
                {

                    
                    this.Parent.Parent.Settings.PlayerTwoScore = this.Score;
                    this.Parent.Parent.Settings.PlayerTwoWave = this.Parent.EnemyManager.CurrentWave;
                    if (!this.Parent.Players[0].Dead())
                    {
                        // Player 1 lever fortfarande.
                    }
                    else
                    {
                        // Player 1 är också död. Bummer
                        this.Parent.ScrollerDisplay.Display("GAME OVER", 10000, 0);
                    }
                }


                this.m_nPlayer = 0; // HIHI. Ingen kan styra honom längre.

            }
        }

        public PlayerIndex PlayerIndex
        {
            get { return m_pPlayerIndex; }
            set { m_pPlayerIndex = value; }
        }

        public bool isPlaying()
        {
            if (m_nPlayer == 0)
                return false;

            return true;
        }

        public void AddScore(int nScore)
        {
            m_nScore += nScore;
        }

        public int Score
        {
            get { return m_nScore; }
            set { m_nScore = value; }
        }

        public bool Dead()
        {
            if (m_nPlayer == 0)
                return true;

            if (m_fHealth <= 0)
                return true;

            return false;
        }

        public int PlayTime
        {
            get { return (int)m_fPlayTime; }
            set { m_fPlayTime = (float)value; }

        }

        public float Speed
        {
            get { return m_fSpeed; }
            set { m_fSpeed = value; }
        }

    }


    /*
// Movement
if (pRightStick.Y > 0)
{
    // Norrut
    if (pRightStick.X < -0.2f)
        m_pDirection = PlayerDirection.NORTHWEST;
    if (pRightStick.X > 0.2f)
        m_pDirection = PlayerDirection.NORTHEAST;
    if (pRightStick.X == 0)
        m_pDirection = PlayerDirection.NORTH;
                
}
if (pRightStick.Y <= 0)
{
    // Söderut
    if (pRightStick.X < -0.2f)
        m_pDirection = PlayerDirection.SOUTHWEST;
    if (pRightStick.X > 0.2f)
        m_pDirection = PlayerDirection.SOUTHEAST;
    if (pRightStick.X == 0)
        m_pDirection = PlayerDirection.SOUTH;
}
if (pRightStick.X == 1)
{
    // Vi kollar rakt till höger.
    m_pDirection = PlayerDirection.EAST;
}
if (pRightStick.X == -1)
    m_pDirection = PlayerDirection.WEST;
*/

}
