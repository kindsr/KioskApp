using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace iBeautyNail.Datas
{
    public class DesignInfo : ObservableObject
    {
        private string category;
        public string Category
        {
            get { return category; }
            //set { Set(() => Category, ref category, value); }
            set
            {
                if (Equals(value, category)) return;
                category = value;
            }

        }

        private string designPath;
        public string DesignPath
        {
            get { return designPath; }
            //set { Set(() => DesignPath, ref designPath, value); }
            set
            {
                if (Equals(value, designPath)) return;
                designPath = value;
            }
        }

        private string designCost;
        public string DesignCost
        {
            get { return designCost; }
            //set { Set(() => DesignCost, ref designCost, value); }
            set
            {
                if (Equals(value, designCost)) return;
                designCost = value;
            }
        }
    }
}
