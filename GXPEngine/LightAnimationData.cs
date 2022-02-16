using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class LightAnimationData
    {

        public Boolean cyan, orange, pink, purple, space;
        public float delay;

        public LightAnimationData(Boolean _cyan, Boolean _orange, Boolean _pink, Boolean _purple, Boolean _space, float _delay)
        {
            cyan = _cyan;
            orange = _orange;
            pink = _pink;
            purple = _purple;
            space = _space;
            delay = _delay;
        }
    }
}
