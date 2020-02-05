using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Humm.InStore
{
	/// <summary>
	/// An interface for components that can decide on the base URL to call for Humm.
	/// </summary>
	/// <remarks>
	/// <para>The default implementation is <see cref="IHummApiUrlSelector"/> and is likely all user code needs. 
	/// However this interface allows custom versions to be built, such as one that allows user code to explicitly specify 
	/// a raw URL or to read config settings from persistent storage which are used to make a choice of base address.</para>
	/// </remarks>
	public interface IHummApiUrlSelector
	{
		/// <summary>
		/// Gets the base URL to use for Humm API calls.
		/// </summary>
		/// <returns>A <see cref="System.Uri"/> representing the base API address to be used.</returns>
		Uri GetUrl();
	}
}