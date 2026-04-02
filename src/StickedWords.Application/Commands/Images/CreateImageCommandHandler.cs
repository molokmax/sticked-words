using MediatR;
using StickedWords.Domain;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.Images;

internal sealed class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, Image>
{
    private readonly IImageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public CreateImageCommandHandler(
        IImageRepository repository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<Image> Handle(CreateImageCommand command, CancellationToken cancellationToken)
    {
        var image = Image.Create(command.Base64Data, _timeProvider);
        _repository.Add(image);
        await _unitOfWork.SaveChanges(cancellationToken);

        return image;
    }
}
