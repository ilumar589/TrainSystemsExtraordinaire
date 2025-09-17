using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trains.Base.DB.Models;

namespace Trains.Base.DB.TableStructure;

public sealed class ClassificationTable : IEntityTypeConfiguration<Classification>
{
    public void Configure(EntityTypeBuilder<Classification> builder)
    {
        builder.ToTable(nameof(Classification), SchemaName.TrainConfigSchema);

        builder.HasKey(x => x.Id);

    }
}
