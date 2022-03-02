using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Hazard : AnimationSprite
    {
        private CookieManager cookieManager;

        private float timeToDie;

        Random random;
        public Hazard(CookieManager _cookieManager, float x, float y) : base("Assets/Hazard/hazard.png", 7, 3)
        {
            cookieManager = _cookieManager;
            collider.isTrigger = true;
            random = new Random();
            
            //rotation = random.Next(0, 360);
            SetOrigin(width / 2, height / 2);
            SetScaleXY(2);
            SetXY(x, y);
            SetRotation();

            timeToDie = Time.time + 30000f;//Die after 30 seconds, when it's way off the screen
        }

        public void Update()
        {
            //Console.WriteLine(String.Format("Rotation: {0}, X: {1}, Y: {2}", rotation, x, y));
            Animate(0.5f);
            
            float v = -125 * Time.deltaTime / 1000;

            Move(0, v);
            if (Time.time > timeToDie)
            {
                LateDestroy();
                cookieManager.removeItems.Add(this);
            }

        }

        private void SetRotation()
        {
            int n = 0;
            if (x < 0)
            {
                if (y < 0)
                {
                    n = random.Next(90, 180);
                }
                else if (y > 0 && y < game.height)
                {
                    n = random.Next(0, 180);
                }
                else
                {
                    n = random.Next(0, 90);

                }
            }

            else if (x > game.width)
            {
                if (y < 0)
                {
                    n = random.Next(180, 270);

                }
                else if (y > 0 && y < game.height)
                {
                    n = random.Next(180, 360);

                }
                else
                {
                    n = random.Next(270, 360);

                }
            }

            else
            {
                if (y < 0)
                {
                    n = random.Next(90, 270);
                }

                else if (y > game.height)
                {
                    n = random.Next(270, 450);
                }
            }
            rotation = n;
        }

        public void OnCollision(GameObject other)
        {
            if (other is Player player)
            {
                player.TouchedHazard();
                LateDestroy();
                cookieManager.removeItems.Add(this);
                
            }

        }
    }
}
