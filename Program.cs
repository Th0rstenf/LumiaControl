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
        private static readonly string token = new string("lumia892089382");
        private static readonly string IP = new string("192.168.1.43");
        private static ILumiaLight left;
        private static ILumiaLight right;



        public static async Task MainTask()
        {
            LumiaSdk framework = new LumiaSdk();
            CommandBuilder builder = new CommandBuilder(framework);
            await framework.init(token, "", IP);

            left = new ILumiaLight
            {
                type = LightBrands.TUYA,
                id = "bf172f7cd299bace36qppv"
            };
            right = new ILumiaLight
            {
                type = LightBrands.TUYA,
                id = "bfe94a8ad680170181ejrq"
            };

            builder.addToLightGroup(right, CommandBuilder.group.RIGHT);
            builder.addToLightGroup(left, CommandBuilder.group.LEFT);

            
            await debugOutput(framework);


            while (true)
            {
                Console.WriteLine("Enter string");
                string str = System.Console.ReadLine();
                Command cmd = builder.analyze(str);
                cmd.execute();
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


        static void Main(string[] args)
        {

			MainTask().GetAwaiter().GetResult();

        }
    }
}
