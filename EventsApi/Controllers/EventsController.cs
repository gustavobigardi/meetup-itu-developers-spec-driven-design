using EventsApi.DTOs;
using EventsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventsApi.Controllers;

[ApiController]
[Route("events")]
[Produces("application/json")]
public class EventsController(IEventsService eventsService) : ControllerBase
{
    /// <summary>Lista todos os eventos com suporte a filtro e paginação.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<EventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListEvents(
        [FromQuery] string? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await eventsService.ListEventsAsync(status, page, pageSize);
        return Ok(result);
    }

    /// <summary>Busca um evento pelo seu identificador único.</summary>
    [HttpGet("{eventId}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventById(string eventId)
    {
        var evt = await eventsService.GetEventByIdAsync(eventId);
        if (evt is null)
            return NotFound(new ErrorResponse("NOT_FOUND", "Evento não encontrado"));

        return Ok(evt);
    }

    /// <summary>Cria um novo evento com status 'draft'.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        if (!ModelState.IsValid)
        {
            var details = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => new ErrorDetail(x.Key, e.ErrorMessage)));

            return BadRequest(new ErrorResponse("VALIDATION_ERROR", "Dados inválidos na requisição", details));
        }

        var created = await eventsService.CreateEventAsync(request);
        return CreatedAtAction(nameof(GetEventById), new { eventId = created.Id }, created);
    }

    /// <summary>Atualiza parcialmente um evento. Apenas os campos enviados são modificados.</summary>
    [HttpPut("{eventId}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEvent(string eventId, [FromBody] UpdateEventRequest request)
    {
        if (!ModelState.IsValid)
        {
            var details = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => new ErrorDetail(x.Key, e.ErrorMessage)));

            return BadRequest(new ErrorResponse("VALIDATION_ERROR", "Dados inválidos na requisição", details));
        }

        var updated = await eventsService.UpdateEventAsync(eventId, request);
        if (updated is null)
            return NotFound(new ErrorResponse("NOT_FOUND", "Evento não encontrado"));

        return Ok(updated);
    }

    /// <summary>Cancela um evento, alterando seu status para 'cancelled'.</summary>
    [HttpDelete("{eventId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelEvent(string eventId)
    {
        var cancelled = await eventsService.CancelEventAsync(eventId);
        if (!cancelled)
            return NotFound(new ErrorResponse("NOT_FOUND", "Evento não encontrado"));

        return NoContent();
    }
}
