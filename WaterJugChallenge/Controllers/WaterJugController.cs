using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterJugChallenge.Services;

namespace WaterJugChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterJugController : ControllerBase
    {
        private readonly WaterJugSolver _jugSolver;

        public WaterJugController(WaterJugSolver jugSolver)
        {
            _jugSolver = jugSolver;
        }

        [HttpPost]
        public IActionResult SolveWaterJug([FromBody] WaterJugRequest request)
        {
            try
            {
                var result = _jugSolver.Solve(request.BucketCapacityY, request.BucketCapacityX, request.TargetAmountZ);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest($"ERROR: {ex.Message}");
            }
        }
    }
    public class WaterJugRequest
    {
        public int BucketCapacityX { get; set; }
        public int BucketCapacityY { get; set; }
        public int TargetAmountZ { get; set; }
    }

}
