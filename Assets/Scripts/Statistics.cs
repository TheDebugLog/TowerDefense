using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace TDL
{
      public class Statistics:IComparable<Statistics>
      {
        public Statistics()
        {
            Score = 0;
            Kills = 0;
            Wave = 0;
        }

        public Statistics(int score, int kills, int wave)
        {
            Score = score;
            Kills = kills;
            Wave = wave;
        }

        #region Properties
        public int Score { get; private set; }

        public int Kills { get; private set; }

        public int Wave { get; private set; }

        #endregion

        public int CompareTo(Statistics other)
        {
            return Score.CompareTo(other.Score);
        }
    }

}
