using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using dangerZone.Gameplay;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace dangerZone.Particles
{
    class ParticleEngine
    {

        Particle[] m_rgParticles = new Particle[Constants.MAXPARTICLES];
        ParticleEmitter[] m_rgParticleEmitter = new ParticleEmitter[Constants.MAXEMITTERS]; 

        Texture2D m_pTexture;
        Random m_pRandom;

        int m_nActiveParticles = 0;
        int m_nActiveEmitters = 0;

        public ParticleEngine(Screen pParent)
        {
            m_pRandom = new Random();
        }

        public void LoadContent(ContentManager pContent)
        {
            m_pTexture = pContent.Load<Texture2D>("GFX\\particle");
            for (int i = 0; i < Constants.MAXPARTICLES; i++)
            {
                
                m_rgParticles[i] = new Particle(this);
            }

            for (int i = 0; i < Constants.MAXEMITTERS; i++)
                m_rgParticleEmitter[i] = new ParticleEmitter(this);
        }

        public void Update(GameTime pGameTime)
        {
            m_nActiveParticles = 0;
            m_nActiveEmitters = 0;
            for (int i = 0; i < Constants.MAXEMITTERS; i++)
            {
                if (!m_rgParticleEmitter[i].Dead)
                    m_nActiveEmitters++;
                m_rgParticleEmitter[i].Update(pGameTime);
            }
            for (int i = 0; i < Constants.MAXPARTICLES; i++)
            {
                if (!m_rgParticles[i].Dead)
                    m_nActiveParticles++;

                if (!m_rgParticles[i].Dead)
                    m_rgParticles[i].Update(pGameTime);
            }
            
        }

        public void Draw(SpriteBatch pSpriteBatch, bool bConvert)
        {
            for (int i = 0; i < Constants.MAXPARTICLES; i++)
            {
                if (!m_rgParticles[i].Dead)
                    m_rgParticles[i].Draw(pSpriteBatch, m_pTexture, bConvert);
            }
            
        }

        public void Emitter(Vector2 pPosition, Vector2 pDirection, Color pColor, float fTTL, float fParticleTTL)
        {
            /*
             * Gör en emitter på positionen. Den ska hålla i sig ett litet tag...
             */
            for (int i = 0; i < Constants.MAXEMITTERS; i++)
            {
                if (m_rgParticleEmitter[i].Dead)
                {
                    m_rgParticleEmitter[i].Init(pPosition, pDirection, pColor, fTTL, fParticleTTL);
                    break;
                }
            }
        }

        public void ExplosionEmitter(Vector2 pPosition, Color pStartColor, Color pEndColor, float fTTL, float fParticleTTL)
        {
            int nIds = 0;
            int[] rgIds = new int[4];
            for (int i = 0; i < Constants.MAXEMITTERS; i++)
            {
                if (nIds == 4)
                    break;
                if (m_rgParticleEmitter[i].Dead)
                {
                    rgIds[nIds] = i;
                    nIds++;
                    //m_rgParticleEmitter[i].Init(pPosition, pDirection, pColor, fTTL, fParticleTTL);
                    //break;
                }
            }

            Vector2 pDirection = Vector2.Zero;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        pDirection = new Vector2(-1, 0);
                        break;
                    case 1:
                        pDirection = new Vector2(1, 0);
                        break;
                    case 2:
                        pDirection = new Vector2(0, 1);
                        break;
                    case 3:
                        pDirection = new Vector2(0, -1);
                        break;
                }
                m_rgParticleEmitter[i].Init(pPosition, pDirection, pStartColor, pEndColor, fTTL, fParticleTTL);
            }
        }

        public void InitParticle(Vector2 pPosition, Vector2 pVelocity, Color pColor, float fTTL)
        {
            for (int i = 0; i < Constants.MAXPARTICLES; i++)
            {
                if (m_rgParticles[i].Dead)
                {
                    m_rgParticles[i].Init(pPosition, pVelocity, pColor, fTTL);
                    break;
                }
            }
        }

        public Random Random
        {
            get { return m_pRandom; }
            set { m_pRandom = value; }
        }

        public int ActiveParticles
        {
            get { return m_nActiveParticles; }
        }
        public int ActiveEmitters
        {
            get { return m_nActiveEmitters; }
        }
    }
}
