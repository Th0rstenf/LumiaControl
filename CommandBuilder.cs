using System;
using System.Collections.Generic;
using System.Text;
using Lumia;

namespace LumiaControl
{

    
    class CommandBuilder
    {
        public enum group { LEFT,RIGHT};
        private static Dictionary<string, RGB> supportedColors;
        private LumiaSdk frameWork;
        internal static  Dictionary<group,List<ILumiaLight>> listOfLights;
        internal static int defaultDuration = 5000;
        internal static int defaultBrightness = 100;
        internal static int transitionTime = 0;


        public CommandBuilder(LumiaSdk theFrameWork)
        {
            frameWork = theFrameWork;
            listOfLights = new Dictionary<group, List<ILumiaLight>>();
            listOfLights[group.LEFT] = new List<ILumiaLight>();
            listOfLights[group.RIGHT] = new List<ILumiaLight>();
        

            supportedColors = new Dictionary<string, RGB>
            {
                { "red", new RGB { r = 255, g = 0, b = 0 } },
                { "blue", new RGB { r = 0, g = 0, b = 255 } },
                { "green", new RGB { r = 0, g = 255, b = 0 } }
            };
        }

        public void addToLightGroup(ILumiaLight theLight, group theGroup)
        {
            listOfLights[theGroup].Add(theLight);
        }
        public Command analyze(string str)
        {
            Command retVal = new Command(frameWork);
            retVal.type = Command.Type.INVALID;
            string[] scenes= str.ToLower().Split("to");
            
            RGB color;

            for (uint i = 0U; i < scenes.Length; i++)
            {
                color =  extractColor(ref scenes[i]);
                if (color != null)
                {
                    retVal.listOfColorsLeft.Add(color);
                    if (scenes[i].Length > 0)
                    {
                        color = extractColor(ref scenes[i]);
                    }
                    retVal.listOfColorsRight.Add(color);
                    retVal.type = Command.Type.SCENE;
                }
            }



            return retVal;
        }

        private static RGB extractColor(ref string str)
        {
            RGB colorFound = null;
            foreach (KeyValuePair<string, RGB> pair in supportedColors)
            {
                
                if (str.IndexOf(pair.Key) == 0)
                {
                    colorFound = pair.Value;
                    str = str.Remove(0, pair.Key.Length);
                    break;
                }
            }

            return colorFound;
        }
    }
}
