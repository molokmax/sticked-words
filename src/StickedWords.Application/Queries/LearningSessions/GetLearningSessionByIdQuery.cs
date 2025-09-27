using MediatR;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Commands.LearningSessions;

public record GetLearningSessionByIdQuery(long LearningSessionId) : IRequest<LearningSession?>;
