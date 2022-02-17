using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class Menu : Canvas
    {

        private Sprite arrow;
        private Sprite background;

        private bool active;

        public bool analogUp = false;
        public bool analogDown = false;
        public bool pressW;
        public bool pressS;
        private bool pressSpace;

        private int state = 1;

        public Menu() : base(1920, 1080, false)
        {
            active = true;

            CreateArrow();
            UpdateArrow();
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


        /// <summary>
        /// Creating menu arrow.
        /// </summary>
        private void CreateArrow()
        {
            background = new Sprite("Assets/Menu/background.png", false, false);
            AddChild(background);

            arrow = new Sprite("Assets/Menu/arrow.png", false, false);
            arrow.SetXY(width / 2 - arrow.width * 2, height / 2);
            AddChild(arrow);

        }

        /// <summary>
        /// Updating menu arrow.
        /// </summary>
        private void UpdateArrow()
        {   
            if ((Input.GetKeyDown(Key.W) || analogUp) && !pressW && state != 1 && visible)
            {
                pressW = true;
                state -= 1;
                arrow.y -= arrow.height * 3;
            }
            if ((Input.GetKeyDown(Key.S)|| analogDown) && !pressS && state != 3 && visible)
            {
                pressS = true;
                state += 1;
                arrow.y += arrow.height * 3;
            }

            if (Input.GetKeyDown(Key.SPACE) && !pressSpace && visible)
            {
                pressSpace = true;
                switch (state)
                {
                    case 1:
                        active = false;
                        visible = false;
                        break;
                    case 2:
                        break;
                    case 3:
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
