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

        public HUD(Game game, PlayerData pData) : base(game.width, game.height, false)
        {
            _pData = pData;

            CreateScore();
        }

        void Update()
        {
            ScoreUpdate();
        }

        private void CreateScore()
        {
            scoreHolder = new EasyDraw(400, 100, false);
            scoreHolder.TextAlign(CenterMode.Center, CenterMode.Center);
            scoreHolder.Fill(Color.White);
            scoreHolder.TextSize(45);
            scoreHolder.Text("SCORE: " + _pData.GetScore());
            scoreHolder.SetXY(width / 2 - scoreHolder.width / 2, scoreHolder.height / 4);
            AddChild(scoreHolder);
        }

        private void ScoreUpdate()
        {
            scoreHolder.ClearTransparent();
            scoreHolder.Text("SCORE: " + _pData.GetScore());
        }
    }
}
