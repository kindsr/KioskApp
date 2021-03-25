using log4net;
using iBeautyNail.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//using System.Windows.Input;

namespace iBeautyNail.Language
{
    public class LanguageMng
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string PATH = "Styles/Language/";

        private Dictionary<string, ResourceDictionary> languageResourceSet;

        public Dictionary<string, ObservableDictionary<string, LanguageChangedImpl>> LanguageSet { get; private set; }

        //public List<LanguageInfo> CultureSet { get; private set; }

        public string CurrentCulture { get; private set; }

        public ObservableDictionary<string, LanguageChangedImpl> CurrentLanguage { get; private set; }

        public ObservableDictionary<string, LanguageChangedImpl> DefaultLanguage { get; private set; }

        public LanguageMng()
        {
            LoadFiles();
            LoadResource();
            ChangeResource();
        }

        private void LoadFiles()
        {
            int i = 0;
            int c = 0;

            DirectoryInfo di = new DirectoryInfo(SystemPath.Languages);
            FileInfo[] fileInfo = di.GetFiles("*.lang", SearchOption.TopDirectoryOnly);

            this.LanguageSet = new Dictionary<string, ObservableDictionary<string, LanguageChangedImpl>>(fileInfo.Length);

            i = 0;
            foreach (FileInfo file in fileInfo)
            {
                string culture = file.Name.Replace(file.Extension, string.Empty).Split('_')[1];
                ObservableDictionary<string, LanguageChangedImpl> lang = new ObservableDictionary<string, LanguageChangedImpl>();

                c = 0;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    string line = string.Empty;
                    string[] pair;

                    while (sr.Peek() >= 0)
                    {
                        line = sr.ReadLine();

                        if ((line.Length > 0) && (!line.StartsWith(";")))    // skip empty, comment lines
                        {
                            pair = line.Split(new char[] { '=' });

                            if ((pair[0].Length > 0) && (pair[1].Length > 0))
                            {
                                lang.Add(pair[0].Trim(), new LanguageChangedImpl { Sentence = pair[1].Trim().Replace("^", " ") });
                                //logger.InfoFormat("{0}={1}", pair[0], this.MultiLangauge[i][pair[0]].Sentence);
                                c++;
                            }
                        }
                    }
                    sr.Close();

                    if (LanguageSet.ContainsKey(culture) == false)
                        LanguageSet.Add(culture, lang);
                }

                if (culture.Equals(ApplicationConfigurationSection.Instance.Machine.DefaultLanguage))
                {
                    SetCulture(culture);

                    this.CurrentLanguage = new ObservableDictionary<string, LanguageChangedImpl>(lang);
                    this.DefaultLanguage = new ObservableDictionary<string, LanguageChangedImpl>(lang);
                }

                i++;
            }
        }


        private void LoadResource()
        {
            languageResourceSet = new Dictionary<string, ResourceDictionary>();
            foreach (var resource in Application.Current.Resources.MergedDictionaries)
            {
                if (resource.Source != null && resource.Source.OriginalString.IndexOf(PATH) == 0)
                {
                    languageResourceSet.Add(resource.Source.OriginalString, resource);
                }
            }
        }

        private void ChangeResource()
        {
            ResourceDictionary resourceDictionary;
            string requestedCulture = $"{PATH}{this.CurrentCulture}.xaml";
            string defaultCulture = $"{PATH}{ApplicationConfigurationSection.Instance.Machine.DefaultLanguage}.xaml";

            if (languageResourceSet.ContainsKey(requestedCulture))
            {
                resourceDictionary = languageResourceSet[requestedCulture];
            }
            else
            {
                resourceDictionary = languageResourceSet[defaultCulture];
            }

            foreach (string key in languageResourceSet.Keys)
            {
                Application.Current.Resources.MergedDictionaries.Remove(languageResourceSet[key]);
            }
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo(this.CurrentCulture);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private void SetCulture(string culture)
        {
            this.CurrentCulture = culture;
        }

        public void ChangeLanguage(string culture)
        {
            if (this.CurrentCulture.Equals(culture))
                return;

            //ChangeTopCountryImage();
            SetCulture(culture);
            ChangeResource();

            foreach (KeyValuePair<string, LanguageChangedImpl> kvp in LanguageSet[culture])
            {
                this.CurrentLanguage[kvp.Key] = kvp.Value;
            }
        }
    }
}
