using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Lumia;


namespace LumiaControl
{
    class Program
    {
        private static string token = new string("lumia892089382");
        private static string IP = new string("192.168.1.43");

        public static async Task MainTask()
        {
            LumiaSdk framework = new LumiaSdk();
            await framework.init(token, "", "ws://"+IP+":39231");

            var r = await framework.GetInfo();

            await checkCommands(framework);
        }

        public static async Task checkCommands(LumiaSdk framework)
        {
            RGB color = new RGB();
            color.r = 255;
            color.g = 0;
            color.b = 255;

            

            await framework.SendColor(color, 100, 10);
        }
        static void Main(string[] args)
        {
            
               
                
            MainTask().GetAwaiter().GetResult();

            

            


        }
    }
}
