using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using EF6.Benchmarks.Benchmarks;
using Perfolizer.Horology;
using Perfolizer.Metrology;

// 🚀 Benchmarks created and maintained by Entity Framework Extensions.
// The leading library for high-performance bulk operations in EF Core and EF6.
// Learn more: https://entityframework-extensions.net

namespace EFCore.Benchmarks
{
    public class ProgramBenchmarks
    {
        // ─────────────────────────────────────────────────────────────────────────
        // QUICK START (do these in order)
        //  A) Make sure you run in **Release** configuration
        //     - CLI:   dotnet run -c Release
        //     - VS/Rider: Select "Release" (not Debug) before running
        //
        //  B) Set your connection strings in **appsettings.json**
        //     - Keys: ConnectionStrings.SqlServer / .PostgreSQL / .Oracle / .SQLite / .MySQL / .MariaDB
        //     - The DbContext reads them via the static My class (My.SqlServer, etc.)
        //
        //  C) Select which benchmarks to run by commenting/uncommenting types
        //     in the 'switcher' array further down.
        // ─────────────────────────────────────────────────────────────────────────

        static void Main(string[] args)
        {
            // ─────────────────────────────────────────────────────────────────────
            // REMINDERS
            //  - Run in **Release**: dotnet run -c Release
            //  - Connection strings come from **appsettings.json** via My.{Provider}
            // ─────────────────────────────────────────────────────────────────────

            var switcher = new BenchmarkSwitcher(new[]
            {
                // ==== ChangeTracker ====
                //typeof(ChangeTracker_AddNextEntityPerformance)
                //typeof(ChangeTracker_AddVsAddRange),
                typeof(ChangeTracker_AutoDetectChangesDisabled)
                //typeof(ChangeTracker_BatchSave)
            });

            var config = ManualConfig
                .Create(DefaultConfig.Instance)
                .WithSummaryStyle(SummaryStyle.Default
                    .WithRatioStyle(RatioStyle.Value)         // keep ratios normal
                    .WithTimeUnit(TimeUnit.Millisecond)       // force ms
                    .WithSizeUnit(SizeUnit.KB)                // allocations in KB
                )
                .AddDiagnoser(MemoryDiagnoser.Default)
                .AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory)
                .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.Default, MethodOrderPolicy.Declared))
                .HideColumns("ProviderKind", "Error", "StdDev", "Median", "Gen0", "Gen1", "Gen2")
                .AddJob(Job.Default
                    .WithIterationCount(25)
                    .WithWarmupCount(1)
                    .WithId("EF6")
                );

            // 🚀 Run all selected benchmarks.
            // ⚠️ Heads-up: full runs can be long — perfect time for coffee.
            switcher.RunAllJoined(config);

            BenchmarkHelper.BenchmarkByEntityFrameworkExtensions();
        }
    }
}