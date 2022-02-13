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
        private int lifes;

        public PlayerData()
        {
            lifes = 4;
            score = 0;
        }

        public int GetScore()
        {
            return score;
        }

        public void IncreaseScore()
        {
            score++;
        }

        public int GetLifes()
        {
            return lifes;
        }

        public void DecreaseLifes()
        {
            lifes--;
        }
    }
}
