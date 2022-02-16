using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class InputName : Canvas
    {
        private PlayerData _pData;

        private const string alpha = "ABCDEFGHIJKLMNOPQRSTUVQXYZ-";

        private Dictionary<EasyDraw, string> buttons;
        private Font font;

        private EasyDraw text;
        private EasyDraw textBox;
        private const int difference = 10;

        public InputName(Game game, PlayerData pData) : base(game.width, game.height)
        {
            _pData = pData;

            buttons = new Dictionary<EasyDraw, string>();
            font = Utils.LoadFont("Assets/HUD/ka1.ttf", 50);

            CreateButtons();
            CreateInputText();
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

                EasyDraw letter = new EasyDraw((int) font.Size * 3, (int) font.Size * 3);
                letter.ShapeAlign(CenterMode.Center, CenterMode.Center);
                letter.Stroke(Color.White);
                letter.StrokeWeight(7);
                letter.Fill(34, 1, 34);
                letter.Rect(letter.width / 2, letter.height / 2, letter.width, letter.height);
                letter.TextFont(font);
                letter.Fill(Color.White);
                letter.TextAlign(CenterMode.Center, CenterMode.Center);
                letter.Text("" + alpha[i]);
                letter.SetXY(letter.width * x - font.Size / 2, height / 2 + (letter.height * row) + difference * 2);
                buttons.Add(letter, alpha[i].ToString());
                AddChildAt(letter, 25 - i);
                x++;
                _pData.SetButtons(buttons);

                Console.WriteLine(alpha[i] + ": row = " + row + ", x = " + x);
                Console.WriteLine(alpha[i].ToString());
            }
        }

        private void CreateInputText()
        {
            textBox = new EasyDraw((int)font.Size * 9, (int)font.Size * 3, false);
            textBox.ShapeAlign(CenterMode.Center, CenterMode.Center);
            textBox.Stroke(Color.White);
            textBox.StrokeWeight(7);
            textBox.Fill(34, 1, 34);
            textBox.Rect(textBox.width / 2, textBox.height / 2, textBox.width, textBox.height);

            textBox.SetXY(width / 2 - textBox.width / 2 - difference, height / 2 - textBox.height);
            AddChild(textBox);
            
            text = new EasyDraw((int)font.Size * 9, (int)font.Size * 3, false);
            
            text.TextFont(font);
            text.Fill(Color.White);
            text.TextAlign(CenterMode.Center, CenterMode.Center);
            text.Text(_pData.GetPlayerName());
            text.SetXY(width / 2 - text.width / 2 - difference, height / 2 - text.height);
            AddChild(text);
        }

        private void UpdateInputText()
        {
            text.ClearTransparent();
            text.Text(_pData.GetPlayerName());
        }
    }
}
