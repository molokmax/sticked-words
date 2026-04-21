using MediatR;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.Application.Queries.Exercises;

public record GetChooseFromForeignQuery(long FlashCardId) : IRequest<ChooseExercise>;
