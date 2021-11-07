using System;
using System.Collections.Generic;
using System.Linq;
using MartianRobots.BL.Robots;
using MartianRobots.BL.Settings;

namespace MartianRobots.BL
{
    public interface IExplorationHandler
    {
        void Process(List<string> instructions);
        void ClearData();
        List<string> GetRobotsPosition();
        List<string> GetRobotsScent();
        GridCoordinate GetMarsGrid();
        int NumberOfActiveRobots();
        int NumberOfLostRobots();
        double PercentageOfExploration();

    }

    internal class ExplorationHandler : IExplorationHandler
    {
        #region Members
        private readonly IFactory _factory;
        private readonly ISettings _settings;
        private readonly Dictionary<OrientationType,GridCoordinate> _scentPositions = new ();
        private readonly List<IRobot> _robots = new ();
        private GridCoordinate _marsGrid = new ();
        #endregion

        #region Constructor
        public ExplorationHandler(IFactory factory, ISettings settings)
        {
            _factory = factory;
            _settings = settings;
        }
        #endregion

        #region Public Methods

        public void Process(List<string> instructions)
        {
            if (instructions.Count < 0)
                return;
            
            _marsGrid = _factory.CreateMars(instructions[0]);
            ProcessMovement(instructions);
        }

        public void ClearData()
        {
            _scentPositions.Clear();
            _robots.Clear();
            _marsGrid = new GridCoordinate();
        }

        public List<string> GetRobotsPosition()
        {
            return _robots.Select(x => x.ToString()).ToList();
        }

        public List<string> GetRobotsScent()
        {
            return _scentPositions.Select(x => $"{x.Value.X} {x.Value.Y} {x.Key}").ToList();
        }

        public GridCoordinate GetMarsGrid()
        {
            return _marsGrid;
        }

        public int NumberOfActiveRobots()
        {
            return _robots.Count(x => !x.Lost);
        }

        public int NumberOfLostRobots()
        {
            return _robots.Count(x => x.Lost);
        }

        public double PercentageOfExploration()
        {
            return 0; //TODO: create surfaceExplorationManager and retrieve %
        }

        #endregion

        #region Private Methods

        private void ProcessMovement(List<string> instructions)
        {
            for (int i = 1; i < instructions.Count; i += 2)
            {
                var initialPosition = instructions[i];
                var movements = i < instructions.Count - 1 ? instructions[i + 1] : String.Empty;

                _robots.Add(MoveRobot(initialPosition, movements));
            }
            
        }

        private IRobot MoveRobot(string initialPosition, string movements)
        {
            var robot = _factory.CreateRobot(initialPosition, _marsGrid);

            if (string.IsNullOrEmpty(movements))
                return robot;

            if (movements.Length > _settings.MaxMovementLength)
                movements = movements.Substring(0, _settings.MaxMovementLength);

            var count = 1;
            for (int i = 1; i < movements.Length; i++)
            {
                if (movements[i] == movements[i - 1])
                {
                    count++;
                }
                else
                {
                    robot.Move(movements[i-1], count, _marsGrid);
                    count = 1;

                    if (robot.Lost)
                    {
                        if (robot.Lost = _scentPositions.TryAdd(robot.Position.Orientation, robot.Position.Coordinate)) // true => new edge point || false => already known edge => not lost
                            return robot;

                    }
                }
            }

            robot.Move(movements.Last(), count, _marsGrid);            // execute last movement command.
            if (robot.Lost)                                              // check if robot was lost during last movement.
                robot.Lost = _scentPositions.TryAdd(robot.Position.Orientation, robot.Position.Coordinate);
            
            return robot;
        }
        
        #endregion
    }
}