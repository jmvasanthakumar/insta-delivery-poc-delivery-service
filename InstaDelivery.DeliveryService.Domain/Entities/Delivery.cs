using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaDelivery.DeliveryService.Domain.Entities;

public class Delivery : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public Guid OrderId { get; set; }

    public Guid? DeliveryAgentId { get; set; }

    [Required]
    public string DeliveryAddress { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public DateTimeOffset? AssignedAt { get; set; }

    public DateTimeOffset? PickedUpAt { get; set; }

    public DateTimeOffset? DeliveredAt { get; set; }

    public DateTimeOffset? EstimatedDeliveryTime { get; set; }

    [MaxLength(500)]
    public string? FailureReason { get; set; }

    [MaxLength(500)]
    public string? CancellationReason { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;


    [ForeignKey(nameof(DeliveryAgentId))]
    public DeliveryAgent? DeliveryAgent { get; set; }
}
