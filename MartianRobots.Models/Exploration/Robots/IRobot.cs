namespace MartianRobots.Common.Robots
{
    public interface IRobot
    {
        IRobotPosition Position { get; set; }
        bool Lost { get; set; }
        string ToString();
        void Move(char movement, int numberOfSteps, GridCoordinate mars);
        bool IsRobotLost( GridCoordinate mars);
    }
}