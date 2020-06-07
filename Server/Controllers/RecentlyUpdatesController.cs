using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWasmApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecentlyUpdatesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            yield return "Reproduce the problem.";
            yield return "Initila commit.";
        }
    }
}
