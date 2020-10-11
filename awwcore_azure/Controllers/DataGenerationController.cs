using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using awwcore_azure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace awwcore_azure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataGenerationController : ControllerBase
    {
        private DataGenerator DataGenerator { get; set; }

        public DataGenerationController(DataGenerator dataGenerator)
        {
            DataGenerator = dataGenerator;
        }

        // GET: api/DataGeneration
        [HttpGet("{table}")]
        public async Task<IActionResult> GenerateData(string table)
        {
            switch(table)
            {
                case "languages":
                    break;
                case "genres":
                    DataGenerator.GenerateGenres();
                    break;
                case "all":
                    DataGenerator.GenerateAll();
                    break;
            }          

            return NoContent();
        }
    }
}
