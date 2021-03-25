using iBeautyNail.Datas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace iBeautyNail.ViewModel
{
    class M700_FinishViewModel : BaseViewModelBase
    {
        System.Windows.Threading.DispatcherTimer timer;

        public M700_FinishViewModel()
        {
            PrevButtonVisible = false;

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(0);
            timer.Tick += (object sender, EventArgs e) =>
            {
                {
                    StopTimer();
                    logger.DebugFormat("{0} :: Finished!", CurrentViewModelName);
                    BaseViewModelBase.CanNavigate = true;

                    if (GlobalVariables.Instance.IsTTSOn)
                    {
                        threadDelegate = new ThreadStart(CommentWork);
                        commentThread = new Thread(threadDelegate);
                        commentThread.Start();
                    }
                }
            };
        }

        protected override void PageLoad()
        {
            StartTimer();
        }

        protected override void PageUnload()
        {
            if (GlobalVariables.Instance.IsTTSOn)
            {
                synthesizer.SpeakAsyncCancelAll();
                //synthesizer.Dispose();
                commentThread.Abort();
            }

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

        protected override void CommentWork()
        {
            try
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.Rate = 2;
                synthesizer.SetOutputToDefaultAudioDevice();

                var builder = new PromptBuilder();
                builder.StartVoice(new CultureInfo(Culture));
                builder.AppendText(App.LanguageMng.LanguageSet[Culture]["M700_ctTbLookForNailSticker"].Sentence);
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
