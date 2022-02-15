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
        private string _name;

        private int score;
        private int lifes;
        private float hudHeight;

        private Dictionary<EasyDraw, string> buttons;

        public PlayerData()
        {
            lifes = 5;
            score = 0;
            _name = "";
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

        public float GetHudHeight()
        {
            return hudHeight;
        }

        public void SetHudHeight(float h)
        {
            hudHeight = h;
        }

        public Dictionary<EasyDraw, string> GetButtons()
        {
            return buttons;
        }

        public void SetButtons(Dictionary<EasyDraw, string> b)
        {
            buttons = b;
        }

        public string GetPlayerName()
        {
            return _name;
        }

        public void ChangeName(string s)
        {
            if (s.Length > 0)
            {
                if (s.Equals("-"))
                {
                    _name = _name.Remove(_name.Length - 1, 1);
                }
                else
                {

                    if (_name.Length < 3)
                    {

                        _name += s;
                        Console.WriteLine(_name);

                    }
                }
            }
        }
    }
}
