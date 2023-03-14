using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebAPI.DataTransferObjects;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Interfaces;

namespace WebAPI.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataBaseContext _dataBaseContext;
        private readonly IMapper _mapper;

        public MessageRepository(DataBaseContext context, IMapper mapper)
        {
            _dataBaseContext = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _dataBaseContext.Messages.Add(message);
        }

        public void UpdateMessage(Message message)
        {
            _dataBaseContext.Messages.Update(message);
        }

        public async Task<PageList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _dataBaseContext.Messages.OrderByDescending(message => message.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(user => user.RecipientUsername == messageParams.Username && user.RecipientDeleted == false),
                "Outbox" => query.Where(user => user.SenderUsername == messageParams.Username && user.SenderDeleted == false),
                _ => query.Where(user => user.RecipientUsername == messageParams.Username && user.SenderDeleted == false && user.RecipientDeleted == false && (user.DateRead == null || user.DateRead == DateTime.MinValue)),
            };

            var messages = query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider);

            return await PageList<MessageDTO>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _dataBaseContext.Messages.FindAsync(id);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var query = _dataBaseContext.Messages
                .Include(user => user.Sender).ThenInclude(photo => photo.Photos)
                .Include(user => user.Recipient).ThenInclude(photo => photo.Photos)
                .Where(message =>
                    message.RecipientUsername == currentUserName && message.SenderUsername == recipientUserName
                    ||
                    message.RecipientUsername == recipientUserName && message.SenderUsername == currentUserName
                )
                .Where(message => message.SenderDeleted == false || message.RecipientDeleted == false)
                .OrderBy(messages => messages.MessageSent)
                .AsQueryable();

            var unreadMessages = query.Where(message => (message.DateRead == DateTime.MinValue || message.DateRead == null) && message.RecipientUsername == currentUserName).ToList();
       
            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _dataBaseContext.SaveChangesAsync();
            }

            return await query.ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public void RemoveMessage(Message message)
        {
            _dataBaseContext.Messages.Remove(message);
        }

        public void AddGroup(Group group)
        {
            _dataBaseContext.Groups.Add(group);
        }

        public void RemoveConnection(Connection connection)
        {
            _dataBaseContext.Connections.Remove(connection);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return (await _dataBaseContext.Connections.FindAsync(connectionId))!;
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _dataBaseContext.Groups.Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _dataBaseContext.Groups.Include(group => group.Connections)
                .Where(group => group.Connections.Any(connection => connection.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }
    }
}
