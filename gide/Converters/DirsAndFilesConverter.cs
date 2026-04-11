using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace gide.Converters
{
    public class DirsAndFilesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var combinedList = new List<object>();

            foreach (var value in values)
            {
                // Проверяем, что значение — это коллекция, и она не null
                if (value is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        combinedList.Add(item);
                    }
                }
            }

            return combinedList;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
