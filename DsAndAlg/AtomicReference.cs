using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>Holds a reference to an immutable class and updates it atomically.</summary>
/// <typeparam name="T">An immutable class to reference.</typeparam>
public class AtomicReference<T> where T : class
{
	private volatile T value;

	public AtomicReference(T initialValue)
	{
		this.value = initialValue;
	}

	/// <summary>Gets the current value of this instance.</summary>
	public T Value { get { return value; } }

	/// <summary>Atomically updates the value of this instance.</summary>
	/// <param name="mutator">A pure function to compute a new value based on the current value of the instance.
	/// This function may be called more than once.</param>
	/// <returns>The previous value that was used to generate the resulting new value.</returns>

	public T Mutate(T newVal)
	{
		T baseVal = value;
		while (true)
		{
#pragma warning disable 420
			T currentVal = Interlocked.CompareExchange(ref value, newVal, baseVal);
#pragma warning restore 420

			if (currentVal == baseVal)
				return baseVal;         // Success!
			else
				baseVal = currentVal;   // Try again
		}
	}
}