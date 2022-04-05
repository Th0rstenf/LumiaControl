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
            StringInterpreter interpreter = new StringInterpreter();
            await framework.init(token, "", IP);
            await debugOutput(framework);
            while (true)
            {
                Console.WriteLine("Enter string");
                string str = System.Console.ReadLine();
                Command cmd = interpreter.InterpretString(str);
                await sendCommand(framework,cmd);
            }
            
        }

        private static async Task debugOutput(LumiaSdk framework)
        {
            framework.error += (string r) =>
            {
                Console.WriteLine("error : " + r);
            };

            framework.closed += (string r) =>
            {
                Console.WriteLine("closed : " + r);
            };


            framework.events += (JObject data) =>
            {
                Console.WriteLine("Event data : " + data.ToString());


                // here we give the context as we know it's an SDK Eent types
                switch (LumiaUtils.getTypeValueFromString<LumiaEventTypes>("LumiaSdkEventTypes", data["type"].Value<string>()))
                {
                    case LumiaEventTypes.STATES:
                        Console.WriteLine("States have been updated:  " + data.ToString());
                        break;

                    case LumiaEventTypes.COMMAND:
                        Console.WriteLine("A Chat Command is being triggered:  " + data.ToString());
                        break;

                    case LumiaEventTypes.CHAT:
                        Console.WriteLine("New chat message:  " + data.ToString());
                        break;

                    case LumiaEventTypes.ALERT:
                        Console.WriteLine("New alert:  " + data.ToString());
                        break;
                }
            };

            var r = await framework.GetInfo();

            Console.WriteLine("get info result : " + r.ToString());
        }

        public static async Task sendCommand(LumiaSdk framework,Command cmd)
        {
            RGB Red = new RGB
            {
                r = 255,
                g = 0,
                b = 0
            };


            await framework.SendColor(Red, 100, 1000,0,false,false, null);
        }
        static void Main(string[] args)
        {

			MainTask().GetAwaiter().GetResult();

        }
    }
}
