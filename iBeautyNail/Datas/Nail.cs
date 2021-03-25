using iBeautyNail.Enums;
using iBeautyNail.ViewModel;
using System.Windows.Controls;

namespace iBeautyNail.Datas
{
    public class Nail : BaseViewModelBase
    {
        public DesignInfo DesignInfo { get; set; }
        public bool UsbImageYN { get; set; }    // USB 이미지인지 여부
        public Image UsbImage { get; set; }     // 사용자가 선택한 USB이미지
        public int Row { get; set; }
        public int Col { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public FINGER_POSITION Position { get; set; }
        //public bool ImageExist { get; set; }
        public bool Vacant { get; set; }       // 사용자가 선택 후 삭제했는지 여부
        public string MaskInfo { get; set; }
        public bool IsRotated { get; set; }

        private double _rotAngle;
        public double RotAngle
        {
            get { return _rotAngle; }
            set
            {
                _rotAngle = value;
                RaisePropertyChanged();
            }
        }
    }
}
