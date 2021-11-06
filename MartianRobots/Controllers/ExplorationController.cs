using MartianRobots.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MartianRobots.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExplorationController : ControllerBase
    {
        private ExplorationProcessor _processor;

        [HttpPost]
        public ExplorationResult Explore([FromBody] ExplorationRequest request)
        {
            if (request.Instructions.Count < 0)
                return null;

            var instructions = request.Instructions.Select(x => x.Trim().ToUpper()).ToList();  // format input
            var result = _processor.Process(instructions);

            return result;
        }
    }

    public class ExplorationRequest
    {
        public List<string> Instructions { get; set; }
    }
    public class ExplorationResult
    {
        public GridCoordinate Mars { get; set; }

    }

}
