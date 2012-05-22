using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dangerZone.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace dangerZone.Gameplay.Lights
{

    public enum LightSizes
    {
        BACKGROUND,
        SMALL,
        MEDIUM,
        BIG
    };

    class LightEngine
    {

        Light[] m_rgLights = new Light[Constants.MAXLIGHTS];
        GameplayScreen m_pParent;
        Texture2D[] m_rgTextures = new Texture2D[4];
        Vector2 m_pBackgroundPosition = new Vector2(0, 0);
        Rectangle m_pBackgroundSize;

        public LightEngine(GameplayScreen pParent)
        {
            m_pParent = pParent;
            m_pBackgroundSize = new Rectangle(0, 0, m_pParent.Parent.GraphicsDevice.PresentationParameters.BackBufferWidth, m_pParent.Parent.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public void LoadContent(ContentManager pContent)
        {
            m_rgTextures[0] = pContent.Load<Texture2D>("GFX\\Lights\\background");
            m_rgTextures[1] = pContent.Load<Texture2D>("GFX\\Lights\\small");
            m_rgTextures[2] = pContent.Load<Texture2D>("GFX\\Lights\\medium");
            m_rgTextures[3] = pContent.Load<Texture2D>("GFX\\Lights\\big");
            for (int i = 0; i < Constants.MAXLIGHTS; i++)
            {
                m_rgLights[i] = new Light();
            }
        }

        public int CreateLight(Vector2 pPosition, LightSizes pSize, Color pColor)
        {
            int nLight = -1;
            for (int i = 0; i < Constants.MAXLIGHTS; i++)
            {
                if (m_rgLights[i].Dead)
                {
                    nLight = i;

                    m_rgLights[i].Init(pPosition, m_rgTextures[(int)pSize], pColor);


                    break;
                }
            }

            return nLight;
        }

        public void Update(GameTime pGameTime)
        {

        }

        public void Draw(Camera pCamera, SpriteBatch pSpriteBatch)
        {
            
            pSpriteBatch.Begin();
            pSpriteBatch.Draw(m_rgTextures[(int)LightSizes.BACKGROUND], m_pBackgroundPosition, m_pBackgroundSize, Color.White);
            pSpriteBatch.End();
            
            pSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, pCamera.View);
            

            for (int i = 0; i < Constants.MAXLIGHTS; i++)
            {
                if (!m_rgLights[i].Dead)
                    m_rgLights[i].Draw(pSpriteBatch);
            }

            pSpriteBatch.End();
        }

        public Light[] Lights
        {
            get { return m_rgLights; }
            set { m_rgLights = value; }
        }

    }
}
