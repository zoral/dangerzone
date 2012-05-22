using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay.GameObjects.Enemies
{
    class BloodMaggot : BaseEnemy
    {

        public BloodMaggot(GameplayScreen pParent) : base(pParent)
        {
            this.EnemyType = EnemyType.BLOODMAGGOT;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager pContent)
        {

            Texture2D pTexture = pContent.Load<Texture2D>("GFX\\Enemies\\blood_maggot");
            this.Animation = new SimpleAnimation(pTexture, Vector2.Zero, 1, 2, this.Parent.GameConfiguration.EnemyAnimationSpeed[(int)this.EnemyType]);
            this.Speed = this.Parent.GameConfiguration.EnemySpeed[(int)this.EnemyType];
            this.Damage = this.Parent.GameConfiguration.EnemyDamage[(int)this.EnemyType];
            this.Health = this.Parent.GameConfiguration.EnemyHealth[(int)this.EnemyType];
            this.Score = (int)this.Parent.GameConfiguration.EnemyScore[(int)this.EnemyType];
            this.Dead = true;

            base.LoadContent(pContent);
        }

        public override void Init(Vector2 pPosition)
        {
            this.Dead = false;
            this.Health = this.Parent.GameConfiguration.EnemyHealth[(int)this.EnemyType];
            base.Init(pPosition);
        }


    }
}
