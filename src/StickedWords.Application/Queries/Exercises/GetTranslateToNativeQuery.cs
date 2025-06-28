using MediatR;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.Application.Queries.Exercises;

public record GetTranslateToNativeQuery(long FlashCardId) : IRequest<TranslateExercise>;
