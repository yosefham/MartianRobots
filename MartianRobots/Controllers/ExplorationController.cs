using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using MartianRobots.BL;
using MartianRobots.BL.Robots;
using MartianRobots.Common.Robots;

namespace MartianRobots.Controllers
{
    [Route("MarsExploration")]
    [ApiController]
    public class ExplorationController : ControllerBase
    {
        private readonly IExplorationHandler _handler;

        public ExplorationController(IExplorationHandler handler)
        {
            _handler = handler;
        }
        

        [HttpPost]
        [Route("explore")]
        public ExploreResult Explore([FromBody]List<string> request)
        {
            if (request.Count < 0)
                return null;

            try
            {
                var instructions = request.Select(x => x.Trim().ToUpper()).ToList();  // format input
                _handler.Process(instructions);

                var result = new ExploreResult()
                {
                    Mars = _handler.GetMarsGrid(),
                    Robots = _handler.GetRobotsPosition()
                };

                return result;
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"The request failed due to: {e.Message}", e);
            }
        }

        [HttpPost]
        [Route("Delete")]
        public void Clear()
        {
            try
            {
                _handler.ClearData();

            }
            catch (Exception e)
            {
                throw new HttpRequestException("There was an internal error. Please retry later.", e);
            }
        }

        [HttpGet]
        [Route("Status")]
        public StatusResponse GetStatus()
        {
            return new()
            {
                ExplorationRatio = _handler.PercentageOfExploration(),
                ActiveRobots = _handler.NumberOfActiveRobots(),
                LostRobots = _handler.NumberOfLostRobots(),
                Mars = _handler.GetMarsGrid(),
                KnownEdges = _handler.GetRobotsScent()
            };
        }
    }

    public class StatusResponse
    {
        public double ExplorationRatio { get; set; }
        public int ActiveRobots { get; set; }
        public int LostRobots { get; set; }
        public GridCoordinate Mars { get; set; }
        public List<string> KnownEdges { get; set; }
    }

    public class ExploreResult
    {
        public List<string> Robots { get; set; }
        public GridCoordinate Mars { get; set; }

    }

}
