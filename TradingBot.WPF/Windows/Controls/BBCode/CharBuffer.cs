
namespace TradingBot.WPF.Windows.Controls.BBCode
{
	using System;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Represents a character buffer.
	/// </summary>
	internal class CharBuffer
	{
		private readonly string _value;
		private int _position;
		private int _mark;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CharBuffer"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public CharBuffer(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			_value = value;
		}

		/// <summary>
		/// Performs a look-ahead.
		/// </summary>
		/// <param name="count">The number of character to look ahead.</param>
		/// <returns></returns>
		public char LA(int count)
		{
			int index = _position + count - 1;

			if (index < _value.Length)
			{
				return _value[index];
			}

			return char.MaxValue;
		}

		/// <summary>
		/// Marks the current position.
		/// </summary>
		public void Mark()
		{
			_mark = _position;
		}

		/// <summary>
		/// Gets the mark.
		/// </summary>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public string GetMark()
		{
			if (_mark < _position)
			{
				return _value.Substring(_mark, _position - _mark);
			}

			return String.Empty;
		}

		/// <summary>
		/// Consumes the next character.
		/// </summary>
		public void Consume()
		{
			_position++;
		}
	}
}