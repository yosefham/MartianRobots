namespace MartianRobots.Common.Configuration
{
    public interface ISettings
    {
        int MaxCoordinateValue { get; }
        int MaxMovementLength { get; }
    }
}