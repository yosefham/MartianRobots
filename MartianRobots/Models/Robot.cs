using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MartianRobots.Models
{
    public class Robot
    {
        public RobotPosition Position { get; set; }
        public bool Lost { get; set; }

        public override string ToString()
        {
            return $"{Position.Coordinate.X} {Position.Coordinate.Y} {Position.Orientation}" + (Lost ? " LOST" : "");
        }

        public void Move(char movement, int numberOfSteps)
        {
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
                default:
                    throw new ArgumentOutOfRangeException($"The provided command is not supported: {movement}");
            }
        }
    }

    

}
