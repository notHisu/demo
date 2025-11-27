using System;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    /// <summary>
    /// Plural format provider.
    /// </summary>
    /// <see cref="https://stackoverflow.com/a/3875477"/>
    /// <example>
    /// Console.WriteLine(String.Format(
    ///     new PluralFormatProvider(),
    ///     "You have {0:life;lives} left, {1:apple;apples} and {2:eye;eyes}.",
    ///     1, 0, 2
    /// );
    /// </example>
    public class PluralFormatProvider : IFormatProvider, ICustomFormatter
    {

        public object GetFormat(Type formatType)
        {
            return this;
        }


        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return arg.ToString();

            string[] forms = format.Split(';');
            int value = (int)arg;
            int form = value == 1 ? 0 : 1;
            return value.ToString() + " " + forms[form];
        }
    }
}