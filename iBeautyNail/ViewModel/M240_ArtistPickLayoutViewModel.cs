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
    class M240_ArtistPickLayoutViewModel : BaseViewModelBase
    {
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
        public M240_ArtistPickLayoutViewModel()
        {
        }

        public M240_ArtistPickLayoutViewModel(List<DesignInfo> selectedCategoryDesigns)
        {
            string line;
            var designCompleteList = new List<DesignListPart>();
            List<string> myPhoto = new List<string>();

            myPhotoPathFiles = new ObservableCollection<ModelNailSetInfo>();
            foreach (DesignInfo di in selectedCategoryDesigns)
            {
                try
                {
                    line = Path.GetFileNameWithoutExtension(di.DesignPath).Split('_')[1].Substring(0, 1);
                }
                catch (Exception ex)
                {
                    line = null;
                    myPhoto.Add(di.DesignPath);
                    
                }

                if (String.IsNullOrEmpty(line) == false)
                {
                    SeparateLineofDesigns(line, di);
                }
            }

            foreach (var dic in designDictionary)
            {
                if (Int32.Parse(dic.Key) % 2 == 0)
                    designCompleteList.Add(new DesignListPart { Line = dic.Key, LineAlign = HorizontalAlignment.Right, Designs = dic.Value.ToObservableCollection<DesignInfo>() });
                else
                    designCompleteList.Add(new DesignListPart { Line = dic.Key, LineAlign = HorizontalAlignment.Left, Designs = dic.Value.ToObservableCollection<DesignInfo>() });
            }

            CurrentPageViewModel = new DesignListViewModel(designCompleteList);

            ModelNailSetInfo mnsi;
            foreach (var a in myPhoto)
            {
                if (Directory.Exists(Path.ChangeExtension(a, null)))
                {
                    mnsi = new ModelNailSetInfo();
                    mnsi.ModelPath = a;

                    var modelNails = Directory.GetFiles(Path.ChangeExtension(a, null));

                    foreach (var m in modelNails)
                    {
                        FileInfo fi = new FileInfo(m);

                        if (fi.Name.Contains("03") || fi.Name.Contains("05") || fi.Name.Contains("07") || fi.Name.Contains("09") || fi.Name.Contains("11"))
                        {
                            DesignInfo di = new DesignInfo();
                            di.DesignPath = m;
                            mnsi.ModelNailList.Add(di);
                        }
                        if (fi.Name.Contains("12") || fi.Name.Contains("14") || fi.Name.Contains("16") || fi.Name.Contains("18") || fi.Name.Contains("20"))
                        {
                            DesignInfo di = new DesignInfo();
                            di.DesignPath = m;
                            mnsi.ModelNailList2.Add(di);
                        }
                    }

                    MyPhotoPathFiles.Add(mnsi);
                }
            }
        }

        private Dictionary<string, List<DesignInfo>> designDictionary = new Dictionary<string, List<DesignInfo>>();
        public void SeparateLineofDesigns(string key, DesignInfo value)
        {
            if (this.designDictionary.ContainsKey(key))
            {
                List<DesignInfo> list = this.designDictionary[key];
                if (list.Contains(value) == false)
                {
                    list.Add(value);
                }
            }
            else
            {
                List<DesignInfo> list = new List<DesignInfo>();
                list.Add(value);
                this.designDictionary.Add(key, list);
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

                    // MyDesigns에 있는 디자인 전체 삭제
                    Messenger.Default.Send<int>(0);

                    for (int i = 0; i < modelNailSetInfo.ModelNailList.Count; i++)
                    {
                        {
                            Console.Write("Selected Nail Path=>{0}\n", modelNailSetInfo.ModelNailList[i].DesignPath);
                            Messenger.Default.Send<DesignInfo>(modelNailSetInfo.ModelNailList[i]);
                        }
                    }
                    for (int i = 0; i < modelNailSetInfo.ModelNailList2.Count; i++)
                    {
                        {
                            Console.Write("Selected Nail Path=>{0}\n", modelNailSetInfo.ModelNailList2[i].DesignPath);
                            Messenger.Default.Send<DesignInfo>(modelNailSetInfo.ModelNailList2[i]);
                        }
                    }
                });
            }
        }
    }
}