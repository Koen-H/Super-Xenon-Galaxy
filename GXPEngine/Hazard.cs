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
        public Hazard(CookieManager _cookieManager, float x, float y) : base("Assets/Hazard/hazard.png", 7, 3)
        {
            cookieManager = _cookieManager;
            collider.isTrigger = true;
            Random random = new Random();
            rotation = random.Next(0, 360);
            SetOrigin(width / 2, height / 2);
            SetScaleXY(2);
            SetXY(x, y);
            timeToDie = Time.time + 30000f;//Die after 30 seconds, when it's way off the screen
        }

        public void Update()
        {
            Animate(0.5f);
            
            float v = -125 * Time.deltaTime / 1000;

            Move(0, v);
            if (Time.time > timeToDie)
            {
                LateDestroy();
                cookieManager.removeItems.Add(this);
            }

        }

        public void OnCollision(GameObject other)
        {
            if (other is Player player)
            {
                Player otherp = (Player)other;
                otherp.TouchedHazard();
                LateDestroy();
                cookieManager.removeItems.Add(this);
                
            }

        }
    }
}
