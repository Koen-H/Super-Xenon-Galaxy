using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class PlayerBody : AnimationSprite
    {
        private const int spriteCols = 7;
        private const int spriteRows = 3;

        public PlayerBody() : base("Assets/Player/Body/pink.png", spriteCols, spriteRows)
        {
            SetOrigin(width / 2, height / 2);
            SetXY(0, height / 1.5f);
            SetScaleXY(4);
        }

        public void Update()
        {
            Animate(0.5f);
        }

        public void UpdateSprite(string s)
        {
            string spriteString = "Assets/Player/Body/" + s + ".png";
            initializeFromTexture(Texture2D.GetInstance(spriteString, false));
            initializeAnimFrames(spriteCols, spriteRows);

            SetOrigin(width / (scale * 2), height / (scale * 2));

            //Console.WriteLine("Player's color has changed to:" + currentColor);
        }
    }
}
