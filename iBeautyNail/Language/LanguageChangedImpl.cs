using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iBeautyNail.Language
{
    public class LanguageChangedImpl : ObservableObject
    {
        private string sentence;

        public string Sentence
        {
            get { return sentence; }
            set { Set(() => Sentence, ref sentence, value); }
        }

        public LanguageChangedImpl()
        {

        }

        public LanguageChangedImpl(string value)
        {
            this.Sentence = value;
        }
    }
}
