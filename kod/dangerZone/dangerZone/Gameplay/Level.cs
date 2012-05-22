using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using dangerZone.Gameplay.GameObjects;

namespace dangerZone.Gameplay
{
    class Level
    {

        Circle m_pBounds;
        Texture2D m_pBackground;
        SimpleAnimation m_pFence;
        SimpleAnimation m_pBackdrop;
        Vector2 m_pBackgroundPosition;
        Vector2 m_pOrigin;
        Rectangle m_pSourceRectangle;

        BoundingBox2D m_pBoundingBox;

        public Level(GameplayScreen pParent)
        {
            m_pBounds = new Circle(Vector2.Zero, 10);
            Console.WriteLine(m_pBounds.Center.ToString());
            m_pBoundingBox = new BoundingBox2D(0, 0, 62, 62);
            Console.WriteLine(m_pBoundingBox.ToString());
        }

        public void LoadContent(ContentManager pContent)
        {
            
            m_pBackgroundPosition = Vector2.Zero;
            m_pBackground = pContent.Load<Texture2D>("GFX\\Level\\arena_400x528");
            m_pFence = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\Level\\props_anim_16ms"), Vector2.Zero, 1, 4, 64);
            m_pBackdrop = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\Level\\backdrop_1440x720"), Vector2.Zero, 1, 1, 0);
            m_pBackdrop.DepthLayer = 0.25f;
            m_pFence.DepthLayer = 0.25f;
            m_pOrigin = new Vector2(m_pBackground.Width/2, m_pBackground.Height/2);
            m_pSourceRectangle = new Rectangle(0, 0, m_pBackground.Width, m_pBackground.Height);

        }

        public void Update(GameTime pGameTime)
        {
            m_pFence.Update(pGameTime);
            m_pBackdrop.Update(pGameTime);
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            //m_pBackdrop.Draw(pSpriteBatch);

            pSpriteBatch.Draw(m_pBackground, ConvertUnits.ToDisplayUnits(m_pBackgroundPosition), m_pSourceRectangle, Color.White, 0f, m_pOrigin, 1f, SpriteEffects.None, 0.25f);
            m_pFence.Draw(pSpriteBatch);
            
        }

        public Circle Bounds
        {
            get { return m_pBounds; }
            set { m_pBounds = value; }
        }

        public BoundingBox2D BoundingBox
        {
            get { return m_pBoundingBox; }
            set { m_pBoundingBox = value; }
        }

    }
}
