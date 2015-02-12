using System;
using ImageDiff;
using ImageDiff.Labelers;
using NUnit.Framework;

namespace ImageDiffTests
{
    [TestFixture]
    public class LabelerFactoryTests
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void FactoryThrowsWithInvalidType()
        {
            LabelerFactory.Create((LabelerTypes)100, 0);
        }

        [Test]
        public void FactoryCreatesBasicLabeler()
        {
            var target = LabelerFactory.Create(LabelerTypes.Basic,  0);
            Assert.IsInstanceOf(typeof(BasicLabeler), target);
        }

        [Test]
        public void FactoryCreatesConnectedComponentLabeler()
        {
            var target = LabelerFactory.Create(LabelerTypes.ConnectedComponentLabeling, 0);
            Assert.IsInstanceOf(typeof(ConnectedComponentLabeler), target);
        }
    }
}
