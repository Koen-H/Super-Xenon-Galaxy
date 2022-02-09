using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {
        private Level _level;

        private float speedX;
        private float speedY;
        public Player(Level level, Vector2 pos) : base("barry.png", 7, 1)
        {
            x = pos.x;
            y = pos.y;

            SetOrigin(width / 2, height / 2);

            speedX = 0;
            speedY = 0;

            _level = level;

            collider.isTrigger = true;
        }

        public void Update()
        {
            Animate(0.05f);
            Move();
        }

        private void Move()
        {
            speedX = 0;
            speedY = 0;

            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D))
            {
                speedX = -1;
            }

            if (Input.GetKey(Key.D) && !Input.GetKey(Key.A))
            {
                speedX = 1;
            }

            if (Input.GetKey(Key.W) && !Input.GetKey(Key.S))
            {
                speedY = -1;
            }

            if (Input.GetKey(Key.S) && !Input.GetKey(Key.W))
            {
                speedY = 1;
            }


            if (MoveUntilCollision(speedX, 0) != null)
            {
                //speedX = 0;
            }
            if (MoveUntilCollision(0, speedY) != null)
            {
                //speedY = 0;
            }
        }
    }

}
