using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Lumia;

namespace LumiaControl
{

    class CommandBuilder
    {       
        private const int FlashDuration = 500;
        private const int DimmedBrightness = 10;
        private const int BrutalBrightness = 100;

        private const string FlashCommand = "flash";
        private const string SmoothCommand = "smooth";
        private const string DimmedCommand = "dimmed";
        private const string BrightCommand = "bright";
        
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
            int duration    = Scene.DefaultDuration;
            int transition  = Scene.DefaultTransitionTime;
            int brightness = Scene.DefaultBrightness;

            applyModifiers(ref str, ref duration, ref transition, ref brightness);
            
            string[] sceneDescriptorString = splitAndClean(str);
            Scene[] scenes = new Scene[sceneDescriptorString.Length];
            for (int i = 0; i < scenes.Length; ++i)
            {
                scenes[i] = new Scene();
            }

            RGB color;

            for (uint i = 0U; i < sceneDescriptorString.Length; i++)
            {
                scenes[i].duration   = duration;
                scenes[i].transition = transition;
                scenes[i].brightness = brightness;

                sceneDescriptorString[i] = sceneDescriptorString[i].Trim();
                color = extractColor(ref sceneDescriptorString[i]);
                if (color != null)
                {
                    scenes[i].colorLeft = color;
                    if (sceneDescriptorString[i].Length > 0)
                    {
                        color = extractColor(ref sceneDescriptorString[i]);
                    }
                    scenes[i].colorRight = color;
                    retVal.type = Command.Type.SCENE;
                }
                else
                {
                    latestLog.msg = "Error: " + sceneDescriptorString[i] + " is not a color";
                    latestLog.type = LogData.Type.ERROR;
                }
                retVal.addScene(scenes[i]);
            }

            return retVal;
        }

        private void applyModifiers(ref string str, ref int duration, ref int transitionTime, ref int brightness)
        {
            if (extractModifier(ref str, FlashCommand))
            {
                duration = FlashDuration;
            }
            if (extractModifier(ref str, SmoothCommand))
            {
                transitionTime = duration;
            }
            if (extractModifier(ref str, DimmedCommand))
            {
                brightness = DimmedBrightness;
                if (str.Contains(BrightCommand))
                {
                    latestLog.msg = "You should decide for either bright or dimmed. Let's go with dimmed for now.";
                    latestLog.type = LogData.Type.LOG;
                }
            }
            else if(extractModifier(ref str, BrightCommand))
            {
                brightness = BrutalBrightness;
            }

        }

        private static bool extractModifier(ref string str, string modifier)
        {   
            bool found = str.ToLower().Contains(modifier);
            if (found)
            {
                str = str.Remove(str.IndexOf(modifier), modifier.Length);
            }
            return found;
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
