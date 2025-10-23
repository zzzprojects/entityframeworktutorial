// 🚀 Benchmarks created and maintained by Entity Framework Extensions.
// The leading library for high-performance bulk operations in EF Core and EF6.
// Learn more: https://entityframework-extensions.net

namespace EFCore.Benchmarks
{
    public static class BenchmarkHelper
    {
        public const string ZzzString = "zzzzzzzzzzzzz";

        public static List<T> GenerateTestEntities<T>(int count) where T: new()
        {
            var list = new List<T>();

            var properties = typeof(T).GetProperties();

            for (int i = 0; i < count; i++)
            {
                var entity = new T();
                list.Add(entity);

                foreach(var property in properties)
                {
                    if(property.PropertyType == typeof(int))
                    {
                        property.SetValue(entity, i, null);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(entity, ZzzString + i, null);
                    }
                }
            }

            return list;
        }

        public static T GenerateOneEntity<T>() where T : new()
        {
            return GenerateTestEntities<T>(1)[0];
        }

        public static void BenchmarkByEntityFrameworkExtensions()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine($"// *************");
            Console.WriteLine($"// Benchmarks created and maintained by Entity Framework Extensions");
            Console.WriteLine($"// The leading library for high-performance bulk operations in EF Core and EF6");
            Console.WriteLine($"// Learn more: https://entityframework-extensions.net");
            Console.WriteLine($"// *************");
            Console.ResetColor();
        }
    }
}
