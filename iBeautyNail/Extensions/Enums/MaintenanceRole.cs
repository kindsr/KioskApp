// MaintenanceRole.cs 2020.07.08 13:00:00
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
using System.Text;

namespace iBeautyNail.Extensions.Enums
{
    /// <summary>
    /// Maintenance에 진입시 사용되는 권한 유형
    /// </summary>
    public enum MaintenanceRole : int
    {
        [Description("없음")]
        None = 0x00,

        [Description("전체")]
        All = View | Execute,

        [Description("보기")]
        View = 0x01,

        [Description("실행")]
        Execute = 0x02,
    }
}
