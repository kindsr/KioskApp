using log4net;
using iBeautyNail.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace iBeautyNail.Datas
{
    public class DesignMng
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, string> CategorySet { get; private set; }
        public Dictionary<string, List<DesignInfo>> DesignSet { get; private set; }

        public DesignMng()
        {
            LoadCategories();
            if (String.IsNullOrEmpty(CategorySet.Keys.First()) == true) return;
            LoadDesigns(CategorySet);
        }

        private void LoadCategories()
        {
            DirectoryInfo di = new DirectoryInfo(SystemPath.Designs);
            DirectoryInfo[] categories = di.GetDirectories();

            this.CategorySet = new Dictionary<string, string>(categories.Length);

            foreach (DirectoryInfo d in categories)
            {
                string category = d.Name.Split('_')[1];
                if (CategorySet.ContainsKey(category) == false)
                    CategorySet.Add(category, d.FullName);
            }
        }

        private void LoadDesigns(Dictionary<string, string> categorySet)
        {
            List<string> designs = new List<string>();
            List<DesignInfo> designInfoList;

            this.DesignSet = new Dictionary<string, List<DesignInfo>>(CategorySet.Keys.Count);

            foreach (var c in categorySet)
            {
                designs = Directory.GetFiles(c.Value, "*.png", SearchOption.TopDirectoryOnly).ToList();
                designInfoList = new List<DesignInfo>();
                foreach (var d in designs)
                {
                    DesignInfo di = new DesignInfo()
                    {
                        Category = c.Key,
                        DesignPath = d.ToString()
                    };
                    designInfoList.Add(di);
                }
                if (DesignSet.ContainsKey(c.Key) == false)
                    DesignSet.Add(c.Key, designInfoList);
            }
        }
    }
}
