using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Models.EntityConfiguration;

public class DepartmentEntityConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        //Seed Departments Table
        builder.HasData(
            new Department { DepartmentId = 1, DepartmentName = "IT" });
        builder.HasData(
            new Department { DepartmentId = 2, DepartmentName = "HR" });
        builder.HasData(
            new Department { DepartmentId = 3, DepartmentName = "Payroll" });
        builder.HasData(
            new Department { DepartmentId = 4, DepartmentName = "Admin" });
    }
}