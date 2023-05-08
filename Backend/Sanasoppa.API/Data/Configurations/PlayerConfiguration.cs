// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations;
public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityAlwaysColumn();

        builder.Property(p => p.IsHost)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.Username).IsRequired();

        builder.HasIndex(e => e.ConnectionId)
            .IsUnique();

        builder.Property(e => e.TotalPoints)
            .HasDefaultValue(0)
            .IsRequired();
    }
}
