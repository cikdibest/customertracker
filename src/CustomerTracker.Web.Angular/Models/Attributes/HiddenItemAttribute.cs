using System;

namespace CustomerTracker.Web.Angular.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HiddenItemAttribute : Attribute
    {
    }
}