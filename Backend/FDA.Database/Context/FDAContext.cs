using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FDA.Database.Model;

namespace FDA.Database.Context;

public class FDAContext : DbContext
{
    public FDAContext(DbContextOptions<FDAContext> options) : base(options)
    {

    }

    public DbSet<Member> Members { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            property.SetColumnType("datetime");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FDAContext).Assembly);
    }
}

public class FDAContextFactory : IDesignTimeDbContextFactory<FDAContext>
{
    public FDAContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FDAContext>();
        optionsBuilder.UseSqlite("Data Source=../../LLPrintServer.Backend/LLPrintServer.db");

        return new FDAContext(optionsBuilder.Options);
    }
}
