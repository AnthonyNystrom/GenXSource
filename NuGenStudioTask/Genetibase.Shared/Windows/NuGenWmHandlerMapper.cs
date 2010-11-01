/* -----------------------------------------------
 * NuGenWmHandlerMapper.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Reflection;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Provides functionality for message maps initialization.
	/// </summary>
	public class NuGenWmHandlerMapper : INuGenWmHandlerMapper
	{
		/// <summary>
		/// Looks for methods marked with <see cref="NuGenWmHandlerAttribute"/> and initializes a message map
		/// of type <see cref="NuGenWmHandlerList"/>.
		/// </summary>
		/// 
		/// <param name="messageProcessor">
		/// Specifies the methods marked with <see cref="NuGenWmHandlerAttribute"/> attribute(s)
		/// to handle Windows messages.
		/// </param>
		/// 
		/// <exception cref="ArgumentNullException">
		/// <paramref name="messageProcessor"/> is <see langword="null"/>.
		/// </exception>
		/// 
		/// <exception cref="NuGenWmHandlerSignatureException">
		/// <paramref name="messageProcessor"/> provides a method marked with <see cref="NuGenWmHandlerAttribute"/>
		/// but not compatible with the <see cref="NuGenWmHandler"/> delegate.
		/// </exception>
		public NuGenWmHandlerList BuildMessageMap(INuGenMessageProcessor messageProcessor)
		{
			if (messageProcessor == null)
			{
				throw new ArgumentNullException("messageProcessor");
			}

			NuGenWmHandlerList handlerList = new NuGenWmHandlerList();

			MethodInfo[] methods = messageProcessor.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo method = methods[i];
				object[] attributes = method.GetCustomAttributes(typeof(NuGenWmHandlerAttribute), false);

				foreach (NuGenWmHandlerAttribute attribute in attributes)
				{
					string methodName = method.Name;
					NuGenWmHandler wmHandler = null;

					try
					{
						wmHandler = (NuGenWmHandler)NuGenWmHandler.CreateDelegate(
							typeof(NuGenWmHandler),
							messageProcessor,
							methodName,
							false,
							true
						);
					}
					catch (ArgumentException)
					{
						throw new NuGenWmHandlerSignatureException(methodName);
					}

					handlerList.AddWmHandler(
						attribute.WmId,
						wmHandler
					);
				}
			}

			return handlerList;
		}

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWmHandlerMapper"/> class.
		/// </summary>
		public NuGenWmHandlerMapper()
		{

		}

		#endregion
	}
}
