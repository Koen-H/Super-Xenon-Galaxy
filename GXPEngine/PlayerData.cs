using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    /// <summary>
    /// Player data class needed to use the data across all the code without messing it up.
    /// </summary>
    public class PlayerData
    {
        private int score;
        private int hearts;

        public PlayerData()
        {
            hearts = 3;
            score = 0;
        }

        public int GetScore()
        {
            return score;
        }

        public int GetHearts()
        {
            return hearts;
        }
    }
}
