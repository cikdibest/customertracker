using System;

namespace CustomerTracker.Web.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HiddenItemAttribute : Attribute
    {
    }
}