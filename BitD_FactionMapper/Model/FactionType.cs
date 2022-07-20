using System;
using System.ComponentModel;
using System.Reflection;

namespace BitD_FactionMapper.Model
{
    public enum FactionType
    {
        [Description("The Fringe")]
        Fringe,
        [Description("Institution")]
        Institution,
        [Description("Labor & Trade")]
        Labor,
        [Description("Underworld")]
        Underworld,
        [Description("Other")]
        Other,
    }

    internal static class Extensions
    {
        public static string ToDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}