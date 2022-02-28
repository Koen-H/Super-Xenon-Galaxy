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
        private HUD hud;
        private Menu menu;

        private string _name;
        private int time;
        private int _score;
        private int _lifes;
        private int maxLifes = 5;

        private LeaderBoard leaderBoard;

        private Dictionary<EasyDraw, string> buttons;
        private bool buttonsActive;

        public PlayerData(string name = "", int score = 0, int lifes = 5)
        {

            maxLifes = lifes;
            _lifes = lifes;
            _score = score;
            _name = name;
        }

        public void Reset()
        {
            hud.visible = true;
            ResetLifes();
            ResetScore();
            ResetPlayerName();
            ResetLeaderBoard();
        }


        public int GetScore()
        {
            return _score;
        }

        public void IncreaseScore(float combo = 0)
        {
            switch (combo)
            {
                case 0:
                    {
                        _score += 5;
                        break;
                    }
                case 1:
                    {
                        _score += 10;
                        break;
                    }
                case 2:
                    {
                        _score += 20;
                        break;
                    }
                case 3:
                    {
                        _score += 50;
                        break;
                    }
            }
        }

        public int GetLifes()
        {
            return _lifes;
        }

        public void DecreaseLifes()
        {
            new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound when player loses 1 life.
            
            _lifes--;
            if (_lifes == 0)
            {
                HUD.goStart = Time.time;
            }
        }

        public bool isButtonActive()
        {
            return buttonsActive;
        }

        public void SetButtonActive(bool b)
        {
            buttonsActive = b;
        }

        public HUD GetHud()
        {
            return hud;
        }

        public void SetHud(HUD h)
        {
            hud = h;
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

        public void SetPlayerName(string s)
        {
            _name = s;
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
                    if (_name.Length == 3)
                    {
                       // Console.WriteLine("HAAAAAAAAAAA");
                        leaderBoard.GetHighScore().UpdateResults();
                        leaderBoard.GetHighScore().visible = true;
                    }
                }
            }
        }

        public int GetTime()
        {
            return time;
        }

        public void SetTime(int t)
        {
            time = t;
        }

        public Menu GetMenu()
        {
            return menu;
        }

        public void SetMenu(Menu m)
        {
            menu = m;
        }

        public LeaderBoard GetLeaderBoard()
        {
            return leaderBoard;
        }

        public void SetLeaderBoard(LeaderBoard l)
        {
            leaderBoard = l;
        }


        private void ResetLeaderBoard()
        {
            leaderBoard.GetHighScore().visible = false;
        }

        public void ResetScore()
        {
            _score = 0;
        }
        public void ResetLifes()
        {
            _lifes = maxLifes;
        }
        public void ResetPlayerName()
        {
            _name = "";
        }
    }
}
