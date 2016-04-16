using System.Data.Entity;
using Ninject;
using Ninject.Modules;

namespace Dexpa.Ioc
{
    public class IocFactory
    {
        /// <summary>
        /// Thread safety
        /// </summary>
        public static IocFactory Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (mSyncObject)
                    {
                        if (mInstance == null)
                        {
                            mInstance = new IocFactory();
                        }
                    }
                }
                return mInstance;
            }
        }

        public object Scope { get; private set; }

        private static IocFactory mInstance;

        private IKernel mKernel;

        private static object mSyncObject = new object();

        private object mGeneralScope = new object();

        public IocFactory()
        {
            mKernel = new StandardKernel(new INinjectModule[] { new CoreModule() });
            Scope = new object();
        }


        public IKernel GetKernel()
        {
            return mKernel;
        }

        public T Create<T>() where T : class
        {
            lock (mSyncObject)
            {
                Scope = new object();
                return mKernel.Get<T>();
            }
        }

        public T Create<T>(object scopeObject) where T : class
        {
            lock (mSyncObject)
            {
                Scope = scopeObject;
                return mKernel.Get<T>();
            }
        }

        public void Release(object releasingObject)
        {
            mKernel.Release(releasingObject);
        }
    }
}
