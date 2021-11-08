using System.Collections.Generic;
using FluentAssertions;
using MartianRobots.BL.Exploration;
using MartianRobots.BL.Exploration.Robots;
using MartianRobots.Common.Exploration;
using MartianRobots.Common.Exploration.Robots;
using Xunit;

namespace MartianRobots.Tests.Unit.BusinessLogic.Exploration
{
    public class PlanetExplorationManagerTests
    {
        private readonly IPlanetExplorationManager _sut;

        public PlanetExplorationManagerTests()
        {
            _sut = new PlanetExplorationManager();
        }

        [Fact(DisplayName = "Should add a planet correctly")]
        public void AddAPlanet()
        {
            var planet = CreatePlanet(1, 1, new (), new ());
            _sut.AddOrUpdatePlanet(planet);

            var result = _sut.GetPlanet(planet.Id);

            result.Should().BeEquivalentTo(planet);
        }

        [Fact(DisplayName = "should return the robots of a planet")]
        public void RetrieveRobots()
        {
            var robots = new List<IRobot>()
                { CreateRobot(1, 2, OrientationType.N, false), CreateRobot(2, 2, OrientationType.E, true) };
            var planet = CreatePlanet(2, 3, robots, new());
            _sut.AddOrUpdatePlanet(planet);

            var result = _sut.GetRobots(planet.Id);

            result.Should().BeEquivalentTo(robots);
        }

        [Fact(DisplayName = "should return the robots' scents")]
        public void RetrieveScents()
        {
            var scents = new Dictionary<OrientationType, GridCoordinate>()
            {
                { OrientationType.E, new() { X = 1, Y = 1 } },
                { OrientationType.N, new() { X = 2, Y = 2 } }
            };
            var planet = CreatePlanet(1, 2, new(), scents);
            _sut.AddOrUpdatePlanet(planet);

            var result = _sut.GetScents(planet.Id);

            result.Should().BeEquivalentTo(scents);
        }


        [Fact(DisplayName = "Should return the planet's grid")]
        public void RetrievePlanetGrid()
        {
            var grid = new GridCoordinate() { X = 1, Y = 2 };
            var planet = CreatePlanet(grid.X, grid.Y, new(), new());
            _sut.AddOrUpdatePlanet(planet);

            var result = _sut.GetPlanetGrid(planet.Id);

            result.Should().BeEquivalentTo(grid);
        }

        [Fact(DisplayName = "Should return null if planet doesn't exist")]
        public void RetrieveInvalidPlanetId()
        {
            var inputPlanet = CreatePlanet(1, 2, new(), new());
            _sut.AddOrUpdatePlanet(inputPlanet);
            var invalidId = -999;

            var planet = _sut.GetPlanet(invalidId);
            var robots = _sut.GetRobots(invalidId);
            var scents = _sut.GetScents(invalidId);
            var grid = _sut.GetPlanetGrid(invalidId);


            planet.Should().BeNull();
            robots.Should().BeNull();
            scents.Should().BeNull();
            grid.Should().BeNull();
        }

        [Fact(DisplayName = "should delete a planet successfully")]
        public void DeletePlanet()
        {
            var planet = CreatePlanet(1, 1, new(), new());
            _sut.AddOrUpdatePlanet(planet);
            _sut.GetPlanet(planet.Id).Should().BeEquivalentTo(planet);

            _sut.DeletePlanet(planet.Id).Should().BeTrue();
            
            _sut.GetPlanet(planet.Id).Should().BeNull();
        }

        [Fact(DisplayName = "should get all active Ids")]
        public void ActivePlanets()
        {
            var planet1 = CreatePlanet(1, 1, new(), new());
            var planet2 = CreatePlanet(2, 3, new(), new());
            var planet3 = CreatePlanet(3, 3, new(), new());

            var ids = new List<int>() { planet1.Id, planet2.Id, planet3.Id };

            _sut.AddOrUpdatePlanet(planet1);
            _sut.AddOrUpdatePlanet(planet2);
            _sut.AddOrUpdatePlanet(planet3);

            var result = _sut.ActivePlanets();

            result.Should().BeEquivalentTo(ids);
        }


        private IPlanet CreatePlanet(int x, int y, List<IRobot> robots, Dictionary<OrientationType, GridCoordinate> scents)
        {
            return new Planet
            {
                Coordinate = new GridCoordinate
                {
                    X = x,
                    Y = y
                },
                Robots = new List<IRobot>(robots),
                Scents = new Dictionary<OrientationType, GridCoordinate>(scents)
            };
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
