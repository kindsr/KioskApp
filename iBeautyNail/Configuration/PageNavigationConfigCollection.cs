using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    [ConfigurationCollection(typeof(PageNavigationConfigElement), AddItemName = "page", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class PageNavigationConfigCollection : ConfigurationElementCollection, IEnumerable<PageNavigationConfigElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PageNavigationConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((PageNavigationConfigElement)element).Name;
        }

        IEnumerator<PageNavigationConfigElement> IEnumerable<PageNavigationConfigElement>.GetEnumerator()
        {
            foreach (var key in this.BaseGetAllKeys())
            {
                yield return (PageNavigationConfigElement)BaseGet(key);
            }
        }

        public PageNavigationConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as PageNavigationConfigElement;
            }
        }

        public new PageNavigationConfigElement this[string name]
        {
            get
            {
                return base.BaseGet(name) as PageNavigationConfigElement;
            }
        }
    }
}
