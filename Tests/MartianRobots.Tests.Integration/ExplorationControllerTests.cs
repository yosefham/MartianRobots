using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using MartianRobots.BL;
using MartianRobots.BL.Exploration;
using MartianRobots.BL.Exploration.Robots;
using MartianRobots.Common.Configuration;
using MartianRobots.Common.Exploration.Robots;
using MartianRobots.Configuration;
using MartianRobots.Controllers;
using Xunit;

namespace MartianRobots.Tests.Integration
{
    public class ExplorationControllerTests
    {
        private readonly ExplorationController _sut;
        private readonly List<string> _instructions = new List<string>() { "5   3", " 1 1 e", "RFRFRFrf", "3 2 N     ", "FRRFLLFFRRFLL" };
        private readonly List<string> _robotPositions = new List<string>() { "1 1 E", "3 3 N LOST" };

        public ExplorationControllerTests()
        {
            ISettings settings = new Settings
            {
                MaxCoordinateValue = 50,
                MaxMovementLength = 100
            };
            _sut = new ExplorationController(new ExplorationHandler(new Factory(settings), settings,
                new PlanetExplorationManager()));
        }

        [Fact(DisplayName = "should calculate final positions")]
        public void ExplorePlanet()
        {
            var instructions = _instructions; 
            var expectedResult = new ExploreResult()
            {
                Mars = new GridCoordinate() { X = 5, Y = 3 },
                Robots = _robotPositions
            };

            var result = _sut.Explore(instructions);

            result.Mars.Should().BeEquivalentTo(expectedResult.Mars);
            result.Robots.Should().BeEquivalentTo(expectedResult.Robots);
        }

        [Fact(DisplayName = "should throw an exception if incorrect format")]
        public void ExplorePlanetWithIncorrectFormat()
        {
            var instructions = _instructions;
            instructions[0] = "5 N";

            _sut.Invoking(x => x.Explore(instructions)).Should().ThrowExactly<HttpRequestException>();
        }

        [Fact(DisplayName = "should calculate final positions")]
        public void UpdatePlanet()
        {
            var instructions = _instructions;
            var expectedResult = new ExploreResult()
            {
                Mars = new GridCoordinate() { X = 5, Y = 3 },
                Robots = _robotPositions
            };
            var id = _sut.Explore(instructions).Id;

            var updateInstructions = new List<string>() {"1 1 E", "RFRFRFRF" };
            expectedResult.Robots.Add("1 1 E");
            
            var result = _sut.Update(updateInstructions, id);

            result.Mars.Should().BeEquivalentTo(expectedResult.Mars);
            result.Robots.Should().BeEquivalentTo(expectedResult.Robots);
        }

        [Fact(DisplayName = "should throw an exception if incorrect format")]
        public void UpdatePlanetWithIncorrectFormat()
        {
            var instructions = _instructions;
            instructions[0] = "5 N";

            _sut.Invoking(x => x.Update(instructions, 1 )).Should().ThrowExactly<HttpRequestException>();
        }

        [Fact(DisplayName = "Should delete a planet")]
        public void DeletePlanet()
        {
            var instructions = _instructions;
            var result = _sut.Explore(instructions);
            result.Should().NotBeNull();

            _sut.Invoking(x => x.Clear(result.Id)).Should().NotThrow();


            var status = _sut.GetStatus(result.Id);
            status.Mars.Should().BeNull();
        }
        
        [Fact(DisplayName = "Should return the planet's status")]
        public void PlanetStatus()
        {
            var instructions = _instructions;
            var expectedResult = new StatusResponse
            {
                ActiveRobots = 1,
                LostRobots = 1,
                Mars = new () { X = 5, Y = 3},
                KnownEdges = new () {"3 3 N"}
            };
            var result = _sut.Explore(instructions);
            result.Should().NotBeNull();
            expectedResult.Id = result.Id;

            StatusResponse status = null;
            _sut.Invoking(x => status = x.GetStatus(result.Id)).Should().NotThrow();


            status.Should().BeEquivalentTo(expectedResult);
        }

        [Fact(DisplayName = "Should get all active Ids")]
        public void ActivePlanet()
        {
            var instructions = _instructions;
            var planet1 = _sut.Explore(instructions);
            var planet2 = _sut.Explore(instructions);
            var planet3 = _sut.Explore(instructions);
            var ids = new List<int>() { planet1.Id, planet2.Id, planet3.Id };

            var result = _sut.ActiveId();

            result.Should().BeEquivalentTo(ids);
        }

    }
}
