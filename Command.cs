using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Lumia;

namespace LumiaControl
{
    class Command
    {
        private LumiaSdk framework;
        public enum Type
        {
            TRANSITION
            , SCENE
            , INVALID
        }
        //TODO: make private, provide set/get
        public Type type;
        //TODO: Also create a dictionary with group enum as key
        public List<RGB> listOfColorsLeft;
        public List<RGB> listOfColorsRight;
        
        public string err;

        public Command(LumiaSdk theFramework)
        {
            framework = theFramework;
            listOfColorsLeft = new List<RGB>();
            listOfColorsRight = new List<RGB>(); 
        }

        public async  void execute()
        {
            if (type != Type.INVALID)
            {
                for (int i = 0; i < listOfColorsLeft.Count; ++i)
                {
                    if (listOfColorsLeft[i] == listOfColorsRight[i])
                    {
                         framework.SendColor(listOfColorsLeft[i], 100, CommandBuilder.defaultDuration, 0, false, false, null);
                         await Task.Delay(CommandBuilder.defaultDuration);
                    }
                    else
                    {
                         framework.SendColor(listOfColorsLeft[i], 100, 5000, 0, false, false, CommandBuilder.listOfLights[CommandBuilder.group.LEFT]);
                         framework.SendColor(listOfColorsRight[i], 100, 5000, 0, false, false, CommandBuilder.listOfLights[CommandBuilder.group.RIGHT]);
                    }
                }
                
            }
        }

    }
}
