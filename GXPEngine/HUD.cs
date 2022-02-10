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
        private EasyDraw scoreHolder;
        private int score;

        public HUD(Game game, PlayerData pData) : base(game.width, game.height, false)
        {
            _pData = pData;
            score = _pData.GetScore();

            CreateScore();
        }

        void Update()
        {
            ScoreUpdate();
        }

        private void CreateScore()
        {
            scoreHolder = new EasyDraw(640, 360, false);
            scoreHolder.TextAlign(CenterMode.Min, CenterMode.Center);
            scoreHolder.Fill(Color.White);
            scoreHolder.TextSize(30);
            scoreHolder.Text("SCORE: " + scoreHolder);
            scoreHolder.SetXY(width / 2, scoreHolder.height / 2);
            //score.alpha = 0;
            AddChild(scoreHolder);
        }

        private void ScoreUpdate()
        {
            scoreHolder.ClearTransparent();
            scoreHolder.Text("SCORE: " + score);
        }
    }
}
