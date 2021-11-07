using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MartianRobots.BL.Robots;
using MartianRobots.Common.Robots;
using Xunit;

namespace MartianRobots.Tests.Unit.BusinessLogic.Robots
{
    public class RobotPositionTests
    {
        [Theory(DisplayName = "Should Rotate left correctly.")]
        [InlineData(1, OrientationType.W)]
        [InlineData(3, OrientationType.E)]
        [InlineData(7, OrientationType.E)]
        [InlineData(-1, OrientationType.E)]     //rotate right
        [InlineData(-5, OrientationType.E)]     //rotate right
        public void ShouldRotateLeft(int steps, OrientationType expectedOrientation)
        {
            var position = CreatePosition(1, 1, OrientationType.N);

            position.RotateLeft(steps);

            position.Orientation.Should().Be(expectedOrientation);

        }

        [Theory(DisplayName = "Should Rotate right correctly.")]
        [InlineData(1, OrientationType.E)]
        [InlineData(3, OrientationType.W)]
        [InlineData(7, OrientationType.W)]
        [InlineData(-1, OrientationType.W)]     //rotate left
        [InlineData(-5, OrientationType.W)]     //rotate left
        public void ShouldRotateRight(int steps, OrientationType expectedOrientation)
        {
            var position = CreatePosition(1, 1, OrientationType.N);

            position.RotateRight(steps);

            position.Orientation.Should().Be(expectedOrientation);

        }


        [Theory(DisplayName = "Should move forward successfully.")]
        [InlineData(1, 1, OrientationType.N, 1, 2)]
        [InlineData(1, 1, OrientationType.E, 2, 1)]
        [InlineData(1, 1, OrientationType.S, 1, 0)]
        [InlineData(1, 1, OrientationType.W, 0, 1)]
        public void MoveForward(int x, int y, OrientationType orientation, int finalX, int finalY)
        {
            var position = CreatePosition(x, y, orientation);
            var expectedPosition = CreatePosition(finalX, finalY, orientation);

            position.Invoking(x => x.MoveForward(1)).Should().NotThrow();

            position.Should().BeEquivalentTo(expectedPosition);
        }

        [Fact(DisplayName = "Should Throw an exception.")]
        public void IncorrectOrientation()
        {
            var position = CreatePosition(1, 1, (OrientationType) 999);

            position.Invoking(x => x.MoveForward(1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        private static RobotPosition CreatePosition(int x, int y, OrientationType orientation)
        {
            var position = new RobotPosition()
            {
                Coordinate = new GridCoordinate()
                {
                    X = x,
                    Y = y
                },
                Orientation = orientation
            };
            return position;
        }
    }
}
