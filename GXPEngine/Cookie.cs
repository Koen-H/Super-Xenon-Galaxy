using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Cookie : AnimationSprite
    {
        public ObjectColor cookieColor;
        private int decayState = 0;
        private float animationSpeed = 0.5f;
        private int timeOfBirth;
        //private float colorIndex;
        public float timeToDie;
        private float nextSpriteTime;
        private int coloms, rows;

        public CookieManager cookieManager;
        //private int decaySpeed;

        public Cookie(float x, float y, string path, ObjectColor _cookieColor , CookieManager _cookieManager) : base(path, 4, 4)
        {
            cookieColor = _cookieColor;
            cookieManager = _cookieManager;
            collider.isTrigger = true;
            SetOrigin(width / 2, height / 2);
            SetScaleXY(2);
            SetXY(x, y);

            //colorIndex = 1;
            timeOfBirth = Time.time;
            timeToDie = Time.time + 10000;//How long should a cookie live for? (in MS)

            nextSpriteTime = Time.time + 10;
           
        }

        void Update()
        {
            Turn(5);
            Animate(animationSpeed);
           // ColorUpdate();
        }
        
        public void SetAnimation()
        {
            float currentTime = Time.time;// the time
            if(currentTime > nextSpriteTime)
            {
                coloms = 5;
                rows = 2;
                switch (decayState)
                {
                    case 0://Spawn
                        coloms = 4;
                        rows = 4;
                        SetPath("Assets/Cookie/spawn/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 1;
                        nextSpriteTime = currentTime + 1000f;
                        animationSpeed = 0.27f;
                        break;

                    case 1://no
                        SetPath("Assets/Cookie/no/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 2;
                        nextSpriteTime = currentTime + 2000f;
                        animationSpeed = 0.135f;
                        break;

                    case 2://Low
                        SetPath("Assets/Cookie/small/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 3;
                        nextSpriteTime = currentTime + 2000f;
                        break;

                    case 3://Medium
                        SetPath("Assets/Cookie/medium/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 4;
                        nextSpriteTime = currentTime + 2000f;
                        break;

                    case 4://BIG
                        SetPath("Assets/Cookie/big/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 5;
                        nextSpriteTime = currentTime + 2000f;
                        break;

                    case 5:// DEATH
                        SetPath("Assets/Cookie/death/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 6;
                        nextSpriteTime = currentTime + 1000f;
                        break;
                }
            }
        }
        private void SetPath(string path)
        {
            initializeFromTexture(Texture2D.GetInstance(path, false));
            initializeAnimFrames(coloms, rows);
            SetOrigin(width / (scale * 2), height / (scale * 2));
        }


       /* public float GetColorIndex()
        {
            return colorIndex;
        }*/

       /* private void ColorUpdate()
        {
            colorIndex -= decaySpeed * 0.001f;
            SetColor(colorIndex, colorIndex, colorIndex);
        }*/

    }
}
