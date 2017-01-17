using System.Runtime.InteropServices;

namespace System.Time
{
	/// <summary>
	/// This class defines the functionality of High Resolution Timer
	/// </summary>
	internal class HighResolutionTimer
	{
#if !CompactFramework && !SILVERLIGHT
		[DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter", SetLastError = true)]
		private static extern bool QueryPerformanceCounter(ref long value);
#else
		[DllImport("coredll.dll", EntryPoint = "QueryPerformanceCounter", SetLastError = true)]
		private static extern bool QueryPerformanceCounter(ref long value);
#endif

#if !CompactFramework && !SILVERLIGHT
		[DllImport("kernel32.dll", EntryPoint = "QueryPerformanceFrequency", SetLastError = true)]
		private static extern bool QueryPerformanceFrequency(ref long frequency);
#else
		[DllImport("coredll.dll", EntryPoint = "QueryPerformanceFrequency", SetLastError = true)]
		private static extern bool QueryPerformanceFrequency(ref long frequency);
#endif

		private static readonly long Frequency;
		private long _currentSplit;
		private long _previousSplit;
		private long _start;
		private long _stop;

		/// <summary>
		/// Initializes the <see cref="HighResolutionTimer"/> class.
		/// </summary>
		/// <exception cref="Exception">If not supported on this platform.</exception>
		static HighResolutionTimer()
		{
			Frequency = QueryFrequency();
			if (Frequency == 0)
			{
				throw new Exception("HighResolutionTimer Not Supported");
			}
		}

		/// <summary>
		/// Creates a new <see cref="HighResolutionTimer"/> instance.
		/// </summary>
		public HighResolutionTimer()
		{
			IsRunning = false;
			Reset();
		}

		/// <summary>
		/// Gets the elapsed time.
		/// </summary>
		/// <value>The elapsed time.</value>
		public double ElapsedTime
		{
			get
			{
				if (IsRunning)
				{
					_stop = QueryValue();
				}
				return (_stop - _start)*1.0/Frequency;
			}
		}

		/// <summary>
		/// Gets a value indicating whether [is running].
		/// </summary>
		/// <value><c>true</c> if [is running]; otherwise, <c>false</c>.</value>
		public bool IsRunning { get; private set; }

		/// <summary>
		/// Gets the lap count.
		/// </summary>
		/// <value>The lap count.</value>
		public int LapCount { get; private set; }

		/// <summary>
		/// Gets the last lap time.
		/// </summary>
		/// <value>The last lap time.</value>
		public double LastLapTime
		{
			get { return (_currentSplit - _previousSplit)*1.0/Frequency; }
		}

		/// <summary>
		/// Gets the last split time.
		/// </summary>
		/// <value>The last split time.</value>
		public double LastSplitTime
		{
			get { return (_currentSplit - _start)*1.0/Frequency; }
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		/// <exception cref="Exception">Cannot reset a high resolution timer while it is running.</exception>
		public void Reset()
		{
			if (IsRunning)
			{
				throw new Exception("Cannot reset a high resolution timer while it is running.");
			}
			_start = 0;
			_currentSplit = 0;
			_previousSplit = 0;
			_stop = 0;
			LapCount = 0;
		}

		/// <summary>
		/// Splits this instance.
		/// </summary>
		public double Split()
		{
			if (IsRunning)
			{
				long next = QueryValue();
				NextSplit(next);
				return LastSplitTime;
			}
			return 0;
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		/// <exception cref="Exception">Cannot start a high resolution timer that is running.</exception>
		public void Start()
		{
			if (IsRunning)
			{
				throw new Exception("Cannot start a high resolution timer that is running.");
			}
			_previousSplit = _currentSplit = _start = QueryValue();
			IsRunning = true;
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public double Stop()
		{
			if (IsRunning)
			{
				_stop = QueryValue();
				NextSplit(_stop);
				IsRunning = false;
				return ElapsedTime;
			}
			return 0;
		}

		/// <summary>
		/// Set the next split time.
		/// </summary>
		/// <param name="next">Next.</param>
		private void NextSplit(long next)
		{
			_previousSplit = _currentSplit;
			_currentSplit = next;
			LapCount++;
		}

		/// <summary>
		/// Queries the system for the value of the native clock.
		/// </summary>
		/// <returns>A long value.</returns>
		private static long QueryValue()
		{
			long value = 0;
			try
			{
				QueryPerformanceCounter(ref value);
			}
			catch
			{
			}
			return value;
		}

		/// <summary>
		/// Queries the system for the frequency of the native clock.
		/// </summary>
		/// <returns>A long frequency value.</returns>
		private static long QueryFrequency()
		{
			long value = 0;
			try
			{
				QueryPerformanceFrequency(ref value);
			}
			catch
			{
			}
			return value;
		}
	}
}