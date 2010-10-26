/* -----------------------------------------------
 * VisiCalcServiceProvider.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

namespace Genetibase.NuGenVisiCalc
{
    /// <summary>
    /// <para>Provides:</para>
    /// <para><see cref="INuGenMenuItemCheckedTracker"/></para>
    /// <para><see cref="INuGenToolStripAutoSizeService"/></para>
    /// <para><see cref="INuGenWindowStateTracker"/></para>\
	/// <para><see cref="SplashStarter"/></para>
    /// </summary>
    internal sealed class VisiCalcServiceProvider : NuGenServiceProvider
    {
        /// <summary>
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns><see langword="null"/> if the specified service was not found.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para>
        ///		<paramref name="serviceType"/> is <see langword="null"/>.
        /// </para>
        /// </exception>
        protected override Object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (serviceType == typeof(INuGenMenuItemCheckedTracker))
            {
                return _menuItemCheckedTracker;
            }
            else if (serviceType == typeof(INuGenToolStripAutoSizeService))
            {
                return _toolStripAutoSizeService;
            }
            else if (serviceType == typeof(INuGenWindowStateTracker))
            {
                return _windowStateTracker;
			}
			else if (serviceType == typeof(OperatorsCache))
			{
				return _operatorsCache;
			}
			else if (serviceType == typeof(ParamsCache))
			{
				return _paramsCache;
			}
			else if (serviceType == typeof(ProgramsCache))
			{
				return _programsCache;
			}
			else if (serviceType == typeof(SplashStarter))
			{
				return _splashStarter;
			}
			else if (serviceType == typeof(TypesCache))
			{
				return _typesCache;
			}

            return base.GetService(serviceType);
        }

        private INuGenMenuItemCheckedTracker _menuItemCheckedTracker;
        private INuGenToolStripAutoSizeService _toolStripAutoSizeService;
        private INuGenWindowStateTracker _windowStateTracker;
		private OperatorsCache _operatorsCache;
		private ParamsCache _paramsCache;
		private ProgramsCache _programsCache;
		private SplashStarter _splashStarter;
		private TypesCache _typesCache;

        public VisiCalcServiceProvider()
        {
            _menuItemCheckedTracker = new NuGenMenuItemCheckedTracker();
            _toolStripAutoSizeService = new NuGenToolStripAutoSizeService();
            _windowStateTracker = new NuGenWindowStateTracker();
			_splashStarter = new SplashStarter();

			Assembly assembly = Assembly.GetExecutingAssembly();

			_operatorsCache = OperatorsCache.FromAssembly(assembly);
			_paramsCache = ParamsCache.FromAssembly(assembly);
			_programsCache = ProgramsCache.FromAssembly(assembly);
			_typesCache = TypesCache.FromAssembly(assembly);
        }
    }
}
