using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class CookieManager : GameObject
    {
        private PlayerData _pData;
        private Level _level;
        private Player player;
        private Random random;

        private float timerMilli;
        private int timerSec;

        private float spawnRate;
        private const float maxSpawnRate = 5;
        private const float minSpawnRate = 1;

        private const int distance = 32;
        private List<Cookie> cookies;

        private List<Cookie> currentPinkCookie = new List<Cookie>();
        private List<Cookie> currentPurpleCookie = new List<Cookie>();
        private List<Cookie> currentCyanCookie = new List<Cookie>();
        private List<Cookie> currentOrangeCookie = new List<Cookie>();
        public List<AnimationSprite> removeItems = new List<AnimationSprite>();
        public List<Hazard> currentHazards = new List<Hazard>();
        public List<PowerUp> currentPowerUps = new List<PowerUp>();


        public CookieManager(Level level, PlayerData pData)
        {
            _pData = pData;
            _level = level;
            player = _level.GetPlayer();

            random = new Random();
            timerMilli = 0;
            spawnRate = maxSpawnRate;
            

            cookies = new List<Cookie>();
            CreateObjects();
        }

        public void Update()
        {
            if(currentHazards.Count > 0)
            {
                foreach (Hazard hazard in currentHazards)
                {
                    hazard.Update();
                }
            }
            if (currentPowerUps.Count > 0)
            {
                foreach (PowerUp powerUp in currentPowerUps)
                {
                    powerUp.Update();
                }
            }
            if(removeItems.Count > 0)
            {
                foreach (AnimationSprite item in removeItems)
                {
                    if(item is PowerUp powerup)
                    {
                        currentPowerUps.Remove(powerup);
                    }
                    else if (item is Hazard hazard)
                    {
                        currentHazards.Remove(hazard);
                    }
                }
            }
            CookieDecay();
            if (_pData.GetLifes() <= 0) return;
            CreateObjects();
        }

        private float GetSpawnRate()
        {
            spawnRate = CalculateSpawnRate();
            if (spawnRate < minSpawnRate) spawnRate = minSpawnRate;
            return spawnRate;
        }

        private float GetTimer()
        {
            timerMilli += Time.deltaTime / 2;
            timerSec = (int) timerMilli / 1000;
            return timerSec;
        }

        private void SetTimer(int t)
        {
            timerMilli = t;
        }


        private void CookieDecay()
        {
            if (cookies != null && cookies.Count > 0)
            {
                foreach (Cookie c in cookies)
                {
                    c.SetAnimation();
                    if (Time.time > c.timeToDie)
                    {
                        if (_pData.GetLifes() > 0) _pData.DecreaseLifes();
                        RemoveCookieFromList(c);
                        c.Destroy();
                        break;
                    }
                    

                    /*
                    if (c.GetColorIndex() <= 0)
                    {
                        if (_pData.GetLifes() > 0) _pData.DecreaseLifes();
                        cookies.Remove(c);
                        c.LateDestroy();
                        break;
                    }
                    */
                }
            }
        }

        private void CreateObjects()
        {
            
            if ((int)GetTimer() < GetSpawnRate()) return;
            SetTimer(0);

            if (GetChildCount() > 60) return;

            Cookie cookie;
            while (true)
            {
                cookie = CreateCookie();
                bool good = (player.DistanceTo(cookie) > player.width * 2);

                GameObject[] collisions = cookie.GetCollisions(true, false);

                if (collisions.Length == 0 && good) break;

            }

            CreateHazard(); // spawns and creates a hazard
            cookies.Add(cookie);
            AddChild(cookie);
            if (_pData.GetTime() < 30000) return;
            CreatePowerUp(); //Spawns and creates a power up
        }

        private Cookie CreateCookie()
        {
            int rC = random.Next(0, 4);
            Array colors = Enum.GetValues(typeof(ObjectColor));
            float rX = random.Next(distance, game.width - distance);
            float rY = random.Next(distance, game.height - distance);

            ObjectColor randomColor = (ObjectColor)colors.GetValue(rC);

            Cookie cookie = new Cookie(rX, rY, "Assets/Cookie/" + randomColor + "3.png", randomColor , this);
            if(randomColor == ObjectColor.PURPLE)
            {
                currentPurpleCookie.Add(cookie);
            }
            if(randomColor == ObjectColor.PINK)
            {
                currentPinkCookie.Add(cookie);
            }
            if(randomColor == ObjectColor.CYAN)
            {
                currentCyanCookie.Add(cookie);
            }
            if(randomColor == ObjectColor.ORANGE)
            {
                currentOrangeCookie.Add(cookie);
            }
            return cookie;
        }


        private void CreatePowerUp()
        {
            int r = random.Next(0, 20);
            if (r != 1) return;

            float rX = random.Next(distance, game.width - distance);
            float rY = random.Next(distance, game.height - distance);

            PowerUp powerUp = new PowerUp(this, rX, rY);
            currentPowerUps.Add(powerUp);
            AddChild(powerUp);
        }
        private void CreateHazard()
        {
            var xValues = new List<int> { random.Next(-distance * 2, 0), random.Next(game.width), random.Next(game.width, game.width + distance * 2) };
            var yValues = new List<int> { random.Next(-distance * 2, 0), random.Next(game.height, game.height + distance * 2) };
            float rX = random.Next(random.Next(-distance * 4, -distance * 2), random.Next(game.width - distance));
            float rY = random.Next(distance, game.height - distance);

            //Need to fix so if it can't spawn in center (using while loop probably) TODO!!!
            Hazard hazard = new Hazard(this, xValues[random.Next(xValues.Count)], yValues[random.Next(yValues.Count)]);
            currentHazards.Add(hazard);
            AddChild(hazard);
        }

        public void RemoveCookieFromList(Cookie cookie)
        {
            cookies.Remove(cookie);
            //TODO: add the color typed list.
            if (cookie.cookieColor == ObjectColor.PURPLE)
            {
                currentPurpleCookie.Remove(cookie);
            }
            if (cookie.cookieColor == ObjectColor.PINK)
            {
                currentPinkCookie.Remove(cookie);
            }
            if (cookie.cookieColor == ObjectColor.CYAN)
            {
                currentCyanCookie.Remove(cookie);
            }
            if (cookie.cookieColor == ObjectColor.ORANGE)
            {
                currentOrangeCookie.Remove(cookie);
            }
        }

        public void ApplyPowerUp(ObjectColor playerColor)
        {
            new Sound("Assets/Sounds/powerup.wav").Play();
            List<Cookie> killCookieList = new List<Cookie>();

            if (playerColor == ObjectColor.PURPLE)
            {
                killCookieList = currentPurpleCookie;
            }
            if (playerColor == ObjectColor.PINK)
            {
                killCookieList = currentPinkCookie;
            }
            if (playerColor == ObjectColor.CYAN)
            {
                killCookieList = currentCyanCookie;
            }
            if (playerColor == ObjectColor.ORANGE)
            {
                killCookieList = currentOrangeCookie;
            }
            if (killCookieList != null)
            {
                List<Cookie> removeCookieList = new List<Cookie>();
                foreach (Cookie cookie in killCookieList)
                {
                    cookie.LateDestroy();
                    _pData.IncreaseScore(0);
                    removeCookieList.Add(cookie);
                    
                }
                foreach(Cookie cookie in removeCookieList)
                {
                    RemoveCookieFromList(cookie);
                }
            }
        }
        /// <summary>
        /// Method for future spawn rate calculation depending on score.
        /// </summary>
        /// <returns></returns>
        private float CalculateSpawnRate()
        {
            return maxSpawnRate - (float) (_pData.GetScore() / 100);
        }

        public void KillAllCookies()
        {
            foreach (Cookie c in cookies) {
                c.CookieDie();
             }
            foreach (AnimationSprite anim in currentPowerUps)
            {
                anim.LateDestroy();
            }
        }
    }
}
