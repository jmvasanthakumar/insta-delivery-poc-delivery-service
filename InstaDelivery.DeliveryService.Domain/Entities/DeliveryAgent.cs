using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaDelivery.DeliveryService.Domain.Entities;

public class DeliveryAgent : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(320)]
    public string? Email { get; set; }

    [Phone]
    [MaxLength(32)]
    public string? PhoneNumber { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [Required]
    public int Capacity { get; set; } = 1;

    [Required]
    public bool IsOnline { get; set; } = true;

    public string? Metadata { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [Required]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
