﻿using MediatR;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models.Exercises;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.Exercises;

internal sealed class GetTranslateToNativeQueryHandler : IRequestHandler<GetTranslateToNativeQuery, TranslateExercise>
{
    private readonly IFlashCardRepository _repository;

    public GetTranslateToNativeQueryHandler(IFlashCardRepository repository)
    {
        _repository = repository;
    }

    public async Task<TranslateExercise> Handle(GetTranslateToNativeQuery request, CancellationToken cancellationToken)
    {
        var flashCard = await _repository.GetById(request.FlashCardId, cancellationToken);
        if (flashCard is null)
        {
            throw new FlashCardNotFoundException(request.FlashCardId);
        }

        return new() { Word = flashCard.Word };
    }
}
