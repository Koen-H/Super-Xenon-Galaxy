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

        public Cookie(float x, float y, string path, ObjectColor _cookieColor) : base(path, 5, 2)
        {
            cookieColor = _cookieColor;
            collider.isTrigger = true;
            SetOrigin(width / 2, height / 2);
            SetScaleXY(2);
            SetXY(x, y);

            colorIndex = 1;
            decaySpeed = 20;
        }

        void Update()
        {
            Turn(5);
            Animate(0.5f);
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
