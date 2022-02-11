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

        private const int distance = 32;
        private List<Cookie> cookies;

        public CookieManager(Level level, PlayerData pData)
        {
            _pData = pData;
            _level = level;
            player = _level.GetPlayer();

            random = new Random();
            timerMilli = 0;
            spawnRate = maxSpawnRate;

            cookies = new List<Cookie>();
            CreateCookies();
        }

        public void Update()
        {
            CookieDecay();
            if (_pData.GetLifes() <= 0) return;
            CreateCookies();
        }

        private float GetSpawnRate()
        {
            spawnRate = CalculateSpawnRate();
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
                    if (c.GetColorIndex() <= 0)
                    {
                        if (_pData.GetLifes() > 0) _pData.DecreaseLifes();
                        cookies.Remove(c);
                        c.LateDestroy();
                        break;
                    }
                }
            }
        }

        private void CreateCookies()
        {
            if ((int)GetTimer() < GetSpawnRate()) return;
            SetTimer(0);

            Cookie cookie;

            while (true)
            {
                cookie = CreateCookie();
                bool good = (player.DistanceTo(cookie) > player.width * 2);

                GameObject[] collisions = cookie.GetCollisions();

                if (collisions.Length == 0 && good) break;

            }
            cookies.Add(cookie);
            AddChild(cookie);
        }

        private Cookie CreateCookie()
        {
            int rC = random.Next(0, 4);
            Array colors = Enum.GetValues(typeof(ObjectColor));
            float rX = random.Next(distance, game.width - distance);
            float rY = random.Next(distance, game.height - distance);

            ObjectColor randomColor = (ObjectColor)colors.GetValue(rC);

            Cookie cookie = new Cookie(rX, rY, "Assets/Cookie/" + randomColor + ".png", randomColor);
            return cookie;
        }

        /// <summary>
        /// Method for future spawn rate calculation depending on score.
        /// </summary>
        /// <returns></returns>
        private float CalculateSpawnRate()
        {
            return maxSpawnRate - (float) _pData.GetScore() / 10;
        }
    }
}
