using AccountsData;
using AccountsFunction;
using AndersonCRMData;
using AndersonCRMFunction;
using AndersonWorkLogsData;
using AndersonWorkLogsFunction;
using System;

using Unity;
using Unity.AspNet.Mvc;

namespace AndersonWorkLogsWeb
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            #region Data
            container.RegisterType<IDAttendance, DAttendance>(new PerRequestLifetimeManager());
            container.RegisterType<IDWorkLog, DWorkLog>(new PerRequestLifetimeManager());
            container.RegisterType<IDUser, DUser>(new PerRequestLifetimeManager());
            container.RegisterType<IDEmployee, DEmployee>(new PerRequestLifetimeManager());
            container.RegisterType<IDDepartment, DDepartment>(new PerRequestLifetimeManager());
            container.RegisterType<IDEmployeeDepartment, DEmployeeDepartment>(new PerRequestLifetimeManager());
            #endregion

            #region Function
            container.RegisterType<IFAttendance, FAttendance>(new PerRequestLifetimeManager());
            container.RegisterType<IFWorkLog, FWorkLog>(new PerRequestLifetimeManager());
            container.RegisterType<IFUser, FUser>(new PerRequestLifetimeManager());
            container.RegisterType<IFEmployee, FEmployee>(new PerRequestLifetimeManager());
            container.RegisterType<IFDepartment, FDepartment>(new PerRequestLifetimeManager());
            container.RegisterType<IFEmployeeDepartment, FEmployeeDepartment>(new PerRequestLifetimeManager());
            #endregion
        }
    }
}