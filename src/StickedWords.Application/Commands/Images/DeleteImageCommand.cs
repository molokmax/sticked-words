using MediatR;

namespace StickedWords.Application.Commands.Images;

public record DeleteImageCommand(long ImageId) : IRequest;
