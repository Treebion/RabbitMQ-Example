namespace RabbitMQ.Shared.Messages;

/// <summary>
/// Represents a notification message to be sent to a recipient.
/// </summary>
public sealed record NotificationMessage
{
    /// <summary>
    /// Gets the unique identifier for the notification message.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the recipient of the notification message.
    /// </summary>
    public string Recipient { get; init; }

    /// <summary>
    /// Gets the content of the notification message.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Gets the type of the notification message.
    /// </summary>
    public MessageType Type { get; init; }
}

/// <summary>
/// Specifies the type of the notification message.
/// </summary>
public enum MessageType
{
    /// <summary>
    /// Represents an email message.
    /// </summary>
    Email,

    /// <summary>
    /// Represents an SMS message.
    /// </summary>
    SMS,

    /// <summary>
    /// Represents a push notification message.
    /// </summary>
    Push
}
