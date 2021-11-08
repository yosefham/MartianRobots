using System.Collections.Generic;
using MartianRobots.Common.Exploration.Robots;

namespace MartianRobots.Common.Exploration
{
    public interface IPlanetExplorationManager
    {
        /// <summary>
        /// Add a planet
        /// </summary>
        /// <param name="planet">Planet to add</param>
        void AddOrUpdatePlanet(IPlanet planet);

        /// <summary>
        /// Get a planet by Id
        /// </summary>
        /// <param name="planetId">Id</param>
        /// <returns>Planet with planetId or null if not found</returns>
        IPlanet GetPlanet(int planetId);

        /// <summary>
        /// Get a planet's robots
        /// </summary>
        /// <param name="planetId">Id</param>
        /// <returns>List of robots of planetId or null if not found</returns>
        List<IRobot> GetRobots(int planetId);

        /// <summary>
        /// Get a planet's scents
        /// </summary>
        /// <param name="planetId">Id</param>
        /// <returns>List of scents of planetId or null if not found</returns>
        Dictionary<OrientationType, GridCoordinate> GetScents(int planetId);

        /// <summary>
        /// Get a planet's grid
        /// </summary>
        /// <param name="planetId">Id</param>
        /// <returns>Grid of planetId or null if not found</returns>
        GridCoordinate GetPlanetGrid(int planetId);
        
        /// <summary>
        /// Delete a planet by Id
        /// </summary>
        /// <param name="planetId">Id</param>
        /// <returns>True if success, otherwise, false</returns>
        bool DeletePlanet(int planetId);

        /// <summary>
        /// Get all planet Ids
        /// </summary>
        /// <returns>Planet Ids</returns>
        List<int> ActivePlanets();
    }
}
