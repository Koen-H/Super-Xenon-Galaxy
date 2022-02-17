using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class Button : AnimationSprite
    {
        private string _name;

        public Button(string name, string filename) : base(filename, 1, 1)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
