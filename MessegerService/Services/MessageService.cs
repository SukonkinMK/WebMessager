using AutoMapper;
using MessegerService.Abstractions;
using MessegerService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;

namespace MessegerService.Services
{
    public class MessageService : IMessageService
    {
        private readonly MessageDbContext _context;
        private readonly IMapper _mapper;

        public MessageService(MessageDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IEnumerable<MessageModel> GetNewMessages(int userId)
        {
            using (_context)
            {
                var messages = _context.Messages.Where(x => x.RecipientId == userId && !x.IsRead).ToList();

                foreach (var message in messages)
                {
                    message.IsRead = true;
                }

                _context.UpdateRange(messages);
                _context.SaveChanges();

                return messages.Select(x => _mapper.Map<MessageModel>(x)).ToList();
            }
        }

        public int SendMessage(MessageModel model)
        {
            using (_context)
            {                              
                var message = _mapper.Map<MessageEntity>(model);
                message.IsRead = false;
                message.CreatedAt = DateTime.UtcNow;

                _context.Messages.Add(message);
                _context.SaveChanges();
                return message.Id;
            }
        }
    }
}
