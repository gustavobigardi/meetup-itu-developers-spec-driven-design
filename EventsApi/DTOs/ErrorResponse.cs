namespace EventsApi.DTOs;

public record ErrorResponse(
    string Code,
    string Message,
    IEnumerable<ErrorDetail>? Details = null
);

public record ErrorDetail(string Field, string Message);
