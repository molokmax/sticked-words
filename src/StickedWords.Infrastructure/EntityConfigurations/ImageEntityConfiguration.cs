using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StickedWords.Domain.Models;

namespace StickedWords.Infrastructure.EntityConfigurations;

internal sealed class ImageEntityConfiguration() : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
