// -----------------------------------------------------------------------
//  <copyright file="VanillaTests.cs" company="Screenmedia">
//      Copyright (c) Screenmedia 2018. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace Screenmedia.Plugin.Vanilla.Test.UnitTest
{
    using FluentAssertions;
    using Screenmedia.Plugin.Vanilla.Abstractions;
    using Xunit;

    public class VanillaTests
    {
        private readonly IIceCreamMachine _iceCreamMachine;

        public VanillaTests()
        {
            _iceCreamMachine = CrossIceCreamMachine.Current;
        }

        [Fact]
        public void DispenseIceCream()
        {
            _iceCreamMachine.Dispense().Should().Be("Chocolate");
        }
    }
}
