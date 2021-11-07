using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MartianRobots.BL.Robots;
using MartianRobots.Common.Robots;
using Moq;
using Xunit;

namespace MartianRobots.Tests.Unit.BusinessLogic.Robots
{
    public class RobotTests
    {
        [Theory(DisplayName = "Should format the values to string correctly")]
        [InlineData(true)]
        [InlineData(false)]
        public void ExpectedStringFormat(bool lost)
        {
            var robot = CreateRobot(1,1,OrientationType.E, lost);
            var expectedOutput =
                $"{robot.Position.Coordinate.X} {robot.Position.Coordinate.Y} {robot.Position.Orientation}" 
                + (lost ? " LOST" : string.Empty);

            var result = robot.ToString();

            result.Should().BeEquivalentTo(expectedOutput);
        }

        [Theory(DisplayName = "Should return if robot is lost.")]
        [InlineData(1, 1, false)]
        [InlineData(3, 1, true)]
        [InlineData(1, 3, true)]
        [InlineData(3, 3, true)]
        [InlineData(-1, 1, true)]
        [InlineData(1, -1, true)]
        [InlineData(-1, -1, true)]
        public void IsLost(int x, int y, bool lost)
        {
            var robot = CreateRobot(x, y, OrientationType.N, false);
            var marsGrid = new GridCoordinate() {X = 2, Y = 2};

            var result = robot.IsRobotLost(marsGrid);

            result.Should().Be(lost);
        }

        [Theory(DisplayName = "should rotate to the correct side.")]
        [InlineData(OrientationType.N, 'L', OrientationType.W)]
        [InlineData(OrientationType.N, 'R', OrientationType.E)]
        public void HandleCommand_L(OrientationType startingOrientation, char movement, OrientationType finalOrientation)
        {
            var robot = CreateRobot(1, 1, startingOrientation, false);
            var steps = 1;
            var marsGrid = new GridCoordinate() { X = 2, Y = 2 };

            robot.Invoking(x => x.Move(movement, steps, marsGrid)).Should().NotThrow();

            robot.Position.Orientation.Should().Be(finalOrientation);
            robot.IsRobotLost(marsGrid).Should().BeFalse();
        }


        [Theory(DisplayName = "should move forward.")]
        [InlineData(1, false, 1, 2)]
        [InlineData(2, true, 1, 2)]
        public void HandleCommand_F(int steps, bool lost, int finalX, int finalY)
        {
            var expectedOrientation = OrientationType.N;
            var robot = CreateRobot(1, 1, expectedOrientation, false);
            var marsGrid = new GridCoordinate() { X = 2, Y = 2 };

            robot.Invoking(x => x.Move('F', steps, marsGrid)).Should().NotThrow();

            robot.Position.Orientation.Should().Be(expectedOrientation);
            robot.Lost.Should().Be(lost);
            robot.Position.Coordinate.Should().BeEquivalentTo(new GridCoordinate() {X = finalX, Y = finalY});

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
