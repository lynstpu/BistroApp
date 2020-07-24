using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BistroApp.Extensions
{
    public static class Extension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }
    }
}
