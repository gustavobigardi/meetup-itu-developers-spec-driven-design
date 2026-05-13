namespace EventsApi.DTOs;

public record EventDto(
    string Id,
    string Name,
    string? Description,
    DateTime Date,
    string Location,
    int Capacity,
    int AvailableSpots,
    decimal TicketPrice,
    string Status,
    DateTime CreatedAt
);

public record PagedResponse<T>(
    IEnumerable<T> Data,
    int Total,
    int Page,
    int PageSize
);
