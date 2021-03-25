using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Datas;
using iBeautyNail.Enums;
using iBeautyNail.Pages;
using iBeautyNail.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace iBeautyNail.ViewModel
{
    class M211_FullLayoutViewModel : BaseViewModelBase
    {
        Stopwatch _buttonHoldStopWatch;

        private DesignListViewModel _currentPageViewModel;
        public DesignListViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                _currentPageViewModel = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ModelNailSetInfo> myPhotoPathFiles;
        public ObservableCollection<ModelNailSetInfo> MyPhotoPathFiles
        {
            get { return myPhotoPathFiles; }
            set
            {
                if (myPhotoPathFiles != value)
                    myPhotoPathFiles = value;
                RaisePropertyChanged("MyPhotoPathFiles");
            }
        }

        private ObservableCollection<ModelNailSetInfo> myPhotoPathFiles2;
        public ObservableCollection<ModelNailSetInfo> MyPhotoPathFiles2
        {
            get { return myPhotoPathFiles2; }
            set
            {
                if (myPhotoPathFiles2 != value)
                    myPhotoPathFiles2 = value;
                RaisePropertyChanged("MyPhotoPathFiles2");
            }
        }

        public M211_FullLayoutViewModel()
        {
            _buttonHoldStopWatch = new Stopwatch();
        }

        public M211_FullLayoutViewModel(List<DesignInfo> selectedCategoryDesigns)
        {
            _buttonHoldStopWatch = new Stopwatch();

            string line;
            string[] modelNails;
            ModelNailSetInfo mnsi = new ModelNailSetInfo();

            myPhotoPathFiles = new ObservableCollection<ModelNailSetInfo>();
            myPhotoPathFiles2 = new ObservableCollection<ModelNailSetInfo>();

            foreach (DesignInfo di in selectedCategoryDesigns)
            {
                if (Directory.Exists(Path.ChangeExtension(di.DesignPath, null)))
                {
                    line = Path.GetFileNameWithoutExtension(di.DesignPath).Substring(0, 1);

                    switch (line)
                    {
                        case "1":
                            mnsi = new ModelNailSetInfo();
                            mnsi.ModelPath = di.DesignPath;
                            modelNails = Directory.GetFiles(Path.ChangeExtension(di.DesignPath, null));

                            foreach (var m in modelNails)
                            {
                                DesignInfo d = new DesignInfo();

                                d.DesignPath = m;
                                mnsi.ModelNailList.Add(d);
                            }

                            MyPhotoPathFiles.Add(mnsi);
                            break;
                        case "2":
                            mnsi = new ModelNailSetInfo();
                            mnsi.ModelPath = di.DesignPath;
                            modelNails = Directory.GetFiles(Path.ChangeExtension(di.DesignPath, null));

                            foreach (var m in modelNails)
                            {
                                DesignInfo d = new DesignInfo();

                                d.DesignPath = m;
                                mnsi.ModelNailList.Add(d);
                            }

                            MyPhotoPathFiles2.Add(mnsi);
                            break;
                    }
                }
            }
        }

        private Point mousePoint = new Point();
        private double _panelX;
        private double _panelY;
        public double PanelX
        {
            get { return _panelX; }
            set
            {
                if (value.Equals(_panelX)) return;
                _panelX = value;
                RaisePropertyChanged("PanelX");
            }
        }

        public double PanelY
        {
            get { return _panelY; }
            set
            {
                if (value.Equals(_panelY)) return;
                _panelY = value;
                RaisePropertyChanged("PanelY");
            }
        }

        private RelayCommand<DesignInfo> selectNailDesignCommand;
        public RelayCommand<DesignInfo> SelectNailDesignCommand
        {
            get
            {
                return new RelayCommand<DesignInfo>((designInfo) =>
                {
                    Console.Write("Selected Nail Path=>{0}\n", designInfo.DesignPath);
                    Messenger.Default.Send<DesignInfo>(designInfo);
                });
            }
        }

        private RelayCommand<DesignInfo> selectNailDesignDownCommand;
        public RelayCommand<DesignInfo> SelectNailDesignDownCommand
        {
            get
            {
                return new RelayCommand<DesignInfo>((designInfo) =>
                {
                    //_buttonHoldStopWatch.Start();
                    Console.WriteLine("MouseDown {0},{1}", PanelX, PanelY);
                    mousePoint.X = PanelX;
                    mousePoint.Y = PanelY;
                });
            }
        }

        private RelayCommand<DesignInfo> selectNailDesignUpCommand;
        public RelayCommand<DesignInfo> SelectNailDesignUpCommand
        {
            get
            {
                return new RelayCommand<DesignInfo>((designInfo) =>
                {
                    Messenger.Default.Send<string>("ResetTimeout");

                    Console.WriteLine("MouseUp {0},{1}", PanelX, PanelY);

                    if (PanelX == mousePoint.X && PanelY == mousePoint.Y)
                    {
                        Console.Write("Selected Nail Path=>{0}\n", designInfo.DesignPath);
                        Messenger.Default.Send<DesignInfo>(designInfo);
                    }
                });
            }
        }

        private RelayCommand<ModelNailSetInfo> selectModelNailDesignCommand;
        public RelayCommand<ModelNailSetInfo> SelectModelNailDesignCommand
        {
            get
            {
                return new RelayCommand<ModelNailSetInfo>((modelNailSetInfo) =>
                {
                    Messenger.Default.Send<string>("ResetTimeout");

                    for (int i = 0; i < modelNailSetInfo.ModelNailList.Count; i++)
                    {
                        if (M200_SelectDesignViewModel.curDesignCount >= 5)
                        {
                            Console.Write("Selected Nail Path=>{0}\n", modelNailSetInfo.ModelNailList[modelNailSetInfo.ModelNailList.Count - 1 - i].DesignPath);
                            Messenger.Default.Send<DesignInfo>(modelNailSetInfo.ModelNailList[modelNailSetInfo.ModelNailList.Count - 1 - i]);
                        }
                        else
                        {
                            Console.Write("Selected Nail Path=>{0}\n", modelNailSetInfo.ModelNailList[i].DesignPath);
                            Messenger.Default.Send<DesignInfo>(modelNailSetInfo.ModelNailList[i]);
                        }
                    }
                });
            }
        }
    }
}