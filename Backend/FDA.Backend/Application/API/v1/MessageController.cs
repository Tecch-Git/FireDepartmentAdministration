using FDA.Backend.Application.API.v1.Models;
using FDA.Backend.Application.Models;
using FDA.Backend.Application.Services;
using FDA.Backend.Functions;
using FDA.Database.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace FDA.Backend.Application.API.v1
{
    [ApiController, Route("api/v{version:apiVersion}/[controller]")]
    public class MessageController(IMemberService memberService) : ControllerBase
    {
        private IMemberService memberService = memberService;
        private CSVProcessing csvFunctions = new CSVProcessing();

        [HttpGet("LoadNextMessage")]
        [ProducesResponseType(typeof(MessageResponse), 200)]
        public async Task<IActionResult>? GetNextMessage()
        {
            var messages = csvFunctions.LoadMessagesFromCSV();

            return Ok(messages.FirstOrDefault());
        }

        [HttpGet("LoadAllMembers")]
        [ProducesResponseType(typeof(List<Member>), 200)]
        public async Task<IActionResult> GetAllMembers()
        {
            return Ok(await memberService.GetAllMembers());
        }

        [HttpPost("AddMember")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> AddMember(AddMemberRequest request)
        {
            return Ok(await memberService.AddMember(request.name, request.phone, request.email));
        }
    }
}
