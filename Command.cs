using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Lumia;

namespace LumiaControl
{
    partial class Command
    {
        private int transitionTime;
        private int duration;
        private int brightness;
        private bool isNewDefault = false;
        private LumiaSdk framework;
        //TODO: make private, provide set/get
        public Type type;
        //TODO: Also create a dictionary with group enum as key
        internal List<RGB> listOfColorsLeft;
        internal List<RGB> listOfColorsRight;

        public string err;

        public Command(LumiaSdk theFramework, int theDuration, int theBrightness, int theTransitionTime)
        {
            framework = theFramework;
            listOfColorsLeft = new List<RGB>();
            listOfColorsRight = new List<RGB>();
            duration = theDuration;
            brightness = theBrightness;
            transitionTime = theTransitionTime;
            err = new String("");
        }

        public async void execute()
        {
            if (type != Type.INVALID)
            {
                for (int i = 0; i < listOfColorsLeft.Count; ++i)
                {
                    if (listOfColorsLeft[i] == listOfColorsRight[i])
                    {
                        _ = framework.SendColor(listOfColorsLeft[i], brightness, duration, transitionTime, isNewDefault, false, null);
                    }
                    else
                    {
                        _ = framework.SendColor(listOfColorsLeft[i], brightness, duration, transitionTime, isNewDefault, false, CommandBuilder.listOfLights[CommandBuilder.group.LEFT]);
                        _ = framework.SendColor(listOfColorsRight[i], brightness, duration, transitionTime, isNewDefault, false, CommandBuilder.listOfLights[CommandBuilder.group.RIGHT]);
                    }
                    await Task.Delay(duration);
                }

            }
        }

        public bool isValid() 
        {
            return type != Type.INVALID;
        }
    }
}
