using MediatR;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.Images;

internal sealed class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
{
    private readonly IImageRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteImageCommandHandler(
        IImageRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteImageCommand command, CancellationToken cancellationToken)
    {
        // TODO: check that user has permissions
        var image = await _repository.GetById(command.ImageId, cancellationToken)
            ?? throw new ImageNotFoundException(command.ImageId);
        _repository.Delete(image);
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}
