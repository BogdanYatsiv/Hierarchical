using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hierarchical
{
    public class ClassHierarchyBuilder
    {
        private Type classType;
        private Assembly assembly;

        public ClassHierarchyBuilder(Type item)
        {
            classType = item;
        }

        public void AddAssembly(Assembly item)
        {
            assembly = item;
        }

        public HierarchicalItem<Type> Build()
        {
            HierarchicalItem<Type> result = new(classType);

            var typeClasses = from item in assembly.GetTypes()
                             where item.IsSubclassOf(classType)
                             select item;

            //find highest classes in hierarchical
            var parents = from parent in typeClasses
                          where parent.BaseType.BaseType.Name == "Object"
                          select parent;

            foreach(var item in parents)
            {
                HierarchicalItem<Type> subTree = new(item);

                //find all childrens from this parent
                var childrens = from child in typeClasses
                                where child.BaseType == item
                                select child;

                foreach(var child in childrens)
                {
                    var harited = from temp in childrens
                                  where temp.IsSubclassOf(child) == true
                                  select temp;

                    //if child has own childrens
                    if(harited.Count() > 0)
                    {
                        HierarchicalItem<Type> treeChild = new(child);

                        foreach(var i in harited)
                        {
                            treeChild.Add(i);
                        }

                        subTree.Add(treeChild);

                    }
                    else subTree.Add(child);
                }

                result.Add(subTree);
            }

            return result;
        }
    }

    public class HierarchicalItem<T>
    {
        private List<T> value = new List<T>();
        private List<HierarchicalItem<T>> children = new List<HierarchicalItem<T>>();

        public HierarchicalItem()
        {
            
        }

        public HierarchicalItem(T item)
        {
            value.Add(item);
        }

        public void Add(T item)
        {
            value.Add(item);
        }

        public void Add(HierarchicalItem<T> item)
        {
            children.Add(item);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("");

            for(int i = 0; i < value.Count; i++)
            {
                if (i == 0)
                {
                    result.AppendLine(value[i].ToString());
                    continue;
                }
                result.AppendLine(" " + value[i].ToString()); 
            }
            
            for(int i = 0; i < children.Count; i++)
            {
                //add space to each line of text represent of children
                string text = children[i].ToString();
                foreach(var line in text.Split("\n"))
                {
                    result.AppendLine(" " + line);
                }
            }

            return result.ToString(); 
        }
    }
}
