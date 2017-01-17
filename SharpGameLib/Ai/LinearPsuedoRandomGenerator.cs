using System;

namespace SharpGameLib.Ai
{
	public class LinearPsuedoRandomGenerator
	{
		private readonly Random rand = new Random();

		private float p;

		public LinearPsuedoRandomGenerator(float pdelta, float pinitial = 0)
		{
			this.Delta = pdelta;
			this.Initial = pinitial;
		}

		public float Initial { get; }

		public float Delta { get; }

		public float Next
		{
			get
			{
				return p;
			}
		}

		public bool NextBool()
		{
			return (p += this.Delta) > rand.NextDouble();
		}

		public void Reset()
		{
			this.p = this.Initial;
		}
	}
}

