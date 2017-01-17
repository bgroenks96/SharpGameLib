using System;

namespace SharpGameLib
{
	public class ValueChangedEventArgs<TValue> : EventArgs
	{
		public ValueChangedEventArgs (TValue oldValue, TValue newValue)
		{
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public TValue OldValue { get; }

		public TValue NewValue { get; }
	}
}

