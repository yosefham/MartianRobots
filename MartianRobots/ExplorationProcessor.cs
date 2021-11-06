using System;
using System.Collections.Generic;
using System.Linq;
using MartianRobots.Controllers;
using MartianRobots.Robots;

namespace MartianRobots
{
    internal class ExplorationProcessor
    {
        private const int MAX_COORDINATE_VALUE = 50;
        private const int MAX_INSTRUCTIONS_LENGTH= 100;
        private readonly RobotFactory _robotFactory;

        private readonly Dictionary<OrientationType,GridCoordinate> _scentPositions = new Dictionary<OrientationType, GridCoordinate>();

        public ExplorationProcessor(RobotFactory robotFactory)
        {
            _robotFactory = robotFactory;
        }
        public ExplorationResult Process(List<string> instructions)
        {
            if (instructions.Count < 0)
                return null;
            
            var result = new ExplorationResult();

            var marsInput = instructions[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim())).ToList();
            result.Mars = CreateMars(marsInput);

            var robots = new List<Robot>();
            for (int i = 1; i < instructions.Count; i+= 2)
            {
                var initialPosition = instructions[i];
                var movements = i < instructions.Count - 1 ? instructions[i + 1] : String.Empty;

                robots.Add(ProcessRobot(initialPosition,movements , result.Mars));
            }

            result.Robots = new List<string>();
            robots.ForEach(x => result.Robots.Add(x.ToString()));

            return result;
        }

        public void ClearData()
        {
            _scentPositions.Clear();
        }

        private Robot ProcessRobot(string initialPosition, string movements, GridCoordinate mars)
        {
            var robot = _robotFactory.CreateRobot(initialPosition, mars);

            if (string.IsNullOrEmpty(movements))
                return robot;

            if (movements.Length > MAX_INSTRUCTIONS_LENGTH)
                movements = movements.Substring(0, MAX_INSTRUCTIONS_LENGTH);

            var count = 1;
            for (int i = 1; i < movements.Length; i++)
            {
                if (movements[i] == movements[i - 1])
                {
                    count++;
                }
                else
                {
                    robot.Move(movements[i-1], count, mars);
                    count = 1;

                    if (robot.Lost)
                    {
                        if (robot.Lost = _scentPositions.TryAdd(robot.Position.Orientation, robot.Position.Coordinate)) // true => new edge point || false => already known edge => not lost
                            return robot;

                    }
                }
            }

            robot.Move(movements.Last(), count, mars);            // execute last movement command.
            if (robot.Lost)
                robot.Lost = _scentPositions.TryAdd(robot.Position.Orientation, robot.Position.Coordinate);
            

            return robot;
        }

        

        private static GridCoordinate CreateMars(List<int> marsInput) 
            => new ()
            {
                X = marsInput[0] > MAX_COORDINATE_VALUE ? MAX_COORDINATE_VALUE : marsInput[0],
                Y = marsInput[1] > MAX_COORDINATE_VALUE ? MAX_COORDINATE_VALUE : marsInput[1]
            };
        
    }
}