using FDA.Backend.Application.API.v1.Models;
using FDA.Backend.Application.Models;
using FDA.Backend.Application.Services;
using FDA.Backend.Functions;
using FDA.Database.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace FDA.Backend.Application.API.v1
{
    [ApiController, Route("api/v{version:apiVersion}/[controller]")]
    public class MessageController() : ControllerBase
    {
        [HttpGet("GetNextNumber")]
        [ProducesResponseType(typeof(MessageResponse), 200)]
        public async Task<IActionResult>? GetNextNumber()
        {
            //var messages = CSVProcessing.LoadMessagesFromCSV();

            //string[] result = [messages.FirstOrDefault().Message, messages.FirstOrDefault().TargetNumber];

            return Ok("06645067851");
        }

        [HttpGet("GetMessageByNumber")]
        public async Task<IActionResult>? GetMessageByNumber()
        {
            //var messages = CSVProcessing.LoadMessagesFromCSV();

            //string[] result = [messages.FirstOrDefault().Message, messages.FirstOrDefault().TargetNumber];
            Console.WriteLine();
            return Ok("Hallo, das ist eine Testnachricht.");
        }

        [HttpGet("GetNextMessage")]
        [ProducesResponseType(typeof(MessageResponse), 200)]
        public async Task<IActionResult>? GetNextMessage()
        {
            var messages = CSVProcessing.LoadMessagesFromCSV();

            string[] result = [messages.FirstOrDefault().Message, messages.FirstOrDefault().TargetNumber];

            return Ok(result);
        }

        [HttpGet("GetMessagesFromCSV")]
        [ProducesResponseType(typeof(MessageResponse), 200)]
        public async Task<IActionResult>? GetMessagesFromCSV()
        {
            var messages = CSVProcessing.LoadMessagesFromCSV();

            return Ok(messages.FirstOrDefault());
        }
    }
}
