using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MartianRobots.BL;
using MartianRobots.BL.Exploration.Robots;
using MartianRobots.Common.Configuration;
using MartianRobots.Common.Exploration;
using MartianRobots.Common.Exploration.Robots;
using MartianRobots.Configuration;
using Moq;
using Xunit;

namespace MartianRobots.Tests.Unit.BusinessLogic
{
    public class ExplorationHandlerTests
    {
        private readonly IExplorationHandler _sut;
        private readonly Mock<IFactory> _factoryMock;
        private readonly Mock<IPlanetExplorationManager> _planetManagerMock;


        public ExplorationHandlerTests()
        {
            ISettings settings = new Settings
            {
                MaxCoordinateValue = 50,
                MaxMovementLength = 100
            };

            _factoryMock = new Mock<IFactory>();
            _planetManagerMock = new Mock<IPlanetExplorationManager>();

            _sut = new ExplorationHandler(_factoryMock.Object, settings, _planetManagerMock.Object);
        }


        [Fact(DisplayName = "should delete the specified planet")]
        public void DeleteSpecifiedPlanet()
        {
            var id = 15;

            _sut.ClearData(id);

            _planetManagerMock.Verify(x => x.DeletePlanet(id), Times.Once);
        }

        [Fact(DisplayName = "Should get robots of the specified planet")]
        public void GetRobots()
        {
            var id = 15;
            var robot = new List<IRobot>() { CreateRobot(1, 1, OrientationType.N, false) };
            var expectedResult = robot.Select(x => x.ToString()).ToList();
            _planetManagerMock.Setup(x => x.GetRobots(It.IsAny<int>())).Returns(robot);

            var result = _sut.GetRobotsPosition(id);

            _planetManagerMock.Verify(x => x.GetRobots(id), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact(DisplayName = "should get scent of the specified planet")]
        public void GetScents()
        {
            var id = 15;
            var scents = new Dictionary<OrientationType, GridCoordinate>()
            {
                { OrientationType.E, new() { X = 1, Y = 1 } },
                { OrientationType.N, new() { X = 2, Y = 2 } }
            };
            var expectedResult = scents.Select(x => $"{x.Value.X} {x.Value.Y} {x.Key}").ToList();
            _planetManagerMock.Setup(x => x.GetScents(It.IsAny<int>())).Returns(scents);

            var result = _sut.GetRobotsScent(id);

            _planetManagerMock.Verify(x => x.GetScents(id), Times.Once);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact(DisplayName = "should get the grid of the specified planet")]
        public void GetGrid()
        {
            var id = 15;
            var grid = new GridCoordinate() { X = 1, Y = 1 };
            _planetManagerMock.Setup(x => x.GetPlanetGrid(It.IsAny<int>())).Returns(grid);

            var result = _sut.GetPlanetGrid(id);

            _planetManagerMock.Verify(x => x.GetPlanetGrid(id), Times.Once);
            result.Should().BeEquivalentTo(grid);
        }

        [Fact(DisplayName = "should return the number of active robots of the specified planet")]
        public void GetActiveRobots()
        {
            var id = 15;
            var robot = new List<IRobot>() { CreateRobot(1, 1, OrientationType.N, false), CreateRobot(1, 1, OrientationType.E, true) };
            var activeRobots = 1;
            _planetManagerMock.Setup(x => x.GetRobots(It.IsAny<int>())).Returns(robot);

            var result = _sut.NumberOfActiveRobots(id);

            _planetManagerMock.Verify(x => x.GetRobots(id), Times.Once);
            result.Should().Be(activeRobots);
        }

        [Fact(DisplayName = "should return the number of lost robots of the specified planet")]
        public void GetLostRobots()
        {
            var id = 15;
            var robot = new List<IRobot>() { CreateRobot(1, 1, OrientationType.N, false), CreateRobot(1, 1, OrientationType.E, true) };
            var activeRobots = 1;
            _planetManagerMock.Setup(x => x.GetRobots(It.IsAny<int>())).Returns(robot);

            var result = _sut.NumberOfLostRobots(id);

            _planetManagerMock.Verify(x => x.GetRobots(id), Times.Once);
            result.Should().Be(activeRobots);
        }

        [Fact(DisplayName = "Should get all active Ids")]
        public void ActiveIds()
        {
            var ids = new List<int>() { 1, 2, 3 };
            _planetManagerMock.Setup(x => x.ActivePlanets()).Returns(ids);

            var result = _sut.GetActivePlanets();

            _planetManagerMock.Verify(x => x.ActivePlanets(), Times.Once);
            result.Should().BeEquivalentTo(ids);
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

        private static GridCoordinate CreateMars()
        {
            return new GridCoordinate();
        }
    }
    
}
