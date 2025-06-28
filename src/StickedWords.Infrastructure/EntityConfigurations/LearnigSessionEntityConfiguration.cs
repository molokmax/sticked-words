using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StickedWords.Domain.Models;

namespace StickedWords.Infrastructure.EntityConfigurations;

internal sealed class LearnigSessionEntityConfiguration : IEntityTypeConfiguration<LearningSession>
{
    public void Configure(EntityTypeBuilder<LearningSession> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
