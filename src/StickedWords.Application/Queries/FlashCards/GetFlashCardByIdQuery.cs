using MediatR;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Queries.FlashCards;

public record GetFlashCardByIdQuery(long FlashCardId) : IRequest<FlashCard>;
