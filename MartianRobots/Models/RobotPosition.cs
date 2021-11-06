using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MartianRobots.Models
{
    public class RobotPosition
    {
        public GridCoordinate Coordinate { get; set; }
        public OrientationType Orientation { get; set; }


        private const int MaxNumberOfOrientations = 4;
        public void RotateLeft(int numberOfRotations)
        {
            int currentDirrection = (int)Orientation + MaxNumberOfOrientations;
            var finalOrientation = (currentDirrection - numberOfRotations) % MaxNumberOfOrientations;
            Orientation = (OrientationType)finalOrientation;

        }
        public void RotateRight(int numberOfRotations)
        {
            int CurrentOrientation = (int)Orientation;
            var finalOrientation = (CurrentOrientation + numberOfRotations) % MaxNumberOfOrientations;
            Orientation = (OrientationType)finalOrientation;

        }

        public void MoveForward(int NumberOfSteps)
        {
            switch (Orientation)
            {
                case OrientationType.N:
                    Coordinate.Y += NumberOfSteps;
                    break;
                case OrientationType.E:
                    Coordinate.X += NumberOfSteps;
                    break;
                case OrientationType.S:
                    Coordinate.Y -= NumberOfSteps;
                    break;
                case OrientationType.W:
                    Coordinate.X -= NumberOfSteps;
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
