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
        public async Task<ActionResult<ResultDocument>> XmlDocument([FromBody] string soapMessage)
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
        public async Task<ActionResult<ResultReader>> XmlReader([FromBody] string soapMessage)
        {
            var result = await XmlProcessor.ProcessWithReader(soapMessage);

            return Ok(result);
        }

        /// <summary>
        /// Process the XML string using XML Document and by query path retrieve content os Header and Bocy
        /// </summary>
        /// <param name="soapMessage"></param>
        /// <returns></returns>
        [HttpPost("similarProcessDocument")]
        public async Task<ActionResult<ResultDocument>> SimilarXmlDocument([FromBody] string soapMessage)
        {
            var result = await XmlProcessor.ProcessSimilarWSHWithDocument(soapMessage);

            return Ok(new
            {
                result.MessageID,
                result.StartingTime,
                result.EndingTime,
                result.DifferenceTime,
                result.Size,
                result.WsaAcceptFaultTo,
                result.WsaAction,
                result.WsaFaultTo,
                result.WsaFrom,
                result.WsaRelatesTo,
                result.WsaReplyTo,
                result.WsaRetryMessages,
                result.WsaTo
            });
        }

        /// <summary>
        /// Process the XML string using a reader and retrieves content while reading and Header and Body are empty.
        /// </summary>
        /// <param name="soapMessage"></param>
        /// <returns></returns>
        [HttpPost("similarProcessReader")]
        public async Task<ActionResult<ResultReader>> SimilarXmlReader([FromBody] string soapMessage)
        {
            var result = await XmlProcessor.ProcessSimilarWSHWithReader(soapMessage);

            return Ok(result);
        }
    }
}