using System;
using NUnit.Framework;
using Screenmedia.Plugin.Vanilla;

namespace Vanilla.Tests.Android
{
	[TestFixture]
	public class Tests
	{
		private IceCreamMachine _iceCreamMachine;

		[SetUp]
		public void Setup() {
			_iceCreamMachine = new IceCreamMachine();
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
