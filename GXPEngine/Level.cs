using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

namespace GXPEngine
{
    public class Level : GameObject
    {
        private PlayerData _pData;
        private Player player;
        private CookieManager cookieManager;

        private TiledLoader tiledLoader;

        public Level(string filename, PlayerData pData) : base(true)
        {
            _pData = pData;

            tiledLoader = new TiledLoader(filename);
            Create();

            cookieManager = new CookieManager(this, pData);
            AddChildAt(cookieManager, GetChildCount() - 1);
        }

        public void Update()
        {
            if (_pData.GetLifes() <= 0) return;
            player.FixedUpdate();
            cookieManager.Update();
        }

        public Player GetPlayer()
        {
            return player;
        }

        private void Create()
        {
            tiledLoader.rootObject = this;

            tiledLoader.autoInstance = true;

            tiledLoader.addColliders = false;
            tiledLoader.LoadTileLayers(0);

            tiledLoader.addColliders = true;
            tiledLoader.LoadTileLayers(1);

            tiledLoader.addColliders = false;
            tiledLoader.LoadTileLayers(2);

            tiledLoader.AddManualType("Player");
            tiledLoader.OnObjectCreated += TiledLoader_OnObjectCreated;
            tiledLoader.LoadObjectGroups();

        }

        private void TiledLoader_OnObjectCreated(Sprite sprite, TiledObject obj)
        {
            if (obj.Type == "Player")
            {
                player = new Player(this, new Vector2(obj.X, obj.Y), _pData);
                AddChild(player);
            }

            //if (obj.Type == "Wall")
            //{
            //    wall = new Wall(obj.Width, obj.Height, obj.X, obj.Y);
            //    AddChild(wall);
            //}
        }
    }
}
