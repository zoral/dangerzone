using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using dangerZone.Screens;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace dangerZone.HighScore
{
    [Serializable()]
    class HighScore : ISerializable, IComparable
    {
        string m_szPlayerName = "";
        int m_nWave = 0;
        int m_nScore = 0;
        int m_nPlayTime = 0;

        public HighScore(string szPlayerName, int nWave, int nScore, int nPlayTime)
        {
            m_szPlayerName = szPlayerName;
            m_nWave = nWave;
            m_nScore = nScore;
            m_nPlayTime = nPlayTime;
        }

        public HighScore(SerializationInfo pInfo, StreamingContext pContext)
        {
            this.PlayerName = (string)pInfo.GetValue("playerName", typeof(string));
            this.Wave = (int)pInfo.GetValue("wave", typeof(int));
            this.Score = (int)pInfo.GetValue("score", typeof(int));
            this.PlayTime = (int)pInfo.GetValue("playTime", typeof(int));

        }

        public string getHash()
        {
            string szHash = this.PlayerName + "_" + this.Wave.ToString() + "_" + this.PlayTime + "_" + this.Score.ToString();

            byte[] pAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(szHash);
            szHash = System.Convert.ToBase64String(pAsBytes);


            return szHash;
        }

        public string PlayerName
        {
            get { return m_szPlayerName; }
            set { m_szPlayerName = value; }
        }
        public int Wave
        {
            get { return m_nWave; }
            set { m_nWave = value; }
        }

        public int Score
        {
            get { return m_nScore; }
            set { m_nScore = value; }
        }
        public int PlayTime
        {
            get { return m_nPlayTime; }
            set { m_nPlayTime = value; }
        }

        public void GetObjectData(SerializationInfo pInfo, StreamingContext pContext)
        {
            pInfo.AddValue("playerName", this.PlayerName);
            pInfo.AddValue("wave", this.Wave);
            pInfo.AddValue("score", this.Score);
            pInfo.AddValue("playTime", this.PlayTime);
        }

        public int CompareTo(object obj)
        {
            HighScore pItem = (HighScore)obj;
            return pItem.Score.CompareTo(this.Score);
            //return this.Score.CompareTo(pItem.Score);

            //return (this.Score);
        }
    }

    class HighScoreManager
    {
        

        HighScore[] m_rgHighScores = new HighScore[10];

        public HighScoreManager()
        {
            
        }

        public void LoadContent(ContentManager pContent)
        {
            /*
             * Finns det ingen fil med highscores så ska vi skapa en och fylla den med "skit".
             */

            for (int i = 0; i < 10; i++)
            {
                m_rgHighScores[i] = new HighScore("", 0, 0, 0);
            }

            

            if (!File.Exists("highscores.lst"))
            {
                CreateFile();
            }

            ReadHighscores();

        }

        public int AddHighscore(string szPlayerName, int nWave, int nScore, int nPlayTime)
        {



            
            int nWantedPlace = -1;
            for (int i = 0; i < 10; i++)
            {
                if (nScore > m_rgHighScores[i].Score)
                {
                    nWantedPlace = i;
                    // Alla andra ska flyttas neråt!
                    break;
                }
            }

            if (nWantedPlace != -1)
            {
                m_rgHighScores[9] = new HighScore(szPlayerName, nWave, nScore, nPlayTime);
                Array.Sort(m_rgHighScores);

                //Spara filen
                this.SaveHighscores();
                
            }

            return nWantedPlace;
        }


        public HighScore[] List
        {
            get { return m_rgHighScores; }
        }

        private void CreateFile()
        {
            Stream pStream = File.Create("highscores.lst");
            BinaryFormatter pFormatter = new BinaryFormatter();
            pFormatter.Serialize(pStream, m_rgHighScores);
            pStream.Close();
        }

        private void ReadHighscores()
        {
            Stream pStream = File.Open("highscores.lst", FileMode.Open);
            BinaryFormatter pFormatter = new BinaryFormatter();
            m_rgHighScores = (HighScore[])pFormatter.Deserialize(pStream);
            pStream.Close();

        }
        private void SaveHighscores()
        {
            File.Delete("highscores.lst");
            this.CreateFile();
        }

        




    }
}
