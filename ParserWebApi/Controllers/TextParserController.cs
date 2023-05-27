using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TextParserLib;
using static System.Net.Mime.MediaTypeNames;

namespace ParserWebApi.Controllers
{
    [Route("api/textController")]
    [ApiController]
    public class TextParserController : ControllerBase
    {
        [HttpPost("ParseText")]
        public Dictionary<string, int> Post(string[] text)
        {
            var parserInstance = new Parser();

            Console.WriteLine(111);

            return parserInstance.MultithreadedParser(text);
        }
    }
}
