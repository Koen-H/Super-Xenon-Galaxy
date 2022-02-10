using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Cookie : AnimationSprite
    {
        public ObjectColor cookieColor;

        public Cookie(float x, float y, string path, ObjectColor _cookieColor) : base (path, 1, 1)
        {
            this.cookieColor = _cookieColor;
            collider.isTrigger = true;
            SetOrigin(width / 2, height / 2);
            SetScaleXY(4);
            SetXY(x, y);
        }

        void Update()
        {

        }

    }
}
