using EventsApi.Models;

namespace EventsApi.Data;

public static class DatabaseSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Events.Any()) return;

        var events = new List<Event>
        {
            new()
            {
                Id = "evt-001",
                Name = "Meetup ITU Developers — Spec Driven Design",
                Description = "Palestra sobre como usar specs OpenAPI para guiar o desenvolvimento com GitHub Copilot. Demo ao vivo: da spec para uma API C# em tempo real.",
                Date = new DateTime(2025, 5, 15, 19, 0, 0, DateTimeKind.Local),
                Location = "Centro de Convenções de Ituiutaba",
                Capacity = 100,
                AvailableSpots = 37,
                TicketPrice = 0.00m,
                Status = EventStatus.Published,
                CreatedAt = new DateTime(2025, 4, 1, 10, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = "evt-002",
                Name = "Workshop: Clean Architecture com .NET",
                Description = "Workshop prático de 4 horas sobre como estruturar projetos .NET usando Clean Architecture. Exemplos reais com EF Core e testes automatizados.",
                Date = new DateTime(2025, 6, 20, 9, 0, 0, DateTimeKind.Local),
                Location = "Faculdade de Tecnologia de Ituiutaba",
                Capacity = 40,
                AvailableSpots = 15,
                TicketPrice = 50.00m,
                Status = EventStatus.Published,
                CreatedAt = new DateTime(2025, 4, 15, 14, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = "evt-003",
                Name = "Tech Talk: IA no Cotidiano do Dev",
                Description = "Mesa redonda com devs locais sobre como a inteligência artificial está mudando o jeito de escrever código. GitHub Copilot, ChatGPT e mais.",
                Date = new DateTime(2025, 7, 10, 18, 30, 0, DateTimeKind.Local),
                Location = "Espaço Coworking Triângulo",
                Capacity = 60,
                AvailableSpots = 60,
                TicketPrice = 0.00m,
                Status = EventStatus.Draft,
                CreatedAt = new DateTime(2025, 5, 1, 8, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = "evt-004",
                Name = "Hackathon ITU Dev 2025",
                Description = "24 horas de código, pizza e networking. Equipes de até 4 pessoas. Prêmios para os 3 primeiros colocados. Tema: soluções para mobilidade urbana.",
                Date = new DateTime(2025, 8, 23, 8, 0, 0, DateTimeKind.Local),
                Location = "Campus UFU Ituiutaba — Bloco B",
                Capacity = 80,
                AvailableSpots = 0,
                TicketPrice = 30.00m,
                Status = EventStatus.Published,
                CreatedAt = new DateTime(2025, 5, 10, 12, 0, 0, DateTimeKind.Utc)
            }
        };

        context.Events.AddRange(events);
        context.SaveChanges();
    }
}
