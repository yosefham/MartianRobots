using System;
using System.Collections.Generic;
using MartianRobots.Common.Exploration;
using MartianRobots.Common.Exploration.Robots;

namespace MartianRobots.BL.Exploration
{
    internal class Planet : IPlanet
    {
        public int Id { get; } = new Random().Next();
        public GridCoordinate Coordinate { get; set; }
        public List<IRobot> Robots { get; set; } = new ();

        public Dictionary<OrientationType, GridCoordinate> Scents { get; set; } = new ();
    }
}
