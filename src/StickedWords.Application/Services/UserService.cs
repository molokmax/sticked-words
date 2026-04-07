using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Services;

internal sealed class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;
    private readonly IUserInfoProvider _userProvider;

    public UserService(
        IUnitOfWork unitOfWork,
        IUserRepository repository,
        IUserInfoProvider userProvider)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _userProvider = userProvider;
    }

    public async Task<User?> GetOrDefault(CancellationToken cancellationToken)
    {
        var userInfo = _userProvider.Get() ?? throw new CurrentUserNotDefinedException();
        return await _repository.GetByYandexId(userInfo.ExternalId, cancellationToken);
    }

    public async Task<User> Get(CancellationToken cancellationToken)
    {
        return await GetOrDefault(cancellationToken) ?? throw new CurrentUserNotDefinedException();
    }

    public async Task<User> GetOrCreate(CancellationToken cancellationToken)
    {
        var userInfo = _userProvider.Get()
            ?? throw new CurrentUserNotDefinedException();
        var user = await _repository.GetByYandexId(userInfo.ExternalId, cancellationToken);
        if (user is null)
        {
            user = User.Create(userInfo.ExternalId);
            _repository.Add(user);
        }
        await _unitOfWork.SaveChanges(cancellationToken);

        return user;
    }
}
