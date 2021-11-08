using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using MartianRobots.BL;
using MartianRobots.Common.Exploration.Robots;
using MartianRobots.Controllers;
using Moq;
using Xunit;

namespace MartianRobots.Tests.Unit.Controllers
{
    public class ExplorationControllerTests
    {
        private readonly Mock<IExplorationHandler> _handlerMock;
        private readonly ExplorationController _sut;

        public ExplorationControllerTests()
        {
            _handlerMock = new Mock<IExplorationHandler>();

            _sut = new ExplorationController(_handlerMock.Object);
        }


        [Fact(DisplayName = "Should return null if explore request is empty.")]
        public void EmptyExploreRequest()
        {
            var request = new List<string>();

            var response1 = _sut.Explore(request);
            var response2 = _sut.Explore(null);

            response1.Should().BeNull();
            response2.Should().BeNull();
            _handlerMock.Verify(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>()), Times.Never);
        }

        [Fact(DisplayName = "Should Process the request correctly")]
        public void ProcessExploreRequest()
        {
            List<string> request = new() { "anything", "anything", "anything" };
            var expectedResult = new ExploreResult { Robots = new List<string>(){ "1 1 N" }, Mars = new GridCoordinate(){X = 1, Y = 1} };
            _handlerMock.Setup(x => x.GetPlanetGrid(It.IsAny<int>())).Returns(expectedResult.Mars);
            _handlerMock.Setup(x => x.GetRobotsPosition(It.IsAny<int>())).Returns(expectedResult.Robots);


            ExploreResult response = null;
            _sut.Invoking(x => response = x.Explore(request)).Should().NotThrow();

            response.Should().BeEquivalentTo(expectedResult);
            _handlerMock.Verify(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>()), Times.Once);
            _handlerMock.Verify(x => x.GetPlanetGrid(It.IsAny<int>()), Times.Once);
            _handlerMock.Verify(x => x.GetRobotsPosition(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Should Throw an exception.")]
        public void HandleExploringException()
        {
            List<string> request = new() { "anything", "anything", "anything" };
            _handlerMock.Setup(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>())).Throws(new Exception());

            _sut.Invoking(x => x.Explore(request)).Should().ThrowExactly<HttpRequestException>();

            _handlerMock.Verify(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>()), Times.Once);
        }
        
        [Fact(DisplayName = "Should return null if update request is empty.")]
        public void EmptyUpdateRequest()
        {
            var request = new List<string>();

            var response1 = _sut.Update(request, 1);
            var response2 = _sut.Update(null,1);

            response1.Should().BeNull();
            response2.Should().BeNull();
            _handlerMock.Verify(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>()), Times.Never);
        }

        [Fact(DisplayName = "Should Process the request correctly")]
        public void ProcessUpdateRequest()
        {
            List<string> request = new() { "anything", "anything", "anything" };
            var expectedResult = new ExploreResult { Robots = new List<string>(){ "1 1 N" }, Mars = new GridCoordinate(){X = 1, Y = 1} };
            _handlerMock.Setup(x => x.GetPlanetGrid(It.IsAny<int>())).Returns(expectedResult.Mars);
            _handlerMock.Setup(x => x.GetRobotsPosition(It.IsAny<int>())).Returns(expectedResult.Robots);


            ExploreResult response = null;
            _sut.Invoking(x => response = x.Update(request, 1)).Should().NotThrow();

            response.Mars.Should().BeEquivalentTo(expectedResult.Mars);
            response.Robots.Should().BeEquivalentTo(expectedResult.Robots);
            _handlerMock.Verify(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>()), Times.Once);
            _handlerMock.Verify(x => x.GetPlanetGrid(It.IsAny<int>()), Times.Once);
            _handlerMock.Verify(x => x.GetRobotsPosition(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Should Throw an exception.")]
        public void HandleUpdatingException()
        {
            List<string> request = new() { "anything", "anything", "anything" };
            _handlerMock.Setup(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>())).Throws(new Exception());

            _sut.Invoking(x => x.Update(request, 1)).Should().ThrowExactly<HttpRequestException>();

            _handlerMock.Verify(x => x.Process(It.IsAny<List<string>>(), It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "should call ClearData method successfully")]
        public void ClearDataSuccessfully()
        {
            _sut.Invoking(x => x.Clear(1)).Should().NotThrow();

            _handlerMock.Verify(x => x.ClearData(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "should throw and exception")]
        public void HandleClearException()
        {
            _handlerMock.Setup(x => x.ClearData(It.IsAny<int>())).Throws(new Exception());

            _sut.Invoking(x => x.Clear(1)).Should().ThrowExactly<HttpRequestException>();

            _handlerMock.Verify(x => x.ClearData(It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Should get all active Ids")]
        public void ActiveIds()
        {
            _sut.Invoking(x => x.ActiveId()).Should().NotThrow();

            _handlerMock.Verify(x => x.GetActivePlanets(), Times.Once);
        }

        [Fact(DisplayName = "Should get the corresponding status info")]
        public void GetStatus()
        {
            var expectedResult = new StatusResponse
            {
                Id = 1,
                ActiveRobots = 5,
                LostRobots = 2,
                Mars = new GridCoordinate() { X = 3, Y = 5 },
                KnownEdges = new List<string>() { "1 0 S", "3 1 E" }
            };
            _handlerMock.Setup(x => x.NumberOfActiveRobots(It.IsAny<int>())).Returns(expectedResult.ActiveRobots);
            _handlerMock.Setup(x => x.NumberOfLostRobots(It.IsAny<int>())).Returns(expectedResult.LostRobots);
            _handlerMock.Setup(x => x.GetRobotsScent(It.IsAny<int>())).Returns(expectedResult.KnownEdges);
            _handlerMock.Setup(x => x.GetPlanetGrid(It.IsAny<int>())).Returns(expectedResult.Mars);

            StatusResponse response = null;
            _sut.Invoking(x => response = x.GetStatus(expectedResult.Id)).Should().NotThrow();
            
            _handlerMock.Verify(x => x.NumberOfActiveRobots(It.IsAny<int>()), Times.Once);
            _handlerMock.Verify(x => x.NumberOfLostRobots(It.IsAny<int>()), Times.Once);
            _handlerMock.Verify(x => x.GetRobotsScent(It.IsAny<int>()), Times.Once);
            _handlerMock.Verify(x => x.GetPlanetGrid(It.IsAny<int>()), Times.Once);

            response.Should().BeEquivalentTo(expectedResult);
        }


    }
}
