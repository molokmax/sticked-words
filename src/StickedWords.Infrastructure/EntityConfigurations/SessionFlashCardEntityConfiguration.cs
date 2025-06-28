using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StickedWords.Domain.Models;

namespace StickedWords.Infrastructure.EntityConfigurations;

internal sealed class SessionFlashCardEntityConfiguration : IEntityTypeConfiguration<SessionFlashCard>
{
    public void Configure(EntityTypeBuilder<SessionFlashCard> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
