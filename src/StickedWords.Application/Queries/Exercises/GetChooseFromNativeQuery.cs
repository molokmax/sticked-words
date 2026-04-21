using MediatR;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.Application.Queries.Exercises;

public record GetChooseFromNativeQuery(long FlashCardId) : IRequest<ChooseExercise>;
