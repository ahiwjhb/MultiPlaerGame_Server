using Logger;
using Microsoft.Extensions.DependencyInjection;
using MultiPlayerGame.Game.RoomManager;
using Network.MessageChannel;
using Network.SteamEncipherer;

namespace MultiPlayerGame.ServicesInjection
{
    public static class Services
    {
        private static readonly IServiceCollection _services;

        private static readonly IServiceProvider _serviceProvider;

        static Services(){
            _services = new ServiceCollection();

            _services.AddSingleton<ILogger, ConsoleLogger>();
            _services.AddSingleton<IStreamEncipherer, NonStreamEncryption>();
            _services.AddSingleton<Server.Server>();
            _services.AddSingleton<RoomManager>();
            _services.AddTransient<MessageChannel>();

            _serviceProvider = _services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : notnull{
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
