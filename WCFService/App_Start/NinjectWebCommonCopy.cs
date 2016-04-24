using WCFService;

//TODO: enable ninject
//[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommonCopy), "Start")]
//[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommonCopy), "Stop")]

//namespace WCFService
//{
//    using System;
//    using System.Web;

//    public static class NinjectWebCommonCopy
//    {
//        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

//        private static IKernel kernel;

//        /// <summary>
//        /// Starts the application
//        /// </summary>
//        public static void Start() 
//        {
//            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
//            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
//            bootstrapper.Initialize(CreateKernel);
            
//            AutoMapperConfiguration.Configure();
//        }
        
//        /// <summary>
//        /// Stops the application.
//        /// </summary>
//        public static void Stop()
//        {
//            bootstrapper.ShutDown();
//        }
        
//        /// <summary>
//        /// Creates the kernel that will manage your application.
//        /// </summary>
//        /// <returns>The created kernel.</returns>
//        public static IKernel CreateKernel()
//        {
//            if (kernel != null)
//            {
//                return kernel;
//            }
//            try
//            {
//                kernel = new StandardKernel();
//                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
//                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
//                RegisterServices(kernel);
//                return kernel;
//            }
//            catch
//            {
//                kernel.Dispose();
//                throw;
//            }
//        }

//        /// <summary>
//        /// Load your modules or register your services here!
//        /// </summary>
//        /// <param name="kernel">The kernel.</param>
//        private static void RegisterServices(IKernel kernel)
//        {
//            kernel.Load<OfflineServiceModule>();
//        }        
//    }
//}
