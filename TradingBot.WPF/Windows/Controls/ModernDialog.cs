namespace TradingBot.WPF.Windows.Controls
{
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using Presentation;

	/// <summary>
	/// Represents a Modern UI styled dialog window.
	/// </summary>
	public class ModernDialog : DpiAwareWindow
	{
		/// <summary>
		/// Identifies the BackgroundContent dependency property.
		/// </summary>
		public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register("BackgroundContent", typeof(object), typeof(ModernDialog));
		
		/// <summary>
		/// Identifies the Buttons dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(IEnumerable<Button>), typeof(ModernDialog));

		private ICommand closeCommand;

		private Button _okButton;
		private Button _cancelButton;
		private Button _yesButton;
		private Button _noButton;
		private Button _closeButton;
		private MessageBoxResult _messageBoxResult = MessageBoxResult.None;

		/// <summary>
		/// Initializes a new instance of the <see cref="ModernDialog"/> class.
		/// </summary>
		public ModernDialog()
		{
			DefaultStyleKey = typeof(ModernDialog);
			WindowStartupLocation = WindowStartupLocation.CenterOwner;

			closeCommand = new RelayCommand(o =>
			{
				var result = o as MessageBoxResult?;

				if (result.HasValue)
				{
					_messageBoxResult = result.Value;

					// sets the Window.DialogResult as well
					if (result.Value == MessageBoxResult.OK || result.Value == MessageBoxResult.Yes)
					{
						DialogResult = true;
					}
					else if (result.Value == MessageBoxResult.Cancel || result.Value == MessageBoxResult.No)
					{
						DialogResult = false;
					}
					else
					{
						DialogResult = null;
					}
				}
				Close();
			});

			Buttons = new[] { CloseButton };

			// set the default owner to the app main window (if possible)
			if (Application.Current != null && Application.Current.MainWindow != this)
			{
				Owner = Application.Current.MainWindow;
			}
		}

		private Button CreateCloseDialogButton(string content, bool isDefault, bool isCancel, MessageBoxResult result)
		{
			return new Button
			{
				Content = content,
				Command = CloseCommand,
				CommandParameter = result,
				IsDefault = isDefault,
				IsCancel = isCancel,
				MinHeight = 21,
				MinWidth = 65,
				Margin = new Thickness(4, 0, 0, 0)
			};
		}

		/// <summary>
		/// Gets the close window command.
		/// </summary>
		public ICommand CloseCommand
		{
			get
			{
				return closeCommand;
			}
		}

		/// <summary>
		/// Gets the Ok button.
		/// </summary>
		public Button OkButton
		{
			get
			{
				if (_okButton == null)
				{
					_okButton = CreateCloseDialogButton(WPF.Resources.Ok, true, false, MessageBoxResult.OK);
				}

				return _okButton;
			}
		}

		/// <summary>
		/// Gets the Cancel button.
		/// </summary>
		public Button CancelButton
		{
			get
			{
				if (_cancelButton == null)
				{
					_cancelButton = CreateCloseDialogButton(WPF.Resources.Cancel, false, true, MessageBoxResult.Cancel);
				}

				return _cancelButton;
			}
		}

		/// <summary>
		/// Gets the Yes button.
		/// </summary>
		public Button YesButton
		{
			get
			{
				if (_yesButton == null)
				{
					_yesButton = CreateCloseDialogButton(WPF.Resources.Yes, true, false, MessageBoxResult.Yes);
				}

				return _yesButton;
			}
		}

		/// <summary>
		/// Gets the No button.
		/// </summary>
		public Button NoButton
		{
			get
			{
				if (_noButton == null)
				{
					_noButton = CreateCloseDialogButton(WPF.Resources.No, false, true, MessageBoxResult.No);
				}

				return _noButton;
			}
		}

		/// <summary>
		/// Gets the Close button.
		/// </summary>
		public Button CloseButton
		{
			get
			{
				if (this._closeButton == null)
				{
					this._closeButton = CreateCloseDialogButton(WPF.Resources.Close, true, false, MessageBoxResult.None);
				}
				return this._closeButton;
			}
		}

		/// <summary>
		/// Gets or sets the background content of this window instance.
		/// </summary>
		public object BackgroundContent
		{
			get
			{
				return GetValue(BackgroundContentProperty);
			}

			set
			{
				SetValue(BackgroundContentProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the dialog buttons.
		/// </summary>
		public IEnumerable<Button> Buttons
		{
			get
			{
				return (IEnumerable<Button>)GetValue(ButtonsProperty);
			}

			set
			{
				SetValue(ButtonsProperty, value);
			}
		}

		/// <summary>
		/// Gets the message box result.
		/// </summary>
		/// <value>
		/// The message box result.
		/// </value>
		public MessageBoxResult MessageBoxResult
		{
			get
			{
				return _messageBoxResult;
			}
		}

		/// <summary>
		/// Displays a messagebox.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="title">The title.</param>
		/// <param name="button">The button.</param>
		/// <param name="owner">The window owning the messagebox. The messagebox will be located at the center of the owner.</param>
		/// <returns></returns>
		public static MessageBoxResult ShowMessage(string text, string title, MessageBoxButton button, Window owner = null)
		{
			var dialog = new ModernDialog
			{
				Title = title,
				Content = new BBCodeBlock { BBCode = text, Margin = new Thickness(0, 0, 0, 8) },
				MinHeight = 0,
				MinWidth = 0,
				MaxHeight = 480,
				MaxWidth = 640,
			};

			if (owner != null)
			{
				dialog.Owner = owner;
			}

			dialog.Buttons = GetButtons(dialog, button);
			dialog.ShowDialog();

			return dialog._messageBoxResult;
		}

		private static IEnumerable<Button> GetButtons(ModernDialog owner, MessageBoxButton button)
		{
			if (button == MessageBoxButton.OK)
			{
				yield return owner.OkButton;
			}
			else if (button == MessageBoxButton.OKCancel)
			{
				yield return owner.OkButton;
				yield return owner.CancelButton;
			}
			else if (button == MessageBoxButton.YesNo)
			{
				yield return owner.YesButton;
				yield return owner.NoButton;
			}
			else if (button == MessageBoxButton.YesNoCancel)
			{
				yield return owner.YesButton;
				yield return owner.NoButton;
				yield return owner.CancelButton;
			}
		}
	}
}