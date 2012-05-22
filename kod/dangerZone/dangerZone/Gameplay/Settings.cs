using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace dangerZone.Gameplay
{
    public class Settings
    {


        PlayerIndex m_pPlayerOneIndex;
        PlayerIndex m_pPlayerTwoIndex;

        bool m_bPlayMusic = true;
        bool m_bPlaySFX = true;

        string m_szPlayerOneName = "Player one";
        string m_szPlayerTwoName = "Player two";

        int m_nPlayerOneWave = 0;
        int m_nPlayerTwoWave = 0;
        int m_nPlayerOneScore = 0;
        int m_nPlayerTwoScore = 0;
        int m_nPlayerOnePlayTime = 0;
        int m_nPlayerTwoPlayTime = 0;

        bool m_bTwoPlayers = false;

        public Settings()
        {
        }

        public PlayerIndex PlayerOne
        {
            get { return m_pPlayerOneIndex; }
            set { m_pPlayerOneIndex = value; }
        }
        public PlayerIndex PlayerTwo
        {
            get { return m_pPlayerTwoIndex; }
            set 
            {
                m_pPlayerTwoIndex = value; 
            }
        }
        public bool TwoPlayers
        {
            get { return m_bTwoPlayers; }
            set { m_bTwoPlayers = value; }
        }

        public bool PlayMusic
        {
            get { return m_bPlayMusic; }
            set { m_bPlayMusic = value; }
        }

        public bool PlaySFX
        {
            get { return m_bPlaySFX; }
            set { m_bPlaySFX = value; }
        }

        public string PlayerOneName
        {
            get { return m_szPlayerOneName; }
            set { m_szPlayerOneName = value; }
        }
        public string PlayerTwoName
        {
            get { return m_szPlayerTwoName; }
            set { m_szPlayerTwoName = value; }
        }

        public int PlayerOneWave
        {
            get { return m_nPlayerOneWave; }
            set { m_nPlayerOneWave = value; }
        }
        public int PlayerTwoWave
        {
            get { return m_nPlayerTwoWave; }
            set { m_nPlayerTwoWave = value; }
        }

        public int PlayerOneScore
        {
            get { return m_nPlayerOneScore; }
            set { m_nPlayerOneScore = value; }
        }

        public int PlayerTwoScore
        {
            get { return m_nPlayerTwoScore; }
            set { m_nPlayerTwoScore = value; }
        }

        public int PlayerOnePlayTime
        {
            get { return m_nPlayerOnePlayTime; }
            set { m_nPlayerOnePlayTime = value; }
        }

        public int PlayerTwoPlayTime
        {
            get {return m_nPlayerTwoPlayTime; }
            set { m_nPlayerTwoPlayTime = value;}
        }

        
    }
}
