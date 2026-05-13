using System.ComponentModel.DataAnnotations;

namespace EventsApi.DTOs;

public class UpdateEventRequest
{
    [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
    [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string? Name { get; set; }

    [MaxLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres")]
    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    [MinLength(3, ErrorMessage = "O local deve ter no mínimo 3 caracteres")]
    [MaxLength(300, ErrorMessage = "O local deve ter no máximo 300 caracteres")]
    public string? Location { get; set; }

    [Range(1, 100000, ErrorMessage = "A capacidade deve ser entre 1 e 100.000")]
    public int? Capacity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "O preço não pode ser negativo")]
    public decimal? TicketPrice { get; set; }

    public string? Status { get; set; }
}
