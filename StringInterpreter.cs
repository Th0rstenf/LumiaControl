using System;
using System.Collections.Generic;
using System.Text;
using Lumia;

namespace LumiaControl
{

    
    class StringInterpreter
    {

        private static Dictionary<string, RGB> supportedColors;
        
        public StringInterpreter()
        {
            supportedColors = new Dictionary<string, RGB>
            {
                { "red", new RGB { r = 255, g = 0, b = 0 } },
                { "blue", new RGB { r = 0, g = 0, b = 255 } },
                { "green", new RGB { r = 0, g = 255, b = 0 } }
            };
        }


        public Command InterpretString(string str)
        {
            Command retVal = new Command();
            retVal.type = Command.Type.INVALID;
            string check = str.ToLower();

            foreach (KeyValuePair<string,RGB> pair in supportedColors)
            {
                if (check.IndexOf(pair.Key) == 0)
                {
                    retVal.listOfColorsLeft.Add(pair.Value);
                    check = check.Remove(0, pair.Key.Length);
                    if (check.IndexOf("to") == 0)
                    {
                        retVal.type = Command.Type.TRANSITION;
                        check = check.Remove(0, 2);
                    }
                    else
                    {
                        retVal.type = Command.Type.SCENE;
                    }
                    break;
               }
            }

            if (retVal.type != Command.Type.INVALID)
            {
                foreach (KeyValuePair<string, RGB> pair in supportedColors)
                {
                    if (check.IndexOf(pair.Key) == 0)
                    {
                        retVal.listOfColorsRight.Add(pair.Value);
                        check = check.Remove(0, pair.Key.Length);
                        break;
                    }
                }
            }



            return retVal;
        }
            
    }
}
