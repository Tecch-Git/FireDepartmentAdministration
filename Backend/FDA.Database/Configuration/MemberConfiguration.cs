using FDA.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FDA.Database.Configuration;

internal sealed class MemberConfiguration
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("USER", x => x.HasComment("User information for login"))
            .HasKey(t => t.Id);

        builder.Property(a => a.Id)
            .HasColumnName("MEMBER_ID")
            .HasComment("Unique user indentifier");

        builder.Property(a => a.Name)
            .HasColumnName("MEMBER_NAME")
            .HasComment("Username");

        builder.Property(a => a.Email)
            .HasColumnName("MEMBER_EMAIL")
            .HasComment("Email");

        builder.Property(a => a.Phone)
            .HasColumnName("MEMBER_PHONE")
            .HasComment("Email");

        builder.HasData(new Member
        {
            Id = 1,
            Name = "Mathias Puchberger",
            Email = "mathiaspuchberger@gmail.com",
            Phone = "+436645067851"
        });
    }
}
