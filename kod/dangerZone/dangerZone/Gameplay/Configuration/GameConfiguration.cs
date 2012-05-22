using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace dangerZone.Gameplay.Configuration
{
    class GameConfiguration
    {

        public float PlayerSpeed = 10;
        public float PlayerLives = 10;
        public float[] WeaponAnimationSpeeds = new float[5];
        public float[] WeaponReloadTime = new float[5];
        public float[] WeaponBulletSpeed = new float[5];
        public float[] WeaponBulletDamage = new float[5];

        public float[] EnemyAnimationSpeed = new float[2];
        public float[] EnemyHealth = new float[2];
        public float[] EnemyScore = new float[2];
        public float[] EnemySpeed = new float[2];
        public float[] EnemyDamage = new float[2];

        public GameConfiguration()
        {
            WeaponAnimationSpeeds[0] = 100;
            WeaponAnimationSpeeds[1] = 100;
            WeaponAnimationSpeeds[2] = 100;
            WeaponAnimationSpeeds[3] = 100;
            WeaponAnimationSpeeds[4] = 100;

            WeaponReloadTime[0] = 128;
            WeaponReloadTime[1] = 64;
            WeaponReloadTime[2] = 1500;
            WeaponReloadTime[3] = 800;
            WeaponReloadTime[4] = 1500;

            WeaponBulletDamage[0] = 1.5f;
            WeaponBulletDamage[1] = 1;
            WeaponBulletDamage[2] = 10;
            WeaponBulletDamage[3] = 0.2f;
            WeaponBulletDamage[4] = 50f;

            WeaponBulletSpeed[0] = 0.2f;
            WeaponBulletSpeed[1] = 0.3f;
            WeaponBulletSpeed[2] = 0.7f;
            WeaponBulletSpeed[3] = 0.2f;
            WeaponBulletSpeed[4] = 0.02f;


            EnemyAnimationSpeed[0] = 200;
            EnemyAnimationSpeed[1] = 200;
            EnemyHealth[0] = 1;
            EnemyHealth[1] = 1;
            EnemyScore[0] = 1000;
            EnemyScore[1] = 1000;
            EnemySpeed[0] = 0.6f;
            EnemySpeed[1] = 0.6f;
            EnemyDamage[0] = 1;
            EnemyDamage[1] = 1;
        }



        

        private string[] getData()
        {
            string[] szData = new string[32];
            szData[0] = "f:PLAYERSPEED:" + PlayerSpeed.ToString() + "";
            szData[1] = "f:PLAYERLIVES:" + PlayerLives.ToString();
            int nId = 2;
            for (int i = 0; i < 5; i++)
            {
                szData[nId] = i + ":WEAPONANIMATIONSPEED:" + WeaponAnimationSpeeds[i].ToString();
                nId++;
            }
            for (int i = 0; i < 5; i++)
            {
                szData[nId] = i + ":WEAPONRELOADTIME:" + WeaponReloadTime[i].ToString();
                nId++;
            }
            for (int i = 0; i < 5; i++)
            {
                szData[nId] = i + ":WEAPONBULLETDAMAGE:" + WeaponBulletDamage[i].ToString();
                nId++;
            }
            for (int i = 0; i < 5; i++)
            {
                szData[nId] = i + ":WEAPONBULLETSPEED:" + WeaponBulletSpeed[i].ToString();
                nId++;
            }
            for (int i = 0; i < 2; i++)
            {
                szData[nId] = i + ":ENEMYANIMATIONSPEED:" + EnemyAnimationSpeed[i].ToString();
                nId++;
            }
            for (int i = 0; i < 2; i++)
            {
                szData[nId] = i + ":ENEMYHEALTH:" + EnemyHealth[i].ToString();
                nId++;
            }
            for (int i = 0; i < 2; i++)
            {
                szData[nId] = i + ":ENEMYSCORE:" + EnemyScore[i].ToString();
                nId++;
            }
            for (int i = 0; i < 2; i++)
            {
                szData[nId] = i + ":ENEMYSPEED:" + EnemySpeed[i].ToString();
                nId++;
            }
            for (int i = 0; i < 2; i++)
            {
                szData[nId] = i + ":ENEMYDAMAGE:" + EnemyDamage[i].ToString();
                nId++;
            }

            
            return szData;
        }


        public void Save()
        {
            // Skriv allting till en fil.
            if (File.Exists("gameConfiguration.cfg"))
                File.Delete("gameConfiguration.cfg");

            File.WriteAllLines("gameConfiguration.cfg", getData());
        }

        public void Load()
        {
            // Ladda in alla värden från en fil.

            if (File.Exists("gameConfiguration.cfg"))
            {
                string[] rgData = File.ReadAllLines("gameConfiguration.cfg");


                foreach (string szData in rgData)
                {
                    string[] rgColumns = szData.Split(':');
                    switch (rgColumns[1])
                    {
                        case "PLAYERSPEED":
                            PlayerSpeed = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "PLAYERLIVES":
                            PlayerLives = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "WEAPONANIMATIONSPEED":
                            WeaponAnimationSpeeds[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "WEAPONRELOADTIME":
                            WeaponReloadTime[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "WEAPONBULLETDAMAGE":
                            WeaponBulletDamage[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "WEAPONBULLETSPEED":
                            WeaponBulletSpeed[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "ENEMYANIMATIONSPEED":
                            EnemyAnimationSpeed[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "ENEMYHEALTH":
                            EnemyHealth[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "ENEMYSCORE":
                            EnemyScore[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "ENEMYSPEED":
                            EnemySpeed[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                        case "ENEMYDAMAGE":
                            EnemyDamage[System.Convert.ToInt32(rgColumns[0])] = System.Convert.ToSingle(rgColumns[2]);
                            break;
                    }
                }

            }
            else
                Save();
            

        }


    }
}
