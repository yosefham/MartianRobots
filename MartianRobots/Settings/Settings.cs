using System.ComponentModel.DataAnnotations;
using MartianRobots.BL.Settings;

namespace MartianRobots.Settings
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