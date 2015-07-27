using UnityEngine;
using System.Collections;

namespace TDL
{
    public class Statistics
    {
        public Statistics()
        {
            Credits = 0;
            Kills = 0;
            Wave = 1;
        }

        public Statistics(int credits, int kills, int wave)
        {
            Credits = credits;
            Kills = kills;
            Wave = wave;
        }

        #region Properties
        public int Credits { get; private set; }

        public int Kills { get; private set; }

        public int Wave { get; private set; }

        #endregion
    }

}
