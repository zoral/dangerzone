using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Gameplay.Lights;
using dangerZone.Gameplay.GameObjects;
using dangerZone.Screens;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace dangerZone.Gameplay.LevelItems
{
    class FlickeringLight : GameObject
    {

        int m_nLightId = -1;

        float m_fTempTime = 0;
        
        bool m_bTurnedOn = true;

        float[] m_rgTimes;
        int m_nMaxStatuses = 10;
        int m_nCurrentTime = 0;

        public FlickeringLight(GameplayScreen pParent)
            : base(pParent)
        {
            this.Collidiable = false;
            m_rgTimes = new float[m_nMaxStatuses];
            m_rgTimes[0] = 1500;
            m_rgTimes[1] = 1800;
            m_rgTimes[2] = 2000;
            m_rgTimes[3] = 2500;
            m_rgTimes[4] = 3000;
            m_rgTimes[5] = 6500;
            m_rgTimes[6] = 7000;
            m_rgTimes[7] = 9000;
            m_rgTimes[8] = 11000;
            m_rgTimes[9] = 16000;
        }

        public override void LoadContent(ContentManager pContent)
        {
            this.Animation = new SimpleAnimation(pContent.Load<Texture2D>("GFX\\LevelItems\\FLICKERINGLIGHT_TEMP"), Vector2.Zero, 1, 2, 0);
            this.Animation.Loop = false;
            this.Animation.Play = false;
            this.Animation.DepthLayer = 0.241f;

            m_nLightId = this.Parent.LightEngine.CreateLight(this.Animation.Position, LightSizes.BIG, Color.Yellow);
            
            base.LoadContent(pContent);
        }

        public override void Update(GameTime pGameTime)
        {

            m_fTempTime += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (m_fTempTime > m_rgTimes[m_nCurrentTime])
            {
                m_nCurrentTime++;
                if (m_nCurrentTime >= m_nMaxStatuses)
                {
                    m_fTempTime = 0;
                    m_nCurrentTime = 0;
                }


                this.m_bTurnedOn = (this.m_bTurnedOn) ? false : true;
                if (m_bTurnedOn)
                {
                    this.Animation.Col = 0;
                }
                else
                    this.Animation.Col = 1;


                this.Animation.UpdateSource();
                this.Parent.LightEngine.Lights[m_nLightId].TurnedOn = m_bTurnedOn;
            }

            this.Animation.Update(pGameTime);
            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            this.Animation.Draw(pSpriteBatch);
            base.Draw(pSpriteBatch);
        }

        

    }
}
