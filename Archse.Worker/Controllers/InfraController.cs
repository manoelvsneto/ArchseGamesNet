using Microsoft.AspNetCore.Mvc;
using System;

namespace Archse.Worker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfraController : ControllerBase
    {

        public InfraController()
        {

        }

        [HttpGet]
        public String Get()
        {
            return "OK";
        }
    }
}