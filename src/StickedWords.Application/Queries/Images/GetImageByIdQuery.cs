using MediatR;

namespace StickedWords.Application.Queries.Images;

public record GetImageByIdQuery(long ImageId) : IRequest<string>;
