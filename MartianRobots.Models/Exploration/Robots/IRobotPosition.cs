namespace MartianRobots.Common.Robots
{
    public interface IRobotPosition
    {
        GridCoordinate Coordinate { get; set; }
        OrientationType Orientation { get; set; }

        void RotateLeft(int numberOfRotations);
        void RotateRight(int numberOfRotations);
        void MoveForward(int numberOfSteps);
    }

    public class GridCoordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public enum OrientationType
    {
        N = 0,
        E = 1,
        S = 2,
        W = 3
    }
}