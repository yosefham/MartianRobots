using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MartianRobots.Common.Exploration;
using MartianRobots.Common.Exploration.Robots;

namespace MartianRobots.BL.Exploration
{
    internal class PlanetExplorationManager : IPlanetExplorationManager
    {
        private readonly ConcurrentDictionary<int, IPlanet> _planets;

        #region Constructor
        public PlanetExplorationManager()
        {
            _planets = new ConcurrentDictionary<int, IPlanet>();
        }
        #endregion

        #region Public Methods

        public void AddOrUpdatePlanet(IPlanet planet)
        {
            _planets.AddOrUpdate(planet.Id, planet,(id, newPlanet) => planet); 
        }

        public IPlanet GetPlanet(int planetId)
        {
            return _planets.TryGetValue(planetId, out var planet) ? planet : null;
        }
        
        public List<IRobot> GetRobots(int planetId)
        {
            return _planets.TryGetValue(planetId, out var planet) ? planet.Robots : null;
        }

        public Dictionary<OrientationType, GridCoordinate> GetScents(int planetId)
        {
            return _planets.TryGetValue(planetId, out var planet) ? planet.Scents : null;
        }

        public GridCoordinate GetPlanetGrid(int planetId)
        {
            return _planets.TryGetValue(planetId, out var planet) ? planet.Coordinate : null;
        }
        
        public bool DeletePlanet(int planetId)
        {
            return _planets.TryRemove(planetId, out _);
        }

        public List<int> ActivePlanets()
        {
            return _planets.Keys.ToList();
        }

        #endregion
    }
}
