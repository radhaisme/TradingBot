
namespace TradingBot.WPF.Presentation
{
	using System;

	/// <summary>
	/// The command that relays its functionality by invoking delegates.
	/// </summary>
	public class RelayCommand : CommandBase
	{
		private readonly Action<object> _action;
		private readonly Func<object, bool> _canExecute;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="action">The execute.</param>
		/// <param name="canExecute">The can execute.</param>
		public RelayCommand(Action<object> action, Func<object, bool> canExecute = null)
		{
			if (action == null)
			{
				throw new ArgumentNullException(nameof(action));
			}

			if (canExecute == null)
			{
				// no can execute provided, then always executable
				canExecute = (o) => true;
			}

			_action = action;
			_canExecute = canExecute;
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		public override bool CanExecute(object parameter)
		{
			return _canExecute(parameter);
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		protected override void OnExecute(object parameter)
		{
			_action(parameter);
		}
	}
}