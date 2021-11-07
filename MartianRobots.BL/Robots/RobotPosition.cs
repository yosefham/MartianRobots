using System;

namespace MartianRobots.BL.Robots
{
    internal class RobotPosition : IRobotPosition
    {
        private const int MaxNumberOfOrientations = 4;
        
        #region members
        public GridCoordinate Coordinate { get; set; }
        public OrientationType Orientation { get; set; }
        #endregion

        #region Public Methods

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

        #endregion
    }


}
