using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MartianRobots.BL;
using MartianRobots.BL.Robots;
using MartianRobots.Common.Configuration;
using MartianRobots.Common.Exploration;
using MartianRobots.Common.Robots;
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

        [Fact(DisplayName = "Should process the instructions correctly")]
        public void ProcessSuccesfully()
        {
            
        }


        [Fact(DisplayName = "Should clear all the saved data.")]
        public void ClearData()
        {

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
