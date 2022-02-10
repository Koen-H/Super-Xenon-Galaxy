using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Cookie : AnimationSprite
    {
        public Cookie(float x, float y, string path) : base (path, 1, 1)
        {
            collider.isTrigger = true;
            SetOrigin(width / 2, height / 2);
            SetXY(x, y);
        }

        void Update()
        {

        }
    }
}
