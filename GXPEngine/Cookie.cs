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
        private float turnSpeed = 5;
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
            timeToDie = Time.time + 15300;//How long should a cookie live for? (in MS)

            nextSpriteTime = Time.time + 10;
           
        }

        void Update()
        {
            Turn(turnSpeed);
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
                        nextSpriteTime = currentTime + 250f;
                        animationSpeed = 0.75f;
                        turnSpeed = 2;
                        break;

                    case 1://no
                        SetPath("Assets/Cookie/no/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 2;
                        nextSpriteTime = currentTime + 2750f;
                        animationSpeed = 0.135f;
                        turnSpeed = 3f;
                        break;

                    case 2://Low
                        SetPath("Assets/Cookie/small/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 3;
                        nextSpriteTime = currentTime + 3800f;
                        turnSpeed = 6;
                        break;

                    case 3://Medium
                        SetPath("Assets/Cookie/medium/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 4;
                        nextSpriteTime = currentTime + 3800f;
                        turnSpeed = 10;
                        break;

                    case 4://BIG
                        SetPath("Assets/Cookie/big/" + cookieColor.ToString().ToLower() + ".png");
                        new Sound("Assets/Sounds/cookie death.wav").Play();
                        decayState = 5;
                        nextSpriteTime = currentTime + 3200f;
                        turnSpeed = 15f;
                        break;

                    case 5:// DEATH
                        SetPath("Assets/Cookie/death/" + cookieColor.ToString().ToLower() + ".png");
                        decayState = 6;
                        nextSpriteTime = currentTime + 1500f;
                        turnSpeed = 17;
                        animationSpeed = 0.1f;
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
        public void CookieDie()
        {
            SetPath("Assets/Cookie/death/" + cookieColor.ToString().ToLower() + ".png");
            decayState = 6;
            turnSpeed = 17;
            animationSpeed = 0.1f;
            timeToDie = Time.time + 1500f;
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
