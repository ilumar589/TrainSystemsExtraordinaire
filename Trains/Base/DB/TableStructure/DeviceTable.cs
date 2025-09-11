using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trains.Base.DB.Models;

namespace Trains.Base.DB.TableStructure;

public sealed class DeviceTable : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable(nameof(Device), SchemaName.TrainConfigSchema);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { });

        builder.HasIndex(x => x.ViIdentifier);
    }
}
