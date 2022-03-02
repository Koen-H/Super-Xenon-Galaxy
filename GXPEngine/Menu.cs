using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Menu : Canvas
    {
        private PlayerData _pData;

        private Sprite howToPlay;
        private Sprite arrow;
        private Sprite background;

        private bool active;

        public bool analogUp = false;
        public bool analogDown = false;
        public bool pressW;
        public bool pressS;
        private bool pressSpace;

        private int state = 1;

        public Menu(PlayerData pData) : base(1920, 1080, false)
        {
            _pData = pData;
            _pData.SetMenu(this);
            active = true;

            CreateArrow();
            UpdateArrow();
            CreateHowToPlay();
        }

        public void Update()
        {
            UpdateArrow();
        }

        public bool isActive()
        {
            return active;
        }

        public void SetActive(bool b)
        {
            active = b;
        }

        public void SetArrow()
        {
            arrow.y += arrow.height * 3.5f;
            state = 3;
        }


        /// <summary>
        /// Creating menu arrow.
        /// </summary>
        private void CreateArrow()
        {
            background = new Sprite("Assets/Menu/background2.png", false, false);
            AddChild(background);

            arrow = new Sprite("Assets/Menu/arrow.png", false, false);
            arrow.SetScaleXY(1.5f);
            arrow.SetXY(width / 2 + arrow.width * 2, height / 2 + arrow.height * 1.5f);
            AddChild(arrow);

        }

        private void CreateHowToPlay()
        {
            howToPlay = new Sprite("Assets/Menu/howtoplay.png", false, false);
            howToPlay.SetOrigin(howToPlay.width / 2, howToPlay.height / 2);
            howToPlay.SetXY(width / 2, height / 2);
            howToPlay.visible = false;
            AddChild(howToPlay);
        }

        /// <summary>
        /// Updating menu arrow.
        /// </summary>
        private void UpdateArrow()
        {   
            if ((Input.GetKeyDown(Key.W) || analogUp) && !pressW && state != 1 && visible)
            {
                pressW = true;
                if (state == 2)
                {
                    arrow.y -= arrow.height * 2;

                }
                else if (state == 3)
                {
                    arrow.y -= arrow.height * 1.5f;
                }
                state -= 1;
            }
            if ((Input.GetKeyDown(Key.S) || analogDown) && !pressS && state != 3 && visible)
            {
                pressS = true;

                if (state == 1)
                {
                    arrow.y += arrow.height * 2;

                }
                else if (state == 2)
                {
                    arrow.y += arrow.height * 1.5f;
                }
                state += 1;
            }

            if (Input.GetKeyDown(Key.SPACE) && !pressSpace && visible)
            {
                new Sound("Assets/Sounds/select.wav").Play(false,0,0.5f);
                MyGame myGame = (MyGame)game;
                myGame.PlayBackgroundMusic("Music game.mp3");
               pressSpace = true;
                switch (state)
                {
                    case 1:
                        active = false;
                        visible = false;
                        _pData.Reset();
                        _pData.GetHud().SetTime(Time.time);
                        HUD.goStart = Time.time;
                        break;
                    case 2:
                        break;
                    case 3:
                        howToPlay.visible = !howToPlay.visible;
                        break;
                }
            }
            
            else
            {
                active = true;
            }

            if (Input.GetKeyUp(Key.W))
            {
                pressW = false;
            }

            if (Input.GetKeyUp(Key.S))
            {
                pressS = false;
            }

            if (Input.GetKeyUp(Key.SPACE))
            {
                pressSpace = false;
            }
        }

    }
}
