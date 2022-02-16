using System;
using System.Collections.Generic;
using System.Drawing;
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


            input = new InputName(game, pData);
            highscore = new HighScore(game, pData);

            AddChild(input);
            AddChild(input);
        }

        public void FixedUpdate()
        {
            input.FixedUpdate();
        }

    }

    public class InputName : Canvas
    {
        private PlayerData _pData;

        private const string alpha = "ABCDEFGHIJKLMNOPQRSTUVQXYZ-";
        private Dictionary<EasyDraw, string> buttons;
        private Font buttonFont;
        private Font boardFont;

        private EasyDraw text;
        private EasyDraw textBox;
        private const int difference = 10;

        public InputName(Game game, PlayerData pData) : base(game.width, game.height)
        {
            _pData = pData;

            buttons = new Dictionary<EasyDraw, string>();
            buttonFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 50);
            boardFont = Utils.LoadFont("Assets/HUD/ka1.ttf", 30);

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

            EasyDraw score = new EasyDraw((int)boardFont.Size * 9, (int)boardFont.Size * 3);
            score.TextFont(boardFont);
            score.Fill(Color.White);
            score.TextAlign(CenterMode.Center, CenterMode.Center);
            score.Text(_pData.GetPlayerName());
            score.SetXY(width / 2 - text.width / 2 - difference, height / 2 - text.height);
            
            EasyDraw time = new EasyDraw((int)boardFont.Size * 9, (int)boardFont.Size * 3);
            time.TextFont(boardFont);
            time.Fill(Color.White);
            time.TextAlign(CenterMode.Center, CenterMode.Center);
            time.Text(_pData.GetPlayerName());
            time.SetXY(width / 2 - text.width / 2 - difference, height / 2 - text.height);

            EasyDraw result = new EasyDraw((int)buttonFont.Size * 9, (int)buttonFont.Size * 3);
            result.TextFont(buttonFont);
            result.Fill(Color.White);
            result.TextAlign(CenterMode.Center, CenterMode.Center);
            result.Text(_pData.GetPlayerName());
            result.SetXY(width / 2 - text.width / 2 - difference, height / 2 - text.height);
        }

        private void UpdateInputText()
        {
            text.ClearTransparent();
            text.Text(_pData.GetPlayerName());
        }
    }

    public class HighScore : Canvas
    {
        private PlayerData _pData;

        private Font font;

        public HighScore(Game game, PlayerData pData) : base(game.width, game.height)
        {
            _pData = pData;

            font = Utils.LoadFont("Assets/HUD/ka1.ttf", 50);

        }
    }
}
