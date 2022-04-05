using System;
using System.Collections.Generic;
using System.Text;
using Lumia;

namespace LumiaControl
{

    
    class CommandBuilder
    {

        private static Dictionary<string, RGB> supportedColors;
        private LumiaSdk frameWork;
        private List<ILumiaLight>;
        
        public CommandBuilder(LumiaSdk theFrameWork)
        {
            frameWork = theFrameWork;

            supportedColors = new Dictionary<string, RGB>
            {
                { "red", new RGB { r = 255, g = 0, b = 0 } },
                { "blue", new RGB { r = 0, g = 0, b = 255 } },
                { "green", new RGB { r = 0, g = 255, b = 0 } }
            };
        }


        public Command InterpretString(string str)
        {
            Command retVal = new Command(frameWork);
            retVal.type = Command.Type.INVALID;
            string check = str.ToLower();
            RGB color;

            if (!check.Contains("to"))
            {
                color =  extractColor(ref check);
                if (color != null)
                {
                    retVal.listOfColorsLeft.Add(color);
                    if (check.Length > 0)
                    {
                        color = extractColor(ref check);
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
