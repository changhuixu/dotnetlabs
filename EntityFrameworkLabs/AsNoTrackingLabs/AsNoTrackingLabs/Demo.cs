using System;
using System.Linq;
using AsNoTrackingLabs.DbContext;
using AsNoTrackingLabs.Models;
using Microsoft.EntityFrameworkCore;

namespace AsNoTrackingLabs
{
    public class Demo
    {
        public static void Run()
        {
            MyDbContext.EnsureDatabaseIsCleaned();

            InitDatabase();

            Console.WriteLine("Step 0: No query yet (in a fresh DB Context).");
            Step0();
            Console.WriteLine();

            Console.WriteLine("Step 1: make a NoTracking Query");
            AsNoTrackingQuery();
            Console.WriteLine();

            Console.WriteLine("Step 2: make a Tracking Query with AsQueryable");
            AsQueryableQuery();
            Console.WriteLine();

            Console.WriteLine("Step 3: make another NoTracking Query");
            AsNoTrackingQuery();
            Console.WriteLine();

            Console.WriteLine("Step 4: make a Tracking Query with AsEnumerable");
            AsEnumerableQuery();
            Console.WriteLine();

        }

        private static void InitDatabase()
        {
            using var dbContext = new MyDbContext();
            Console.WriteLine("Initialize Database.");
            dbContext.Database.Migrate();

            var todos = Enumerable.Range(0, 100000).Select(x => new Todo { TaskName = $"Task {x}" });
            dbContext.Todos.AddRange(todos);
            dbContext.SaveChanges();
            Console.WriteLine("Database initialization done.");
            Console.WriteLine();
        }

        private static void Step0()
        {
            using var dbContext = new MyDbContext();
            var trackingCount = dbContext.ChangeTracker.Entries().Count();
            Console.WriteLine($"\t {trackingCount} ChangeTracker Entries.");
            // output: 0 ChangeTracker Entries.
        }

        private static void AsNoTrackingQuery()
        {
            using var dbContext = new MyDbContext();
            _ = dbContext.Todos.AsNoTracking().ToList();
            var trackingCount = dbContext.ChangeTracker.Entries().Count();
            Console.WriteLine($"\t {trackingCount} ChangeTracker Entries.");
            // output: 0 ChangeTracker Entries.
        }

        private static void AsQueryableQuery()
        {
            using var dbContext = new MyDbContext();
            _ = dbContext.Todos.AsQueryable().ToList();
            var trackingCount = dbContext.ChangeTracker.Entries().Count();
            Console.WriteLine($"\t {trackingCount} ChangeTracker Entries.");
            // output: 100000 ChangeTracker Entries.
        }

        private static void AsEnumerableQuery()
        {
            using var dbContext = new MyDbContext();
            _ = dbContext.Todos.AsQueryable().ToList();
            var trackingCount = dbContext.ChangeTracker.Entries().Count();
            Console.WriteLine($"\t {trackingCount} ChangeTracker Entries.");
            // output: 100000 ChangeTracker Entries.
        }

        private static void Test7()
        {
            using var dbContext = new MyDbContext();
            Console.WriteLine("make a Tracking Query with AsEnumerable");
            _ = dbContext.Todos.AsEnumerable().ToList();
            WritePerf(dbContext);
        }

        private static void WritePerf(Microsoft.EntityFrameworkCore.DbContext dbContext)
        {
            var trackingCount = dbContext.ChangeTracker.Entries().Count();
            var mem = GC.GetTotalMemory(true) / 1024 / 1024; 
            Console.WriteLine($"\t {trackingCount} ChangeTracker Entries. Total Memory after GC: {mem}mb");
            Console.WriteLine();
        }
    }
}
