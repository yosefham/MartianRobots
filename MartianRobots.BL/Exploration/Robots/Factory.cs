using System;
using System.Linq;
using MartianRobots.Common.Configuration;
using MartianRobots.Common.Robots;

namespace MartianRobots.BL.Robots
{
    public interface IFactory
    {
        IRobot CreateRobot(string initialPosition, GridCoordinate mars);

        GridCoordinate CreateMars(string size);
    }

    internal class Factory : IFactory
    {
        private readonly ISettings _settings;

        #region Constructor

        public Factory(ISettings settings)
        {
            _settings = settings;
        }

        #endregion

        #region Public Methods

        public IRobot CreateRobot(string initialPosition, GridCoordinate mars)
        {
            try
            {
                var inputs = initialPosition.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                IRobot robot = new Robot
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
                    $"The robot's initial position's format is incorrect. Expected: (number) (number) N|E|S|W. Received: {initialPosition}", e);
            }
        }

        public GridCoordinate CreateMars(string size)
        {
            try
            {
                var marsInput = size.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x.Trim())).ToList();

                return new()
                {
                    X = marsInput[0] > _settings.MaxCoordinateValue ? _settings.MaxCoordinateValue : marsInput[0],
                    Y = marsInput[1] > _settings.MaxCoordinateValue ? _settings.MaxCoordinateValue : marsInput[1]
                };
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    $"The grid' format is incorrect. Expected: X(number) Y(number). Received: {size}", e);
            }
        }
        #endregion

        #region Private Methods
        private OrientationType GetOrientation(string orientation) =>
            orientation switch
            {
                "N" => OrientationType.N,
                "E" => OrientationType.E,
                "S" => OrientationType.S,
                "W" => OrientationType.W,
                _ => throw new ArgumentOutOfRangeException($"The provided orientation is not supported: {orientation}")
            };
        #endregion
    }
}
