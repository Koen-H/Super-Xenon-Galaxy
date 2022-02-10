using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{

    public enum PlayerColor
    {
        PINK, PURPLE, ORANGE, CYAN
    }

    public class Player : AnimationSprite
    {
        private Level _level;
        private float speedX;
        private float speedY;

        private int spriteCols = 7;
        private int spriteRows = 1;
        private PlayerColor currentColor;

        private float speed;
        public Player(Level level, Vector2 pos) : base("barry.png", 7, 1)
        {
            x = pos.x;
            y = pos.y;

            SetOrigin(width / 2, height / 2);
            SetScaleXY(3);

            speedX = 0;
            speedY = 0;
            speed = 3;

            currentColor = PlayerColor.PINK;//The default color for the player
            UpdateSprite();

            _level = level;

            collider.isTrigger = true;
        }

        public void Update()
        {
            Animate(0.05f);
            Move();
            ChangeColor();
        }

        private void Move()
        {
            speedX = 0;
            speedY = 0;

            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D))
            {
                speedX = -speed;
            }

            else if (Input.GetKey(Key.D) && !Input.GetKey(Key.A))
            {
                speedX = speed;
            }

            if (Input.GetKey(Key.W) && !Input.GetKey(Key.S))
            {
                speedY = -speed;
            }

            else if (Input.GetKey(Key.S) && !Input.GetKey(Key.W))
            {
                speedY = speed;
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
        
        private void ChangeColor()
        {
            if (Input.GetKeyDown(Key.UP))
            {
                currentColor = PlayerColor.CYAN;
                UpdateSprite();
            }
            if (Input.GetKeyDown(Key.LEFT))
            {
                currentColor = PlayerColor.ORANGE;
                UpdateSprite();
            }
            if (Input.GetKeyDown(Key.RIGHT))
            {
                currentColor = PlayerColor.PINK;
                UpdateSprite();
            }
            if (Input.GetKeyDown(Key.DOWN))
            {
                currentColor = PlayerColor.PURPLE;
                UpdateSprite();
            }
        }
        private void UpdateSprite()
        {
            string spriteString = "PLAYER_" + currentColor + ".png";
            this.initializeFromTexture(Texture2D.GetInstance(spriteString, false));
            initializeAnimFrames(spriteCols, spriteRows);
            Console.WriteLine("Player's color has changed to:" + currentColor);
        }
    }

}
