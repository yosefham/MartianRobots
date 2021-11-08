namespace MartianRobots.Common.Exploration.Robots
{
    public interface IRobot
    {
        IRobotPosition Position { get; set; }
        bool Lost { get; set; }
        
        

        /// <summary>
        /// Transform instance to a string equivalence
        /// </summary>
        /// <returns>String representation</returns>
        string ToString();

        /// <summary>
        /// Executes movement command numberOfSteps times, and check if is lost using planet.
        /// If Lost is true, command is ignored.
        /// </summary>
        /// <param name="movement">movement command</param>
        /// <param name="numberOfSteps">number of times performed</param>
        /// <param name="planet">planet</param>
        void Move(char movement, int numberOfSteps, GridCoordinate planet);

        /// <summary>
        /// Updates Lost status
        /// </summary>
        /// <param name="planet">planet</param>
        /// <returns>Lost status</returns>
        bool IsRobotLost( GridCoordinate planet);
    }
}