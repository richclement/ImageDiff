[![Build status](https://ci.appveyor.com/api/projects/status/qg5rrd1rc1ioetum?svg=true)](https://ci.appveyor.com/project/RichClement/imagediff)

ImageDiff
==========

A .Net library for comparing images and highlighting the differences.

ImageDiff has three stages to its processing.
 
1. Analyze the provided images.
2. Detect and label the differences between the images.
3. Build the bounding boxes for the identified blobs.

After processing is finished, it will generate a new image which is derived from the second image provided to the `Compare` method. This `diff` image will contain highlighted bounding boxes around the differences between the two images.

![first image](/docs/images/firstImage.png) compared to ![second image](/docs/images/secondImage.png) produces 
![image diff](/docs/images/diffImage.png)

Usage
-----

Default usage:

    var firstImage = new Bitmap("path/to/first/image");
    var secondImage = new Bitmap("path/to/second/image);

    var comparer = new BitmapComparer();
    var diff = comparer.Compare(firstImage, secondImage);

When initialized without options, the following values are used:

- AnalyzerType: ExactMatch
- Labeler: Basic
- JustNoticeableDifference: 2.3
- DetectionPadding: 2
- BoundingBoxPadding: 2
- BoundingBoxColor: Red
- BoundingBoxMode: Single


The compare object can be configured to use different settings for the different stages of processing.

    var options = new CompareOptions 
    {
		AnalyzerType = AnalyzerTypes.CIE76,
        JustNoticableDifference = 2.3,
        DetectionPadding = 2,
        Labeler = LabelerTypes.ConnectedComponentLabeling,
        BoundingBoxColor = Color.Red,
        BoundingBoxPadding = 2,
        BoundingBoxMode = BoundingBoxModes.Multiple
    };
    var comparer = new BitmapComparer(options);

#### Analyzer Type
Two forms of image analysis are currently supported:

- ExactMatch - requires that the RGB values of each pixel in the image be equal.
- CIE76 - follows the [color difference formula](http://en.wikipedia.org/wiki/Color_difference "color difference formula") to generate a Euclidean distance between the colors in the pixels and flags a difference when the Just Noticeable Difference (JND) is greater than a value of 2.3.

#### Just Noticeable Difference
Specify this to control how distant two pixels can be in the color space before they are marked as different.

#### Detection Padding
How many pixels away from the current pixel to look, for neighbors that should be grouped together for labeling purposes.

#### Labeler
Two forms of blob labeling are currently supported:

- Basic - basic labeling will group all differences together into a single group. This labeling format does not support `BoundingBoxMode.Multiple`.
- [Connected Component Labeling](http://en.wikipedia.org/wiki/Connected-component_labeling "Connected Component Labeling") - Uses a two-pass algorithm to label the differences in an image and then aggregate the labels. The Detection Padding option is used to determine how far to travel when checking neighbor pixels.

#### Bounding Box Color
The color of the bounding box to be drawn when highlighting detected differences.

#### Bounding Box Padding
The number of pixels of padding to include around the detected difference when drawing a bounding box.

#### Bounding Box Mode
Specifies how to build the bounding boxes when highlighting the detected differences.

- Single - Only generate one bounding box that encompasses all of the detected differences in the image.
- Multiple - Generate a bounding box around each separate group of detected differences. This bounding box mode is not supported by `LabelerTypes.Basic`.
