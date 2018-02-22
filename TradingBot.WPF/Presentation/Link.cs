
namespace TradingBot.WPF.Presentation
{
	using System;

	/// <summary>
	/// Represents a displayable link.
	/// </summary>
	public class Link : Displayable
	{
		private Uri _source;

		/// <summary>
		/// Gets or sets the source uri.
		/// </summary>
		/// <value>The source.</value>
		public Uri Source
		{
			get
			{
				return _source;
			}

			set
			{
				if (_source != value)
				{
					_source = value;
					OnPropertyChanged("Source");
				}
			}
		}
	}
}