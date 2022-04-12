using System;
using System.Collections.Generic;
using System.Text;
using Lumia;

namespace LumiaControl
{
    class Scene
    {
        private const int DefaultDuration = 5000;
        private const int DefaultBrightness = 50;
        private const int DefaultTransitionTime = 500;

        internal RGB colorLeft;
        internal RGB colorRight;
        internal int duration;
        internal int brightness;
        internal int transition;

        public Scene()
        {
            colorLeft = new RGB();
            colorRight = new RGB();
            duration = DefaultDuration;
            brightness = DefaultBrightness;
            transition = DefaultTransitionTime;
        }
    }
}
