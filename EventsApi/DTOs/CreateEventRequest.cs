using System.ComponentModel.DataAnnotations;

namespace EventsApi.DTOs;

public class CreateEventRequest
{
    [Required(ErrorMessage = "O nome do evento é obrigatório")]
    [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
    [MaxLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "A data do evento é obrigatória")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "O local do evento é obrigatório")]
    [MinLength(3, ErrorMessage = "O local deve ter no mínimo 3 caracteres")]
    [MaxLength(300, ErrorMessage = "O local deve ter no máximo 300 caracteres")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "A capacidade é obrigatória")]
    [Range(1, 100000, ErrorMessage = "A capacidade deve ser entre 1 e 100.000")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "O preço do ingresso é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "O preço não pode ser negativo")]
    public decimal TicketPrice { get; set; }
}
