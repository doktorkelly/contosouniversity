using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContosoUniversity.NUTest.UT.Helpers
{
    public class NameValueProviderFactory
    {
        public static NameValueCollectionValueProvider Create()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("name", "value");
            NameValueCollectionValueProvider valueProvider = 
                new NameValueCollectionValueProvider(collection, CultureInfo.CurrentCulture);
            return valueProvider;
        }
    }
}
