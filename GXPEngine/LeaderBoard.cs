using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class LeaderBoard : Canvas
    {
        private PlayerData _pData;
        private InputName input;
        private HighScore highscore;

        public LeaderBoard(Game game, PlayerData pData) : base(game.width, game.height)
        {
            _pData = pData;
            _pData.SetHighScore(this);

            input = new InputName(game, pData);
            highscore = new HighScore(game, pData);


            AddChild(input);
            AddChild(highscore);

            visible = false;
        }

        public void FixedUpdate()
        {

            input.FixedUpdate();
        }

        public HighScore GetHighScore()
        {
            return highscore;
        }
    }

    public class InputName : Canvas
    {
        private PlayerData _pData;

        private const string alpha = "ABCDEFGHIJKLMNOPQRSTUVQXYZ-";
        private Dictionary<EasyDraw, string> buttons;
        private Font buttonFont;
        private Font boardFont;

        EasyDraw score;
        EasyDraw time;
        EasyDraw result;

        private EasyDraw text;
        private EasyDraw textBox;
        private const int difference = 10;

        public InputName(Game game, PlayerData pData) : base(game.width, game.height)
        {
            _pData = pData;

            buttons = new Dictionary<EasyDraw, string>();
            buttonFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 50);
            boardFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 25);

            CreateButtons();
            CreateInputText();
            CreateBoard();
        }

        public void FixedUpdate()
        {
            foreach (EasyDraw e in buttons.Keys)
            {
                e.alpha = 0.5f;
            }
            UpdateInputText();
            UpdateBoard();
        }

        private void CreateButtons()
        {
            int x = 2;
            for (int i = 0; i < alpha.Length; i++)
            {
                if (x == 11 || x == 20) x = 2;
                int row = 0;
                if (i > 17)
                {
                    row = 2;
                }
                else if (i > 8)
                {
                    row = 1;
                }

                EasyDraw letter = new EasyDraw((int)buttonFont.Size * 3, (int)buttonFont.Size * 3);
                letter.ShapeAlign(CenterMode.Center, CenterMode.Center);
                letter.Stroke(Color.White);
                letter.StrokeWeight(7);
                letter.Fill(34, 1, 34);
                letter.Rect(letter.width / 2, letter.height / 2, letter.width, letter.height);
                letter.TextFont(buttonFont);
                letter.Fill(Color.White);
                letter.TextAlign(CenterMode.Center, CenterMode.Center);
                letter.Text("" + alpha[i]);
                letter.SetXY(letter.width * x - buttonFont.Size / 2, height / 2 + (letter.height * row) + difference * 2);
                buttons.Add(letter, alpha[i].ToString());
                AddChildAt(letter, 25 - i);

                x++;
                _pData.SetButtons(buttons);
            }
        }

        private void CreateInputText()
        {
            textBox = new EasyDraw((int)buttonFont.Size * 9, (int)buttonFont.Size * 3, false);
            textBox.ShapeAlign(CenterMode.Center, CenterMode.Center);
            textBox.Stroke(Color.White);
            textBox.StrokeWeight(7);
            textBox.Fill(34, 1, 34);
            textBox.Rect(textBox.width / 2, textBox.height / 2, textBox.width, textBox.height);
            textBox.SetXY(width / 2 - textBox.width / 2 - difference, height / 2 - textBox.height);
            AddChild(textBox);

            text = new EasyDraw((int)buttonFont.Size * 9, (int)buttonFont.Size * 3, false);
            text.TextFont(buttonFont);
            text.Fill(Color.White);
            text.TextAlign(CenterMode.Center, CenterMode.Center);
            text.Text(_pData.GetPlayerName());
            text.SetXY(width / 2 - text.width / 2 - difference, height / 2 - text.height);
            AddChild(text);
        }

        private void CreateBoard()
        {
            Sprite board = new Sprite("Assets/HUD/scorecalc.png");
            AddChild(board);

            score = new EasyDraw((int)boardFont.Size * 9, (int)boardFont.Size * 2);
            score.SetOrigin(score.width / 2, score.height / 2);
            score.TextFont(boardFont);
            score.Fill(Color.White);
            score.TextAlign(CenterMode.Center, CenterMode.Center);
            score.Text(_pData.GetScore().ToString());
            score.SetXY(score.width * 1.3f, board.height / 4 - boardFont.Size / 6);
            AddChild(score);
            
            time = new EasyDraw((int)boardFont.Size * 15, (int)boardFont.Size * 2);
            time.SetOrigin(time.width / 2, time.height / 2);
            time.TextFont(boardFont);
            time.Fill(Color.White);
            time.TextAlign(CenterMode.Center, CenterMode.Center);
            time.Text(TimeSpan.FromMilliseconds(_pData.GetTime()).ToString("mm\\.ss\\.ff"));
            time.SetXY(board.width / 2 - time.width / 2.1f, board.height / 4 - boardFont.Size / 6);
            AddChild(time);

            result = new EasyDraw((int)buttonFont.Size * 9, (int)buttonFont.Size * 2);
            result.SetOrigin(result.width / 2, result.height / 2);
            result.TextFont(buttonFont);
            result.Fill(Color.White);
            result.TextAlign(CenterMode.Center, CenterMode.Center);
            result.Text((_pData.GetTime() / 250 + _pData.GetScore()).ToString());
            result.SetXY(board.width - result.width * 1.08f, board.height / 4 + result.height / 3);
            AddChild(result);
        }

        private void UpdateInputText()
        {
            text.ClearTransparent();
            text.Text(_pData.GetPlayerName());
        }

        private void UpdateBoard()
        {
            score.ClearTransparent();
            time.ClearTransparent();
            result.ClearTransparent();
            score.Text(_pData.GetScore().ToString());
            time.Text(TimeSpan.FromMilliseconds(_pData.GetTime()).ToString("mm\\.ss\\.ff"));
            result.Text((_pData.GetTime() / 250 + _pData.GetScore()).ToString());
        }
    }

    public class HighScore : Canvas
    {
        private PlayerData _pData;
        private Font normalFont;
        private Font timeFont;

        private Sprite board;

        private Dictionary<int, string> highscores;

        public HighScore(Game game, PlayerData pData) : base(game.width, game.height)
        {
            visible = false;
            _pData = pData;

            highscores = new Dictionary<int, string>();
            var array = File.ReadAllLines("LeaderBoard");
            for (var i = 0; i < array.Length; i += 2)
            {
                highscores.Add(int.Parse(array[i]), array[i + 1]);
                Console.WriteLine("{0}\n{1}", array[i], array[i + 1]);
            }

            //var list = highscores.Keys.ToList();
            //list.Sort();
            //var sorted = new Dictionary<int, string>();
            //foreach (var key in list)
            //{
            //    sorted.Add(key, highscores[key]);
            //}

            normalFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 30);
            timeFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 20);
            CreateBoard();
        }

        private void CreateBoard()
        {
            board = new Sprite("Assets/HUD/lbgrid.png");
            board.SetOrigin(board.width / 2, board.height / 2);
            board.SetXY(game.width / 2, game.height / 2);

            for (int i = 0; i < 5; i++)
            {
                Pivot row = new Pivot();

                EasyDraw name = new EasyDraw((int)normalFont.Size * 6, (int)normalFont.Size * 2);
                name.SetOrigin(name.width / 2, name.height / 2);
                name.TextFont(normalFont);
                name.Fill(Color.White);
                name.TextAlign(CenterMode.Center, CenterMode.Center);
                name.Text("MAT");
                name.SetXY(-name.width * 1.1f, -name.height * 1.95f + (name.height * i * 1.25f));
                row.AddChild(name);

                EasyDraw score = new EasyDraw((int)normalFont.Size * 6, (int)normalFont.Size * 2);
                score.SetOrigin(score.width / 2, score.height / 2);
                score.TextFont(normalFont);
                score.Fill(Color.White);
                score.TextAlign(CenterMode.Center, CenterMode.Center);
                score.Text("9999");
                score.SetXY(score.width / 5.25f, -name.height * 1.95f + (name.height * i * 1.25f));
                row.AddChild(score);

                EasyDraw time = new EasyDraw((int)normalFont.Size * 15, (int)normalFont.Size * 2);
                time.SetOrigin(name.width / 2, name.height / 2);
                time.TextFont(timeFont);
                time.Fill(Color.White);
                time.TextAlign(CenterMode.Center, CenterMode.Center);
                time.Text("00.00.00");
                time.SetXY(time.width / 2.9f, -name.height * 1.8f + (name.height * i * 1.25f));
                row.AddChild(time);

                board.AddChild(row);
            }
            AddChild(board);
        }

        private void UpdateBoard()
        {
            //for (int i = 0; i < highscores.Count; i++)
            //{
            //    for (int k = 0; k < 3; k++)
            //    {
            //        boar
            //    }
            //}
        }

        public void UpdateResults()
        {
            highscores.Add(_pData.GetTime() / 250 + _pData.GetScore()
                , string.Format("{0};{1}", _pData.GetPlayerName() , TimeSpan.FromMilliseconds(_pData.GetTime()).ToString("mm\\.ss\\.ff")));

            var sorted = highscores.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            while (sorted.Count > 5)
            {
                sorted.Remove(sorted.Keys.Last());
            }
            highscores = sorted;
            WriteToFile(highscores);
        }

        private void WriteToFile(Dictionary<int, string> dict)
        {
            using (StreamWriter file = new StreamWriter("Leaderboard"))
                foreach (var entry in dict)
                    file.WriteLine("{0}\n{1}", entry.Key, entry.Value);
        }
    }
}
