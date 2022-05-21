using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Lumia;

namespace LumiaControl
{
    partial class Command
    {
        private const int DefaultDuration = 10000;
        private const int DefaultBrightness = 60;
        private const int DefaultTransitionTime = 0;
        private int transitionTime;
        private int duration;
        private int brightness;
        private bool isNewDefault = false;
        private LumiaSdk framework;
        //TODO: make private, provide set/get
        public Type type;
        internal List<Scene> listOfScenes;        


        public string err;

        public Command(LumiaSdk theFramework)
        {
            framework = theFramework;
            listOfScenes = new List<Scene>();
            err = new String("");
        }

        public async void execute()
        {
            if (type != Type.INVALID)
            {                
                foreach(Scene scene in listOfScenes)
                {
                    if(scene.colorLeft == scene.colorRight)
                    {
                        _ = framework.SendColor(scene.colorLeft, scene.brightness, scene.duration, scene.transition, isNewDefault, false, null);
                    }
                    else
                    {
                        _ = framework.SendColor(scene.colorLeft, scene.brightness, 1, scene.transition, isNewDefault, false, CommandBuilder.listOfLights[CommandBuilder.group.LEFT]);
                        _ = framework.SendColor(scene.colorRight, scene.brightness, scene.duration, scene.transition, isNewDefault, false, CommandBuilder.listOfLights[CommandBuilder.group.RIGHT]);

                    }
                    await Task.Delay(scene.duration);
                }                
            }
        }

        public bool isValid() 
        {
            return type != Type.INVALID;
        }

        public void addScene(Scene scene)
        {
            listOfScenes.Add(scene);
        }
    }
}
