using System;
using System.Reflection;

namespace Hierarchical
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassHierarchyBuilder builder = new ClassHierarchyBuilder(typeof(Exception));

            Assembly assembly = typeof(Exception).Assembly;
            builder.AddAssembly(assembly);

            HierarchicalItem<Type> tree = builder.Build();

            Console.WriteLine(tree.ToString());
        }
    }
}
