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

        public HUD(Game game, PlayerData pData) : base(game.width, game.height, false)
        {
            _pData = pData;

            CreateHUD();
        }

        public void Update()
        {
            ScoreUpdate();
            LifesUpdate();
        }

        private void CreateHUD()
        {
            CreateScore();
            CreateLifes();
            //CreateTimer();
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
    }
}
