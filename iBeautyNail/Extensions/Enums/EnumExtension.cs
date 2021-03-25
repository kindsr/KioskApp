// EnumExtension.cs 2020.07.08 13:00:00
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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Extensions.Enums
{
    /// <summary>
    /// enum의 공통 기능을 제공
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// enum의 Description명을 반환
        /// </summary>
        /// <param name="en">enum 타입</param>
        /// <returns>Description명</returns>
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return en.ToString();
        }

        /// <summary>
        /// enum의 Description명을 반환
        /// </summary>
        /// <param name="en">enum 타입</param>
        /// <returns>Description명</returns>
        public static T DescriptionOf<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}
