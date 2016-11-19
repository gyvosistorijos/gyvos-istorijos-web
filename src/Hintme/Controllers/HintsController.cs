using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hintme.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hintme.Controllers
{
    [Route("api/hints")]
    public class HintsController : Controller
    {
        private readonly IHintRepository _repository;

        public HintsController(IHintRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetHints());
        }

        [HttpPost]
        public IActionResult Post([FromBody]Hint hint)
        {
            _repository.SaveHint(hint);

            return Ok(hint);
        }
    }
}
