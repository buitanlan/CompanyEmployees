using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyEmployees.IDP.Entities.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
        [
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
            },
            new IdentityRole
            {
                Name = "Visitor",
                NormalizedName = "VISITOR",
                Id = "a9ea0f25-b964-409f-bcce-c923266249b4",
            }
        ]);
    }
}
