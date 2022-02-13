using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class HUD : Canvas
    {
        private PlayerData _pData;

        private EasyDraw score;
        private EasyDraw lifes;
        private EasyDraw timer;

        private TimeSpan time;

        public HUD(Game game, PlayerData pData) : base(game.width, game.height, false)
        {
            _pData = pData;
            time = TimeSpan.FromMilliseconds(Time.time);
            CreateHUD();
        }

        public void Update()
        {
            ScoreUpdate();
            LifesUpdate();
            //TimerUpdate();
        }

        private void CreateHUD()
        {
            CreateScore();
            CreateLifes();
            CreateTimer();
        }

        private void CreateScore()
        {
            score = new EasyDraw(400, 100, false);
            score.TextAlign(CenterMode.Center, CenterMode.Center);
            score.Fill(Color.White);
            score.TextSize(45);
            score.Text("SCORE: " + _pData.GetScore());
            score.SetXY(width / 2 - score.width / 2, score.height / 4);
            AddChild(score);
        }

        private void CreateLifes()
        {
            lifes = new EasyDraw(400, 100, false);
            lifes.TextAlign(CenterMode.Min, CenterMode.Center);
            lifes.Fill(Color.White);
            lifes.TextSize(45);
            lifes.Text("LIFES: " + _pData.GetLifes());
            lifes.SetXY(lifes.width / 4, lifes.height / 4);
            AddChild(lifes);
        }

        private void CreateTimer()
        {
            timer = new EasyDraw(500, 100, false);

            timer.TextAlign(CenterMode.Min, CenterMode.Center);
            timer.Fill(Color.White);
            timer.TextSize(45);
            timer.Text(("" + TimeSpan.FromMilliseconds(Time.time)).Substring(1, 11));
            timer.SetXY(width - timer.width, timer.height / 4);
            AddChild(timer);
        }

        private void ScoreUpdate()
        {
            score.ClearTransparent();
            score.Text("SCORE: " + _pData.GetScore());
        }

        private void LifesUpdate()
        {
            lifes.ClearTransparent();
            lifes.Text("LIFES: " + _pData.GetLifes());
        }

        private void TimerUpdate()
        {
            timer.ClearTransparent();
            string temp = TimeSpan.FromMilliseconds(Time.time).ToString();
            string text = ("" + TimeSpan.FromMilliseconds(Time.time));
            timer.Text(temp.Substring(1, 10));
            Console.WriteLine(temp.Length);
        }
    }
}
