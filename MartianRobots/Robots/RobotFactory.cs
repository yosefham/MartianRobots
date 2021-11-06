using System;
using System.Linq;

namespace MartianRobots.Robots
{
    public class RobotFactory
    {
        public Robot CreateRobot(string initialPosition, GridCoordinate mars)
        {
            try
            {
                var inputs = initialPosition.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
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
                robot.Lost = robot.IsRobotLost(mars);

                return robot;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    $"The robot's initial position's format is incorrect. Expected: (number) (number) N|E|S|W. Received: {initialPosition}");
            }
        }

        private OrientationType GetOrientation(string orientation) =>
            orientation switch
            {
                "N" => OrientationType.N,
                "E" => OrientationType.E,
                "S" => OrientationType.S,
                "W" => OrientationType.W,
                _ => throw new ArgumentOutOfRangeException($"The provided orientation is not supported: {orientation}")
            };
    }
}
