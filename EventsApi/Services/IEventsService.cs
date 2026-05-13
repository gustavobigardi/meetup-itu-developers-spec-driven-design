using EventsApi.DTOs;

namespace EventsApi.Services;

public interface IEventsService
{
    Task<PagedResponse<EventDto>> ListEventsAsync(string? status, int page, int pageSize);
    Task<EventDto?> GetEventByIdAsync(string id);
    Task<EventDto> CreateEventAsync(CreateEventRequest request);
    Task<EventDto?> UpdateEventAsync(string id, UpdateEventRequest request);
    Task<bool> CancelEventAsync(string id);
}
