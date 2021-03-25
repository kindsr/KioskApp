// UrlConfigElement.cs 2020.07.08 13:00:00
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
    /// UrlConfigElement configuration
    /// </summary>
    public class UrlConfigElement : ConfigurationElement
    {
        /// <summary>
        /// 명
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// 설명
        /// </summary>
        [ConfigurationProperty("desc", IsKey = false, IsRequired = true)]
        public string Description
        {
            get { return (string)base["desc"]; }
            set { base["desc"] = value; }
        }

        /// <summary>
        /// 요청 URL
        /// </summary>
        [ConfigurationProperty("url", IsKey = false, IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }

        /// <summary>
        /// 타임아웃, 기본 10000ms
        /// </summary>
        [ConfigurationProperty("timeout", IsRequired = false, DefaultValue = 10000)]
        public int Timeout
        {
            get { return Convert.ToInt32(base["timeout"]); }
            set { base["timeout"] = value; }
        }
    }
}
