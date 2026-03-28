using MediatR;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Commands.FlashCards;

public record DeleteFlashCardCommand(long FlashCardId) : IRequest<FlashCard>;
