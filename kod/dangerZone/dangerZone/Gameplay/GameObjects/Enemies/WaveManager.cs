using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace dangerZone.Gameplay.GameObjects.Enemies
{
    class WaveManager
    {

        EnemyManager m_pParent;

        Wave[] m_rgWaves = new Wave[Constants.MAXWAVES];

        Wave m_pCurrentWave = null;
        int m_nCurrentWave = 0;

        float m_fWaveDisplayTime = 0;
        float m_fMaxWaveDisplayTime = 5000;
        bool m_bDisplayingWave = false;

        public WaveManager(EnemyManager pParent)
        {
            m_pParent = pParent;
        }

        public void LoadContent(ContentManager pContent)
        {
            // Ladda in alla vågor.
            for (int i = 0; i < Constants.MAXWAVES; i++)
            {
                m_rgWaves[i] = new Wave(this, i, i*2);
            }
        }

        public void Update(GameTime pGameTime)
        {
            
            if(m_pCurrentWave != null)
                m_pCurrentWave.Update(pGameTime);

            if (m_bDisplayingWave)
            {
                m_fWaveDisplayTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
                //Console.WriteLine("NEXT WAVE IN: " + (m_fMaxWaveDisplayTime - m_fWaveDisplayTime) + " sec");
                if (m_fWaveDisplayTime > m_fMaxWaveDisplayTime)
                {
                    m_bDisplayingWave = false;

                    m_fWaveDisplayTime = 0;
                }
            }
            else
            {
                if (m_pCurrentWave != null)
                {
                    // Hit kommer vi om nedräkningen för nästa våg är AVKLARAD och vi ska säga till att spawna den.
                    if (m_pCurrentWave.SpawnNext)
                    {
                        m_bDisplayingWave = true;
                        m_nCurrentWave++;
                        this.SpawnWaveDisplay(m_nCurrentWave);
                        m_pCurrentWave = null;

                    }
                }
                else
                {
                    m_pCurrentWave = m_rgWaves[m_nCurrentWave];
                    m_pCurrentWave.Start();
                    
                }

            }
            

        }

        private void SpawnWaveDisplay(int nWaveNumber)
        {

            //Console.WriteLine("NEW WAVE!!!");
            
            this.Parent.Parent.ScrollerDisplay.Display("Wave " + nWaveNumber, m_fMaxWaveDisplayTime, 0);
        }

        public EnemyManager Parent
        {
            get { return m_pParent; }
            set { m_pParent = value; }
        }

        public void Start()
        {   
            m_pCurrentWave = m_rgWaves[m_nCurrentWave];
            m_pCurrentWave.Start();
        }

        public int CurrentWave
        {
            get { return m_nCurrentWave; }
        }


    }
}
