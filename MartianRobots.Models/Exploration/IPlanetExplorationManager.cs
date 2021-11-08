using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MartianRobots.Common.Robots;

namespace MartianRobots.Common.Exploration
{
    public interface IPlanetExplorationManager
    {
        void AddOrUpdatePlanet(IPlanet planet);
        IPlanet GetPlanet(int planetId);
        List<IRobot> GetRobots(int planetId);
        List<IRobotPosition> GetScents(int planetId);
        GridCoordinate GetPlanetGrid(int planetId);
        double GetExplorationRatio(int planetId);
        bool DeletePlanet(int planetId);

    }
}
