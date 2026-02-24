using System;

namespace API.Entities;

public class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }

    //nav Property
    public required string SenderId { get; set; }
    public Members Sender { get; set; } = null!;
    public required string RecipientId { get; set; }
    public Members Recipient { get; set; } = null!;
}
