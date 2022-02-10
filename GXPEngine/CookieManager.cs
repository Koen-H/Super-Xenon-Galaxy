﻿using System;
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

        private const int distance = 32;
        private List<Cookie> cookies;

        public CookieManager(Level level, PlayerData pData)
        {
            _pData = pData;
            _level = level;
            player = _level.GetPlayer();

            random = new Random();
            timerMilli = 0;
            spawnRate = 1;

            cookies = new List<Cookie>();
            CreateCookies();
        }

        void Update()
        {
            //Console.WriteLine(GetTimer());
            CreateCookies();
            Console.WriteLine(spawnRate);
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

        private void CreateCookies()
        {
            if ((int)GetTimer() < GetSpawnRate()) return;
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
            return 2 - (float) _pData.GetScore() / 100;
        }
    }
}
