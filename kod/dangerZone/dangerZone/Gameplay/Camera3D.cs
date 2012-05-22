using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay
{
    class Camera3D
    {
        DangerZone m_pParent;
        float m_fAspectRation = 0;
        float m_fFieldOfView = 90f;

        Vector3 m_pPosition = new Vector3(0, 0, 5000);
        Vector3 m_pLookAt = Vector3.Zero;

        Matrix m_pView;
        Matrix m_pProjection;

        public Camera3D(DangerZone pParent)
        {
            m_pParent = pParent;
            m_fAspectRation = m_pParent.GraphicsDevice.Viewport.AspectRatio;
            m_pView = Matrix.CreateLookAt(m_pPosition, m_pLookAt, new Vector3(0, 1, 0));
            m_pProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(m_fFieldOfView), m_fAspectRation, 0.1f, 10000.0f);
        }

        public Matrix getView()
        {
            return m_pView;
        }

        public Matrix getProjection()
        {
            return m_pProjection;
        }

        public Matrix getWorld(Vector3 pPosition)
        {
            return Matrix.CreateTranslation(pPosition);
        }

        public Matrix getWorld(Vector3 pPosition, float fScale)
        {
            return (Matrix.CreateScale(fScale, fScale, fScale) * Matrix.CreateTranslation(pPosition));
        }

        public Matrix getWorld(Vector3 pPosition, Vector3 pRotation)
        {
            return (Matrix.CreateRotationX(pRotation.X) * Matrix.CreateRotationY(pRotation.Y) * Matrix.CreateRotationZ(pRotation.Z) * Matrix.CreateTranslation(pPosition));
        }

        public Matrix getWorld(Vector3 pPosition, Vector3 pRotation, float fScale)
        {
            return (Matrix.CreateScale(fScale, fScale, fScale) * Matrix.CreateRotationX(pRotation.X) * Matrix.CreateRotationY(pRotation.Y) * Matrix.CreateRotationZ(pRotation.Z) * Matrix.CreateTranslation(pPosition));
        }


    }
}
