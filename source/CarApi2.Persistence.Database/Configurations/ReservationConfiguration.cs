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
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("uniqueidentifier").ValueGeneratedOnAdd();
            
            builder.Property(t => t.EndDuration).IsRequired();
            builder.Property(t => t.StartDuration).IsRequired();
            builder.Property(t => t.ReservationDate).IsRequired();
            builder.ToTable("Reservation");
            builder.HasIndex(p => new { p.StartDuration, p.CarId,p.ReservationDate })
            .IsUnique(true);

            builder.HasOne<Car>(x=> x.Car).WithMany(x=>x.Reservations).HasForeignKey(x => x.CarId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
