using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class GameManager : GameObject
    {
        private Player player;
        private HUD hud;
        private Menu menu;
        private PlayerData pData;
        private Level level;
        private InputName input;


        public GameManager()
        {
            pData = new PlayerData();
            menu = new Menu();
            AddChild(menu);
        }

        public void Update()
        {
            

            if (!menu.isActive())
            {
                player = new Player(game.width / 2, game.height / 2, pData);
                hud = new HUD(game, pData);
		        level = new Level("Assets/background.png", player, pData);
                input = new InputName(game, pData);
                input.visible = false;

                AddChild(level);
                AddChild(input);
                AddChild(player);
                AddChild(hud);
            }


            menu.Update();


            if (level != null)
            {
                level.Update();
            }

            if (input != null)
            {
                if (pData.GetLifes() == 0)
                {
                    pData.SetButtonActive(true);
                    input.visible = true;
                    input.FixedUpdate();
                }
                else
                {
                    pData.SetButtonActive(false);
                    input.visible = false;
                }
            }

            if (player != null)
            {
                player.FixedUpdate();
            }

            if (hud != null)
            {
                hud.Update();
            }
        }
    }
}
