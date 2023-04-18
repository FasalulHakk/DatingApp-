using System.Collections;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public MessagesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;

        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CraetMessage(CreateMessageDto createMessageDto)
        {
            var username= User.GetUserName();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _uow.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message =new Message
            {
                Sender =sender,
                Recipient = recipient,
                SenderUsername= sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _uow.MessageRepository.AddMessage(message);

            if (await _uow.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Faild to send request");


        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUSer([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUserName();

            var messages = await _uow.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginatioHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize,
             messages.TotalCount,messages.TotalPages));

             return messages;
        }

      
        [HttpDelete("{id}")]
        public async Task<ActionResult>DeleteMessage(int id)
        {
            var username = User.GetUserName();

            var message = await _uow.MessageRepository.GetMessage(id);

            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();

            if (message.SenderUsername == username) message.SenderDeleted = true;
            if (message.RecipientUsername == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _uow.MessageRepository.DelelteMessage(message);
            }

            if (await _uow.Complete()) return Ok();
            return BadRequest ("Problem in deleteting the message");

        }

    }
}