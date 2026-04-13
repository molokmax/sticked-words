using MediatR;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class GetLearningSessionByIdQueryHandler : IRequestHandler<GetLearningSessionByIdQuery, LearningSession?>
{
    private readonly ILearningSessionRepository _sessionRepository;

    public GetLearningSessionByIdQueryHandler(ILearningSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<LearningSession?> Handle(GetLearningSessionByIdQuery query, CancellationToken cancellationToken)
    {
        // TODO: check that user has permissions
        return await _sessionRepository.GetById(query.LearningSessionId, cancellationToken);
    }
}
