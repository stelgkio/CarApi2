using CarApi2.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarApi2.Persistence.Database.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("uniqueidentifier").ValueGeneratedOnAdd();

            builder.Property(t => t.Model).IsRequired();
            builder.Property(t => t.Mark).IsRequired();
            builder.Property(t => t.CarId).HasComputedColumnSql("C +'_'+[Id]", stored: true);
            builder.ToTable("Cars");
        
            builder.HasMany<Reservation>().WithOne(x=>x.Car).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
