using AutoMapper;
using WebAPI.Interfaces;

namespace WebAPI.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly IMapper _mapper;

    public UnitOfWork(DataBaseContext dataBaseContext, IMapper mapper)
    {
        _dataBaseContext = dataBaseContext;
        _mapper = mapper;
    }

    public IUserRepository UserRepository => new UserRepository(_dataBaseContext, _mapper);
    public IMessageRepository MessageRepository => new MessageRepository(_dataBaseContext, _mapper);
    public ILikesRepository LikesRepository => new LikesRepository(_dataBaseContext);
    
    
    public async Task<bool> Complete()
    {
        return await _dataBaseContext.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return _dataBaseContext.ChangeTracker.HasChanges();
    }
}