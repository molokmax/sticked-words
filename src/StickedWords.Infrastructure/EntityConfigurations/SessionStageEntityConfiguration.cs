using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StickedWords.Domain.Models;

namespace StickedWords.Infrastructure.EntityConfigurations;

internal sealed class SessionStageEntityConfiguration : IEntityTypeConfiguration<SessionStage>
{
    public void Configure(EntityTypeBuilder<SessionStage> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
