using MediatR;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;

namespace StickedWords.Application.Queries.FlashCards;

public record GetFlashCardsQuery(PageQuery PageQuery) : IRequest<PageResult<FlashCard>>;
