using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CustomerTracker.Web.Utilities.Helpers
{
    public class EnumHelper
    { 
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        
        public static IList<T> GetValues<T>()
        {
            IList<T> list = new List<T>();
            foreach (var value in Enum.GetValues(typeof(T)))
                list.Add((T)value);
            return list;
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

        public static string GetDisplayDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DisplayAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute))
                        as DisplayAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        
    }
}