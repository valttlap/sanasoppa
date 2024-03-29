// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Data.Configurations;
public class ExplanationConfiguration : IEntityTypeConfiguration<Explanation>
{
    public void Configure(EntityTypeBuilder<Explanation> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityAlwaysColumn();

        builder.Property(e => e.Text).IsRequired();

        builder.Property(e => e.IsRight).IsRequired();

        builder.HasOne(e => e.Round)
            .WithMany(r => r.Explanations)
            .HasForeignKey(e => e.RoundId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Player)
            .WithMany()
            .HasForeignKey(e => e.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Votes)
            .WithOne(v => v.Explanation)
            .HasForeignKey(v => v.ExplanationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
