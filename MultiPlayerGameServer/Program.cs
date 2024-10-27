using MultiPlayerGame.Server;
using MultiPlayerGame.ServicesInjection;
using Logger;

internal class Program
{
    private static void Main(string[] args) {
        Console.SetBufferSize(1024, 4096);
        ILogger logger = Services.GetService<ILogger>();

        logger.Log("初始化服务器中");
        Server server = Services.GetService<Server>();
        logger.Log("初始化服务器完毕");
        server.StartServer(7758);
    }
}