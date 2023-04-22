using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data;

namespace Sanasoppa.API.Tests.Repositories;

public static class RepostitoryTestUtils
{
    public static DbContextOptions<DataContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
}