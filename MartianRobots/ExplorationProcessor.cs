using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MartianRobots.Models;

namespace MartianRobots.Controllers
{
    internal class ExplorationProcessor
    {
        public ExplorationResult Process(List<string> instructions)
        {
            if (instructions.Count < 0)
                return null;
            
            var result = new ExplorationResult();

            var marsInput = instructions[0].Split("", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => int.Parse(x.Trim())).ToList();
            result.Mars = CreateMars(marsInput);
            var robots = new List<Robot>();
            for (int i = 1; i < instructions.Count; i+= 2)
            {
                robots.Add(ProcessRobot(instructions[i], i < instructions.Count -1 ? instructions[i+1] : String.Empty, result.Mars));
            }



            return result;
        }

        private Robot ProcessRobot(string initialPosistion, string movements, GridCoordinate mars)
        {
            var robot = CreateRobot(initialPosistion, mars);

            if (string.IsNullOrEmpty(movements))
                return robot;

            for (int i = 1; i < movements.Length && !robot.Lost; i++)
            {
                robot.Move(movements[i], 1);
            }

            return robot;
        }

        private Robot CreateRobot(string initialPosistion, GridCoordinate mars)
        {
            try
            {
                var inputs = initialPosistion.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                var robot = new Robot
                {
                    Position = new RobotPosition()
                    {
                        Coordinate = new GridCoordinate()
                        {

                        
                        X = int.Parse(inputs[0]),
                        Y = int.Parse(inputs[1])
                        },
                        Orientation = GetOrientation(inputs[2])
                    }
                };
                robot.Lost = IsRobotLost(robot.Position.Coordinate, mars);

                return robot;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    $"The robot's initial position's format is incorrect. Expected: (number) (number) N|E|S|W. Received: {initialPosistion}");
            }
        }

        private bool IsRobotLost(GridCoordinate robotPosition, GridCoordinate mars)
            => robotPosition.X > mars.X || robotPosition.Y > mars.Y;

        private OrientationType GetOrientation(string orientation) =>
            orientation switch
            {
                "N" => OrientationType.N,
                "E" => OrientationType.E,
                "S" => OrientationType.S,
                "W" => OrientationType.W,
                _ => throw new ArgumentOutOfRangeException($"The provided orientation is not supported: {orientation}")
            };

        private static GridCoordinate CreateMars(List<int> marsInput) 
            => new ()
            {
                X = marsInput[0],
                Y = marsInput[1]
            };
        
    }
}