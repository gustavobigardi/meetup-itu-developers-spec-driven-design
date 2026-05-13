using EventsApi.Data;
using EventsApi.DTOs;
using EventsApi.Mapping;
using EventsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Services;

public class EventsService(AppDbContext db) : IEventsService
{
    public async Task<PagedResponse<EventDto>> ListEventsAsync(string? status, int page, int pageSize)
    {
        var query = db.Events.AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<EventStatus>(status, ignoreCase: true, out var eventStatus))
            query = query.Where(e => e.Status == eventStatus);

        var total = await query.CountAsync();

        var events = await query
            .OrderBy(e => e.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => e.ToDto())
            .ToListAsync();

        return new PagedResponse<EventDto>(events, total, page, pageSize);
    }

    public async Task<EventDto?> GetEventByIdAsync(string id) =>
        (await db.Events.FindAsync(id))?.ToDto();

    public async Task<EventDto> CreateEventAsync(CreateEventRequest request)
    {
        var evt = new Event
        {
            Id = $"evt-{Guid.NewGuid():N}"[..14],
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            Location = request.Location,
            Capacity = request.Capacity,
            AvailableSpots = request.Capacity,
            TicketPrice = request.TicketPrice,
            Status = EventStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        db.Events.Add(evt);
        await db.SaveChangesAsync();
        return evt.ToDto();
    }

    public async Task<EventDto?> UpdateEventAsync(string id, UpdateEventRequest request)
    {
        var evt = await db.Events.FindAsync(id);
        if (evt is null) return null;

        if (request.Name is not null) evt.Name = request.Name;
        if (request.Description is not null) evt.Description = request.Description;
        if (request.Date is not null) evt.Date = request.Date.Value;
        if (request.Location is not null) evt.Location = request.Location;
        if (request.Capacity is not null) evt.Capacity = request.Capacity.Value;
        if (request.TicketPrice is not null) evt.TicketPrice = request.TicketPrice.Value;

        if (request.Status is not null && Enum.TryParse<EventStatus>(request.Status, ignoreCase: true, out var status))
            evt.Status = status;

        await db.SaveChangesAsync();
        return evt.ToDto();
    }

    public async Task<bool> CancelEventAsync(string id)
    {
        var evt = await db.Events.FindAsync(id);
        if (evt is null) return false;

        evt.Status = EventStatus.Cancelled;
        await db.SaveChangesAsync();
        return true;
    }
}
