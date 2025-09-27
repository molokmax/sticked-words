using MediatR;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Commands.LearningSessions;

public record GetLearningSessionResultsQuery(long LearningSessionId) : IRequest<LearningSessionResults>;
