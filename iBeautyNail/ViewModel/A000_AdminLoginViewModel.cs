using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Configuration;
using iBeautyNail.Enums;
using iBeautyNail.Extensions.Converters;
using iBeautyNail.Language;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace iBeautyNail.ViewModel
{
    public class A000_AdminLoginViewModel : BaseViewModelBase
    {
        private StringBuilder Input { get; set; }

        private string inputValue;
        public string InputValue
        {
            get { return inputValue; }
            set { Set(() => InputValue, ref inputValue, value); }
        }

        #region Define Variables Number 0~9
        private string number0 = "0";
        public string Number0
        {
            get { return number0; }
            set { Set(() => Number0, ref number0, value); }
        }
        private string number1 = "1";
        public string Number1
        {
            get { return number1; }
            set { Set(() => Number1, ref number1, value); }
        }
        private string number2 = "2";
        public string Number2
        {
            get { return number2; }
            set { Set(() => Number2, ref number2, value); }
        }
        private string number3 = "3";
        public string Number3
        {
            get { return number3; }
            set { Set(() => Number3, ref number3, value); }
        }
        private string number4 = "4";
        public string Number4
        {
            get { return number4; }
            set { Set(() => Number4, ref number4, value); }
        }
        private string number5 = "5";
        public string Number5
        {
            get { return number5; }
            set { Set(() => Number5, ref number5, value); }
        }
        private string number6 = "6";
        public string Number6
        {
            get { return number6; }
            set { Set(() => Number6, ref number6, value); }
        }
        private string number7 = "7";
        public string Number7
        {
            get { return number7; }
            set { Set(() => Number7, ref number7, value); }
        }
        private string number8 = "8";
        public string Number8
        {
            get { return number8; }
            set { Set(() => Number8, ref number8, value); }
        }
        private string number9 = "9";
        public string Number9
        {
            get { return number9; }
            set { Set(() => Number9, ref number9, value); }
        }
        #endregion

        System.Windows.Threading.DispatcherTimer timer;
        public A000_AdminLoginViewModel()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(0);
            timer.Tick += (object sender, EventArgs e) =>
            {
                {
                    StopTimer();
                    BaseViewModelBase.CanNavigate = true;
                }
            };
        }

        protected override void PageLoad()
        {
            this.Input = new StringBuilder();
            this.InputValue = string.Empty;

            this.StirKeypad();
            StartTimer();
        }

        protected override void PageUnload()
        {
            StopTimer();
        }

        private void StartTimer()
        {
            timer.Start();
        }

        private void StopTimer()
        {
            if (timer.IsEnabled) timer.Stop();
        }

        private RelayCommand<string> clickCommand;

        public RelayCommand<string> ClickCommand
        {
            get
            {
                return clickCommand ?? new RelayCommand<string>((btn) =>
                {
                    switch (Convert.ToInt32(btn))
                    {
                        case 10:    // delete
                            if (InputValue.Equals("INCORRECT"))
                            {
                                InputValue = string.Empty;
                                this.Input.Clear();
                            }

                            if (this.Input.Length > 0)
                            {
                                this.Input.Remove(this.Input.Length - 1, 1);
                                InputValue = this.ToAsterisk(this.Input);
                            }
                            break;

                        case 11:    // submit
                            if (this.Input.Length > 0)
                            {
                                if (this.VerifyPassword(this.Input.ToString()))
                                {
                                    CommandAction(NAVIGATION_TYPE.Next);
                                }
                                else
                                {
                                    InputValue = "INCORRECT";
                                    this.StirKeypad();
                                }
                            }
                            break;
                        case 12:    // close
                            CommandAction(NAVIGATION_TYPE.Previous);
                            break;
                        default:
                            if (InputValue.Equals("INCORRECT"))
                            {
                                InputValue = string.Empty;
                                this.Input.Clear();
                            }

                            if (this.Input.ToString().Length < 4)
                            {
                                this.Input.Append(btn);
                                InputValue = this.ToAsterisk(this.Input);
                            }
                            break;
                    }
                });
            }
        }

        private string ToAsterisk(StringBuilder sb)
        {
            try
            {
                StringBuilder asb = new StringBuilder();

                for (int i = 0; i < sb.ToString().Length; i++)
                {
                    asb.Append("*");
                }
                return asb.ToString();
            }
            catch (Exception exp)
            {
                CommonException();
                return string.Empty;
            }
        }

        private bool VerifyPassword(string passwordInBarcode)
        {
            try
            {
                string passwordInFile = null;
                using (StreamReader sr = new StreamReader(Path.Combine(SystemPath.Data, "admin.auth")))
                {
                    passwordInFile = sr.ReadLine().Replace("\n", "");
                }

                if (DataConverter.String2md5(passwordInBarcode).Equals(passwordInFile))
                {
                    logger.InfoFormat("Administrator Password is verified: {0}", passwordInBarcode);
                    return true;
                }
                else
                {
                    logger.ErrorFormat("Failed to verify Administrator Password: [{0}], [{1}], [{2}]", passwordInBarcode, DataConverter.String2md5(passwordInBarcode), passwordInFile);
                    return false;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }
        }

        private void StirKeypad()
        {
            try
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                int[] p = Enumerable.Range(0, 10).ToArray();

                int index, old;
                for (int k = 0; k < 9; k++)
                {
                    index = rand.Next(9);
                    old = p[k];
                    p[k] = p[index];
                    p[index] = old;
                }

                int i = 0;
                this.Number0 = p[i++].ToString();
                this.Number1 = p[i++].ToString();
                this.Number2 = p[i++].ToString();
                this.Number3 = p[i++].ToString();
                this.Number4 = p[i++].ToString();
                this.Number5 = p[i++].ToString();
                this.Number6 = p[i++].ToString();
                this.Number7 = p[i++].ToString();
                this.Number8 = p[i++].ToString();
                this.Number9 = p[i++].ToString();
            }
            catch (Exception exp)
            {
                CommonException();
            }
        }
    }
}
