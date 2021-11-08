using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MartianRobots.BL;
using MartianRobots.BL.Exploration;
using MartianRobots.BL.Exploration.Robots;
using MartianRobots.Common.Configuration;
using MartianRobots.Configuration;
using Xunit;

namespace MartianRobots.Tests.Integration
{
    public class ExplorationHandlerTests
    {
        private readonly IExplorationHandler _sut;

        public ExplorationHandlerTests()
        {
            ISettings settings = new Settings
            {
                MaxCoordinateValue = 50,
                MaxMovementLength = 100
            }; 
            _sut = new ExplorationHandler(new Factory(settings), settings, new PlanetExplorationManager());
        }

        [Theory(DisplayName = "Should process the instructions successfully")]
        [InlineData( new string[] { "5 3", "1 1 E", "RFRFRFRF" }, "1 1 E")]
        [InlineData( new string[] { "5 3", "3 2 N", "FRRFLLFFRRFLL" }, "3 3 N LOST")]
        [InlineData( new string[] { "5 3", "0 5 W", "LLFFFRFLFL" }, "0 5 W LOST")]
        [InlineData( new string[] { "5 3", "1 2 S" }, "1 2 S")]
        public void ProcessInstructions(string[] input, string finalPosition)
        {
            var instructions = input.ToList();

            var id = _sut.Process(instructions, -999);

            var grid = _sut.GetPlanetGrid(id);
            var robots = _sut.GetRobotsPosition(id);
            input[0].Should().BeEquivalentTo($"{grid.X} {grid.Y}");
            robots[0].Should().BeEquivalentTo(finalPosition);
        }

        [Fact(DisplayName = "Should return invalid id if instructions are empty")]
        public void EmptyInstructions()
        {
            var id = _sut.Process(new(), -999);

            id.Should().BeNegative();
            _sut.GetPlanetGrid(id).Should().BeNull();
        }

        [Fact(DisplayName = "should add robot to planet if is exists")]
        public void AddRobotToPlanet()
        {
            var instructions = new List<string>() { "5 3", "1 1 E", "RFRFRFRF" };
            var expectedResult = new List<string>() { "1 1 E" };

            var id = _sut.Process(instructions, 4545);
            _sut.GetRobotsPosition(id).Should().BeEquivalentTo(expectedResult);

            var newRobotInstructions = new List<string>() { "3 2 N", "FRRFLLFFRRFLL" };
            expectedResult.Add("3 3 N LOST");


            _sut.Process(newRobotInstructions, id).Should().Be(id);
            _sut.GetRobotsPosition(id).Should().BeEquivalentTo(expectedResult);
        }
    }
}
