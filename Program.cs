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
				switch (LumiaUtils.getTypeValueFromString<LumiaSdkEventTypes>("LumiaSdkEventTypes", data["type"].Value<string>()))
				{
					case LumiaSdkEventTypes.STATES:
						Console.WriteLine("States have been updated:  " + data.ToString());
						break;

					case LumiaSdkEventTypes.COMMAND:
						Console.WriteLine("A Chat Command is being triggered:  " + data.ToString());
						break;

					case LumiaSdkEventTypes.CHAT:
						Console.WriteLine("New chat message:  " + data.ToString());
						break;

					case LumiaSdkEventTypes.ALERT:
						Console.WriteLine("New alert:  " + data.ToString());
						break;
				}
			};

			var r = await framework.GetInfo();

			Console.WriteLine("get info result : " + r.ToString());

			await checkCommands(framework);
		}

        public static async Task checkCommands(LumiaSdk framework)
        {
            RGB Red = new RGB
            {
                r = 255,
                g = 0,
                b = 255
            };


            await framework.SendColor(Red, 100, 10);
        }
        static void Main(string[] args)
        {

			MainTask().GetAwaiter().GetResult();

        }
    }
}
