using System.Windows;
using iBeautyNail.ViewModel;
using System;
using System.Windows.Interop;
using System.IO;
using System.Diagnostics;

namespace iBeautyNail
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IntPtr HWND { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            Closing += (s, e) => ViewModelLocator.Cleanup();

            // AutoUpdater Kill
            string progName = Path.GetFileNameWithoutExtension("iBeautyNailAutoUpdater.exe");
            Process[] processList = Process.GetProcessesByName(progName);

            if (processList.Length >= 1)
            {
                for (int i = 0; i < processList.Length; i++)
                    processList[i].Kill();
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HWND = new WindowInteropHelper(this).Handle;

            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg.Contains("/Topmost"))
                {
                    this.Topmost = true;
                }
            }

            Initialize();
        }

        private void Initialize()
        {
            this.DataContext = new MainViewModel(layerArea);
        }
    }
}