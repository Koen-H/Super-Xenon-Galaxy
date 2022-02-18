using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class PlayerTail : AnimationSprite
    {
        private const int spriteCols = 7;
        private const int spriteRows = 3;

        private Player _player;

        private float maxHeight;

        public PlayerTail(Player player) : base("Assets/Player/Tail/pink.png", spriteCols, spriteRows)
        {
            _player = player;
            maxHeight = height / 1.5f;
            SetOrigin(width / 2, height / 2);
            SetXY(0, maxHeight);
            SetScaleXY(4);
        }

        public void Update()
        {
            Animate(0.5f);
            if (_player.GetSpeed() != 0)
            {
                visible = true;
            }
            else
            {
                visible = false;
            }

            //if (height < _player.GetSpeed() * 1.28f)
            //{
            //    height += 3;
            //}
            //else if (height > _player.GetSpeed() * 1.28f)
            //{
            //    height -= 1;
            //    if (height < 100) height = 100;
            //}
            //SetXY(0, maxHeight);

            //height = (int)(_player.GetSpeed() * 1.28f);
        }

        public void UpdateSprite(string s)
        {
            string spriteString = "Assets/Player/Tail/" + s + ".png";
            initializeFromTexture(Texture2D.GetInstance(spriteString, false));
            initializeAnimFrames(spriteCols, spriteRows);

            SetOrigin(width / (scale * 2), height / (scale * 2));



            //Console.WriteLine("Player's color has changed to:" + currentColor);
        }
    }
}
