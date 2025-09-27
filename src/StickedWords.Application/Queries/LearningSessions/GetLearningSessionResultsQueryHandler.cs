using MediatR;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class GetLearningSessionResultsQueryHandler : IRequestHandler<GetLearningSessionResultsQuery, LearningSessionResults>
{
    private readonly ILearningSessionRepository _sessionRepository;

    public GetLearningSessionResultsQueryHandler(ILearningSessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<LearningSessionResults> Handle(GetLearningSessionResultsQuery query, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetById(query.LearningSessionId, cancellationToken);
        if (session is null)
        {
            throw new LearningSessionNotFoundException(query.LearningSessionId);
        }

        return new()
        {
            State = session.State,
            FlashCardCount = session.FlashCards.Count
        };
    }
}
