#region

using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog;

public class Address : ValueObject
{
    public Address(
        string country,
        string city,
        string district,
        string street,
        string postalCode,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        if (string.IsNullOrWhiteSpace(district))
            throw new ArgumentException("District cannot be empty", nameof(district));
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

        Country = country.Trim();
        City = city.Trim();
        District = district.Trim();
        Street = street.Trim();
        PostalCode = postalCode.Trim();
        Description = description?.Trim();
    }

    public string Country { get; }
    public string City { get; }
    public string District { get; }
    public string Street { get; }
    public string PostalCode { get; }
    public string? Description { get; }

    // Formatting methods
    public string GetFullAddress()
    {
        List<string> parts = new List<string> { Street, District, City, Country };

        if (!string.IsNullOrEmpty(Description))
            parts.Insert(0, Description);

        return string.Join(", ", parts);
    }

    public string GetShortAddress()
    {
        return $"{District}, {City}";
    }

    public string GetAddressWithPostalCode()
    {
        return $"{GetFullAddress()} {PostalCode}";
    }

    public string GetCityInfo()
    {
        return $"{City}, {Country}";
    }


    public bool IsInSameCity(Address other)
    {
        return string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInSameDistrict(Address other)
    {
        return IsInSameCity(other) &&
               string.Equals(District, other.District, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInSameCountry(Address other)
    {
        return string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase);
    }

    public bool HasDescription()
    {
        return !string.IsNullOrWhiteSpace(Description);
    }

    public bool IsCompleteAddress()
    {
        return !string.IsNullOrWhiteSpace(Country) &&
               !string.IsNullOrWhiteSpace(City) &&
               !string.IsNullOrWhiteSpace(District) &&
               !string.IsNullOrWhiteSpace(Street) &&
               !string.IsNullOrWhiteSpace(PostalCode);
    }

    public static Address Create(string country, string city, string district, string street, string postalCode,
        string? description = null)
    {
        return new Address(country, city, district, street, postalCode, description);
    }

    public Address WithDescription(string newDescription)
    {
        return new Address(Country, City, District, Street, PostalCode, newDescription);
    }

    public Address WithoutDescription()
    {
        return new Address(Country, City, District, Street, PostalCode);
    }

    // Distance/location methods (if needed)
    public string GetRegionIdentifier()
    {
        return $"{Country}-{City}-{District}";
    }

    // String representation
    public override string ToString()
    {
        return GetFullAddress();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return District;
        yield return Street;
        yield return PostalCode;
        yield return Description ?? string.Empty;
    }
}