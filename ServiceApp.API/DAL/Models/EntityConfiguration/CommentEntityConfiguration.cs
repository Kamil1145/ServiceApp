using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceApp.Models;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Models.EntityConfiguration;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{

    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        builder.HasKey(cr => cr.Id);
    }
}