using System.Text.Json.Serialization;

namespace QuickToken.Facade.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionState
{
    InProgress,
    Completed
}