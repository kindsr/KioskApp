// UrlConfigCollection.cs 2020.07.08 13:00:00
// Copyright (c) 2020 iRobo Tech Corporation.
// A-808, 809 Tera Tower, Songpadaero 167, Songpa-gu, Seoul 05855 Korea
// All rights reserved.
//
// This software is the confidential and proprietary information of 
// iRobo Tech Corporation. ("Confidential Information").  You shall not
// disclose such Confidential Information and shall use it only in
// accordance with the terms of the license agreement you entered into
// with iRobo Tech Corporation.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Configuration
{
    /// <summary>
    /// UrlConfig configuration
    /// </summary>
    [ConfigurationCollection(typeof(UrlConfigElement), AddItemName = "url", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class UrlConfigCollection : ConfigurationElementCollection, IEnumerable<UrlConfigElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((UrlConfigElement)element).Name;
        }

        IEnumerator<UrlConfigElement> IEnumerable<UrlConfigElement>.GetEnumerator()
        {
            foreach (var key in this.BaseGetAllKeys())
            {
                yield return (UrlConfigElement)BaseGet(key);
            }
        }

        public UrlConfigElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as UrlConfigElement;
            }
        }

        public new UrlConfigElement this[string name]
        {
            get
            {
                return base.BaseGet(name) as UrlConfigElement;
            }
        }
    }
}
