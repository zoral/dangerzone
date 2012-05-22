using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace dangerZone.Particles
{
    class ParticleEmitter
    {

        bool m_bDead = true;
        ParticleEngine m_pParent;

        Vector2 m_pPosition;
        Vector2 m_pDirection;

        Color m_pColor;
        Color m_pEndColor;
        float m_fTTL = 0;
        float m_fAliveTime = 0;
        float m_fPTTL = 0;

        float m_fSpawnTimer = 16; // Random på denna? 
        float m_fLastSpawn = 0;

        int m_nColor = 0;

        public ParticleEmitter(ParticleEngine pParent)
        {
            m_pParent = pParent;
        }

        public void Update(GameTime pGameTime)
        {
            m_fAliveTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_fAliveTime > m_fTTL)
            {
                m_bDead = true;
                return;
            }


            m_fLastSpawn += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_fLastSpawn > m_fSpawnTimer)
            {
                m_fLastSpawn = 0;
                // Spawna en ny partikel med lite variation på direction. 
                Vector2 pVelocity = m_pDirection;
                pVelocity.Normalize();
                pVelocity.X += ((float)m_pParent.Random.Next(-10, 10) / 10);// *(float)pGameTime.ElapsedGameTime.TotalSeconds;
                pVelocity.Y += ((float)m_pParent.Random.Next(-10, 10) / 10);// *(float)pGameTime.ElapsedGameTime.TotalSeconds;

                Vector2.Clamp(pVelocity, new Vector2(-1, -1), new Vector2(1, 1));

                Color pColor;
                if (m_pColor != m_pEndColor)
                {
                    // Kör random på vilken färg vi ska ta.
                    if (this.m_pParent.Random.Next(1) == 0)
                        pColor = m_pColor;
                    else
                        pColor = m_pEndColor;

                }
                else
                    pColor = m_pColor;

                m_pParent.InitParticle(m_pPosition, pVelocity, m_pColor, m_fPTTL);
            }
        }


        public bool Dead
        {
            get { return m_bDead; }
            set { m_bDead = value; }
        }

        public void Init(Vector2 pPosition, Vector2 pDirection, Color pColor, float fTTL, float fPTTL)
        {
            m_pPosition = pPosition;
            m_pDirection = pDirection;
            m_pColor = pColor;
            m_pEndColor = pColor;
            m_fTTL = fTTL;
            m_fPTTL = fPTTL;
            m_bDead = false;
            m_fAliveTime = 0;
        }

        public void Init(Vector2 pPosition, Vector2 pDirection, Color pStartColor, Color pEndColor, float fTTL, float fPTTL)
        {
            m_pPosition = pPosition;
            m_pDirection = pDirection;
            m_pColor = pStartColor;
            m_pEndColor = pEndColor;
            m_fTTL = fTTL;
            m_fPTTL = fPTTL;
            m_bDead = false;
            m_fAliveTime = 0;

            

        }
    }
}
