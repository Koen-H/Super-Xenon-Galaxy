using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class PlayerPiece : Sprite
    {
        private Player _player;

        public PlayerPiece(Player player) : base("nothing")
        {
            _player = player;
        }
        public void Update()
        {

        }
    }

}
