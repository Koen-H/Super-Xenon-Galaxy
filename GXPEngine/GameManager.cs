using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class GameManager : GameObject
    {
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
                hud = new HUD(game, pData);
		        level = new Level("Map.tmx", pData);

                AddChild(level);
                AddChild(hud);

                input = new InputName(game, pData);
                AddChild(input);
            }


            menu.Update();

            if (level != null && hud != null)
            {
                level.Update();
                hud.Update();
                input.Update();
            }
        }
    }
}
