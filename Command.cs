using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Lumia;

namespace LumiaControl
{
    partial class Command
    {
        private const int DefaultDuration = 5000;
        private const int DefaultBrightness = 60;
        private const int DefaultTransitionTime = 0;
        private int transitionTime;
        private int duration;
        private int brightness;
        private LumiaSdk framework;
        //TODO: make private, provide set/get
        public Type type;
        //TODO: Also create a dictionary with group enum as key
        internal List<RGB> listOfColorsLeft;
        internal List<RGB> listOfColorsRight;


        
        
        public string err;

        public Command(LumiaSdk theFramework, int theDuration = DefaultDuration, int theBrightness = DefaultBrightness, int theTransitionTime = DefaultTransitionTime)
        {
            framework = theFramework;
            listOfColorsLeft = new List<RGB>();
            listOfColorsRight = new List<RGB>();
            duration = theDuration;
            brightness = theBrightness;
            transitionTime = theTransitionTime;
            err = new String("");
        }

        public async  void execute()
        {
            if (type != Type.INVALID)
            {
                for (int i = 0; i < listOfColorsLeft.Count; ++i)
                {
                    if (listOfColorsLeft[i] == listOfColorsRight[i])
                    {
                        _ = framework.SendColor(listOfColorsLeft[i], brightness, duration, transitionTime, false, false, null);                         
                    }
                    else
                    {
                        _ = framework.SendColor(listOfColorsLeft[i], brightness, duration, transitionTime, false, false, CommandBuilder.listOfLights[CommandBuilder.group.LEFT]);
                        _ = framework.SendColor(listOfColorsRight[i], brightness, duration, transitionTime, false, false, CommandBuilder.listOfLights[CommandBuilder.group.RIGHT]);
                    }
                    await Task.Delay(duration);
                }
                
            }
        }

    }
}
