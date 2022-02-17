using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class PowerUp : AnimationSprite
    {
        private CookieManager cookieManager;
        public PowerUp(CookieManager _cookieManager, float x, float y) : base("Assets/Powerup/powerup.png", 7, 3)
        {
            cookieManager = _cookieManager;
            collider.isTrigger = true;
            SetOrigin(width / 2, height / 2);
            SetScaleXY(2);
            SetXY(x, y);
        }
        public void OnCollision(GameObject other)
        {
            if (other is Player player)
            {
                cookieManager.ApplyPowerUp(player.currentColor);
                LateDestroy();
                cookieManager.currentPowerUps.Remove(this);
            }

        }
        public void Update()
        {

            float v = -60 * Time.deltaTime / 1000;

            Move(0, v);
            if (x < -width / 4) x = game.width + width / 4;
            if (x > game.width + width / 4) x = width / 4;
            if (y < height / 4) y = game.height + height / 4;
            if (y > game.height + height / 4) y = height / 4;
        }
    }
}
