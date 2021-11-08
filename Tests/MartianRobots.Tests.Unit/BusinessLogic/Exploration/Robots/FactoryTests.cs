using System;
using FluentAssertions;
using MartianRobots.BL.Exploration.Robots;
using MartianRobots.Common.Configuration;
using MartianRobots.Common.Exploration.Robots;
using MartianRobots.Configuration;
using Xunit;

namespace MartianRobots.Tests.Unit.BusinessLogic.Exploration.Robots
{
    public class FactoryTests
    {
        private readonly IFactory _sut;

        public FactoryTests()
        {
            ISettings settings = new Settings
            {
                MaxCoordinateValue = 50,
                MaxMovementLength = 100
            };
            _sut = new Factory(settings);
        }

        [Fact(DisplayName = "Should create a robot successfully.")]
        public void CreateRobotSuccessfully()
        {
            var expectedRobot = CreateRobot(1, 1, OrientationType.N, false);
            var gridCoordinate = new GridCoordinate() {X = 2, Y = 2};
            var initialPosition = "1 1 N";

            var result = _sut.CreateRobot(initialPosition, gridCoordinate);

            result.Should().BeEquivalentTo(expectedRobot);
        }

        [Fact(DisplayName = "Should throw an exception")]
        public void WrongInputFormat()
        {
            var gridCoordinate = new GridCoordinate() { X = 2, Y = 2 };
            var initialPosition = "1 N N";

            var result = _sut.Invoking(x => x.CreateRobot(initialPosition, gridCoordinate)).Should()
                .Throw<ArgumentException>();
            
        }

        [Fact(DisplayName = "Should create mars successfully.")]
        public void CreateMarsSuccessfully()
        {
            var expectedCoordinate = new GridCoordinate() { X = 1, Y = 1 };
            var size = "1 1";

            var result = _sut.CreateMars(size);

            result.Should().BeEquivalentTo(expectedCoordinate);
        }

        [Fact(DisplayName = "Should throw an exception")]
        public void WrongInput()
        {
            var size = "1 N";

            var result = _sut.Invoking(x => x.CreateMars(size)).Should()
                .Throw<ArgumentException>();

        }


        private static Robot CreateRobot(int x, int y, OrientationType orientation, bool lost)
        {
            var robot = new Robot()
            {
                Position = new RobotPosition()
                {
                    Coordinate = new GridCoordinate()
                    {
                        X = x,
                        Y = y,
                    },
                    Orientation = orientation
                },
                Lost = lost
            };
            return robot;
        }
        

    }
}
