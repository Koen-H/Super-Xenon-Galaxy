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
        private bool buttonsActive;

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

        public void IncreaseScore(float combo = 0)
        {
            switch (combo)
            {
                case 0:
                    {
                        score += 5;
                        break;
                    }
                case 1:
                    {
                        new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound chain 2
                        score += 10;
                        break;
                    }
                case 2:
                    {
                        new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound chain 3
                        score += 20;
                        break;
                    }
                case 3:
                    {
                        new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound chain 4
                        score += 50;
                        break;
                    }
            }
        }

        public int GetLifes()
        {
            return lifes;
        }

        public void DecreaseLifes()
        {
            new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound when player loses 1 life.
            lifes--;
        }

        public bool isButtonActive()
        {
            return buttonsActive;
        }

        public void SetButtonActive(bool b)
        {
            buttonsActive = b;
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
                if (s.Equals("-") && _name.Length > 0)
                {
                    _name = _name.Remove(_name.Length - 1, 1);
                }
                else if(!s.Equals("-") && _name.Length < 3)
                {
                    _name += s;
                }
            }
        }
    }
}
