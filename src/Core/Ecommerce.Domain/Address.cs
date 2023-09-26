using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class Address : BaseDomainModel
{
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? Line1 { get; set; }
    public string? PostalCode { get; set; }
    public string? UserName { get; set; }
    public string? Country { get; set; }
}