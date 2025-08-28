using System.Collections.Generic;
using System.ComponentModel;

namespace Dataverse.ConfigurationMigrationTool.XrmToolBox.Common
{
    public static class BindingListExtensions
    {
        public static void AddRange<T>(this BindingList<T> list, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                list.Add(value);
            }
        }
        public static void RemoveRange<T>(this BindingList<T> list, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                list.Remove(value);
            }
        }
    }
}
