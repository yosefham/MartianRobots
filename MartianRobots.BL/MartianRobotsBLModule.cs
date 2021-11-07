using MartianRobots.BL.Robots;
using Microsoft.Extensions.DependencyInjection;

namespace MartianRobots.BL
{
    public static class MartianRobotsBLModule
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddSingleton<IFactory, Factory>();
            services.AddSingleton<IExplorationHandler, ExplorationHandler>();

            return services;
        }
    }
}
