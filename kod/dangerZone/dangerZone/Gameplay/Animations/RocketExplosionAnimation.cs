using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using dangerZone.Gameplay.GameObjects;

namespace dangerZone.Gameplay.Animations
{
    class RocketExplosionAnimation
    {

        SimpleAnimation m_pAnimation;
        AnimationManager m_pParent;
        bool m_bPlaying = false;


        public RocketExplosionAnimation(AnimationManager pParent, Texture2D pTexture, int nMaxColumns, float fTimePerFrame)
        {
            m_pParent = pParent;
            m_pAnimation = new SimpleAnimation(pTexture, Vector2.Zero, 0, nMaxColumns, fTimePerFrame);
            m_pAnimation.Loop = false;
            m_pAnimation.Play = false;
        }

        public void Init(Vector2 pPosition)
        {
            m_pAnimation.Position = pPosition;
            m_bPlaying = true;
            m_pAnimation.Play = true;
            m_pAnimation.Reset();
        }

        public void Update(GameTime pGameTime)
        {
            // Om loopen är färdig. Då spelas inte animationen längre.
            if (!m_pAnimation.Play)
                m_bPlaying = false;

            if (m_bPlaying)
                m_pAnimation.Update(pGameTime);
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            if (m_bPlaying)
                m_pAnimation.Draw(pSpriteBatch);
        }

        public bool Playing
        {
            get { return m_bPlaying; }
            set { m_bPlaying = value; }
        }


    }
}
