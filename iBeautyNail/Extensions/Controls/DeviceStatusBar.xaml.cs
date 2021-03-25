// 2020.07.08 13:00:00
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace iBeautyNail.Extensions.Controls
{
    /// <summary>
    /// ClassName 설명
    /// </summary>
    public partial class DeviceStatusBar : UserControl
    {
         /// <summary>
        /// 생성자
        /// </summary>
        public DeviceStatusBar()
        {
            InitializeComponent();
        }

        public static DependencyProperty DeviceNameProperty
             = DependencyProperty.Register("DeviceName", typeof(string), typeof(DeviceStatusBar), new FrameworkPropertyMetadata(OnDeviceNamePropertyChanged));

        [Description("DeviceName"), Category("Common Properties")]
        public string DeviceName
        {
            get { return (string)this.GetValue(DeviceNameProperty); }
            set { this.SetValue(DeviceNameProperty, value); }
        }

        public static DependencyProperty StatusProperty
            = DependencyProperty.Register("Status", typeof(bool), typeof(DeviceStatusBar), new FrameworkPropertyMetadata(true, OnStatusPropertyChanged));

        public bool Status
        {
            get { return (bool)this.GetValue(StatusProperty); }
            set { this.SetValue(StatusProperty, value); }
        }

        private static void OnDeviceNamePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DeviceStatusBar deviceStatusBar = obj as DeviceStatusBar;
            deviceStatusBar.deviceName.Text = args.NewValue.ToString();
        }

        private static void OnStatusPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DeviceStatusBar deviceStatusBar = obj as DeviceStatusBar;
            if ((bool)args.NewValue)
            {
                VisualStateManager.GoToState(deviceStatusBar, "Normal", true);
            }
            else
            {
                VisualStateManager.GoToState(deviceStatusBar, "Abnormal", true);
            }
        }
    }
}