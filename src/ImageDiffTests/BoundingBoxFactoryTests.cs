using System;
using ImageDiff;
using ImageDiff.BoundingBoxes;
using NUnit.Framework;

namespace ImageDiffTests
{
    [TestFixture]
    public class BoundingBoxFactoryTests
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void FactoryThrowsWithInvalidType()
        {
            BoundingBoxIdentifierFactory.Create((BoundingBoxModes)100, 0);
        }

        [Test]
        public void FactoryCreatesSingleBoundingBoxIdentifier()
        {
            var target = BoundingBoxIdentifierFactory.Create(BoundingBoxModes.Single, 0);
            Assert.IsInstanceOf(typeof(SingleBoundingBoxIdentifer), target);
        }

        [Test]
        public void FactoryCreatesMultipleBoundingBoxIdentifier()
        {
            var target = BoundingBoxIdentifierFactory.Create(BoundingBoxModes.Multiple, 0);
            Assert.IsInstanceOf(typeof(MultipleBoundingBoxIdentifier), target);
        }
    }
}
