using System;

namespace MartianRobots.Robots
{
    public class RobotPosition
    {

        public GridCoordinate Coordinate { get; set; }
        public OrientationType Orientation { get; set; }


        private const int MaxNumberOfOrientations = 4;
        public void RotateLeft(int numberOfRotations)
        {
            int currentOrientation = (int)Orientation + MaxNumberOfOrientations;
            var finalOrientation = Math.Abs((currentOrientation - numberOfRotations) % MaxNumberOfOrientations);
            Orientation = (OrientationType)finalOrientation;

        }
        public void RotateRight(int numberOfRotations)
        {
            int currentOrientation = (int)Orientation;
            var finalOrientation = (currentOrientation + numberOfRotations) % MaxNumberOfOrientations;
            Orientation = (OrientationType)finalOrientation;

        }

        public void MoveForward(int numberOfSteps)
        {
            switch (Orientation)
            {
                case OrientationType.N:
                    Coordinate.Y += numberOfSteps;
                    break;
                case OrientationType.E:
                    Coordinate.X += numberOfSteps;
                    break;
                case OrientationType.S:
                    Coordinate.Y -= numberOfSteps;
                    break;
                case OrientationType.W:
                    Coordinate.X -= numberOfSteps;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"the current orientation is not supported: {Orientation}");
            }

        }
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
