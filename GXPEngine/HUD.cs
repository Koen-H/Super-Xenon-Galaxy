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
        private InputName input;

        private Sprite hudBoard;
        private EasyDraw score;
        private EasyDraw timer;
        private EasyDraw lifes;
        private List<AnimationSprite> hearts;

        private TimeSpan time;

        private Font font;

        public HUD(Game game, PlayerData pData) : base(game.width, game.height)
        {
            _pData = pData;
            time = TimeSpan.FromMilliseconds(Time.time);
            font = Utils.LoadFont("Assets/HUD/ka1.ttf", 30);
            CreateHUD();
        }

        public void Update()
        {
            LifesUpdate();
            if (_pData.GetLifes() > 0)
            {
                ScoreUpdate();
                TimerUpdate();
            }
        }

        private void CreateHUD()
        {
            CreateHudBoard();
            CreateScore();
            CreateLifes();
            CreateTimer();
        }

        private void CreateHudBoard()
        {
            hudBoard = new Sprite("Assets/HUD/top.png");
            hudBoard.collider.isTrigger = true;
            AddChild(hudBoard);

            _pData.SetHudHeight(hudBoard.height);
        }

        private void CreateScore()
        {
            score = new EasyDraw(400, 100, false);
            score.TextAlign(CenterMode.Center, CenterMode.Min);
            score.Fill(Color.White);
            score.TextFont(font);
            score.Text("SCORE: " + _pData.GetScore());
            score.SetXY(width / 2 - score.width / 2, score.height / 5);
            AddChild(score);
        }

        private void CreateLifes()
        {
            lifes = new EasyDraw(400, 100, false);
            lifes.TextAlign(CenterMode.Center, CenterMode.Min);
            lifes.Fill(Color.White);
            lifes.TextFont(font);
            lifes.Text("LIFES: " + _pData.GetLifes());
            lifes.SetXY(lifes.width / 4, lifes.height / 4);
            //AddChild(lifes);

            hearts = new List<AnimationSprite>();
            for (int i = 0; i < _pData.GetLifes(); i++)
            {
                AnimationSprite heart = new AnimationSprite("Assets/HUD/heart.png", 1, 1);
                heart.SetScaleXY(0.1f);
                heart.SetXY(125 + ((heart.width + 10) * i), heart.height / 2.75f);
                hearts.Add(heart);
                AddChild(heart);
            }
        }

        private void CreateTimer()
        {
            timer = new EasyDraw(500, 100, false);

            timer.TextAlign(CenterMode.Min, CenterMode.Min);
            timer.Fill(Color.White);
            timer.TextFont(font);
            timer.Text((TimeSpan.FromMilliseconds(Time.time) - time).ToString("h\\.mm\\.ss\\.ff"));
            timer.SetXY(width - timer.width * 1.1f, timer.height / 5);
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

            if (hearts.Count > 0 && hearts.Count > _pData.GetLifes())
            {
                hearts[hearts.Count - 1].LateRemove();
                hearts.RemoveAt(hearts.Count - 1);
            }
        }

        private void TimerUpdate()
        {
            timer.ClearTransparent();
            string temp = (TimeSpan.FromMilliseconds(Time.time) - time).ToString("h\\.mm\\.ss\\.ff");
            timer.Text(temp);
        }
    }
}
