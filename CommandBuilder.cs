using System;
using System.Collections.Generic;
using System.Globalization;
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

        public struct LogData
        {
            internal string msg;
            internal enum Type 
            {
                LOG,
                ERROR
            }
            internal Type type;
        }
        private LogData latestLog;

        public CommandBuilder(LumiaSdk theFrameWork)
        {
            latestLog = new LogData();
            frameWork = theFrameWork;
            listOfLights = new Dictionary<group, List<ILumiaLight>>();
            listOfLights[group.LEFT] = new List<ILumiaLight>();
            listOfLights[group.RIGHT] = new List<ILumiaLight>();
        

            supportedColors = new Dictionary<string, RGB>
            {
                { "red",    new RGB{r = 0xFF, g = 0x00, b = 0x00}},
                { "blue",   new RGB{r = 0x00, g = 0x00, b = 0xff}},
                { "green",  new RGB{r = 0x00, g = 0xff, b = 0x00}},
                { "yellow", new RGB{r = 0xff, g = 0xff, b = 0x00}},
                { "orange", new RGB{r = 0xff, g = 0xa5, b = 0x00}},
                { "teal",   new RGB{r = 0x00, g = 0x80, b = 0x80}},
                { "aqua",   new RGB{r = 0x1f, g = 0xba, b = 0xed}},
                { "pink",   new RGB{r = 0xfc, g = 0x05, b = 0xbe}},
                { "purple", new RGB{r = 0xad, g = 0x00, b = 0xff}}

            };
        }

        public void addToLightGroup(ILumiaLight theLight, group theGroup)
        {
            listOfLights[theGroup].Add(theLight);
        }
        public Command analyze(string str)
        {
            Command retVal = new Command(frameWork)
            {
                type = Command.Type.INVALID
            };
            string[] scenes = splitAndClean(str);

            RGB color;

            for (uint i = 0U; i < scenes.Length; i++)
            {
                scenes[i] = scenes[i].Trim();
                color = extractColor(ref scenes[i]);
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
                else
                {
                    latestLog.msg = "Error: " + scenes[i] + " is not a color";
                    latestLog.type = LogData.Type.ERROR;
                }
            }



            return retVal;
        }

        private static string[] splitAndClean(string str)
        {
            string[] arr = str.ToLower().Split("to");
            for (int i = 0; i < arr.Length; ++i)
            {
                arr[i] = arr[i].Trim();
            }
            return arr;
        }

        private  RGB extractColor(ref string str)
        {
            RGB colorFound = null;
            str = str.Trim();

            foreach (KeyValuePair<string, RGB> pair in supportedColors)
            {

                if (str.IndexOf(pair.Key) == 0)
                {
                    colorFound = pair.Value;
                    str = str.Remove(0, pair.Key.Length);
                    break;
                }
                else if (str.IndexOf("hex") == 0)
                {
                    str = str.Remove(0, 3);
                    colorFound = extractColorFromHex(ref str);
                }
                else if(str.IndexOf("0x") == 0)
                {
                    str = str.Remove(0, 2);
                    colorFound = extractColorFromHex(ref str);
                }
            }

            return colorFound;
        }

        private RGB extractColorFromHex(ref string str)
        {
            RGB colorFound;
            if (str.Length < 6)
            {
                latestLog.msg = "Hex number needs to have six digits";
                latestLog.type = LogData.Type.ERROR;
            }
            colorFound = new RGB();
            string red = str.Substring(0, 2);
            string green = str.Substring(2, 2);
            string blue = str.Substring(4, 2);
            if (!Int32.TryParse(red, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out colorFound.r)
            || !Int32.TryParse(green, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out colorFound.g)
            || !Int32.TryParse(blue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out colorFound.b)
            )
            {
                latestLog.msg = "Error: " + str.Substring(0, 5) + " is not a legal hex value";
                latestLog.type = LogData.Type.ERROR;
            }
            else
            {
                str = str.Remove(0, 6);
            }

            return colorFound;
        }

        public LogData getLatestLog()
        {
            return latestLog;
        }
    }
}
