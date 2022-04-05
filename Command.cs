using System;
using System.Collections.Generic;
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

        public async void execute()
        {
            if (type != Type.INVALID)
            {
                if( listOfColorsLeft.Count == listOfColorsRight.Count)
                {
                    if (listOfColorsLeft[0] == listOfColorsRight[0])
                    {
                        await framework.SendColor(listOfColorsLeft[0], 100, 5000, 0, false, false, null);
                    }
                    else
                    {
                        await framework.SendColor(listOfColorsLeft[0], 100, 5000, 0, false, false, CommandBuilder.listOfLights[CommandBuilder.group.LEFT]);
                        await framework.SendColor(listOfColorsRight[0], 100, 5000, 0, false, false, CommandBuilder.listOfLights[CommandBuilder.group.RIGHT]);
                    }
                }
                else
                {
                    Console.WriteLine("Light definitions should always be equal");
                }
            }
        }

    }
}
