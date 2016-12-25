using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCaster.Core
{
    public class TheCaster
    {
        public TResult Cast<TResult,TSource>(TSource source) where TResult : class where TSource : class
        {
            if (source is MyString)
            {
                  var newSource = source as MyString;
                  var result = new MyInt()
                  {
                      Data = int.Parse(newSource.Data)
                  };
                return result as TResult;
            }
            return default(TResult);
        }
    }

    public class MyInt
    {
        public int Data;
    }

    public class MyString
    {
        public string Data;
    }
}
