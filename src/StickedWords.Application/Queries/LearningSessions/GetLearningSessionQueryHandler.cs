using MediatR;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class GetLearningSessionQueryHandler : IRequestHandler<GetLearningSessionQuery, LearningSession?>
{
    private readonly ILearningSessionRepository _sessionRepository;

    public GetLearningSessionQueryHandler(ILearningSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<LearningSession?> Handle(GetLearningSessionQuery query, CancellationToken cancellationToken)
    {
        return await _sessionRepository.GetActive(cancellationToken);
    }
}
