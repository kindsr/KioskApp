using iBeautyNail.Configuration;
using iBeautyNail.Enums;
using iBeautyNail.Messages;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace iBeautyNail.Pages
{
    /// <summary>
    /// M060_MessagePopup.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M060_MessagePopup : UserControl
    {
        /// <summary>
        /// 화면을 닫기 위한 타이머
        /// </summary>
        DispatcherTimer CloseTimer;
        private int Popuptime;

        private POPUP_QUANTITY Quntity { get; set; } = POPUP_QUANTITY.BUTTON_ONE;

        /// <summary>
        /// 
        /// </summary>
        public M060_MessagePopup()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// POPUP_STYLE을 파라미터로 갖는 생성자
        /// </summary>
        /// <param name="style"></param>
        public M060_MessagePopup(PopupMessage message)
        {
            InitializeComponent();

            var configInfo = PageNavigationConfigSection.Instance.Page[message.CurrentViewModel.GetType().Name];

            //버튼 수량
            Quntity = message.Quantity;

            // config에서 설정한 값이 있을 때
            if (Quntity == POPUP_QUANTITY.BUTTON_DEFAULT)
            {
                int qty = -1;

                if( int.TryParse(configInfo.Popup.Quantity, out qty) == true)
                {
                    this.Quntity = (POPUP_QUANTITY)qty;
                }
                else
                {
                    this.Quntity = POPUP_QUANTITY.BUTTON_ONE;
                }
            }

            int.TryParse(configInfo.Popup.Popuptime, out Popuptime);

            ChangeStyle();
        }
        /// <summary>
        /// 팝업의 스타일 변경
        /// </summary>
        /// <param name="style">CONFIRM 스타일이 Default</param>
        public void ChangeStyle()
        {
            switch (Quntity)
            {
                case POPUP_QUANTITY.BUTTON_ZERO:
                    SetVisibility(Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed);
                    Timeup();
                    break;
                case POPUP_QUANTITY.BUTTON_ONE:
                    SetAlignment(HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right);
                    SetVisibility(Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed);
                    break;
                case POPUP_QUANTITY.BUTTON_TWO:
                    SetAlignment(HorizontalAlignment.Left, HorizontalAlignment.Right, HorizontalAlignment.Right);
                    SetVisibility(Visibility.Visible, Visibility.Visible, Visibility.Collapsed);
                    //SetWidth(220,220,220);
                    break;
                case POPUP_QUANTITY.BUTTON_THREE:
                    SetAlignment(HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Right);
                    SetVisibility(Visibility.Visible, Visibility.Visible, Visibility.Visible);
                    break;
                default:
                    SetAlignment(HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right);
                    SetVisibility(Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed);
                    break;
            }
        }

        /// <summary>
        /// 버튼의 정렬방식 변경
        /// </summary>
        /// <param name="confirm">확인 버튼</param>
        /// <param name="no">취소 버튼</param>
        private void SetAlignment(HorizontalAlignment button0, HorizontalAlignment button1, HorizontalAlignment button2)
        {
            ctBtButton0.HorizontalAlignment = button0;
            ctBtButton1.HorizontalAlignment = button1;
            ctBtButton2.HorizontalAlignment = button2;
        }

        /// <summary>
        /// 버튼의 visibility 변경
        /// </summary>
        /// <param name="confirm">확인 버튼</param>
        /// <param name="no">취소 버튼</param>
        private void SetVisibility(Visibility button0, Visibility button1, Visibility button2)
        {
            ctBtButton0.Visibility = button0;
            ctBtButton1.Visibility = button1;
            ctBtButton2.Visibility = button2;
        }

        /// <summary>
        /// Notification 팝업의 경우 일정 시간 경과 후에 화면을 닫는다
        /// </summary>
        private void Timeup()
        {
            CloseTimer = new DispatcherTimer();
            CloseTimer.Interval = new TimeSpan(0, 0, Popuptime);
            CloseTimer.Tick += handler;
            CloseTimer.Start();
        }

        /// <summary>
        /// Notification 팝업의 경우 타이머 종료, 팝업 Collapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handler(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            CloseTimer.Stop();
        }
    }
}
