using Unity;
using Microsoft.Practices.Unity.Configuration;

namespace iBeautyNail.Extensions
{
    public interface IIoCContainer
    {
        void CreateContainer();

        TType Resolve<TType>(string name);
    }

    public class UnityContainerManager : IIoCContainer
    {
        private static object lockObj = new object();

        private static UnityContainerManager instance;

        public static UnityContainerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnityContainerManager();
                }
                return instance;
            }
        }

        UnityContainer container;

        public void CreateContainer()
        {
            CreateContainer("UI", "Device");
        }

        public void CreateContainer(params string[] sections)
        {
            container = new UnityContainer();
            foreach (string section in sections)
            {
                container.LoadConfiguration(section);
            }
        }

        public TType Resolve<TType>(string name)
        {
            return container.Resolve<TType>(name);
        }

        public TType Resolve<TType>()
        {
            return container.Resolve<TType>();
        }
    }
}
