using MessegerService.Models;

namespace MessegerService.Abstractions
{
    public interface IMessageService
    {
        public IEnumerable<MessageModel> GetNewMessages(int userId);
        public int SendMessage(MessageModel model);
    }
}
