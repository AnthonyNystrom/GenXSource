/* -----------------------------------------------
 * NuGenRibbonServiceProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Diagnostics;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Default <see cref="NuGenRibbonManager"/> service provider.
	/// </summary>
	public class NuGenRibbonServiceProvider : INuGenServiceProvider
	{
		#region INuGenServiceProvider Members

		/// <summary>
		/// Provides:
		/// <para><see cref="INuGenFormStateStore"/></para>
		/// <para><see cref="INuGenRibbonFormProperties"/></para>
        /// <para><see cref="INuGenMessageFilter"/></para>
		/// <para><see cref="INuGenMessageProcessor"/></para>
		/// <para><see cref="INuGenWmHandlerMapper"/></para>
		/// </summary>
		public T GetService<T>() where T : class
		{
			return (T)this.GetService(typeof(T));
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * FormStateStore
		 */

		private INuGenFormStateStore _formStateStore = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected virtual INuGenFormStateStore FormStateStore
		{
			get
			{
				if (_formStateStore == null)
				{
					_formStateStore = new NuGenFormStateStore();
				}

				return _formStateStore;
			}
		}

        /*
         * MessageFilter
         */

        private INuGenMessageFilter _messageFilter = null;

        /// <summary>
        /// Read-only.
        /// </summary>
        protected virtual INuGenMessageFilter MessageFilter
        {
            get
            {
                if (_messageFilter == null)
                {
                    _messageFilter = new NuGenMessageFilter();
                }

                return _messageFilter;
            }
        }

		/*
		 * MessageProcessor
		 */

		private INuGenMessageProcessor _messageProcessor = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenMessageProcessor MessageProcessor
		{
			get
			{
				if (_messageProcessor == null)
				{
					_messageProcessor = new NuGenFormMessageProcessor(this);
				}

				return _messageProcessor;
			}
		}

		/*
		 * RibbonFormProperties
		 */

		private INuGenRibbonFormProperties _ribbonFormProperties = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected virtual INuGenRibbonFormProperties RibbonFormProperties
		{
			get
			{
				if (_ribbonFormProperties == null)
				{
					_ribbonFormProperties = new NuGenRibbonFormProperties();
				}

				return _ribbonFormProperties;
			}
		}

		/*
		 * WmHandlerMapper
		 */

		private INuGenWmHandlerMapper _wmHandlerMapper = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected virtual INuGenWmHandlerMapper WmHandlerMapper
		{
			get
			{
				if (_wmHandlerMapper == null)
				{
					_wmHandlerMapper = new NuGenWmHandlerMapper();
				}

				return _wmHandlerMapper;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * GetService
		 */

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="serviceType"/> is <see langword="null"/>.
		/// </exception>
		protected virtual object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenFormStateStore))
			{
				Debug.Assert(this.FormStateStore != null, "this.FormStateStore != null");
				return this.FormStateStore;
			}

			if (serviceType == typeof(INuGenRibbonFormProperties))
			{
				Debug.Assert(this.RibbonFormProperties != null, "this.RibbonFormProperties != null");
				return this.RibbonFormProperties;
			}

            if (serviceType == typeof(INuGenMessageFilter))
            {
                Debug.Assert(this.MessageFilter != null, "this.MessageFilter != null");
                return this.MessageFilter;
            }

			if (serviceType == typeof(INuGenMessageProcessor))
			{
				Debug.Assert(this.MessageProcessor != null, "this.MessageProcessor != null");
				return this.MessageProcessor;
			}

			if (serviceType == typeof(INuGenWmHandlerMapper))
			{
				Debug.Assert(this.WmHandlerMapper != null, "this.WmHandlerMapper != null");
				return this.WmHandlerMapper;
			}

			return null;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonServiceProvider"/> class.
		/// </summary>
		public NuGenRibbonServiceProvider()
		{

		}

		#endregion
	}
}
