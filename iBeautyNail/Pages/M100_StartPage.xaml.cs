using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace iBeautyNail.Pages
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M100_StartPage : UserControl
    {
        #region Field
        /// <summary>
        /// 비트맵 이미지 리스트
        /// </summary>
        private List<BitmapImage> bitmapImageList = new List<BitmapImage>();

        /// <summary>
        /// 디스패처 타이머
        /// </summary>
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        /// <summary>
        /// 현재 이미지 스토리보드 시작 여부
        /// </summary>
        private bool startCurrentImageStoryboard = false;

        /// <summary>
        /// 현재 인덱스
        /// </summary>
        private int currentIndex = 0;
        #endregion

        public M100_StartPage()
        {
            InitializeComponent();

            //meVideo.Volume = 0;
            //meVideo.Play();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.dispatcherTimer.Interval = TimeSpan.FromSeconds(10);

            this.dispatcherTimer.Tick += dispatcherTimer_Tick;

            this.dispatcherTimer.Start();

            SetBitmapImageList();


            txtScanText.Focus();
        }

        //private void Media_Ended(object sender, EventArgs e)
        //{
        //    meVideo.Position = TimeSpan.Zero;
        //    meVideo.Volume = 0;
        //    meVideo.Play();
        //}

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //meVideo.Stop();
            if (dispatcherTimer.IsEnabled)
                this.dispatcherTimer.Stop();
        }

        /// <summary>
        /// 디스패처 타이머 틱 발생시 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.currentIndex++;

            if (this.currentIndex > this.bitmapImageList.Count - 1)
            {
                this.currentIndex = 0;
            }

            if (this.startCurrentImageStoryboard)
            {
                this.currentImage.Source = this.bitmapImageList[this.currentIndex];

                (Resources["CurrentImageStoryboardKey"] as Storyboard).Begin(this);
            }
            else
            {
                this.nextImage.Source = this.bitmapImageList[this.currentIndex];

                (Resources["NextImageStoryboardKey"] as Storyboard).Begin(this);
            }

            this.startCurrentImageStoryboard = !this.startCurrentImageStoryboard;
        }

        /// <summary>
        /// 리소스 URI 구하기
        /// </summary>
        /// <param name="assemblyName">어셈블리명</param>
        /// <param name="resourcePath">리소스 경로</param>
        /// <returns>리소스 URI</returns>
        private Uri GetResourceURI(string assemblyName, string resourcePath)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                return new Uri(string.Format("pack://siteoforigin:,,,/Resources/Images/{0}", resourcePath));
            }
            else
            {
                return new Uri(string.Format("pack://application:,,,/{0};component/{1}", assemblyName, resourcePath));
            }
        }

        /// <summary>
        /// 비트맵 이미지 리스트 설정하기
        /// </summary>
        private void SetBitmapImageList()
        {
            this.bitmapImageList.Clear();

            string[] resourcePathArray = new string[]
            {
                "pod01.jpg",
                "pod02.jpg",
                "pod03.jpg",
                "pod04.jpg",
                "pod05.jpg",
                "pod06.jpg",
                "pod07.jpg",
                "pod08.jpg",
                "pod09.jpg",
                "pod10.jpg",
                "pod11.jpg",
                "pod12.jpg"
            };

            foreach (string resourcePath in resourcePathArray)
            {
                Uri uri = GetResourceURI(null, resourcePath);

                BitmapImage bitmapImage = new BitmapImage(uri);

                this.bitmapImageList.Add(bitmapImage);
            }

            this.currentImage.Source = this.bitmapImageList[this.currentIndex];
        }
    }
}
