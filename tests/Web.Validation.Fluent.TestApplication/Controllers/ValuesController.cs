using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Validation.Fluent.TestApplication.Controllers
{
    using FluentValidation;

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Validation(typeof(ValueCreateDtoValidator))]
        public string Post([FromBody]ValueCreateDto dto)
        {
            return dto.First + dto.Second;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class ValueCreateDto
    {
        public string First { get; set; }
        public long Second{ get; set; }
    }

    public class ValueCreateDtoValidator : AbstractValidator<ValueCreateDto>
    {
        public ValueCreateDtoValidator()
        {
            RuleFor(x => x.First).NotNull();
            RuleFor(x => x.Second).NotEqual(0);
        }
    }
}
