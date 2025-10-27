namespace InstaDelivery.DeliveryService.Application.Dto;

public class DeliveryAgentBaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsOnline { get; set; } = true;

    public string? Metadata { get; set; }
}

public class CreateDeliveryAgentDto : DeliveryAgentBaseDto
{
}

public class DeliveryAgentDto : DeliveryAgentBaseDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}