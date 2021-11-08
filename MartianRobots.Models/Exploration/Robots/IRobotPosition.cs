namespace MartianRobots.Common.Exploration.Robots
{
    public interface IRobotPosition
    {
        GridCoordinate Coordinate { get; set; }
        OrientationType Orientation { get; set; }


        /// <summary>
        /// Change the orientation counterclockwise
        /// </summary>
        /// <param name="numberOfRotations">numberOfRotations</param>
        void RotateLeft(int numberOfRotations);

        /// <summary>
        /// Change the orientation clockwise
        /// </summary>
        /// <param name="numberOfRotations">numberOfRotations</param>
        void RotateRight(int numberOfRotations);

        /// <summary>
        /// Increase the corresponding axe (using Orientation field) numberOfSteps.
        /// </summary>
        /// <param name="numberOfSteps">numberOfSteps</param>
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