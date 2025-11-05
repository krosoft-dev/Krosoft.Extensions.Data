using Krosoft.Extensions.Data.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Krosoft.Extensions.Data.EntityFramework.Tests.Repositories;

[TestClass]
public class WriteRepositoryExecuteUpdateTests
{
    private TestDbContext _dbContext = null!;
    private WriteRepository<FlowExecution> _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;

        _dbContext = new TestDbContext(options);
        _repository = new WriteRepository<FlowExecution>(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Dispose();
        _repository.Dispose();
    }

    [TestMethod]
    public async Task UpdateRangeAsync_Should_Update_Single_Property()
    {
        // Arrange
        var agent = new Agent { Id = 1, Name = "Agent1" };
        var flow1 = new FlowExecution { Id = 1, Name = "Flow1", AgentId = 1 };
        var flow2 = new FlowExecution { Id = 2, Name = "Flow2", AgentId = 1 };
        var flow3 = new FlowExecution { Id = 3, Name = "Flow3", AgentId = 2 };

        _dbContext.Agents.AddRange(agent);
        _dbContext.FlowExecutions.AddRange(flow1, flow2, flow3);
        await _dbContext.SaveChangesAsync();

        // Act
        var affectedRows = await _repository.UpdateRangeAsync(fe => fe.AgentId == 1,
                                                              builder => builder.SetProperty(f => f.AgentId, null),
                                                              CancellationToken.None);

        // Assert
        Check.That(affectedRows).IsEqualTo(2);

        var updatedFlows = await _dbContext.FlowExecutions
                                           .Where(f => f.Id == 1 || f.Id == 2)
                                           .ToListAsync();

        Check.That(updatedFlows).HasSize(2);
        Check.That(updatedFlows.All(f => f.AgentId == null)).IsTrue();

        var unchangedFlow = await _dbContext.FlowExecutions.FindAsync(3);
        Check.That(unchangedFlow!.AgentId).IsEqualTo(2);
    }

    [TestMethod]
    public async Task UpdateRangeAsync_Should_Update_Multiple_Properties()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var flow1 = new FlowExecution { Id = 1, Name = "Flow1", AgentId = 1, Status = "Active", UpdatedAt = now.AddDays(-1) };
        var flow2 = new FlowExecution { Id = 2, Name = "Flow2", AgentId = 1, Status = "Active", UpdatedAt = now.AddDays(-1) };
        var flow3 = new FlowExecution { Id = 3, Name = "Flow3", AgentId = 2, Status = "Active", UpdatedAt = now.AddDays(-1) };

        _dbContext.FlowExecutions.AddRange(flow1, flow2, flow3);
        await _dbContext.SaveChangesAsync();

        // Act
        var affectedRows = await _repository.UpdateRangeAsync(
                                                              fe => fe.AgentId == 1,
                                                              builder => builder
                                                                         .SetProperty(f => f.AgentId, null)
                                                                         .SetProperty(f => f.Status, "Inactive")
                                                                         .SetProperty(f => f.UpdatedAt, now),
                                                              CancellationToken.None);

        // Assert
        Check.That(affectedRows).IsEqualTo(2);

        var updatedFlows = await _dbContext.FlowExecutions
                                           .Where(f => f.Id == 1 || f.Id == 2)
                                           .ToListAsync();

        Check.That(updatedFlows).HasSize(2);
        Check.That(updatedFlows.All(f => f.AgentId == null)).IsTrue();
        Check.That(updatedFlows.All(f => f.Status == "Inactive")).IsTrue();
        Check.That(updatedFlows.All(f => f.UpdatedAt >= now.AddSeconds(-1))).IsTrue();

        var unchangedFlow = await _dbContext.FlowExecutions.FindAsync(3);
        Check.That(unchangedFlow!.AgentId).IsEqualTo(2);
        Check.That(unchangedFlow.Status).IsEqualTo("Active");
    }

    [TestMethod]
    public async Task UpdateRangeAsync_Should_Throw_When_No_Properties_Specified()
    {
        // Arrange
        var flow = new FlowExecution { Id = 1, Name = "Flow1", AgentId = 1 };
        _dbContext.FlowExecutions.Add(flow);
        await _dbContext.SaveChangesAsync();

        // Act & Assert
        Check.ThatCode(async () =>
                                await _repository.UpdateRangeAsync(
                                                                   fe => fe.AgentId == 1,
                                                                   builder => { }, // Aucune propriété
                                                                   CancellationToken.None))
             .Throws<ArgumentException>()
             .WithMessage("Au moins une propriété doit être spécifiée");
    }

    [TestMethod]
    public async Task UpdateRangeAsync_Should_Return_Zero_When_No_Matching_Entities()
    {
        // Arrange
        var flow = new FlowExecution { Id = 1, Name = "Flow1", AgentId = 1 };
        _dbContext.FlowExecutions.Add(flow);
        await _dbContext.SaveChangesAsync();

        // Act
        var affectedRows = await _repository.UpdateRangeAsync(
                                                              fe => fe.AgentId == 999, // Aucune entité ne correspond
                                                              builder => builder.SetProperty(f => f.AgentId, null),
                                                              CancellationToken.None);

        // Assert
        Check.That(affectedRows).IsEqualTo(0);

        var unchangedFlow = await _dbContext.FlowExecutions.FindAsync(1);
        Check.That(unchangedFlow!.AgentId).IsEqualTo(1);
    }
}

// Classes de test
public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<FlowExecution> FlowExecutions { get; set; } = null!;
    public DbSet<Agent> Agents { get; set; } = null!;
}

public class FlowExecution
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? AgentId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}

public class Agent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}