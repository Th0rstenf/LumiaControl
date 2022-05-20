using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Lumia;
using System.Net.Sockets;
using System.Net;
using System.Text;

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
                id = "bfe94a8ad680170181ejrq"
            };
            right = new ILumiaLight
            {
                type = LightBrands.TUYA,
                id = "bf172f7cd299bace36qppv"
            };

            builder.addToLightGroup(right, CommandBuilder.group.RIGHT);
            builder.addToLightGroup(left, CommandBuilder.group.LEFT);

            await debugOutput(framework);

            await startServer(framework, builder, 1337).ConfigureAwait(false);
            /*
            while (true)
            {
                Console.WriteLine("Enter string");
                string str = System.Console.ReadLine();
                Command cmd = builder.analyze(str);
                if (cmd.isValid())
                {
                    cmd.execute();
                }
                CommandBuilder.LogData log = builder.getLatestLog();
                if (log.type == CommandBuilder.LogData.Type.ERROR)
                {
                    Console.WriteLine(log.msg);
                }
            }
            */
        }

        public static async Task startServer(LumiaSdk frameWork, CommandBuilder builder, int port)
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = iPHostEntry.AddressList[1];
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);
            listener.Bind(localEndPoint);
            listener.Listen(1);
            
            while(true)
            {
                Console.WriteLine("Waiting for connections on {0}", listener.LocalEndPoint.ToString());
                Socket handle = listener.Accept();
                Console.WriteLine("Accepted connection from :" + handle.RemoteEndPoint.ToString());
                string data = null;
                var bytes = new byte[1024];

                int bytesReceived = handle.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                
                Console.WriteLine("Received: {0}", data);

                Command cmd = builder.analyze(data);
                if (cmd.isValid())
                {
                    cmd.execute();
                }
                CommandBuilder.LogData log = builder.getLatestLog();
                if (log.msg != null)
                {
                    handle.Send(Encoding.ASCII.GetBytes(log.msg));
                }
                handle.Shutdown(SocketShutdown.Both);
                handle.Close();         
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

        static void Main()
        {
			MainTask().GetAwaiter().GetResult();
        }
    }
}
