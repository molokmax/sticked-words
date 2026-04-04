using MediatR;
using Microsoft.Extensions.Options;
using StickedWords.Application.Specifications;
using StickedWords.Domain;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class ExpireLearningSessionsCommandHandler : IRequestHandler<ExpireLearningSessionsCommand>
{
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly LearningSessionOptions _options;

    public ExpireLearningSessionsCommandHandler(
        ILearningSessionRepository sessionRepository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IOptions<LearningSessionOptions> options)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
        _options = options.Value;
    }

    public async Task Handle(ExpireLearningSessionsCommand command, CancellationToken cancellationToken)
    {
        var expireDate = _timeProvider.GetUtcNow() - _options.UpdateExpireStateDelay;
        var specification = new LearningSessionsToExpireSpecification(expireDate, take: _options.FlashCardCount);
        var learningSessions = await _sessionRepository.GetBySpecification(specification, false, cancellationToken);
        foreach (var session in learningSessions.Data)
        {
            session.Expire(_options, _timeProvider);
        }
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}
