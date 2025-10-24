using InstaDelivery.DeliveryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstaDelivery.DeliveryService.Repository.Context;

internal class DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : DbContext(options)
{
    public DbSet<Delivery> Deliveries{ get; set; }
    public DbSet<DeliveryAgent> DeliveryAgents { get; set; }
}
