using MediatR;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class GetActiveLearningSessionQueryHandler : IRequestHandler<GetActiveLearningSessionQuery, LearningSession?>
{
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly TimeProvider _timeProvider;

    public GetActiveLearningSessionQueryHandler(
        ILearningSessionRepository sessionRepository,
        TimeProvider timeProvider)
    {
        _sessionRepository = sessionRepository;
        _timeProvider = timeProvider;
    }

    public async Task<LearningSession?> Handle(GetActiveLearningSessionQuery query, CancellationToken cancellationToken)
    {
        return await _sessionRepository.GetActiveNotExpired(_timeProvider.GetUtcNow(), cancellationToken);
    }
}
