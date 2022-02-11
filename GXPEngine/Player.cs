using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{

    public enum ObjectColor
    {
        PINK, PURPLE, ORANGE, CYAN
    }

    public class Player : AnimationSprite
    {
        private PlayerData _pData;

        private bool pressUp;
        private bool pressDown;
        private bool pressLeft;
        private bool pressRight;
        private bool pressSpace;

        private Level _level;
        private float speedX;
        private float speedY;

        private const int spriteCols = 1;
        private const int spriteRows = 1;
        private ObjectColor currentColor;

        private float speed;
        public Player(Level level, Vector2 pos, PlayerData pData) : base("Assets/Player/pink.png", spriteCols, spriteRows)
        {
            _pData = pData;

            x = pos.x;
            y = pos.y;

            speedX = 0;
            speedY = 0;
            speed = 180;

            currentColor = ObjectColor.PINK;    //The default color for the player
            UpdateSprite();

            SetOrigin(width / 2, height / 2);
            SetScaleXY(4);

            _level = level;

            collider.isTrigger = true;
        }

        public void Update()
        {
            Animate(0.05f);
            Move();
            ChangeColor();
            EatCookie();
        }

        private void Move()
        {
            speedX = 0;
            speedY = 0;

            if (rotation > 360) rotation -= 360;

            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D))
            {
                //SetOrigin(0, height);
                //rotation--;
                //speedY = 0;

                speedX = -speed;
                rotation = 270;

                //if (rotation > 270) rotation = 270;
                //else rotation += 5;
            }

            else if (Input.GetKey(Key.D) && !Input.GetKey(Key.A))
            {
                //SetOrigin(width, height);
                //rotation++;
                //speedY = 0;

                speedX = speed;
                rotation = 90;

                //if (rotation > 90) rotation = 90;
                //else rotation += 5;
            }

            if (Input.GetKey(Key.W) && !Input.GetKey(Key.S))
            {
                speedY = -speed;
                rotation = 0;

                //if (rotation > 0) rotation = 0;
                //else rotation += 5;

            }

            else if (Input.GetKey(Key.S) && !Input.GetKey(Key.W))
            {
                speedY = speed;
                rotation = 180;

                //if (rotation > 180) rotation = 180;
                //else rotation += 5;
            }

            float vx = speedX * Time.deltaTime / 1000;
            float vy = speedY * Time.deltaTime / 1000;


            if (MoveUntilCollision(vx, 0) != null)
            {
                //speedX = 0;
            }
            if (MoveUntilCollision(0, vy) != null)
            {
                //speedY = 0;
            }


            //Edge control
            if (x < width / 2) x = width / 2;
            if (x > game.width - width / 2) x = game.width - width / 2;
            if (y < height / 2) y = height / 2;
            if (y > game.height - height / 2) y = game.height - height / 2;
        }
        
        private void ChangeColor()
        {
            if (Input.GetKeyDown(Key.UP) && !pressUp)
            {
                pressUp = true;
                currentColor = ObjectColor.CYAN;
                UpdateSprite();
            }
            if (Input.GetKeyDown(Key.LEFT) && !pressLeft)
            {
                pressLeft = true;
                currentColor = ObjectColor.ORANGE;
                UpdateSprite();
            }
            if (Input.GetKeyDown(Key.RIGHT) && !pressRight)
            {
                pressRight = true;
                currentColor = ObjectColor.PINK;
                UpdateSprite();
            }
            if (Input.GetKeyDown(Key.DOWN) && !pressDown)
            {
                pressDown = true;
                currentColor = ObjectColor.PURPLE;
                UpdateSprite();
            }

            if (Input.GetKeyUp(Key.UP))
            {
                pressUp = false;

            }
            if (Input.GetKeyUp(Key.LEFT))
            {
                pressLeft = false;

            }
            if (Input.GetKeyUp(Key.RIGHT))
            {
                pressRight = false;

            }
            if (Input.GetKeyUp(Key.DOWN))
            {
                pressDown = false;
            }
        }

        private void UpdateSprite()
        {
            string spriteString = "Assets/Player/" + currentColor.ToString().ToLower() + ".png";
            this.initializeFromTexture(Texture2D.GetInstance(spriteString, false));
            initializeAnimFrames(spriteCols, spriteRows);

            SetOrigin(width / 8, height / 8);

            //Console.WriteLine("Player's color has changed to:" + currentColor);
        }

        private void EatCookie()
        {
            if (Input.GetKeyDown(Key.SPACE) && !pressSpace)
            {
                pressSpace = true;

                GameObject[] collisions = GetCollisions();//Get all the collisions
                foreach (GameObject collision in collisions)
                {
                    if (collision is Cookie)//If one of them is a cookie
                    {
                        Cookie cookie = (Cookie)collision;
                        if (cookie.cookieColor == currentColor)//Check if the cookie is the same color as the player
                        {
                            _pData.IncreaseScore();
                            cookie.Destroy();//DESTROY THE COOKIE!
                        }
                    }

                }
            }

            if (Input.GetKeyUp(Key.SPACE))
            {
                pressSpace = false;
            }
        }
    }

}
