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
        private LeaderBoard leaderBoard;


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
                leaderBoard = new LeaderBoard(game, pData);
                leaderBoard.visible = false;

                AddChild(level);
                AddChild(leaderBoard);
                AddChild(player);
                AddChild(hud);
            }


            menu.Update();


            if (level != null)
            {
                level.Update();
            }

            if (leaderBoard != null)
            {
                if (pData.GetLifes() == 0)
                {
                    new Sound("Assets/Sounds/wolf growl.wav").Play();//should be game over sound.
                    pData.SetButtonActive(true);
                    leaderBoard.visible = true;
                    leaderBoard.FixedUpdate();
                }
                else
                {
                    pData.SetButtonActive(false);
                    leaderBoard.visible = false;
                }
            }

            if (player != null)
            {
                player.FixedUpdate();
            }

            if (hud != null)
            {
                if (leaderBoard.visible)
                {
                    hud.visible = false;
                    return;
                }
                hud.Update();
            }
        }
    }
}
