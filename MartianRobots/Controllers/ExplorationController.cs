using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MartianRobots.Robots;

namespace MartianRobots.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExplorationController : ControllerBase
    {
        private ExplorationProcessor _processor = new ExplorationProcessor(new RobotFactory());

        [HttpPost]
        public ExplorationResult Explore([FromBody]List<string> request)
        {
            if (request.Count < 0)
                return null;

            var instructions = request.Select(x => x.Trim().ToUpper()).ToList();  // format input
            var result = _processor.Process(instructions);

            return result;
        }
    }

    public class ExplorationResult
    {
        public List<string> Robots { get; set; }
        public GridCoordinate Mars { get; set; }

    }

}
