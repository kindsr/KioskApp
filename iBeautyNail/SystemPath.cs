using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail
{
    class SystemPath
    {
        public static string Base { get { return (AppDomain.CurrentDomain.BaseDirectory); } }

        /// <summary>
        /// Base\Configs
        /// </summary>
        public static string Configs { get { return (Path.Combine(Base, "Configs")); } }

        /// <summary>
        /// Base\Resources
        /// </summary>
        public static string Resources { get { return (Path.Combine(Base, "Resources")); } }

        /// <summary>
        /// Base\Resources\Images
        /// </summary>
        public static string Images { get { return (Path.Combine(Resources, "Images")); } }

        /// <summary>
        /// Base\Resources\Data
        /// </summary>
        public static string Data { get { return (Path.Combine(Resources, "Data")); } }

        /// <summary>
        /// Base\Resources\Languages
        /// </summary>
        public static string Languages { get { return (Path.Combine(Resources, "Languages")); } }

        /// <summary>
        /// Base\Resources\Images\Flags
        /// </summary>
        public static string Flags { get { return (Path.Combine(Images, "Flags")); } }

        /// <summary>
        /// Base\Resources\Images\Usage
        /// </summary>
        public static string Usage { get { return (Path.Combine(Images, "Usage")); } }

        public static string NailBase { get { return @"C:\nails\"; } }

        /// <summary>
        /// Base\Configs
        /// </summary>
        public static string Designs { get { return (Path.Combine(NailBase, "models")); } }

    }
}
