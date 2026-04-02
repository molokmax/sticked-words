using MediatR;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Commands.Images;

public record CreateImageCommand(string Base64Data) : IRequest<Image>;
