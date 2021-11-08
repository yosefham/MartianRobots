using System;
using MartianRobots.Common.Exploration.Robots;

namespace MartianRobots.BL.Exploration.Robots
{
    public class Robot : IRobot
    {
        #region Members
        public IRobotPosition Position { get; set; }
        public bool Lost { get; set; } = false;
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return $"{Position.Coordinate.X} {Position.Coordinate.Y} {Position.Orientation}" + (Lost ? " LOST" : String.Empty);
        }

        public void Move(char movement, int numberOfSteps, GridCoordinate planet)
        {
            if (Lost) return;

            switch (movement)
            {
                case 'L':
                    Position.RotateLeft(numberOfSteps);
                    break;
                case 'R':
                    Position.RotateRight(numberOfSteps);
                    break;
                case 'F':
                    Position.MoveForward(numberOfSteps);
                    break;
               // default:  throw new ArgumentOutOfRangeException($"The provided command is not supported: {movement}");
            }

            if (IsRobotLost(planet))
                CalculateScent(planet);
        }

        public bool IsRobotLost( GridCoordinate planet)
        => Lost =  Position.Coordinate.X < 0 || Position.Coordinate.Y < 0  || Position.Coordinate.X > planet.X || Position.Coordinate.Y > planet.Y;

        #endregion

        #region Private Methods
        private void CalculateScent(GridCoordinate mars)
        {
            Position.Coordinate.X = Position.Coordinate.X > mars.X ? mars.X : Position.Coordinate.X < 0 ? 0 : Position.Coordinate.X;
            Position.Coordinate.Y = Position.Coordinate.Y > mars.Y ? mars.Y : Position.Coordinate.Y < 0 ? 0 : Position.Coordinate.Y;
        }
        #endregion

    }


}
