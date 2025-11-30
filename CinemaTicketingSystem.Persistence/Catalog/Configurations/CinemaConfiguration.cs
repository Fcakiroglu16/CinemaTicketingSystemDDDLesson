#region

using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Catalog.Configurations;

public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
{
    public void Configure(EntityTypeBuilder<Cinema> builder)
    {
        // Table configuration
        builder.ToTable("Cinemas", "catalogs");

        // Primary key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(CinemaConst.NameMaxLength);

        // Owned type for Address (Value Object)
        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(CinemaConst.CountryMaxLength)
                .HasColumnName("Country");

            address.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(CinemaConst.CityMaxLength)
                .HasColumnName("City");

            address.Property(a => a.District)
                .IsRequired()
                .HasMaxLength(CinemaConst.DistrictMaxLength)
                .HasColumnName("District");

            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(CinemaConst.StreetMaxLength)
                .HasColumnName("Street");

            address.Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(CinemaConst.PostalCodeMaxLength)
                .HasColumnName("PostalCode");

            address.Property(a => a.Description)
                .HasMaxLength(CinemaConst.AddressDescriptionMaxLength)
                .HasColumnName("AddressDescription");
        });


        builder.HasMany(x => x.Halls).WithOne(x => x.Cinema);

        builder.Metadata.FindNavigation(nameof(Cinema.Halls))!.SetField("cinemaHalls");
    }
}