using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace dangerZone.Gameplay.GameObjects.Weapons
{

    public enum GunTypes
    {
        STANDARD,
        SHOTGUN,
        ROCKET,
        RAILGUN
    };

    class Weapon
    {
        string m_szName = "DEAFULT";
        float m_fDamage = 0;
        Color m_pColor = Color.White;
        float m_fSpeed = 0;
        float m_fReloadTime = 16; // i ms
        Texture2D m_pIcon;
        SoundEffect m_pSound;

        GunTypes m_pGunType = GunTypes.STANDARD;

        float m_fTTL = 4000; // 4 sekunder...

        SimpleAnimation m_pAnimation;

        float m_fTimeSinceFire = 0f;

        public Weapon()
        {

        }

        public void Fire()
        {
            m_fTimeSinceFire = 0;
        }

        public void Update(GameTime pGameTime)
        {

            m_pAnimation.Update(pGameTime);

            if (m_fTimeSinceFire > m_fReloadTime)
                return;

            m_fTimeSinceFire += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            
        }

        public bool CanFire()
        {
            if (m_fTimeSinceFire > m_fReloadTime)
                return true;

            return false;
        }

        public string Name
        {
            get { return m_szName; }
            set { m_szName = value; }
        }

        public float Damage
        {
            get { return m_fDamage; }
            set { m_fDamage = value; }
        }

        public Color Color
        {
            get { return m_pColor; }
            set { m_pColor = value; }
        }

        public float Speed
        {
            get { return m_fSpeed; }
            set { m_fSpeed = value; }
        }

        public float ReloadTime
        {
            get { return m_fReloadTime; }
            set { m_fReloadTime = value; }
        }

        public Texture2D Icon
        {
            get { return m_pIcon; }
            set { m_pIcon = value; }
        }

        public SoundEffect Sound
        {
            get { return m_pSound; }
            set { m_pSound = value; }
        }

        public SimpleAnimation Animation
        {
            get { return m_pAnimation; }
            set { m_pAnimation = value; }
        }

        public GunTypes GunType
        {
            get { return m_pGunType; }
            set { m_pGunType = value; }
        }

        public float TTL
        {
            get { return m_fTTL; }
            set { m_fTTL = value; }
        }

    }
}
