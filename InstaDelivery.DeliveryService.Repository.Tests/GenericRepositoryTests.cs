using InstaDelivery.DeliveryService.Domain.Entities;
using InstaDelivery.DeliveryService.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InstaDelivery.DeliveryService.Repository.Tests;

[TestFixture]
public class GenericRepository_DeliveryTests
{
    private string _dbName = string.Empty;
    private DbContextOptions<DeliveryDbContext> _options = null!;

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static DbContextOptions<DeliveryDbContext> CreateNewContextOptions(string dbName) =>
        new DbContextOptionsBuilder<DeliveryDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

    private static async Task<List<Delivery>> LoadDeliveriesFromJsonAsync()
    {
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "deliveries.json");

        if (!File.Exists(path))
            return [];

        var json = await File.ReadAllTextAsync(path).ConfigureAwait(false);
        try
        {

            var list = JsonSerializer.Deserialize<List<Delivery>>(json, jsonSerializerOptions);
            return list ?? [];
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Failed to parse JSON seed file at '{path}'.", ex);
        }
    }

    private static async Task<List<Delivery>> SeedFromJsonAsync(DeliveryDbContext ctx)
    {
        var items = await LoadDeliveriesFromJsonAsync().ConfigureAwait(false);

        if (items.Count > 0)
        {
            ctx.Deliveries.AddRange(items);
            await ctx.SaveChangesAsync().ConfigureAwait(false);
        }

        return items;
    }

    [SetUp]
    public async Task SetUp()
    {
        _dbName = Guid.NewGuid().ToString();
        _options = CreateNewContextOptions(_dbName);

        await using var ctx = new DeliveryDbContext(_options);
        await ctx.Database.EnsureDeletedAsync().ConfigureAwait(false);
        await ctx.Database.EnsureCreatedAsync().ConfigureAwait(false);

        await SeedFromJsonAsync(ctx).ConfigureAwait(false);
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_options != null)
        {
            await using var ctx = new DeliveryDbContext(_options);
            await ctx.Database.EnsureDeletedAsync().ConfigureAwait(false);
        }
    }

    [Test]
    public void Constructor_WithNullDb_ThrowsArgumentNullException()
    {
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => new GenericRepository<Delivery>(null!));
    }

    [Test]
    public async Task AddAsync_AddsDelivery()
    {
        //Arrange
        var options = CreateNewContextOptions(_dbName);
        await using var ctx = new DeliveryDbContext(options);
        var repo = new GenericRepository<Delivery>(ctx);

        var delivery = new Delivery
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            DeliveryAddress = "B Street",
            Status = "Pending"
        };

        //Act
        var result = await repo.AddAsync(delivery).ConfigureAwait(false);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(delivery.Id));
    }

    [Test]
    public async Task WhenEntityExists_GetByIdAsync_ReturnsEntity()
    {
        //Arrange
        var options = CreateNewContextOptions(_dbName);

        var seededList = await LoadDeliveriesFromJsonAsync().ConfigureAwait(false);
        var deliveryId = seededList.First().Id;

        await using var ctx = new DeliveryDbContext(options);
        var repo = new GenericRepository<Delivery>(ctx);

        //Act
        var found = await repo.GetByIdAsync(deliveryId).ConfigureAwait(false);

        //Assert
        Assert.That(found, Is.Not.Null);
        Assert.That(found!.Id, Is.EqualTo(deliveryId));
    }

    [Test]
    public async Task GetAllAsync_ReturnsAllDeliveries()
    {
        //Arrange
        var options = CreateNewContextOptions(_dbName);
        await using var ctx = new DeliveryDbContext(options);
        var repo = new GenericRepository<Delivery>(ctx);

        //Act
        var all = (await repo.GetAllAsync().ConfigureAwait(false)).ToList();

        //Assert
        var expectedCount = (await LoadDeliveriesFromJsonAsync().ConfigureAwait(false)).Count;
        Assert.That(all, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public async Task FindAsync_FiltersByStatus()
    {
        //Arrange
        var options = CreateNewContextOptions(_dbName);
        await using var ctx = new DeliveryDbContext(options);
        var repo = new GenericRepository<Delivery>(ctx);

        //Act
        var results = await repo.FindAsync(d => d.Status == "Delivered").ConfigureAwait(false);

        //Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.First().Status, Is.EqualTo("Delivered"));
    }

    [Test]
    public async Task GetPagedAsync_ReturnsCorrectPage()
    {
        //Arrange
        var options = CreateNewContextOptions(_dbName);
        await using var ctx = new DeliveryDbContext(options);
        var repo = new GenericRepository<Delivery>(ctx);

        var expectedTotal = (await LoadDeliveriesFromJsonAsync().ConfigureAwait(false)).Count;

        var page = 1;
        var pageSize = 5;

        //Act
        var (items, total) = await repo.GetPagedAsync(page: page, pageSize: pageSize).ConfigureAwait(false);

        //Assert
        Assert.Multiple(() =>
        {

            Assert.That(total, Is.EqualTo(expectedTotal));
            Assert.That(items.Count(), Is.EqualTo(Math.Min(pageSize, expectedTotal)));
        });
    }

    [Test]
    public async Task WhenEntityExists_AnyAsync_ReturnsTrue()
    {
        //Arrange
        var options = CreateNewContextOptions(_dbName);

        var seededList = await LoadDeliveriesFromJsonAsync().ConfigureAwait(false);
        var seededOrderId = seededList.First().OrderId;

        await using var ctx = new DeliveryDbContext(options);
        var repo = new GenericRepository<Delivery>(ctx);

        //Act
        var exists = await repo.AnyAsync(d => d.OrderId == seededOrderId).ConfigureAwait(false);

        //Assert
        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task CountAsync_ReturnsExpectedValues()
    {
        //Arrange
        var options = CreateNewContextOptions(_dbName);
        await using var ctx = new DeliveryDbContext(options);
        var repo = new GenericRepository<Delivery>(ctx);

        //Act
        var total = await repo.CountAsync().ConfigureAwait(false);
        var delivered = await repo.CountAsync(d => d.Status == "Delivered").ConfigureAwait(false);


        //Assert
        var deliveries = await LoadDeliveriesFromJsonAsync().ConfigureAwait(false);
        var seededCount = deliveries.Count;
        var deliveredCount = deliveries.Count(d => d.Status == "Delivered");

        Assert.Multiple(() =>
        {
            Assert.That(total, Is.EqualTo(seededCount));
            Assert.That(delivered, Is.LessThanOrEqualTo(deliveredCount));
        });
    }
}
