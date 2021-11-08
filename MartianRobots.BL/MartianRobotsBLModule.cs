﻿using MartianRobots.BL.Exploration;
using MartianRobots.BL.Robots;
using MartianRobots.Common.Exploration;
using Microsoft.Extensions.DependencyInjection;

namespace MartianRobots.BL
{
    public static class MartianRobotsBLModule
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddTransient<IFactory, Factory>();
            services.AddTransient<IExplorationHandler, ExplorationHandler>();
            services.AddSingleton<IPlanetExplorationManager, PlanetExplorationManager>();

            return services;
        }
    }
}
