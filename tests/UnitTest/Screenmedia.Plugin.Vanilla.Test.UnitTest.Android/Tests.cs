using System;
using NUnit.Framework;
using Screenmedia.Plugin.Vanilla.Abstractions;

namespace Screenmedia.Plugin.Vanilla.Test.UnitTest.Android
{
	[TestFixture]
	public class Tests
	{
		private IIceCreamMachine _iceCreamMachine;

		[SetUp]
		public void Setup() {
			_iceCreamMachine = CrossIceCreamMachine.Current;
		}

		[Test]
		public void DispenseCorrectIceCream()
		{
			Console.WriteLine("Dispense Test");
			var expectedValue = "Chocolate";
			var actualValue = _iceCreamMachine.Dispense();
			Assert.AreEqual(expectedValue, actualValue, "Not Dispensed Correct IceCream \n Actual: {1} \n Expected: {0}", new object[] { expectedValue, actualValue });
		}
	}
}