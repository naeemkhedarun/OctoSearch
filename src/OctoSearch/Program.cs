using System;
using LightInject;
using OctoSearch.Cache;
using OctoSearch.Login;
using OctoSearch.Search;

namespace OctoSearch
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var container = new ServiceContainer();
            container.SetDefaultLifetime<PerContainerLifetime>();
            container.Register<CommandProcessor>();
            container.Register<CacheRepository>();
            container.Register<LoginCommandHandler>();
            container.Register<CacheCommandHandler>();
            container.Register<SearchCommandHandler>();

            var process = container.GetInstance<CommandProcessor>().Process(args);

            Environment.Exit(process);

            return process;
        }
    }
}
