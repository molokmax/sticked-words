using MediatR;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class GetActiveLearningSessionQueryHandler : IRequestHandler<GetActiveLearningSessionQuery, LearningSession?>
{
    private readonly ILearningSessionRepository _sessionRepository;

    public GetActiveLearningSessionQueryHandler(ILearningSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<LearningSession?> Handle(GetActiveLearningSessionQuery query, CancellationToken cancellationToken)
    {
        return await _sessionRepository.GetActive(cancellationToken);
    }
}
