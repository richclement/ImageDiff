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


        [Test]
        public void ExactMatch_BasicLabeling_SingleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.Basic
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "ExactMatch_BasicLabeling_SingleBox"), SecondImage.RawFormat);
        }

        [Test]
        public void ExactMatch_ConnectedComponentLabeling_SingleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.ConnectedComponentLabeling
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "ExactMatch_ConnectedComponentLabeling_SingleBox"), SecondImage.RawFormat);
        }

        [Test]
        public void ExactMatch_ConnectedComponentLabeling_MultipleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Multiple,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.ConnectedComponentLabeling
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "ExactMatch_ConnectedComponentLabeling_MultipleBox"), SecondImage.RawFormat);
        }

        [Test]
        public void ExactMatch_BasicLabeling_MultipleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Multiple,
                AnalyzerType = AnalyzerTypes.ExactMatch,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.Basic
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "ExactMatch_BasicLabeling_MultipleBox"), SecondImage.RawFormat);
        }

        [Test]
        public void CIE76JND_BasicLabeling_MultipleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Multiple,
                AnalyzerType = AnalyzerTypes.CIE76,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.Basic
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CIE76JND_BasicLabeling_MultipleBox"), SecondImage.RawFormat);
        }

        [Test]
        public void CIE76JND_ConnectedComponentLabeling_MultipleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Green,
                BoundingBoxMode = BoundingBoxModes.Multiple,
                AnalyzerType = AnalyzerTypes.CIE76,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.ConnectedComponentLabeling
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CIE76JND_ConnectedComponentLabeling_MultipleBox"), SecondImage.RawFormat);
        }

        [Test]
        public void CIE76JND_BasicLabeling_SingleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.CIE76,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.Basic
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CIE76JND_BasicLabeling_SingleBox"), SecondImage.RawFormat);
        }

        [Test]
        public void CIE76JND_ConnectedComponentLabeling_SingleBox()
        {
            var target = new BitmapComparer(new CompareOptions
            {
                BoundingBoxColor = Color.Red,
                BoundingBoxMode = BoundingBoxModes.Single,
                AnalyzerType = AnalyzerTypes.CIE76,
                DetectionPadding = 2,
                BoundingBoxPadding = 2,
                Labeler = LabelerTypes.ConnectedComponentLabeling
            });
            var result = target.Compare(FirstImage, SecondImage);
            result.Save(string.Format(OutputFormat, "CIE76JND_ConnectedComponentLabeling_SingleBox"), SecondImage.RawFormat);
        }

    }
}
