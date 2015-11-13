﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extellect.Utilities.Collections;

namespace Extellect.Utilities.Math
{
    /// <summary>
    /// A mathematical matrix of any size
    /// </summary>
    public class Matrix : IEquatable<Matrix>
    {
        /// <summary>
        /// The number of columns in the matrix
        /// </summary>
        public int M { get { return data.GetLength(0); } }

        /// <summary>
        /// The number of rows in the matrix
        /// </summary>
        public int N { get { return data.GetLength(1); } }

        private double[,] data;

        /// <summary>
        /// Creates an empty MxN matrix
        /// </summary>
        public Matrix(int m, int n)
        {
            data = new double[m, n];
        }

        /// <summary>
        /// Creates a new Matrix with a copy of the provided rectangular array
        /// </summary>
        public Matrix(double[,] data)
            : this(data, true)
        {   
        }

        /// <summary>
        /// Internal constructor that allows us to create matrices from 
        /// arrays without the overhead of copying.
        /// </summary>
        private Matrix(double[,] data, bool copy)
        {
            if (copy)
            {
                this.data = new double[data.GetLength(0), data.GetLength(1)];
                Array.Copy(data, 0, this.data, 0, data.Length);
            }
            else
            {
                this.data = data;
            }
        }

        /// <summary>
        /// Constructs the identity matrix of the specified size
        /// </summary>
        public static Matrix Identity(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", "Value must be positive");

            var identity = new Matrix(size, size);
            for (var s = 0; s < size; s++)
            {
                identity.data[s, s] = 1;
            }
            return identity;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Determinant
        {
            get
            {
                if (M != N)
                    throw new InvalidOperationException();
                if (M == 2)
                    return data[0, 0] * data[1, 1] - data[0, 1] * data[1, 0];
                else if (M == 3)
                    return data[0, 0] * Minor(0, 0).Determinant
                        - data[0, 1] * Minor(0, 1).Determinant
                        + data[0, 2] * Minor(0, 2).Determinant;
                else
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix Minor(int i, int j)
        {
            if (i < 0 || i >= M)
                throw new ArgumentOutOfRangeException("i");
            if (j < 0 || j >= N)
                throw new ArgumentOutOfRangeException("j");

            var data = new double[M - 1, N - 1];

            for (var m = 0; m < M - 1; m++)
            {
                for (var n = 0; n < N - 1; n++)
                {
                    data[m, n] = this.data[m + (m >= i ? 1 : 0), n + (n >= j ? 1 : 0)];
                }
            }

            return new Matrix(data, false);
        }

        /// <summary>
        /// Gets the transpose of this matrix
        /// </summary>
        public Matrix Transpose()
        {
            var data = new double[N, M];
            for (var m = 0; m < M; m++)
            {
                for (var n = 0; n < N; n++)
                {
                    data[n, m] = this.data[m, n];
                }
            }

            return new Matrix(data, false);
        }

        /// <summary>
        /// Adds another matrix to this matrix. Both matrices must have the same dimensions.
        /// </summary>
        public bool TryAdd(Matrix other, out Matrix matrix)
        {
            matrix = null;
            if (M != other.M || N != other.N)
                return false;

            matrix = new Matrix(this.data, true);

            for (var m = 0; m < M; m++)
            {
                for (var n = 0; n < N; n++)
                {
                    matrix[m, n] += other[m, n];
                }
            }

            return true;
        }

        /// <summary>
        /// Tries to multiply two matrices. Returns true if the matrices can be multiplied; otherwise false.
        /// </summary>
        public bool TryMultiply(Matrix other, out Matrix matrix)
        {
            matrix = null;
            if (N != other.M)
                return false;

            var result = new double[M, other.N];

            for (var m = 0; m < M; m++)
            {
                for (var n = 0; n < other.N; n++)
                {
                    for (var x = 0; x < N; x++)
                    {
                        result[m, n] += data[m, x] * other.data[x, n];
                    }
                }
            }

            matrix = new Matrix(result, false);

            return true;
        }

        /// <summary>
        /// Tries to invert the matrix. Returns true if the inversion succeeds; otherwise false.
        /// The inverted Matrix is set in the out parameter.
        /// </summary>
        public bool TryInvert(out Matrix matrix)
        {
            matrix = null;
            if (M != N)
                return false;
            var temp = new double[M, 2 * N];
            var identity = Identity(M);

            Plane.Write(data, 0, 0, temp, 0, 0, M, N);
            Plane.Write(identity.data, 0, 0, temp, 0, N, M, N);

            for (var i = 0; i < M; i++)
            {
                var x = temp[i, i];
                var f = 1 / x;

                for (var tn = 0; tn < 2 * N; tn++)
                {
                    temp[i, tn] *= f;
                }

                for (var tm = 0; tm < M; tm++)
                {
                    if (tm == i)
                        continue;

                    x = temp[tm, i];
                    f = 0 - x;

                    for (var tn = 0; tn < 2 * N; tn++)
                    {
                        temp[tm, tn] += temp[i, tn] * f;
                    }
                }
            }

            var result = new double[M, N];

            Plane.Write(temp, 0, N, result, 0, 0, M, N);

            matrix = new Matrix(result, false);

            return true;
        }

        /// <summary>
        /// Gets or sets the value at index M and N
        /// </summary>
        public double this[int m, int n]
        {
            get
            {
                return data[m, n];
            }
            set
            {
                data[m, n] = value;
            }
        }

        /// <summary>
        /// Gets a human readable representation of this matrix.
        /// </summary>
        public override string ToString()
        {
            var result = new StringBuilder();
            for (var m = 0; m < M; m++)
            {
                if (m != 0)
                {
                    result.AppendLine();
                }
                for (var n = 0; n < N; n++)
                {
                    if (n != 0)
                    {
                        result.Append('\t');
                    }
                    result.Append(data[m, n]);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns true if the two arrays are of the same dimensions and
        /// contain equal data.
        /// </summary>
        public bool Equals(Matrix other)
        {
            if (M != other.M || N != other.N)
                return false;
            return data.Cast<double>().SequenceEqual(other.data.Cast<double>());
        }

        /// <summary>
        /// Override the Equals method
        /// </summary>
        public override bool Equals(object other)
        {
            var otherMatrix = other as Matrix;
            if (otherMatrix != null)
                return Equals(otherMatrix);
            return false;
        }

        /// <summary>
        /// Override the GetHashCode method
        /// </summary>
        public override int GetHashCode()
        {
            return data.Cast<double>().Aggregate(0, (x, y) => x ^ y.GetHashCode());
        }
    }
}