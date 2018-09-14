using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extellect.Utilities.Math
{
    /// <summary>
    /// Linear transformation matrix helper for 2D transformations, using a 3D matrix internally 
    /// as done in the PDF specification.
    /// </summary>
    public class TransformationMatrix
    {
        private readonly Matrix _matrix;

        /// <summary>
        /// 
        /// </summary>
        public TransformationMatrix()
            : this(Matrix.Identity(3))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformationMatrix(double scaleX, double scaleY, double skewX, double skewY, double translateX, double translateY)
            : this(CreateMatrix(scaleX, scaleY, skewX, skewY, translateX, translateY))
        {
        }

        private TransformationMatrix(Matrix matrix)
        {
            _matrix = matrix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Matrix CreateMatrix(double scaleX, double scaleY, double skewX, double skewY, double translateX, double translateY)
        {
            return new Matrix(new double[,] { { scaleX, skewX, 0d }, { skewY, scaleY, 0d }, { translateX, translateY, 1d } });
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformationMatrix Transform(double scaleX, double scaleY, double skewX, double skewY, double translateX, double translateY)
        {
            return new TransformationMatrix(_matrix * CreateMatrix(scaleX, scaleY, skewX, skewY, translateX, translateY));
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformationMatrix Scale(double x, double y)
        {
            return new TransformationMatrix(_matrix * CreateMatrix(x, y, 0d, 0d, 0d, 0d));
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformationMatrix Translate(double x, double y)
        {
            return new TransformationMatrix(_matrix * CreateMatrix(1d, 1d, 0d, 0d, x, y));
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformationMatrix Skew(double x, double y)
        {
            return new TransformationMatrix(_matrix * CreateMatrix(1d, 1d, x, y, 0d, 0d));
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformationMatrix Rotate(double theta)
        {
            return new TransformationMatrix(_matrix * CreateMatrix(System.Math.Cos(theta), System.Math.Cos(theta), 0 - System.Math.Sin(theta), System.Math.Sin(theta), 0d, 0d));
        }

        /// <summary>
        /// 
        /// </summary>
        public Triplet ApplyTo(Triplet value)
        {
            return new Triplet(
                value.X * _matrix[0, 0] + value.Y * _matrix[0, 1] + _matrix[2, 0],
                value.Y * _matrix[1, 1] + value.X * _matrix[1, 0] + _matrix[2, 1],
                value.Z);
        }
    }
}
