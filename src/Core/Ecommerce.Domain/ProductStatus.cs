using System.Runtime.Serialization;

namespace Ecommerce.Domain;

public enum ProductStatus {
    [EnumMember(Value ="Inactive Product")]
    Inactive,
    [EnumMember(Value ="Active Product")]
    Active
}