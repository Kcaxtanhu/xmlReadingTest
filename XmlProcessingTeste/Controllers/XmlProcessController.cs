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
        /// <summary>
        /// Process the XML string using XML Document and by query path retrieve content os Header and Bocy
        /// </summary>
        /// <param name="soapMessage"></param>
        /// <returns></returns>
        [HttpPost("processDocument")]
        public async Task<ActionResult<Result>> XmlDocument([FromBody] string soapMessage)
        {
            var result = await XmlProcessor.ProcessWithDocument(soapMessage);

            return Ok(result);
        }

        /// <summary>
        /// Process the XML string using a reader and retrieves content while reading and Header and Body are empty.
        /// </summary>
        /// <param name="soapMessage"></param>
        /// <returns></returns>
        [HttpPost("processReader")]
        public async Task<ActionResult<Result>> XmlReader([FromBody] string soapMessage)
        {
            var result = await XmlProcessor.ProcessWithReader(soapMessage);

            return Ok(result);
        }
    }
}