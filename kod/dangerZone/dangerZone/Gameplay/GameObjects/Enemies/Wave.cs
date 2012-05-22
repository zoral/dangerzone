using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay.GameObjects.Enemies
{


    class Wave
    {

        WaveManager m_pParent;

        int m_nWaveNumber = 0;
        int m_nMaxEnemies = 0;

        float m_fAliveTime = 0;
        float m_fMaxAliveTime = 10000; // 20 sekunder ?
        bool m_bSpawnNext = false;

        EnemyStatus[] m_rgEnemyStatus;

        public Wave(WaveManager pParent, int nWaveNumber, int nMaxEnemies)
        {
            m_pParent = pParent;
            m_nWaveNumber = nWaveNumber;
            m_nMaxEnemies = nMaxEnemies;
            m_rgEnemyStatus = new EnemyStatus[nMaxEnemies];
            for(int i = 0; i < nMaxEnemies; i++)
                m_rgEnemyStatus[i] = new EnemyStatus();
        }

        public void Start()
        {
            // Då ska vi "fånga" antalet fiender som vi vill ha samt sätta deras HP till något bra.
            for(int i = 0; i < m_nMaxEnemies; i++)
            {
                int nId = m_pParent.Parent.InitRandom(m_nWaveNumber);
                m_rgEnemyStatus[i].ID = nId;
            }
        }

        public void Update(GameTime pGameTime)
        {
            /*
             * Kolla ifall alla fiender har dött. 
             * Isf kan vi sätta oss själv till att spawn nästa.
             */
            bool bAllDead = true;
            for (int i = 0; i < m_nMaxEnemies; i++)
            {
                if (m_pParent.Parent.Enemies[m_rgEnemyStatus[i].ID].Dead)
                {
                    m_rgEnemyStatus[i].DEAD = true;
                }
                if (!m_rgEnemyStatus[i].DEAD)
                {
                    bAllDead = false;
                }
            }

            if (bAllDead)
            {
                Console.WriteLine("ALL ENEMIES DEAD. SPAWNING NEXT WAVE");
                m_bSpawnNext = true;
            }

            m_fAliveTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_fAliveTime > m_fMaxAliveTime)
            {
                m_bSpawnNext = true;
            }
        }

        public bool SpawnNext
        {
            get { return m_bSpawnNext; }
            set { m_bSpawnNext = value; }
        }



    }
}
