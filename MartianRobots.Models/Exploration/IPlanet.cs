using System.Collections.Generic;
using MartianRobots.Common.Exploration.Robots;

namespace MartianRobots.Common.Exploration
{
    public interface IPlanet
    {
        public int Id { get; }
        public GridCoordinate Coordinate { get; set; }
        public List<IRobot> Robots { get; set; }
        public Dictionary<OrientationType, GridCoordinate> Scents { get; set; }
    }
}
