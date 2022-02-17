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
        private ArduinoController gameController;
        private Boolean gameOverOnce;


        public GameManager(ArduinoController _gameController = null)
        {
            gameController = _gameController;
            pData = new PlayerData();
            menu = new Menu();
            AddChild(menu);

        }

        public void Update()
        {
            if (!menu.isActive())
            {
                if (gameController != null)
                {
                    gameController.playLightAnimation = false;
                    gameController.SendString("COLORS_OFF");
                }

                player = new Player(game.width / 2, game.height / 2, pData);
                hud = new HUD(game, pData);
		        level = new Level("Assets/background.png", player, pData);
                leaderBoard = new LeaderBoard(game, pData);

                AddChild(level);
                AddChild(leaderBoard);
                AddChild(player);
                AddChild(hud);
            }
            if (gameController != null)
            {
                if (gameController.analogRotation > 280 || gameController.analogRotation < 80 && gameController.analogForce > 40)
                {
                    menu.analogUp = true;

                }
                else
                {
                    menu.analogUp = false;
                    menu.pressW = false;
                }
                if (gameController.analogRotation < 250 && gameController.analogRotation > 100 && gameController.analogForce > 40)
                {
                    menu.analogDown = true;
                }
                else
                {
                    menu.analogDown = false;
                    menu.pressS = false;
                }
            }
            menu.Update();


            if (level != null)
            {
                level.Update();
            }


            if (hud != null)
            {
                if (leaderBoard.visible)
                {
                    hud.visible = false;
                }
                else
                {
                    hud.visible = true;
                    hud.Update();
                }
            }

            if (leaderBoard != null)
            {
                if (pData.GetLifes() == 0 && !hud.GetGameOver().visible)
                {
                    if (!gameOverOnce){
                        level.cookieManager.KillAllCookies();
                        new Sound("Assets/Sounds/playerdeath.wav").Play();//should be game over sound.
                        MyGame myGame = (MyGame)game;
                        myGame.PlayBackgroundMusic("music endscreen.wav");
                        pData.SetButtonActive(true);
                        leaderBoard.visible = true;
                        gameOverOnce = true;
                    }
                    leaderBoard.FixedUpdate();
                }
                else
                {
                    gameOverOnce = false;
                    pData.SetButtonActive(false);
                    leaderBoard.visible = false;
                }
            }

            if (player != null)
            {
                player.FixedUpdate();
            }

        }

    }
}
