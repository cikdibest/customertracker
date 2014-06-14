using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CustomerTracker.Common.Helpers
{
    public class EnumHelper
    { 
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        
        public static Dictionary<int,string> ToDictionary<T>()
        {
            var dict = Enum.GetValues(typeof(T))
               .Cast<Enum>()
               .ToDictionary(t => t.GetHashCode(), t => t.GetDescription());

            return dict;
        }

        public static T GetAttributeOfType<T>(Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

       

        
    }
}