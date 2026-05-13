using EventsApi.DTOs;
using EventsApi.Models;

namespace EventsApi.Mapping;

public static class MappingExtensions
{
    public static EventDto ToDto(this Event e) => new(
        e.Id,
        e.Name,
        e.Description,
        e.Date,
        e.Location,
        e.Capacity,
        e.AvailableSpots,
        e.TicketPrice,
        e.Status.ToString().ToLower(),
        e.CreatedAt
    );
}
