using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MartianRobots.BL.Exploration;
using MartianRobots.BL.Exploration.Robots;
using MartianRobots.Common.Configuration;
using MartianRobots.Common.Exploration;
using MartianRobots.Common.Exploration.Robots;

namespace MartianRobots.BL
{
    internal class ExplorationHandler : IExplorationHandler
    {
        #region Members
        private readonly IFactory _factory;
        private readonly ISettings _settings;
        private readonly IPlanetExplorationManager _planetManager;
        private IPlanet _planet;
        #endregion

        #region Constructor
        public ExplorationHandler(IFactory factory, ISettings settings, IPlanetExplorationManager planetManager)
        {
            _factory = factory;
            _settings = settings;
            _planetManager = planetManager;
        }
        #endregion

        #region Public Methods

        public int Process(List<string> instructions, int planetId)
        {
            if (instructions.Count <= 0)
                return planetId;

            if ((_planet = _planetManager.GetPlanet(planetId)) == null || Regex.IsMatch(instructions[0], "^[0-9]+\\s+[0-9]+$"))
            {
                if (_planet == null)
                {
                    _planet = new Planet();
                    _planet.Coordinate = _factory.CreateMars(instructions[0]);
                }
                instructions.RemoveAt(0);               // Remove planet grid input
            }


            ProcessMovement(instructions);

            return _planet.Id;
        }


        public void ClearData(int planetId)
        {
            _planetManager.DeletePlanet(planetId);
        }

        public List<string> GetRobotsPosition(int planetId)
        {
            return _planetManager.GetRobots(planetId).Select(x => x.ToString()).ToList();
        }

        public List<string> GetRobotsScent(int planetId)
        {
            return _planetManager.GetScents(planetId)?.Select(x => $"{x.Value.X} {x.Value.Y} {x.Key}").ToList();
        }

        public GridCoordinate GetPlanetGrid(int planetId)
        {
            return _planetManager.GetPlanetGrid(planetId);
        }

        public int NumberOfActiveRobots(int planetId)
        {
            return _planetManager.GetRobots(planetId)?.Count(x => !x.Lost) ?? 0;
        }

        public int NumberOfLostRobots(int planetId)
        {
            return _planetManager.GetRobots(planetId)?.Count(x => x.Lost) ?? 0;
        }

        public List<int> GetActivePlanets()
        {
            return _planetManager.ActivePlanets();
        }

        #endregion

        #region Private Methods

        private void ProcessMovement(List<string> instructions)
        {
            for (int i = 0; i < instructions.Count; i += 2)
            {
                var initialPosition = instructions[i];
                var movements = i < instructions.Count - 1 ? instructions[i + 1] : String.Empty;

                _planet.Robots.Add(MoveRobot(initialPosition, movements));
            }

            _planetManager.AddOrUpdatePlanet(_planet);
        }

        private IRobot MoveRobot(string initialPosition, string movements)
        {
            var robot = _factory.CreateRobot(initialPosition, _planet.Coordinate);

            if (string.IsNullOrEmpty(movements))
                return robot;

            if (movements.Length > _settings.MaxMovementLength)
                movements = movements.Substring(0, _settings.MaxMovementLength);

            var count = 1;
            for (int i = 1; i < movements.Length; i++)
            {
                if (movements[i] == movements[i - 1])
                    count++;
                else
                {
                    robot.Move(movements[i-1], count, _planet.Coordinate);
                    count = 1;

                    if (robot.Lost)
                        if (robot.Lost = _planet.Scents.TryAdd(robot.Position.Orientation, robot.Position.Coordinate)) // true => new edge point || false => already known edge => not lost
                            return robot;
                }
            }

            robot.Move(movements.Last(), count, _planet.Coordinate);            // execute last movement command.
            if (robot.Lost)                                              // check if robot was lost during last movement.
                robot.Lost = _planet.Scents.TryAdd(robot.Position.Orientation, robot.Position.Coordinate);
            
            return robot;
        }
        
        #endregion
    }
}