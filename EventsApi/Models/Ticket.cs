namespace EventsApi.Models;

public class Ticket
{
    public string Id { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public Event Event { get; set; } = null!;
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public TicketStatus Status { get; set; } = TicketStatus.Active;
}

public enum TicketStatus
{
    Active,
    Cancelled
}
