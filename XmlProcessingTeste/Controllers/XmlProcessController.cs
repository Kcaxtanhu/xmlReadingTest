using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XmlProcessingTeste.Controllers
{
    [Route("api/{controller}")]
    public class XmlProcessController : Controller
    {
        [HttpPost("processDocument")]
        public async Task<ActionResult<Result>> XmlDocument([FromBody] string soapMessage)
        {
            var result = await XmlProcessor.ProcessWithDocument(soapMessage);

            return Ok(result);
        }

        [HttpPost("processReader")]
        public async Task<ActionResult<Result>> XmlReader([FromBody] string soapMessage)
        {
            var result = await XmlProcessor.ProcessWithReader(soapMessage);

            return Ok(result);
        }
    }
}