using System.ComponentModel.DataAnnotations;
using MartianRobots.Common.Configuration;

namespace MartianRobots.Configuration
{
    internal class Settings : ISettings
    {
        public static string ConfigName = "Settings";

        [Required]
        public int MaxCoordinateValue { get; set; }
        [Required]
        public int MaxMovementLength { get; set; }
    }
}