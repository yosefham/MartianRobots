using System.Collections.Generic;
using MartianRobots.Common.Exploration.Robots;

namespace MartianRobots.BL
{
    public interface IExplorationHandler
    {
        /// <summary>
        /// Process a list of instructions to create a planet.
        /// If planetId exist, update existing planet
        /// </summary>
        /// <param name="instructions">list of instructions</param>
        /// <param name="planetId">planetId</param>
        /// <returns>planetId</returns>
        int Process(List<string> instructions, int planetId);

        /// <summary>
        /// Delete a planet
        /// </summary>
        /// <param name="planetId">planetId</param>
        void ClearData(int planetId);

        /// <summary>
        /// Get list of robots' positions from a planet
        /// </summary>
        /// <param name="planetId">planetId</param>
        /// <returns>Robots' positions</returns>
        List<string> GetRobotsPosition(int planetId);

        /// <summary>
        /// Get list of scent from a planet
        /// </summary>
        /// <param name="planetId">planetId</param>
        /// <returns>Robots' scents</returns>
        List<string> GetRobotsScent(int planetId);

        /// <summary>
        /// Get grid of a planet
        /// </summary>
        /// <param name="planetId">planetId</param>
        /// <returns>Planet's grid</returns>
        GridCoordinate GetPlanetGrid(int planetId);

        /// <summary>
        /// Get number of active robots in a planet
        /// </summary>
        /// <param name="planetId">planetId</param>
        /// <returns>Active robots count</returns>
        int NumberOfActiveRobots(int planetId);

        /// <summary>
        /// Get number of lost robots in a planet
        /// </summary>
        /// <param name="planetId">planetId</param>
        /// <returns>Lost robots count</returns>
        int NumberOfLostRobots(int planetId);

        /// <summary>
        /// Get all active planet Ids
        /// </summary>
        /// <returns>List of planet Ids</returns>
        List<int> GetActivePlanets();
    }
}