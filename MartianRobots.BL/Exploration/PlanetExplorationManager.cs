using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MartianRobots.BL.Robots;
using MartianRobots.Common.Exploration;
using MartianRobots.Common.Robots;

namespace MartianRobots.BL.Exploration
{
    internal class PlanetExplorationManager : IPlanetExplorationManager
    {
        private readonly ConcurrentDictionary<int, IPlanet> _planets;

        public PlanetExplorationManager()
        {
            _planets = new ConcurrentDictionary<int, IPlanet>();
        }
        public void AddOrUpdatePlanet(IPlanet planet)
        {
            _planets.AddOrUpdate(planet.Id, planet,(id, newPlanet) => planet); 
        }

        public IPlanet GetPlanet(int planetId)
        {
            if (!_planets.TryGetValue(planetId, out var planet))
                return null;

            return planet;
        }
        
        public List<IRobot> GetRobots(int planetId)
        {
            return _planets.TryGetValue(planetId, out var planet) ? planet.Robots : null;
        }

        public List<IRobotPosition> GetScents(int planetId)
        {
            if (!_planets.TryGetValue(planetId, out var planet)) 
                return null;

            var result = new List<IRobotPosition>();
            foreach (var scent in planet.Scents)
            {
                result.Add(new RobotPosition
                {
                    Orientation = scent.Key,
                    Coordinate = scent.Value
                });
            }

            return result;

        }


        public GridCoordinate GetPlanetGrid(int planetId)
        {
            if (!_planets.TryGetValue(planetId, out var planet))
                return null;

            return planet.Coordinate;
        }

        public double GetExplorationRatio(int planetId)
        {
            return 0; //TODO: create surfaceExplorationManager and retrieve %
        }

        public bool DeletePlanet(int planetId)
        {
            return _planets.TryRemove(planetId, out _);
        }
    }
}
