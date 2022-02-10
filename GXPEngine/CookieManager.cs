using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class CookieManager : GameObject
    {
        private Level _level;
        private Player player;
        private Random random;

        private float timerMilli;
        private int timerSec;
        private float spawnRate;

        private const int distance = 32;
        private List<Cookie> cookies;
        private List<string> cookiePaths;

        public CookieManager(Level level)
        {
            _level = level;
            player = _level.GetPlayer();

            random = new Random();
            timerMilli = 0;
            spawnRate = 1;

            cookies = new List<Cookie>();
            cookiePaths = new List<string> {"circle.png", "square.png", "checkers.png", "colors.png" };
            CreateCookies();
        }

        void Update()
        {
            Console.WriteLine(GetTimer());
            CreateCookies();
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

        private void CreateCookies()
        {
            if ((int)GetTimer() < spawnRate) return;
            SetTimer(0);

            Cookie cookie;

            while (true)
            {
                cookie = CreateCookie();
                if (player.DistanceTo(cookie) > player.width * 2)
                {
                    break;
                }
            }
            
            cookies.Add(cookie);
            AddChild(cookie);
        }

        private Cookie CreateCookie()
        {
            int rC = random.Next(0, 4);
            float rX = random.Next(distance, game.width - distance);
            float rY = random.Next(distance, game.height - distance);

            Cookie cookie = new Cookie(rX, rY, cookiePaths[rC]);
            return cookie;
        }

        /// <summary>
        /// Method for future spawn rate calculation depending on score.
        /// </summary>
        /// <returns></returns>
        private float CurrentSpawnSpeed()
        {
            return 0;
        }
    }
}
