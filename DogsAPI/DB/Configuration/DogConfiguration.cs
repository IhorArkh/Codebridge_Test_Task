using DogsAPI.DB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogsAPI.DB.Configuration
{
    public class DogConfiguration : IEntityTypeConfiguration<Dog>
    {
        public void Configure(EntityTypeBuilder<Dog> builder)
        {
            builder.HasKey(d => d.Name);
            //cant create migration with predefined dogs without PK.
            //considered adding the id column, but since this is not in the task and the names must be unique,
            //I decided to choose Name as PK

            builder.Property(d => d.Name).HasColumnName("name");
            builder.Property(d => d.Color).HasColumnName("color");
            builder.Property(d => d.TailLength).HasColumnName("tail_length");
            builder.Property(d => d.Weight).HasColumnName("weight");

            builder.HasData(new Dog
            {
                Name = "Jessy",
                Color = "red & amber",
                TailLength = 22,
                Weight = 32
            });

            builder.HasData(new Dog
            {
                Name = "Neo",
                Color = "black & whiteeee",
                TailLength = 7,
                Weight = 14
            });
        }
    }
}
