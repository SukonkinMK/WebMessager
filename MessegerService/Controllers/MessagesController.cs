using AutoMapper;
using MessegerService.Abstractions;
using MessegerService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MessegerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public MessagesController(IMapper mapper, IMessageService messageService)
        {
            _mapper = mapper;
            _messageService = messageService;
        }

        
        [HttpGet("get")]
        [Authorize(Roles = "1, 2")]
        public ActionResult GetNewMessages()
        {
            var userId = GetCurrentUserId();

            var response = _messageService.GetNewMessages(userId);            

            return Ok(response);
        }
        
        [HttpPost("send")]
        [Authorize(Roles = "1, 2")]
        public ActionResult SendMessage([FromBody] int recipientId, string text)
        {
            var senderId = GetCurrentUserId();

            var message = new MessageModel
            {
                RecipientId = recipientId,
                SenderId = senderId,
                Text = text
            };

            var response = _messageService.SendMessage(message);
            return Ok(response);
        }
        private int GetCurrentUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return int.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);
            }
            return -1;
        }
    }
}
