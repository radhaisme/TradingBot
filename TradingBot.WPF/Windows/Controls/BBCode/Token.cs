
namespace TradingBot.WPF.Windows.Controls.BBCode
{
	using System;
	using System.Globalization;

	/// <summary>
	/// Represents a single token.
	/// </summary>
	internal class Token
	{
		/// <summary>
		/// Represents the token that marks the end of the input.
		/// </summary>
		public static readonly Token End = new Token(string.Empty, Lexer.TokenEnd);
		private readonly string _value;
		private readonly int _tokenType;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Token"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="tokenType">Type of the token.</param>
		public Token(string value, int tokenType)
		{
			_value = value;
			_tokenType = tokenType;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get
			{
				return _value;
			}
		}

		/// <summary>
		/// Gets the type of the token.
		/// </summary>
		/// <value>The type.</value>
		public int TokenType
		{
			get
			{
				return _tokenType;
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}: {1}", _tokenType, _value);
		}
	}
}