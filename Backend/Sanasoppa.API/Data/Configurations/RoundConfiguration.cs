// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations;
public class RoundConfiguration : IEntityTypeConfiguration<Round>
{
    public void Configure(EntityTypeBuilder<Round> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityAlwaysColumn();

        builder.Property(e => e.IsCurrent)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.Word).IsRequired();

        builder.HasOne(r => r.Dasher)
            .WithMany()
            .HasForeignKey(r => r.DasherId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(r => r.Explanations)
            .WithOne(e => e.Round)
            .HasForeignKey(e => e.RoundId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(r => r.Votes)
            .WithOne(v => v.Round)
            .HasForeignKey(v => v.RoundId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Game)
            .WithMany(g => g.Rounds)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
