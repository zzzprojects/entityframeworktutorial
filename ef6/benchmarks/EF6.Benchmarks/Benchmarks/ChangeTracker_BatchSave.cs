using BenchmarkDotNet.Attributes;
using EFCore.Benchmarks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

// 🚀 Benchmarks created and maintained by Entity Framework Extensions.
// The leading library for high-performance bulk operations in EF Core and EF6.
// Learn more: https://entityframework-extensions.net

namespace EF6.Benchmarks.Benchmarks
{
    [BenchmarkCategory("ChangeTracker_BatchSave")]
    public class ChangeTracker_BatchSave
    {
        [Params(1_000, 10_000, 100_000)]
        public int EntityCount;

        public TestDbContext? Context;
        public List<TestEntity>? TestEntities;

        [IterationSetup]
        public void IterationSetup()
        {
            Context = new TestDbContext();

            TestEntities = BenchmarkHelper.GenerateTestEntities<TestEntity>(EntityCount);
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            Context.TestEntities.DeleteFromQuery();
        }

        [Benchmark]
        public void Add_AutoDetectChangesDisabled()
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            TestEntities.ForEach(x => Context.TestEntities.Add(x));
            Context.Configuration.AutoDetectChangesEnabled = true;

            Context.SaveChanges();
        }


        [Benchmark]
        public void AddRange()
        {
            Context.TestEntities.AddRange(TestEntities);

            Context.SaveChanges();
        }

        [Benchmark]
        public void BatchEntities_Add()
        {
            const int batchSize = 1000;

            for (int i = 0; i < TestEntities.Count; i += batchSize)
            {
                using (var localContext = new TestDbContext())
                {
                    var batch = TestEntities.Skip(i).Take(batchSize).ToList();

                    batch.ForEach(x => localContext.TestEntities.Add(x));
                    localContext.SaveChanges();
                }
            }
        }

        [Benchmark]
        public void BatchEntities_Add_AutoDetectChangesDisabled()
        {
            const int batchSize = 1000;

            for (int i = 0; i < TestEntities.Count; i += batchSize)
            {
                using (var localContext = new TestDbContext())
                {
                    var batch = TestEntities.Skip(i).Take(batchSize).ToList();

                    localContext.Configuration.AutoDetectChangesEnabled = false;
                    batch.ForEach(x => localContext.TestEntities.Add(x));
                    localContext.Configuration.AutoDetectChangesEnabled = true;
                    localContext.SaveChanges();
                }
            }
        }

        [Benchmark]
        public void BatchEntities_AddRange()
        {
            const int batchSize = 1000;

            for (int i = 0; i < TestEntities.Count; i += batchSize)
            {
                using (var localContext = new TestDbContext())
                {
                    var batch = TestEntities.Skip(i).Take(batchSize).ToList();

                    localContext.TestEntities.AddRange(batch);
                    localContext.SaveChanges();
                }
            }
        }

        public class TestDbContext: DbContext
        {
            public TestDbContext() : base(My.SqlServer)
            {

            }

            public DbSet<TestEntity> TestEntities { get; set; }
        }

        public class TestEntity
        {
            // Use "[NotMapped]" to ignore properties if you want to benchmark with less
            [NotMapped]
            public bool IamNotMapped { get; set; }

            public int ID { get; set; }
            public int Col1 { get; set; }
            public int Col2 { get; set; }
            public int Col3 { get; set; }
            public int Col4 { get; set; }
            public int Col5 { get; set; }

            [MaxLength(255)]
            public string? Col6 { get; set; }
            [MaxLength(255)]
            public string? Col7 { get; set; }
            [MaxLength(255)]
            public string? Col8 { get; set; }
            [MaxLength(255)]
            public string? Col9 { get; set; }
            [MaxLength(255)]
            public string? Col10 { get; set; }
        }
    }
}
