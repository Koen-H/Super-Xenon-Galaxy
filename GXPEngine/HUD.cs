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

        private Sprite hudBoard;
        private EasyDraw score;
        private EasyDraw timer;
        private List<AnimationSprite> hearts;
        private Sprite gameOver;

        private const int goLimit = 2000;
        public static int goStart;
        private int goTimer;

        private int time;

        private Font font;

        public HUD(Game game, PlayerData pData) : base(game.width, game.height)
        {
            _pData = pData;
            time = Time.time;
            font = Utils.LoadFont("Assets/HUD/ka1.ttf", 30);
            CreateHUD();
        }

        public void Update()
        {
            LifesUpdate();
            if (_pData.GetLifes() == 0)
            {
                GameOverUpdate();
            }
            if (_pData.GetLifes() > 0)
            {
                ScoreUpdate();
                TimerUpdate();
            }
        }

        public Sprite GetGameOver()
        {
            return gameOver;
        }

        public int GetGameOverTime()
        {
            return time;
        }

        public void SetGameOverTime(int t)
        {
            time = t;
        }

        private void CreateHUD()
        {
            CreateHudBoard();
            CreateScore();
            CreateLifes();
            CreateTimer();
            CreateGameOver();
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

            hearts = new List<AnimationSprite>();
            for (int i = 0; i < _pData.GetLifes(); i++)
            {
                AnimationSprite heart = new AnimationSprite("Assets/HUD/heart.png", 1, 1);
                //heart.SetOrigin(width / 2, height / 2);
                //heart.SetXY(game.width / 2, hudBoard.height / 2);
                heart.SetXY(heart.width * 1.25f + ((heart.width - 20) * i), 1);
                hearts.Add(heart);
                AddChild(heart);

                Console.WriteLine(heart.y);

            }
        }

        private void CreateTimer()
        {
            timer = new EasyDraw(500, 100, false);

            timer.TextAlign(CenterMode.Min, CenterMode.Min);
            timer.Fill(Color.White);
            timer.TextFont(font);
            timer.Text((TimeSpan.FromMilliseconds(Time.time - time)).ToString("mm\\.ss\\.ff"));
            timer.SetXY(width - timer.width, timer.height / 5);
            AddChild(timer);
        }
        private void CreateGameOver()
        {
            gameOver = new Sprite("Assets/HUD/gameover.png");
            gameOver.SetOrigin(gameOver.width / 2, gameOver.height / 2);
            gameOver.SetXY(game.width / 2, game.height / 3);
            gameOver.visible = false;
            AddChild(gameOver);
        }


        private void ScoreUpdate()
        {
            score.ClearTransparent();
            score.Text("SCORE: " + _pData.GetScore());
        }

        private void LifesUpdate()
        {
            if (hearts.Count > 0 && hearts.Count > _pData.GetLifes())
            {
                hearts[hearts.Count - 1].LateRemove();
                hearts.RemoveAt(hearts.Count - 1);
            }
        }

        private void TimerUpdate()
        {
            timer.ClearTransparent();
            string temp = (TimeSpan.FromMilliseconds(Time.time - time)).ToString("mm\\.ss\\.ff");
            _pData.SetTime(Time.time - time);
            timer.Text(temp);
        }
        private void GameOverUpdate()
        {
            if (goTimer <= goLimit)
            {
                gameOver.visible = true;
                goTimer = Time.time - goStart;
            }
            else
            {
                gameOver.visible = false;
            }
        }
    }
}
