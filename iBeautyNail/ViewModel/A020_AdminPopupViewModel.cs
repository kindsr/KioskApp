using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Configuration;
using iBeautyNail.Enums;
using iBeautyNail.Extensions;
using iBeautyNail.Extensions.Converters;
using iBeautyNail.Http;
using iBeautyNail.Language;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;

namespace iBeautyNail.ViewModel
{
    public class A020_AdminPopupViewModel : BaseViewModelBase
    {
        public A020_AdminPopupViewModel()
        {

        }

        /// <summary>
        /// 클릭에 대한 액션을 처리하는 메소드
        /// </summary>
        /// <param name="action"></param>
        protected override void ClickAction(string action)
        {
            if (action.ToLower() == CLICK_COMMAND.RESTART.ToString().ToLower())
            {
                Task.Run(() => UpdateMachineStatus(Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID), "OFF"));
                SystemCommand.ShutDown("-s -f");
            }
            else if (action.ToLower() == CLICK_COMMAND.SHUTDOWN.ToString().ToLower())
            {
                Task.Run(() => UpdateMachineStatus(Int32.Parse(ApplicationConfigurationSection.Instance.Machine.ID), "OFF"));
                Application.Current.Shutdown();
            }
        }

        // UpdateMachineStatus
        private async Task UpdateMachineStatus(int machineId, string status)
        {
            NailApi Api = NailApi.GetDevelopmentInstance("");
            await Api.MonitoringInfo.UpdateMachineStatusAsync(machineId, status);
        }
    }
}
