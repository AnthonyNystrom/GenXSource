using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Threading;
using System.Windows.Forms;

namespace Dile.UI
{
	public class NuGenQuickSearch
	{
		private FoundItem foundItemCallback;
		private FoundItem FoundItemCallback
		{
			get
			{
				return foundItemCallback;
			}
			set
			{
				foundItemCallback = value;
			}
		}

		private NuGenBasePanel callbackForm;
        private NuGenBasePanel CallbackForm
		{
			get
			{
				return callbackForm;
			}
			set
			{
				callbackForm = value;
			}
		}

        public NuGenQuickSearch(NuGenBasePanel callbackForm, FoundItem foundItemCallback)
		{
			FoundItemCallback = foundItemCallback;
			CallbackForm = callbackForm;
		}

		public void StartSearch(string searchText)
		{
			if (CallbackForm != null && FoundItemCallback != null)
			{
				Thread searchThread = new Thread(new ParameterizedThreadStart(Search));
				searchThread.Name = "Quick Finder thread";
				searchThread.Priority = ThreadPriority.Lowest;
				searchThread.Start(searchText);
			}
		}

		private void Search(object searchTextObject)
		{
			bool cancel = false;
			string searchText = (string)searchTextObject;

			if (NuGenProject.Instance.Assemblies != null)
			{
				List<NuGenAssembly>.Enumerator assemblyEnumerator = NuGenProject.Instance.Assemblies.GetEnumerator();

				while (!cancel && assemblyEnumerator.MoveNext())
				{
					Dictionary<uint, NuGenTokenBase>.ValueCollection.Enumerator tokenEnumerator = assemblyEnumerator.Current.AllTokens.Values.GetEnumerator();

					if (((assemblyEnumerator.Current.SearchOptions & SearchOptions.Assembly) == SearchOptions.Assembly
						&& assemblyEnumerator.Current.Name != null && assemblyEnumerator.Current.Name.Contains(searchText))
						|| (assemblyEnumerator.Current.SearchOptions & SearchOptions.TokenValues) == SearchOptions.TokenValues
						&& assemblyEnumerator.Current.Token.ToString("x").Contains(searchText))
					{
						NuGenFoundItemEventArgs eventArgs = OnFoundItem(assemblyEnumerator.Current, assemblyEnumerator.Current);

						cancel = eventArgs.Cancel;
					}

					while (!cancel && tokenEnumerator.MoveNext())
					{
						NuGenTokenBase tokenObject = tokenEnumerator.Current;
						SearchOptions searchOptions = assemblyEnumerator.Current.SearchOptions;

						if ((tokenObject.ItemType != SearchOptions.None
							&& (searchOptions & tokenObject.ItemType) == tokenObject.ItemType
							&& ((searchText.Length == 0 ||
							(tokenObject.Name != null && tokenObject.Name.Contains(searchText)))))
							|| ((searchOptions & SearchOptions.TokenValues) == SearchOptions.TokenValues
							&& tokenObject.Token.ToString("x").Contains(searchText)
							&& tokenObject.ItemType != SearchOptions.None))
						{
							NuGenFoundItemEventArgs eventArgs = OnFoundItem(assemblyEnumerator.Current, tokenObject);

							cancel = eventArgs.Cancel;
						}
					}
				}
			}
		}

		private NuGenFoundItemEventArgs OnFoundItem(NuGenAssembly assembly, NuGenTokenBase tokenObject)
		{
			NuGenFoundItemEventArgs eventArgs = new NuGenFoundItemEventArgs(assembly, tokenObject);                        
			IAsyncResult asyncResult = CallbackForm.BeginInvoke(FoundItemCallback, this, eventArgs);
			asyncResult.AsyncWaitHandle.WaitOne();

			return eventArgs;
		}
	}
}