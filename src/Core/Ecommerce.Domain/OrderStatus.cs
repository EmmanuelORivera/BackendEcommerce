using System.Runtime.Serialization;

namespace Ecommerce.Domain;

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Payment was completed")]
    Completed,
    [EnumMember(Value = "Product was send")]
    Send,
    [EnumMember(Value = "There is an error on the payment")]
    Error
}