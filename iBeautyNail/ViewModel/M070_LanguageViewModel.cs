using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using iBeautyNail.Enums;
using iBeautyNail.Language;
using iBeautyNail.Datas;

namespace iBeautyNail.ViewModel
{
    public class M070_LanguageViewModel : BaseViewModelBase
    {
        public List<LanguageInfo> LanguageList
        {
            get
            {
                List<LanguageInfo> languageInfoList = new List<LanguageInfo>();

                string[] culture = App.LanguageMng.LanguageSet.Keys.ToArray();
                List<string> counties = new List<string>(culture.Length);

                foreach (var c in culture)
                {
                    LanguageInfo info = new LanguageInfo()
                    {
                        Culture = c,
                        Country = App.LanguageMng.LanguageSet[c]["LANG2"].Sentence
                    };

                    languageInfoList.Add(info);
                }

                return languageInfoList;
            }
        }

        private RelayCommand<string> changeLanguageCommand;

        public RelayCommand<string> ChangeLanguageCommand
        {
            get
            {
                return new RelayCommand<string>((culture) =>
                {
                    App.LanguageMng.ChangeLanguage(culture);
                    CommandAction(NAVIGATION_TYPE.Back);
                });
            }
        }

        public M070_LanguageViewModel()
        {
        }

        protected override void PageLoad()
        {
            GlobalVariables.Instance.LanguagePopup = true;
            CanNavigate = true;
        }
    }
}
