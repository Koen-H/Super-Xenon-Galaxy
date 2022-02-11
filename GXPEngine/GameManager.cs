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

        public GameManager()
        {
            menu = new Menu();
            AddChild(menu);

            pData = new PlayerData();
        }

        public void Update()
        {
            

            if (!menu.isActive())
            {
                hud = new HUD(game, pData);
		        level = new Level("Map.tmx", pData);

                AddChild(level);
                AddChild(hud);
            }


            menu.Update();

            if (level != null && hud != null)
            {
                level.Update();
                hud.Update();
            }
        }
    }
}
