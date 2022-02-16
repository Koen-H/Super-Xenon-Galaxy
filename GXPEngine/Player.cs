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
        private PlayerBody bodyAnimation;
        private PlayerTail tailAnimation;

        private bool pressUp;
        private bool pressDown;
        private bool pressLeft;
        private bool pressRight;
        private bool pressSpace;

        private const int spriteCols = 7;
        private const int spriteRows = 3;
        private ObjectColor currentColor;

        private ArduinoController gameController;

        private ObjectColor lastColor;
        private float combo;

        private float speedBoostStage;
        private float speedBoost;
        private float speedBoostInterval;

        private float speed;
        public Player(float x, float y, PlayerData pData) : base("square.png", 1, 1)
        {
            combo = 0;
            _pData = pData;
            this.x = x;
            this.y = y;

            bodyAnimation = new PlayerBody();
            tailAnimation = new PlayerTail(this);
            currentColor = ObjectColor.PINK;    //The default color for the player
            UpdateSprite();

            SetOrigin(width / 2, height / 2);
            SetScaleXY(0.5f);

            collider.isTrigger = true;
            alpha = 0;

            MyGame myGame = (MyGame)game;
            if(myGame.gameController != null) gameController = myGame.gameController;

            AddChild(bodyAnimation);
            AddChild(tailAnimation);
        }

        public void FixedUpdate()
        {
            bodyAnimation.Update();
            tailAnimation.Update();
            Move();
            ChangeColor();
            PressSpace();
        }

        public float GetSpeed()
        {
            if (speedBoostStage > 0 && speed > 0)//apply buff from speedchain
            {
                speed += speedBoost;
                if (Time.time > speedBoostInterval)
                {
                    speedBoostStage--;
                    SetSpeedBoost();

                }
            }
            return speed * -1;
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
                speed = 5 * 40;
            }

            else if (Input.GetKey(Key.S) && !Input.GetKey(Key.W))
            {
                
            }

            if (gameController != null)
            {
                rotation = gameController.analogRotation;
                speed = (float)Math.Floor(gameController.analogForce / 10) * 40;
                //speed *= 1.5f;
            }



            if (speedBoostStage > 0 && speed > 0)//apply buff from speedchain
            {
                speed += speedBoost;
                if (Time.time > speedBoostInterval)
                {
                    speedBoostStage--;
                    SetSpeedBoost();

                }
            }


            float v = -speed * Time.deltaTime / 1000;


            Move(0, v);

            //Edge control
            if (x < -bodyAnimation.width / 4) x = game.width + bodyAnimation.width / 4;
            if (x > game.width + bodyAnimation.width / 4) x = -bodyAnimation.width / 4;
            if (y < _pData.GetHudHeight() -bodyAnimation.height / 4) y = game.height + bodyAnimation.height / 4;
            if (y > game.height + bodyAnimation.height / 4) y = _pData.GetHudHeight() - bodyAnimation.height / 4;
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
            bodyAnimation.UpdateSprite(currentColor.ToString().ToLower());
            tailAnimation.UpdateSprite(currentColor.ToString().ToLower());
            //string spriteString = "Assets/Player/Body/" + currentColor.ToString().ToLower() + ".png";
            //initializeFromTexture(Texture2D.GetInstance(spriteString, false));
            //initializeAnimFrames(spriteCols, spriteRows);

            //SetOrigin(width / (scale * 2), height / (scale * 2));

            if (gameController != null)
            {
                gameController.ChangeLight(currentColor);
            }
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
                            cookie.cookieManager.RemoveCookieFromList(cookie);
                            new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound chain 1
                            cookie.Destroy();//DESTROY THE COOKIE!

                            if (cookie.cookieColor == lastColor)
                            {
                                combo += 1f;
                            }
                            else
                            {
                                combo = 0;
                            }

                            _pData.IncreaseScore(combo);
                            lastColor = cookie.cookieColor;
                        }
                        speedBoostStage += 1;
                        SetSpeedBoost();
                    }

                    if (collision is EasyDraw button && _pData.isButtonActive())
                    {
                        _pData.ChangeName(_pData.GetButtons()[button]);
                    }

                }
            }

            if (Input.GetKeyUp(Key.SPACE))
            {
                pressSpace = false;
            }
        }

        private void SetSpeedBoost()
        {
            switch (speedBoostStage)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound chain 2
                        speedBoostInterval = Time.time + 1500f;
                        speedBoost = 260;
                        break;
                    }
                case 2:
                    {
                        new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound chain 3
                        speedBoostInterval = Time.time + 500f;
                        speedBoost = 338;
                        break;
                    }
                case 3:
                    {
                        new Sound("Assets/Sounds/wolf growl.wav").Play();//should be sound chain 4
                        speedBoostInterval = Time.time + 250f;
                        speedBoost = 440;
                        break;
                    }
            }
        }

        void OnCollision(GameObject other)
        {
            if (other is EasyDraw button)
            {
                button.alpha = 1f;
            }
        }
    }

}
