using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using iBeautyNail.Datas;
using iBeautyNail.Devices.CardReader;
using iBeautyNail.Enums;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace iBeautyNail.ViewModel
{
    public class Usages
    {
        public string Path { get; set; }
    }
    class U100_UsageViewModel : BaseViewModelBase
    {
        private ObservableCollection<Usages> usageFiles;
        public ObservableCollection<Usages> UsageFiles
        {
            get { return usageFiles; }
            set
            {
                if (usageFiles != value)
                    usageFiles = value;
                RaisePropertyChanged("UsageFiles");
            }
        }

        public U100_UsageViewModel()
        {
            usageFiles = new ObservableCollection<Usages>();
            string[] usages = Directory.GetFiles(SystemPath.Usage);

            foreach (var u in usages)
            {
                Usages usage = new Usages();
                usage.Path = u;
                UsageFiles.Add(usage);
            }

            PrevButtonVisible = false;
            NextButtonVisible = false;
        }

        protected override void PageLoad()
        {
            IsAtTheLeft = false;
            IsAtTheRight = true;
        }

        bool isAtTheLeft = false;
        bool isAtTheRight = true;
        public bool IsAtTheLeft
        {
            get { return isAtTheLeft; }
            set
            {
                isAtTheLeft = value;
                RaisePropertyChanged("IsAtTheLeft");
            }
        }
        public bool IsAtTheRight
        {
            get { return isAtTheRight; }
            set
            {
                isAtTheRight = value;
                RaisePropertyChanged("IsAtTheRight");
            }
        }

        private ScrollBar _horizontalScrollBar;

        private RelayCommand<object> scrollChangedCommand;
        public RelayCommand<object> ScrollChangedCommand
        {
            get
            {
                return scrollChangedCommand ?? new RelayCommand<object>((scrollChanged) =>
                {
                    ScrollViewer scrollViewer = scrollChanged as ScrollViewer;
                    _horizontalScrollBar = scrollViewer.Template.FindName("PART_HorizontalScrollBar", scrollViewer) as ScrollBar;
                    
                    if (_horizontalScrollBar == null) return;

                    if (_horizontalScrollBar.Value == _horizontalScrollBar.Minimum)
                    {
                        IsAtTheLeft = false;
                    }
                    else
                    {
                        IsAtTheLeft = true;
                    }

                    if (_horizontalScrollBar.Value == _horizontalScrollBar.Maximum)
                    {
                        IsAtTheRight = false;
                    }
                    else
                    {
                        IsAtTheRight = true;
                    }
                });
            }
        }
    }
}
