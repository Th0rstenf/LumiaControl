using System;
using System.Collections.Generic;
using System.Text;
using Lumia;

namespace LumiaControl
{
    class Command
    {

        public enum Type
        {
            TRANSITION
               , SCENE
               , INVALID
        }
        public Type type;
        public List<RGB> listOfColorsLeft;
        public List<RGB> listOfColorsRight;
        public string err;

        public Command() { listOfColorsLeft = new List<RGB>(); listOfColorsRight = new List<RGB>(); }

        public void execute() { }

    }
}
