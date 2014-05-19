using System;
using System.Linq;

namespace CustomerTracker.Web.Utilities.Helpers
{
    public static class EnumExtensions
    { 
        static EnumExtensions()
        {
            
        }
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            return EnumHelper.GetAttributeOfType<T>(enumVal);
        }

        public static string GetDescription(this Enum value)
        {
            return EnumHelper.GetDescription(value);
        }

        public static string GetDisplayDescription(this Enum value)
        {
            return EnumHelper.GetDisplayDescription(value);
        }
    }
}