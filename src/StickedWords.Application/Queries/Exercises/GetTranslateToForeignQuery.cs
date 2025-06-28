using MediatR;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.Application.Queries.Exercises;

public record GetTranslateToForeignQuery(long FlashCardId) : IRequest<TranslateExercise>;
