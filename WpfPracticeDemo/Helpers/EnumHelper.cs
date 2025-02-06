using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WpfPracticeDemo.Attributes;

namespace WpfPracticeDemo.Helpers
{
    internal static class EnumHelper
    {
        public static IEnumerable<T> GetEnums<T>()
        {
            var enums = typeof(T).GetFields();

            List<T> result = new List<T>();

            foreach (var item in enums.ToList())
            {
                var isHaveIgnoreAttribute= item.CustomAttributes.Any(x=>x.AttributeType.Equals(typeof(IgnoreAttribute)));
                if (!isHaveIgnoreAttribute)
                {
                    result.Add((T)Enum.Parse(typeof(T), item.Name));
                }                
            }

            return result;
        }

        public static T GetEnum<T>(string description)
        {
            var enums = typeof(T).GetFields();

            foreach (var item in enums.ToList())
            {
                var isHaveIgnoreAttribute = item.CustomAttributes.Any(x => x.AttributeType.Equals(typeof(IgnoreAttribute)));

                if (isHaveIgnoreAttribute)
                {
                    continue;
                }

                var descriptionAttribute = item.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute!=null && descriptionAttribute.Description.Equals(description))
                {
                    return (T)Enum.Parse(typeof(T),item.Name);
                }
            }

            return default;
        }

        public static IEnumerable<string> GetEnumDescriptions<T>()
        {
            var enums = typeof(T).GetFields();
            List<string> result = new List<string>();

            foreach (var item in enums.ToList())
            { 
                var isHaveIgnoreAttribute =item.CustomAttributes.Any(x => x.AttributeType.Equals(typeof(IgnoreAttribute)));
                if (isHaveIgnoreAttribute)
                {
                    continue;
                }
                
                var descriptionAttribute = item.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    result.Add(descriptionAttribute.Description);
                }
            }

            return result;
        }

    }
}
