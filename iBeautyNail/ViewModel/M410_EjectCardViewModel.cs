using iBeautyNail.Datas;
using iBeautyNail.Devices.CardReader;
using iBeautyNail.Enums;
using iBeautyNail.Messages;
using iBeautyNail.Messages.Exceptions.Enums;
using iBeautyNail.SDK;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;

namespace iBeautyNail.ViewModel
{
    class M410_EjectCardViewModel : BaseViewModelBase, INotifyPropertyChanged
    {
        public M410_EjectCardViewModel()
        {
            HomeButtonVisible = false;
            PrevButtonVisible = false;
            NextButtonVisible = false;
        }

        protected override void PageLoad()
        {
            // Language 팝업은 Load안함
            if (GlobalVariables.Instance.LanguagePopup)
            {
                GlobalVariables.Instance.LanguagePopup = false;
                return;
            }

            if (GlobalVariables.Instance.IsTTSOn)
            {
                threadDelegate = new ThreadStart(CommentWork);
                commentThread = new Thread(threadDelegate);
                commentThread.Start();
            }

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5000);
            timer.Tick += (object sender, EventArgs e) =>
            {
                int state = SDKManager.CardPayment.State();
                if (state == CardPaymentWParamType.CardEmpty)
                {
                    timer.Stop();
                    CommandAction(NAVIGATION_TYPE.Next);
                    logger.DebugFormat("{0} :: Card State :: CardEmpty", CurrentViewModelName);
                }
                else if (state == CardPaymentWParamType.CardInserted)
                {
                    if (GlobalVariables.Instance.IsTTSOn)
                    {
                        if (!commentThread.IsAlive)
                        {
                            threadDelegate = new ThreadStart(CommentWork);
                            commentThread = new Thread(threadDelegate);
                            commentThread.Start();
                        }
                    }
                    logger.DebugFormat("{0} :: Card State :: Card is in the reader", CurrentViewModelName);
                }
                else if (state == CardPaymentWParamType.Failure)
                {
                    logger.DebugFormat("{0} :: Card State :: Card Failure", CurrentViewModelName);
                }
            };
            timer.Start();
        }

        protected override void PageUnload()
        {
            if (GlobalVariables.Instance.IsTTSOn)
            {
                synthesizer.SpeakAsyncCancelAll();
                //synthesizer.Dispose();
                commentThread.Abort();
            }
        }

        protected override void CommentWork()
        {
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Rate = 2;
                synthesizer.SetOutputToDefaultAudioDevice();

                var builder = new PromptBuilder();
                builder.StartVoice(new CultureInfo(Culture));
                builder.AppendText(App.LanguageMng.LanguageSet[Culture]["M400_ctTbFinish"].Sentence);
                builder.EndVoice();
                synthesizer.SpeakAsync(builder);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("{0} :: CommentWork Exception :: {1}", CurrentViewModelName, ex.ToString());
            }
        }
    }
}
