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
    [BenchmarkCategory("ChangeTracker_AddNextEntityPerformance")]
    public class ChangeTracker_AddNextEntityPerformance
    {
        [Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)]
        public int EntityCount;

        public TestDbContext? Context;
        public TestEntity_ZeroRelation ZeroRelation = null;
        public TestEntity_OneRelation OneRelation = null;
        public TestEntity_TwoRelation TwoRelation = null;

        [IterationSetup(Target = "Add_WithZeroRelation")]
        public void IterationSetup_NoRelation()
        {
            Context = new TestDbContext();

            Context.TestEntity_ZeroRelations.AddRange(BenchmarkHelper.GenerateTestEntities<TestEntity_ZeroRelation>(EntityCount));
            Context.ChangeTracker.DetectChanges();
            ZeroRelation = BenchmarkHelper.GenerateOneEntity<TestEntity_ZeroRelation>();
        }

        [IterationSetup(Target = "Add_WithOneRelation")]
        public void IterationSetup_OneRelation()
        {
            Context = new TestDbContext();

            Context.TestEntity_OneRelations.AddRange(BenchmarkHelper.GenerateTestEntities<TestEntity_OneRelation>(EntityCount));
            Context.ChangeTracker.DetectChanges();
            OneRelation = BenchmarkHelper.GenerateOneEntity<TestEntity_OneRelation>();       
        }

        [IterationSetup(Target = "Add_WithTwoRelation")]
        public void IterationSetup_TwoRelation()
        {
            Context = new TestDbContext();

            Context.TestEntity_TwoRelations.AddRange(BenchmarkHelper.GenerateTestEntities<TestEntity_TwoRelation>(EntityCount));
            Context.ChangeTracker.DetectChanges();
            TwoRelation = BenchmarkHelper.GenerateOneEntity<TestEntity_TwoRelation>();
        }

        [Benchmark]
        public void Add_WithZeroRelation()
        {
            Context.TestEntity_ZeroRelations.Add(ZeroRelation);
        }

        [Benchmark]
        public void Add_WithOneRelation()
        {
            Context.TestEntity_OneRelations.Add(OneRelation);
        }

        [Benchmark]
        public void Add_WithTwoRelation()
        {
            Context.TestEntity_TwoRelations.Add(TwoRelation);
        }

        public class TestDbContext : DbContext
        {
            public TestDbContext() : base(My.SqlServer)
            {

            }

            public DbSet<TestEntity_ZeroRelation> TestEntity_ZeroRelations { get; set; }
            public DbSet<TestEntity_OneRelation> TestEntity_OneRelations { get; set; }
            public DbSet<TestEntity_TwoRelation> TestEntity_TwoRelations { get; set; }
        }

        public class TestEntity_ZeroRelation
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

        public class TestEntity_OneRelation
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

            public List<RelationOne> FirstRelations { get; set; }
        }

        public class TestEntity_TwoRelation
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

            public List<RelationOne> FirstRelations { get; set; }

            public List<RelationOne> SecondRelations { get; set; }
        }

        public class RelationOne
        {
            public int ID { get; set; }
        }

        public class RelationTwo
        {
            public int ID { get; set; }
        }
    }
}
