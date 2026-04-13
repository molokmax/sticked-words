using MediatR;
using StickedWords.Application.Queries.Images;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.FlashCards;

internal sealed class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, string>
{
    private readonly IImageRepository _repository;

    public GetImageByIdQueryHandler(
        IImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(
        GetImageByIdQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: check that user has permissions
        var image = await _repository.GetById(request.ImageId, cancellationToken)
            ?? throw new ImageNotFoundException(request.ImageId);

        return image.Base64Data;
    }
}
