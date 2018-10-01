namespace Byndyusoft.Dotnet.Core.Samples.Web.Application.Controllers.ValuesController
{
    using Microsoft.AspNetCore.Mvc;
    using ProtoBuf;

    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public Address Address { get; set; }
    }
    [ProtoContract]
    public class Address
    {
        [ProtoMember(1)]
        public string Line1 { get; set; }
        [ProtoMember(2)]
        public string Line2 { get; set; }
    }

    [Route("persons")]
    public class PersonController : Controller
    {
        [HttpGet]
        public Person Get()
        {
            var person = new Person
                         {
                             Id = 12345,
                             Name = "Fred",
                             Address = new Address
                                       {
                                           Line1 = "Flat 1",
                                           Line2 = "The Meadows"
                                       }
                         };
            return person;
        }

        [HttpGet("{id}.{format=json}"), FormatFilter]
        public Person Get(int id)
        {
            var person = new Person
                         {
                             Id = id,
                             Name = "Fred",
                             Address = new Address
                                       {
                                           Line1 = "Flat 1",
                                           Line2 = "The Meadows"
                                       }
                         };
            return person;
        }
    }
}