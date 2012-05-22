using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace dangerZone.Gameplay.GameObjects.Enemies
{

    enum EnemyType
    {        
        BLOODMAGGOT = 0,
        TEMP = 1,
        NONE
    };

    class EnemyStatus
    {
        public int ID = 0;
        public bool DEAD = false;
    }

    class EnemyManager
    {

        GameplayScreen m_pParent;

        // Behöver en "våg"-manager...

        BaseEnemy[] m_rgEnemies = new BaseEnemy[Constants.MAXENEMIES];

        WaveManager m_pWaveManager;

        int m_nAliveEnemies = 0;

        public EnemyManager(GameplayScreen pParent)
        {
            m_pParent = pParent;
        }

        public void LoadContent(ContentManager pContent)
        {
            for (int i = 0; i < Constants.MAXENEMIES; i += 2)
            {
                m_rgEnemies[i] = new BloodMaggot(m_pParent);
                m_rgEnemies[i].LoadContent(pContent);

                // Detta är temporärt. Behöver väl ladda in dessa separat(kanske en egen array?)
                m_rgEnemies[i + 1] = new TempEnemy(m_pParent);
                m_rgEnemies[i + 1].LoadContent(pContent);
            }

            m_pWaveManager = new WaveManager(this);
            m_pWaveManager.LoadContent(pContent);

        }

        public void InitRandom()
        {
            Vector2 pPosition = new Vector2(-5,0);
            for (int i = 0; i < Constants.MAXENEMIES; i++)
            {
                if (m_rgEnemies[i].Dead)
                {
                    m_rgEnemies[i].Init(pPosition);
                    break;
                }
            }
        }

        public int InitRandom(int nHPMultiplier)
        {
            float fXPosition = m_pParent.Parent.Random.Next(-10, 10);
            float fYPosition = m_pParent.Parent.Random.Next(-10, 10);
            
            

            Vector2 pPosition = new Vector2(fXPosition, fYPosition);
            for (int i = 0; i < Constants.MAXENEMIES; i++)
            {
                if (m_rgEnemies[i].Dead)
                {
                    m_rgEnemies[i].Init(pPosition);
                    m_rgEnemies[i].Health = m_rgEnemies[i].Health * nHPMultiplier;
                    return i;
                    
                }
            }
            return 0;
        }

        public void Update(GameTime pGameTime)
        {
            m_pWaveManager.Update(pGameTime);

            m_nAliveEnemies = 0;
            for (int i = 0; i < Constants.MAXENEMIES; i++)
            {
                if (!m_rgEnemies[i].Dead)
                {
                    m_nAliveEnemies++;
                    m_rgEnemies[i].Update(pGameTime);
                }
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {

            

            for (int i = 0; i < Constants.MAXENEMIES; i++)
            {
                if (!m_rgEnemies[i].Dead)
                    m_rgEnemies[i].Draw(pSpriteBatch);
            }
        }

        public void DrawHUD(SpriteBatch pSpriteBatch)
        {
            
        }

        public BaseEnemy[] Enemies
        {
            get { return m_rgEnemies; }
            set { m_rgEnemies = value; }
        }

        public GameplayScreen Parent
        {
            get { return m_pParent; }
            set { m_pParent = value; }
        }

        public int CurrentWave
        {
            get { return m_pWaveManager.CurrentWave; }
        }

        public int ActiveEnemies
        {
            get { return m_nAliveEnemies; }
        }
    }
}
