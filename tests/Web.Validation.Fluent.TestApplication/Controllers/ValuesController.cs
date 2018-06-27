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
        [Validation]
        public string Get(int id)
        {
            return "id";
        }

        // POST api/values
        [HttpPost]
        [Validation(typeof(ValueCreateDtoValidator))]
        public string Post([FromBody]ValueCreateDto dto)
        {
            return dto.First + dto.Second;
        }

        // PUT api/values/5
        [HttpPost("composite")]
        [Validation(typeof(CompositeDtoValidator))]
        public void Composite([FromBody]CompositeDto dto)
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

    public class CompositeDto
    {
        public int Id { get; set; }
        public ValueCreateDto Value { get; set; }
    }

    public class CompositeDtoValidator : AbstractValidator<CompositeDto>
    {
        public CompositeDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Value).SetValidator(new ValueCreateDtoValidator());
        }
    }
}
