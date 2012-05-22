using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace dangerZone.Gameplay.GameObjects
{
    class _3DModel
    {

        Vector3 m_pPosition = Vector3.Zero;
        Vector3 m_pRotation = Vector3.Zero;
        Model m_pModel;
        float m_fScale = 1.0f;
        string m_szModelName = "";

        public _3DModel(string szModelName)
        {
            m_szModelName = szModelName;
        }

        public void LoadContent(ContentManager pContent)
        {
            m_pModel = pContent.Load<Model>(m_szModelName);
        }

        public void Update(GameTime pGameTime)
        {
            m_pRotation.Y += MathHelper.ToRadians(1);
        }

        public void Draw(Camera3D pCamera)
        {
            Matrix[] pTransforms = new Matrix[m_pModel.Bones.Count];
            m_pModel.CopyAbsoluteBoneTransformsTo(pTransforms);
            foreach (ModelMesh pMesh in m_pModel.Meshes)
            {
                foreach (BasicEffect pEffect in pMesh.Effects)
                {
                    pEffect.EnableDefaultLighting();
                    
                    pEffect.World = pTransforms[pMesh.ParentBone.Index] * (pCamera.getWorld(m_pPosition, m_pRotation, m_fScale));
                    pEffect.View = pCamera.getView();
                    pEffect.Projection = pCamera.getProjection();
                }
                pMesh.Draw();
            }
        }

        public Vector3 Position
        {
            get { return m_pPosition; }
            set { m_pPosition = value; }
        }

        public Vector3 Rotation
        {
            get { return m_pRotation; }
            set { m_pRotation = value; }
        }

        public float Scale
        {
            get { return m_fScale; }
            set { m_fScale = value; }
        }

    }
}
