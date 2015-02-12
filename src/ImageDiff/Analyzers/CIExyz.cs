using System;
using System.Drawing;

namespace ImageDiff.Analyzers
{
    internal struct CIExyz
    {

        public const double RefX = 95.047;
        public const double RefY = 100.000;
        public const double RefZ = 108.883;

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public CIExyz(double x, double y, double z)
            : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static CIExyz FromRGB(Color color)
        {
            // normalize red, green, blue values
            var rLinear = color.R / 255.0;
            var gLinear = color.G / 255.0;
            var bLinear = color.B / 255.0;

            // convert to a sRGB form
            var r = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / (1.055), 2.4) : (rLinear / 12.92);
            var g = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / (1.055), 2.4) : (gLinear / 12.92);
            var b = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / (1.055), 2.4) : (bLinear / 12.92);

            // converts
            return new CIExyz((r * 0.4124 + g * 0.3576 + b * 0.1805),
                (r * 0.2126 + g * 0.7152 + b * 0.0722),
                (r * 0.0193 + g * 0.1192 + b * 0.9505));
        }
    }
}