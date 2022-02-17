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
            _pData.SetLeaderBoard(this);

            input = new InputName(game, pData);
            highscore = new HighScore(game, pData);


            AddChild(input);
            AddChild(highscore);

            visible = false;
        }

        public void FixedUpdate()
        {
            if (!highscore.visible)
            {
                input.FixedUpdate();
            } 
            else
            {
                highscore.FixedUpdate();
            }
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
                letter.Text(alpha[i].ToString());
                letter.SetXY(letter.width * x - buttonFont.Size / 2, height / 2 + (letter.height * row) + difference * 2);
                buttons.Add(letter, alpha[i].ToString());
                AddChild(letter);

                x++;


            }
            _pData.SetButtons(buttons);

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

        private Button playAgain;
        private Sprite board;

        private List<PlayerData> playerDatas;
        private Dictionary<int, List<EasyDraw>> textRows = new Dictionary<int, List<EasyDraw>>();

        public HighScore(Game game, PlayerData pData) : base(game.width, game.height)
        {
            visible = false;
            _pData = pData;

            playerDatas = new List<PlayerData>();
            var array = File.ReadAllLines("LeaderBoard");
            for (var i = 0; i < array.Length; i++)
            {
                string[] values = array[i].Split(',');

                PlayerData p = new PlayerData(values[1], int.Parse(values[0]));
                p.SetTime(int.Parse(values[2]));
                playerDatas.Add(p);
            }

            normalFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 30);
            timeFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 20);
            CreateBoard();
        }

        public void FixedUpdate()
        {
            if (visible)
            {
                playAgain.alpha = 0.5f;
                UpdateBoard();
            }
        }

        public Button GetPlayAgain()
        {
            return playAgain;
        }

        private void CreateBoard()
        {
            board = new Sprite("Assets/HUD/lb.png");
            board.SetOrigin(board.width / 2, board.height / 2);
            board.SetXY(game.width / 2, game.height / 2);

            for (int i = 0; i < 5; i++)
            {
                List<EasyDraw> list = new List<EasyDraw>();

                EasyDraw name = new EasyDraw((int)normalFont.Size * 6, (int)normalFont.Size * 2);
                name.SetOrigin(name.width / 2, name.height / 2);
                name.TextFont(normalFont);
                name.Fill(Color.White);
                name.TextAlign(CenterMode.Center, CenterMode.Center);
                name.Text("MAT");
                name.SetXY(-name.width * 1.1f, -name.height * 1.95f + (name.height * i * 1.25f));
                board.AddChild(name);
                list.Add(name);

                EasyDraw score = new EasyDraw((int)normalFont.Size * 6, (int)normalFont.Size * 2);
                score.SetOrigin(score.width / 2, score.height / 2);
                score.TextFont(normalFont);
                score.Fill(Color.White);
                score.TextAlign(CenterMode.Center, CenterMode.Center);
                score.Text("9999");
                score.SetXY(score.width / 5.25f, -name.height * 1.95f + (name.height * i * 1.25f));
                board.AddChild(score);
                list.Add(score);

                EasyDraw time = new EasyDraw((int)normalFont.Size * 15, (int)normalFont.Size * 2);
                time.SetOrigin(name.width / 2, name.height / 2);
                time.TextFont(timeFont);
                time.Fill(Color.White);
                time.TextAlign(CenterMode.Center, CenterMode.Center);
                time.Text("00.00.00");
                time.SetXY(time.width / 2.9f, -name.height * 1.8f + (name.height * i * 1.25f));
                board.AddChild(time);
                list.Add(time);

                textRows.Add(i, list);
            }

            playAgain = new Button("again", "Assets/HUD/playagain.png");
            playAgain.SetOrigin(playAgain.width / 2, playAgain.height / 2);
            playAgain.SetXY(0, playAgain.height * 2.5f);
            Console.WriteLine(playAgain.y);
            board.AddChild(playAgain);
            AddChild(board);
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < playerDatas.Count; i++)
            {
                PlayerData p = playerDatas[i];
                List<EasyDraw> textToUpdate = textRows[i];


                textToUpdate[0].ClearTransparent();
                textToUpdate[0].Text(p.GetPlayerName());
                textToUpdate[1].ClearTransparent();
                textToUpdate[1].Text(p.GetScore().ToString());
                textToUpdate[2].ClearTransparent();
                textToUpdate[2].Text(TimeSpan.FromMilliseconds(p.GetTime()).ToString("mm\\.ss\\.ff"));
            }
        }


        public void UpdateResults()
        {
            PlayerData p = new PlayerData(_pData.GetPlayerName(), _pData.GetTime() / 250 + _pData.GetScore());
            p.SetTime(_pData.GetTime());
            playerDatas.Add(p);

            var sorted = playerDatas.OrderByDescending(x => x.GetScore()).ToList();

            while (sorted.Count > 5)
            {
                sorted.Remove(sorted.Last());
            }
            playerDatas = sorted;
            WriteToFile(playerDatas);
        }

        private void WriteToFile(List<PlayerData> list)
        {
            using (StreamWriter file = new StreamWriter("Leaderboard"))
                foreach (var entry in list)
                {
                    file.WriteLine("{0},{1},{2}", entry.GetScore(), entry.GetPlayerName(), entry.GetTime());
                }
        }
    }
}
