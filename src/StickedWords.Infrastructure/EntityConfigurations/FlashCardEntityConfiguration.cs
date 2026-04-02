using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StickedWords.Domain.Models;

namespace StickedWords.Infrastructure.EntityConfigurations;

internal sealed class FlashCardEntityConfiguration : IEntityTypeConfiguration<FlashCard>
{
    public void Configure(EntityTypeBuilder<FlashCard> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.Word, x.DeletedAt }).IsUnique(true);
    }
}
