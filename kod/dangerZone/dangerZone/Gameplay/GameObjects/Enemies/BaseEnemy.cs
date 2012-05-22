using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay.GameObjects.Enemies
{
    class BaseEnemy : GameObject
    {

        

        float m_fSpeed = 0;
        float m_fDamage = 0;
        Vector2 m_pTarget = Vector2.Zero;
        Vector2 m_pVelocity = Vector2.Zero;
        bool m_bDead = true;
        float m_fHealth = 0;
        PlayerDirection m_pDirection;

        int m_nScore = 1000;

        EnemyType m_pType = EnemyType.NONE;

        public virtual void Init(Vector2 pPosition)
        {
            this.Animation.Position = pPosition;
            this.BoundingBox.UpdateSize(2, 2); // px
            this.BoundingBox.Update(pPosition.X, pPosition.Y);
        }

        public BaseEnemy(GameplayScreen pParent) : base(pParent)
        {
            
        }

        public override void Update(GameTime pGameTime)
        {

            if (!this.Dead)
            {
                // Då ska vi kolla vilken spelaren som är närmast och gå mot honom.
                // Måste ställa in en korrekt vilken också(genom velocity/direction).
                Vector2 pTarget = this.Parent.Players[0].Animation.Position;
                bool bUsingPlayerTwo = false;
                float fDistance = Vector2.Distance(pTarget, this.Animation.Position);



                if (this.Parent.Players[1].isPlaying())
                {
                    float fPlayerTwoDistance = Vector2.Distance(this.Parent.Players[1].Animation.Position, this.Animation.Position);
                    if (fPlayerTwoDistance < fDistance)
                    {
                        pTarget = this.Parent.Players[1].Animation.Position;
                        bUsingPlayerTwo = true;
                    }
                }

                // Kontrollera om player 1 är död. Isf ska vi gå till player two direkt.
                if (!this.Parent.Players[0].Dead())
                {
                    pTarget = this.Parent.Players[0].Animation.Position;
                }

                if (!this.Parent.Players[1].Dead())
                {
                    pTarget = this.Parent.Players[1].Animation.Position;
                }


                

                // Direction = target - position ? 
                if (Vector2.Distance(pTarget, this.Animation.Position) < 0.2f)
                {
                    // Man är så nära att man ska sprängas / skapa spelaren.
                    this.dealDamage(this.Health + 1);

                    if (bUsingPlayerTwo)
                    {
                        this.Parent.Players[1].dealDamage(1);

                    }
                    else
                        this.Parent.Players[0].dealDamage(1);
                }

                Vector2 pDirection = pTarget - this.Animation.Position;
                pDirection.Normalize(); 

                this.Velocity = pDirection * ((float)(this.Speed * pGameTime.ElapsedGameTime.TotalSeconds));

                if (this.Velocity.Y > 0)
                {
                    // Norrut
                    if (this.Velocity.X < -0.2f)
                        this.Direction = PlayerDirection.NORTHWEST;
                    if (this.Velocity.X > 0.2f)
                        this.Direction = PlayerDirection.NORTHEAST;
                    if (this.Velocity.X == 0)
                        this.Direction = PlayerDirection.NORTH;

                }
                if (this.Velocity.Y <= 0)
                {
                    // Söderut
                    if (this.Velocity.X < -0.2f)
                        this.Direction = PlayerDirection.SOUTHWEST;
                    if (this.Velocity.X > 0.2f)
                        this.Direction = PlayerDirection.SOUTHEAST;
                    if (this.Velocity.X == 0)
                        this.Direction = PlayerDirection.SOUTH;
                }
                if (this.Velocity.X == 1)
                {
                    // Vi kollar rakt till höger.
                    this.Direction = PlayerDirection.EAST;
                }
                if (this.Velocity.X == -1)
                    this.Direction = PlayerDirection.WEST;

                this.Animation.Rotation = (float)Math.Atan2(this.Velocity.X, this.Velocity.Y * -1); //(float)AngleBetweenVectors2(pTarget, this.Animation.Position);//

                this.Animation.Position += this.Velocity;

                this.BoundingBox.Update(this.Animation.Position.X, this.Animation.Position.Y);

                this.Animation.Update(pGameTime);

            }

            base.Update(pGameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch)
        {
            if (!this.Dead)
                this.Animation.Draw(pSpriteBatch);

            base.Draw(pSpriteBatch);
        }

        public static double AngleBetweenVectors2(Vector2 v1, Vector2 v2)
        {
            return ((v1.X - v2.X) > 0 ? -1 : 1) * (float)Math.Acos((double)Vector2.Dot(Vector2.Normalize(v1), Vector2.Normalize(v2)));
        }

        public float Speed
        {
            get { return m_fSpeed; }
            set { m_fSpeed = value; }
        }
        public float Damage
        {
            get { return m_fDamage; }
            set { m_fDamage = value; }
        }
        public Vector2 Target
        {
            get { return m_pTarget; }
            set { m_pTarget = value; }
        }
        public Vector2 Velocity
        {
            get { return m_pVelocity; }
            set { m_pVelocity = value; }
        }
        public float Health
        {
            get { return m_fHealth; }
            set { m_fHealth = value; }
        }
        public bool Dead
        {
            get { return m_bDead; }
            set { m_bDead = value; }
        }

        public virtual bool dealDamage(float fDamage)
        {
            // Kolla ifall man dör osv... ?
            this.Health -= fDamage;


            if (this.Health <= 0)
            {
                // Då har man dött. Spela upp dödsexplosionen(säg till en animationsmanager eller nåt). Ska även visa upp splash här.
                //this.Parent.AnimationManager.InitKillAnimation(this.Animation.Position);

                this.Parent.ParticleEngine.ExplosionEmitter(this.Animation.Position, Color.Red, Color.Yellow, 500, 50);

                this.Parent.AnimationManager.InitSplash(this.Animation.Position);
                this.Dead = true;
                    
            }

            return this.Dead;
        }

        public PlayerDirection Direction
        {
            get { return m_pDirection; }
            set { m_pDirection = value; }
        }

        public int Score
        {
            get { return m_nScore; }
            set { m_nScore = value; }
        }

        public EnemyType EnemyType
        {
            get { return m_pType; }
            set { m_pType = value; }
        }
        

    }
}
