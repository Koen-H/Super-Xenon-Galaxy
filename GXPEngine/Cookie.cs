using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Cookie : AnimationSprite
    {
        public ObjectColor cookieColor;

        private float colorIndex;
        private int decaySpeed;

        public Cookie(float x, float y, string path, ObjectColor _cookieColor) : base (path, 1, 1)
        {
            cookieColor = _cookieColor;
            collider.isTrigger = true;
            SetOrigin(width / 2, height / 2);
            SetScaleXY(4);
            SetXY(x, y);

            colorIndex = 1;
            decaySpeed = 1;
        }

        void Update()
        {
            ColorUpdate();
        }

        public float GetColorIndex()
        {
            return colorIndex;
        }

        private void ColorUpdate()
        {
            colorIndex -= decaySpeed * 0.001f;
            SetColor(colorIndex, colorIndex, colorIndex);
        }

    }
}
