using System.ComponentModel;
using System.Reflection;

namespace ReleaseNotes_WebAPI.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString<TEnum>(this TEnum @enum)
        {
            // the "@" in front of enum is to make it a valid name, since enum is a reserved keyword in C#
            FieldInfo info = @enum.GetType().GetField(@enum.ToString());
            var attributes = (DescriptionAttribute[])
                info.GetCustomAttributes(typeof(DescriptionAttribute),
                    false);
            return attributes?[0].Description ?? @enum.ToString();
        }
    }
}