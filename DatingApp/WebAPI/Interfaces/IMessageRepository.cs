 using WebAPI.DataTransferObjects;
using WebAPI.Entities;
using WebAPI.Helpers;

namespace WebAPI.Interfaces
{
    public interface IMessageRepository
    {
        public void AddMessage(Message message);
        public void RemoveMessage(Message message);
        public void UpdateMessage(Message message);
        public Task<Message> GetMessage(int id);
        public Task<PageList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);
        public Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName, string recipientUserName);
        public void AddGroup(Group group);
        public void RemoveConnection(Connection connection);
        public Task<Connection> GetConnection(string connectionId);
        public Task<Group> GetMessageGroup(string groupName);
        public Task<Group> GetGroupForConnection(string connectionId);
    }
}
