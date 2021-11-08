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
            if (request == null || request.Count <= 0)
                return null;

            try
            {
                var invalidId = -999;         // Valid ids are all positive
                var instructions = request.Select(x => x.Trim().ToUpper()).ToList();  // format input
                var id = _handler.Process(instructions, invalidId);

                var result = new ExploreResult()
                {
                    Mars = _handler.GetMarsGrid(id),
                    Robots = _handler.GetRobotsPosition(id),
                    Id = id
                };

                return result;
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"The request failed due to: {e.Message}", e);
            }
        }[HttpPost]
        [Route("{id}/Update")]
        public ExploreResult Update([FromBody]List<string> request, [FromRoute]int id)
        {
            if (request == null || request.Count <= 0)
                return null;

            try
            {
                var instructions = request.Select(x => x.Trim().ToUpper()).ToList();  // format input
                _handler.Process(instructions, id);

                var result = new ExploreResult()
                {
                    Mars = _handler.GetMarsGrid(id),
                    Robots = _handler.GetRobotsPosition(id),
                    Id = id
                };

                return result;
            }
            catch (Exception e)
            {
                throw new HttpRequestException($"The request failed due to: {e.Message}", e);
            }
        }

        [HttpPost]
        [Route("{id}/Delete")]
        public void Clear([FromRoute] int id)
        {
            try
            {
                _handler.ClearData(id);

            }
            catch (Exception e)
            {
                throw new HttpRequestException("There was an internal error. Please retry later.", e);
            }
        }

        [HttpGet]
        [Route("{id}/Status")]
        public StatusResponse GetStatus([FromRoute] int id)
        {
            return new()
            {
                Id = id,
                ExplorationRatio = _handler.PercentageOfExploration(id),
                ActiveRobots = _handler.NumberOfActiveRobots(id),
                LostRobots = _handler.NumberOfLostRobots(id),
                Mars = _handler.GetMarsGrid(id),
                KnownEdges = _handler.GetRobotsScent(id)
            };
        }
    }

    public class StatusResponse
    {
        public int Id { get; set; }
        public double ExplorationRatio { get; set; }
        public int ActiveRobots { get; set; }
        public int LostRobots { get; set; }
        public GridCoordinate Mars { get; set; }
        public List<string> KnownEdges { get; set; }
    }

    public class ExploreResult
    {
        public int Id { get; set; }
        public List<string> Robots { get; set; }
        public GridCoordinate Mars { get; set; }
    }

}
