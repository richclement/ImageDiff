using System;
using System.Drawing;
using ImageDiff;
using NUnit.Framework;

namespace ImageDiffTests
{
    [TestFixture]
    public class BitmapComparerTests
    {
        protected const string TestImage1 = "./images/TestImage1.png";
        protected const string TestImage2 = "./images/TestImage2.png";
        protected const string OutputFormat = "output_{0}.png";
        protected Bitmap FirstImage { get; set; }
        protected Bitmap SecondImage { get; set; }

        [SetUp]
        public void Setup()
        {
            FirstImage = new Bitmap(TestImage1);
            SecondImage = new Bitmap(TestImage2);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CompareThrowsWhenFirstImageIsNull()
        {
            var target = new BitmapComparer(null);
            target.Compare(null, SecondImage);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CompareThrowsWhenSecondImageIsNull()
        {
            var target = new BitmapComparer(null);
            target.Compare(FirstImage, null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void CompareThrowsWhenImagesAreNotSameWidth()
        {
            var firstBitmap = new Bitmap(10, 10);
            var secondBitmap = new Bitmap(20, 10);

            var target = new BitmapComparer(null);
            target.Compare(firstBitmap, secondBitmap);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void CompareThrowsWhenImagesAreNotSameHeight()
        {
            var firstBitmap = new Bitmap(10, 10);
            var secondBitmap = new Bitmap(10, 20);

            var target = new BitmapComparer(null);
            target.Compare(firstBitmap, secondBitmap);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ImageDiffThrowsWhenBoundingBoxPaddingIsLessThanZero()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = 2,
                BoundingBoxPadding = -2,
                Labeler = LabelerTypes.Basic
            });
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ImageDiffThrowsWhenDetectionPaddingIsLessThanZero()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = -2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.Basic
            });
        }

        [Test]
        public void CompareWorksWithNoOptions()
        {
            var target = new BitmapComparer();
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CompareWorksWithNullOptions"), SecondImage.RawFormat);
        }

        [Test]
        public void CompareWorksWithNullOptions()
        {
            var target = new BitmapComparer(null);
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CompareWorksWithNullOptions"), SecondImage.RawFormat);
        }

        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void CompareWorksWithIdenticalImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                AnalyzerType = aType,
                BoundingBoxMode = bMode,
                Labeler = lType
            });
            var result = target.Compare(FirstImage, FirstImage);
            result.Save(string.Format(OutputFormat, string.Format("CompareWorksWithIdenticalImages_{0}_{1}_{2}", aType, bMode, lType)),
                SecondImage.RawFormat);
        }


        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void CompareWorksWithDifferentImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, string.Format("CompareWorksWithDifferentImages_{0}_{1}_{2}", aType, bMode, lType)), SecondImage.RawFormat);
        }

        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsTrueWithSameImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var newInstanceOfFirstImage = new Bitmap(TestImage1);
            var result = target.Equals(FirstImage, newInstanceOfFirstImage);
            Assert.IsTrue(result);
        }

        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsFalseWithDifferentImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(FirstImage, SecondImage);
            Assert.IsFalse(result);
        }

        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsTrueWithNullImages(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(null, null);
            Assert.IsTrue(result);
        }

        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsFalseWithNullFirstImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(null, SecondImage);
            Assert.IsFalse(result);
        }

        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.CIE76, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Single, LabelerTypes.ConnectedComponentLabeling)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.Basic)]
        [TestCase(AnalyzerTypes.ExactMatch, BoundingBoxModes.Multiple, LabelerTypes.ConnectedComponentLabeling)]
        public void EqualsReturnsFalseWithNullSecondImage(AnalyzerTypes aType, BoundingBoxModes bMode, LabelerTypes lType)
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = bMode,
                AnalyzerType = aType,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = lType
            });
            var result = target.Equals(FirstImage, null);
            Assert.IsFalse(result);
        }
    }
}
