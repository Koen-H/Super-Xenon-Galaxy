using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

namespace GXPEngine
{
    public class Level : Sprite
    {
        private PlayerData _pData;
        private Player _player;
        private CookieManager cookieManager;

        private Sprite background;
        private Sprite midground;
        private Sprite foreground;

        private int direction;

        public Level(string filename, Player player, PlayerData pData) : base("Assets/Background/background.png", false, true)
        {
            _pData = pData;
            _player = player;

            direction = 1;
            background = new Sprite("Assets/Background/2.png");
            background.SetOrigin(background.width / 2, background.height / 2);
            background.SetScaleXY(0.85f);
            background.SetXY(game.width / 2, game.height / 2);
            midground = new Sprite("Assets/Background/3.png");
            midground.SetOrigin(midground.width / 2, midground.height / 2);
            midground.SetXY(game.width / 2, game.height / 2);
            foreground = new Sprite("Assets/Background/4.png");
            foreground.SetOrigin(foreground.width / 2, foreground.height / 2);
            foreground.SetXY(game.width / 2, game.height / 2);

            AddChild(background);
            //AddChild(backgroundL);
            AddChild(midground);
            AddChild(foreground);

            cookieManager = new CookieManager(this, pData);
            AddChild(cookieManager);
        }

        public void Update()
        {
            background.x += 0.07f * direction;
            if (background.x > game.width * 0.66f)
            {
                direction = -1;
            }
            else if (background.x < game.width / 3)
            {
                direction = 1;
            }

            background.Turn(0.005f);
            midground.Turn(-0.008f);
            foreground.Turn(0.02f);

            if (background.rotation > 360) background.rotation -= 360;
            if (midground.rotation > 360) midground.rotation -= 360;
            if (foreground.rotation > 360) foreground.rotation -= 360;
            cookieManager.Update();
        }

        public Player GetPlayer()
        {
            return _player;
        }
    }
}
