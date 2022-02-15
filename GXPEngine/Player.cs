using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
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


        private const int spriteCols = 7;
        private const int spriteRows = 3;
        private ObjectColor currentColor;

        private ArduinoController gameController;

        private float speed;
        public Player(Vector2 pos, PlayerData pData) : base("Assets/Player/Body/pink.png", spriteCols, spriteRows)
        {
            _pData = pData;
            x = pos.x;
            y = pos.y;
            //speed = 180;

            currentColor = ObjectColor.PINK;    //The default color for the player
            UpdateSprite();

            SetOrigin(width / 2, height / 2);
            SetScaleXY(2f);

            collider.isTrigger = true;

            MyGame myGame = (MyGame)game;
            if(myGame.gameController != null) gameController = myGame.gameController;
        }

        public void FixedUpdate()
        {
            Animate(0.5f);
            Move();
            ChangeColor();
            PressSpace();
        }

        private void Move()
        {
            speed = 0;
            
            if (rotation > 360) rotation -= 360;

            if (Input.GetKey(Key.A) && !Input.GetKey(Key.D))
            {
                rotation -= 3;
            }

            else if (Input.GetKey(Key.D) && !Input.GetKey(Key.A))
            {
                rotation += 3;
            }

            if (Input.GetKey(Key.W) && !Input.GetKey(Key.S))
            {
                speed = 5 * 60;
            }

            else if (Input.GetKey(Key.S) && !Input.GetKey(Key.W))
            {
                
            }

            if (gameController != null)
            {
                rotation = gameController.analogRotation;
                speed = (float)Math.Floor(gameController.analogForce / 10);
                speed *= 1.5f;
            }

            float v = -speed * Time.deltaTime / 1000;

            Move(0, v);

            

            //Edge control
            if (x < -width / 2) x = game.width + width / 2;
            if (x > game.width) x = -width / 2;
            if (y < -height / 2) y = game.height + height / 2;
            if (y > game.height + height / 2) y = -height / 2;
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
            string spriteString = "Assets/Player/Body/" + currentColor.ToString().ToLower() + ".png";
            initializeFromTexture(Texture2D.GetInstance(spriteString, false));
            initializeAnimFrames(spriteCols, spriteRows);

            SetOrigin(width / (scale * 2), height / (scale * 2));

            //Console.WriteLine("Player's color has changed to:" + currentColor);
        }

        private void PressSpace()
        {
            if (Input.GetKeyDown(Key.SPACE) && !pressSpace)
            {
                pressSpace = true;

                GameObject[] collisions = GetCollisions();//Get all the collisions
                foreach (GameObject collision in collisions)
                {
                    if (collision is Cookie cookie)//If one of them is a cookie
                    {
                        if (cookie.cookieColor == currentColor)//Check if the cookie is the same color as the player
                        {
                            _pData.IncreaseScore();
                            cookie.Destroy();//DESTROY THE COOKIE!
                        }
                    }

                    if (collision is EasyDraw button)
                    {
                        _pData.ChangeName(_pData.GetButtons()[button]);
                        Console.WriteLine(_pData.GetButtons()[button]);
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
