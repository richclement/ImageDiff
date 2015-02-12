using System;
using System.Drawing;

namespace ImageDiff.Analyzers
{
    internal struct CIELab
    {
        public double L { get; set; }
        public double a { get; set; }
        public double b { get; set; }

        public CIELab(double l, double a, double b)
            : this()
        {
            this.L = l;
            this.a = a;
            this.b = b;
        }

        public static CIELab FromRGB(Color color)
        {
            return FromCIExyz(CIExyz.FromRGB(color));
        }

        public static CIELab FromCIExyz(CIExyz xyzColor)
        {
            var transformedX = Transformxyz(xyzColor.x/CIExyz.RefX);
            var transformedY = Transformxyz(xyzColor.y/CIExyz.RefY);
            var transformedZ = Transformxyz(xyzColor.z/CIExyz.RefZ);
            
            var L = 116.0 * transformedY - 16;
            var a = 500.0 * (transformedX - transformedY);
            var b = 200.0 * (transformedY - transformedZ);

            return new CIELab(L, a, b);
        }

        private static double Transformxyz(double t)
        {
            return ((t > 0.008856) ? Math.Pow(t, (1.0 / 3.0)) : ((7.787 * t) + (16.0 / 116.0)));
        }
    }
}