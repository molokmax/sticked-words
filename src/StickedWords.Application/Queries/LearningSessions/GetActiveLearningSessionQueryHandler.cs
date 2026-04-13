using MediatR;
using StickedWords.Application.Services;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class GetActiveLearningSessionQueryHandler : IRequestHandler<GetActiveLearningSessionQuery, LearningSession?>
{
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly UserService _userService;
    private readonly TimeProvider _timeProvider;

    public GetActiveLearningSessionQueryHandler(
        ILearningSessionRepository sessionRepository,
        UserService userService,
        TimeProvider timeProvider)
    {
        _sessionRepository = sessionRepository;
        _userService = userService;
        _timeProvider = timeProvider;
    }

    public async Task<LearningSession?> Handle(GetActiveLearningSessionQuery query, CancellationToken cancellationToken)
    {
        var user = await _userService.GetOrDefault(cancellationToken);
        if (user is null)
        {
            return null;
        }

        return await _sessionRepository.GetActiveNotExpired(user, _timeProvider.GetUtcNow(), cancellationToken);
    }
}
