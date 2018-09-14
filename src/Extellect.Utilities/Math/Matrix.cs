using System;
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
        private readonly double[,] _data;

        /// <summary>
        /// Creates an empty MxN matrix
        /// </summary>
        public Matrix(int m, int n)
        {
            _data = new double[m, n];
        }

        /// <summary>
        /// Creates a new Matrix with a copy of the provided rectangular array
        /// </summary>
        public Matrix(double[,] data)
            : this(data, true)
        {
        }

        /// <summary>
        /// Creates a new Matrix with a copy of the provided Matrix's data
        /// </summary>
        public Matrix(Matrix source)
            : this(source._data, true)
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
                _data = new double[data.GetLength(0), data.GetLength(1)];
                Array.Copy(data, 0, _data, 0, data.Length);
            }
            else
            {
                _data = data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix(double[] data, int rows, int columns, bool isByRow = false)
        {
            if (data.Length != rows * columns)
            {
                throw new ArgumentException();
            }

            _data = isByRow ? new double[rows, columns] : new double[columns, rows];

            Buffer.BlockCopy(data, 0, _data, 0, data.Length * sizeof(double));

            if (!isByRow)
            {
                _data = Transpose(_data);
            }
        }

        /// <summary>
        /// Constructs the identity matrix of the specified size
        /// </summary>
        public static Matrix Identity(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size", "Value must be positive");
            }

            var identity = new Matrix(size, size);
            for (var s = 0; s < size; s++)
            {
                identity._data[s, s] = 1;
            }
            return identity;
        }

        /// <summary>
        /// Constructs a matrix of basis vectors
        /// </summary>
        public static Matrix Diagonal(double[] values)
        {
            var result = Identity(values.Length);
            for (var i = 0; i < values.Length; i++)
            {
                result[i, i] = values[i];
            }
            return result;
        }

        /// <summary>
        /// Gets the number of rows in the matrix
        /// </summary>
        public int M => _data.GetLength(0);

        /// <summary>
        /// Gets the number of columns in the matrix
        /// </summary>
        public int N => _data.GetLength(1);

        /// <summary>
        /// 
        /// </summary>
        public double Determinant
        {
            get
            {
                if (M != N)
                {
                    throw new InvalidOperationException();
                }
                if (M == 2)
                {
                    return _data[0, 0] * _data[1, 1] - _data[0, 1] * _data[1, 0];
                }
                else if (M == 3)
                {
                    return _data[0, 0] * Minor(0, 0).Determinant
                        - _data[0, 1] * Minor(0, 1).Determinant
                        + _data[0, 2] * Minor(0, 2).Determinant;
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Matrix Minor(int i, int j)
        {
            if (i < 0 || i >= M)
            {
                throw new ArgumentOutOfRangeException(nameof(i));
            }
            if (j < 0 || j >= N)
            {
                throw new ArgumentOutOfRangeException(nameof(j));
            }

            var data = new double[M - 1, N - 1];

            for (var m = 0; m < M - 1; m++)
            {
                for (var n = 0; n < N - 1; n++)
                {
                    data[m, n] = _data[m + (m >= i ? 1 : 0), n + (n >= j ? 1 : 0)];
                }
            }

            return new Matrix(data, false);
        }

        /// <summary>
        /// Note parameter order is a bit odd if you're used to X,Y,Width,Height
        /// but that's because we think of vertical axis first in MxN matrices.
        /// </summary>
        public Matrix Submatrix(int top, int left, int height, int width)
        {
            var data = new double[height, width];

            Plane.Write(_data, top, left, data, 0, 0, width, height);

            return new Matrix(data, false);
        }

        /// <summary>
        /// Gets the transpose of this matrix
        /// </summary>
        public Matrix Transpose()
        {
            return new Matrix(Transpose(_data), false);
        }

        private static double[,] Transpose(double[,] data)
        {
            var transposed = new double[data.GetLength(1), data.GetLength(0)];
            for (var m = 0; m < data.GetLength(0); m++)
            {
                for (var n = 0; n < data.GetLength(1); n++)
                {
                    transposed[n, m] = data[m, n];
                }
            }
            return transposed;
        }

        /// <summary>
        /// Adds another matrix to this matrix. Both matrices must have the same dimensions.
        /// </summary>
        public bool TryAdd(Matrix other, out Matrix matrix)
        {
            matrix = null;

            if (M != other.M || N != other.N)
            {
                return false;
            }

            matrix = new Matrix(_data, true);

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
        /// Subtracts another matrix from this matrix. Both matrices must have the same dimensions.
        /// </summary>
        public bool TrySubtract(Matrix other, out Matrix matrix)
        {
            matrix = null;

            if (M != other.M || N != other.N)
            {
                return false;
            }

            matrix = new Matrix(_data, true);

            for (var m = 0; m < M; m++)
            {
                for (var n = 0; n < N; n++)
                {
                    matrix[m, n] -= other[m, n];
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
            {
                return false;
            }

            var result = new double[M, other.N];

            for (var m = 0; m < M; m++)
            {
                for (var n = 0; n < other.N; n++)
                {
                    for (var x = 0; x < N; x++)
                    {
                        result[m, n] += _data[m, x] * other._data[x, n];
                    }
                }
            }

            matrix = new Matrix(result, false);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TryAugment(Matrix other, out Matrix result)
        {
            if (M != other.M)
            {
                result = null;
                return false;
            }
            var temp = new double[M, N + other.N];
            Plane.Write(_data, 0, 0, temp, 0, 0, N, M);
            Plane.Write(other._data, 0, 0, temp, 0, N, other.N, M);
            result = new Matrix(temp, false);
            return true;
        }

        /// <summary>
        /// Tries to invert the matrix. Returns true if the inversion succeeds; otherwise false.
        /// The inverted Matrix is set in the out parameter.
        /// </summary>
        public bool TryInvert(out Matrix matrix)
        {
            matrix = null;
            if (M != N || Determinant == 0)
            {
                return false;
            }

            _ = TryAugment(Identity(M), out Matrix augmented);

            var temp = augmented._data;

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
                    {
                        continue;
                    }

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
        /// 
        /// </summary>
        public void Echelon()
        {
            if (N - M != 1)
            {
                // for input, we want a square matrix augmented with a single column
                throw new InvalidOperationException();
            }

            // Rules:
            // 1. swap rows
            // 2. multiply a row by a non-zero scalar
            // 3. add one row to a scalar multiple of another

            // TODO: swap rows if any of the diagonal values has a zero

            for (var i = 0; i < M - 1; i++)
            {
                for (var m = i + 1; m < M; m++)
                {
                    var scale = _data[m, i] / _data[i, i];
                    for (var n = 0; n < N; n++)
                    {
                        _data[m, n] -= _data[i, n] * scale;
                    }
                }
            }

            // matrix is now in echelon form (also called triangular form)
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Gaussian_elimination
        /// </summary>
        public void ReducedRowEchelon()
        {
            if (N - M != 1)
            {
                // for input, we want a square matrix augmented with a single column
                throw new InvalidOperationException();
            }

            if (!IsTriangular)
            {
                Echelon();
            }

            for (var i = M - 1; i > 0; i--)
            {
                for (var m = i - 1; m >= 0; m--)
                {
                    var scale = _data[m, i] / _data[i, i];
                    for (var n = 0; n < N; n++)
                    {
                        _data[m, n] -= _data[i, n] * scale;
                    }
                }
            }

            for (var i = 0; i < M; i++)
            {
                _data[i, N - 1] /= _data[i, i];
                _data[i, i] = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTriangular
        {
            get
            {
                for (var m = 1; m < M; m++)
                {
                    for (var n = 0; n < m; n++)
                    {
                        if (_data[m, n] != 0d)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Gets the real eigenvalues (eigenvalues with non-zero imaginary parts are ignored).
        /// </summary>
        public IEnumerable<double> Eigenvalues
        {
            get
            {
                if (M != N || M != 2)
                {
                    throw new NotImplementedException();
                }
                //λ2−λ(a+d)+(a×d)−(c×b)=0
                var b = 0 - _data[0, 0] - _data[1, 1];
                var c = _data[0, 0] * _data[1, 1] - _data[1, 0] * _data[0, 1];
                var d = b * b - 4 * c;
                if (System.Math.Sign(d) >= 0)
                {
                    var s = System.Math.Sqrt(d);
                    yield return (s - b) / 2;
                    yield return (0 - s - b) / 2;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                if (M != N)
                {
                    return false;
                }
                for (var m = 0; m < M; m++)
                {
                    for (var n = 0; n < N; n++)
                    {
                        if (_data[m, n] != ((m == n) ? 1 : 0))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Matrix> Eigenvectors(double eigenvalue)
        {
            if (M != N || M != 2)
            {
                throw new NotImplementedException();
            }

            var rhs = new Matrix(M, 1);
            if (TryAugment(rhs, out Matrix augmented))
            {
                augmented.Echelon();

                var zeroes = Enumerable.Range(0, augmented.M)
                    .Where(x => augmented[x, x] == 0d);

                // by definition, the multiplication of matrix and eigenvector must result in a single 
                // non-zero vector?

                if (zeroes.Any())
                {
                    // we don't have a unique solution because two or more of the rows were the same line

                    // TODO: this next bit is not correct for all sitations

                    var firstNonZero = Enumerable.Range(0, augmented.M)
                        .Where(x => augmented[x, x] != 0d)
                        .First();

                    if (augmented[firstNonZero, augmented.N - 1] != 0)
                    {
                        throw new InvalidOperationException();
                    }

                    var result = augmented.Submatrix(firstNonZero, 0, 1, augmented.N - 1);

                    if (result.N != 2)
                    {
                        throw new InvalidOperationException();
                    }

                    yield return new Matrix(new[] { result[0, 1], 0 - result[0, 0] }, 2, 1, true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the value at index M and N
        /// </summary>
        public double this[int m, int n]
        {
            get
            {
                return _data[m, n];
            }
            set
            {
                _data[m, n] = value;
            }
        }

        /// <summary>
        /// Gets a human readable representation of this matrix.
        /// </summary>
        public override string ToString()
        {
            //return ToString("\t", Environment.NewLine);
            return ToString(",", ";");
        }

        /// <summary>
        /// 
        /// </summary>
        public string ToString(string columnSeparator, string lineSeparator)
        {
            var result = new StringBuilder();
            for (var m = 0; m < M; m++)
            {
                if (m != 0)
                {
                    result.Append(lineSeparator);
                }
                for (var n = 0; n < N; n++)
                {
                    if (n != 0)
                    {
                        result.Append(columnSeparator);
                    }
                    result.Append(_data[m, n]);
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
            {
                return false;
            }
            return _data.Cast<double>().SequenceEqual(other._data.Cast<double>());
        }

        /// <summary>
        /// Override the Equals method
        /// </summary>
        public override bool Equals(object other)
        {
            if (other is Matrix otherMatrix)
            {
                return Equals(otherMatrix);
            }
            return false;
        }

        /// <summary>
        /// Override the GetHashCode method
        /// </summary>
        public override int GetHashCode()
        {
            return _data.Cast<double>().Aggregate(0, (x, y) => x ^ y.GetHashCode());
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (!left.TryAdd(right, out Matrix result))
            {
                throw new InvalidOperationException();
            }
            return result;
        }

        /// <summary>
        /// Subtracts one matrix from another
        /// </summary>
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (!left.TrySubtract(right, out Matrix result))
            {
                throw new InvalidOperationException();
            }
            return result;
        }

        /// <summary>
        /// Multiplies two matrices
        /// </summary>
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (!left.TryMultiply(right, out Matrix result))
            {
                throw new InvalidOperationException();
            }
            return result;
        }

        /// <summary>
        /// Multiplies a matrix with a scalar
        /// </summary>
        public static Matrix operator *(double left, Matrix right)
        {
            var result = new Matrix(right);
            for (var m = 0; m < result.M; m++)
            {
                for (var n = 0; n < result.N; n++)
                {
                    result[m, n] *= left;
                }
            }
            return result;
        }

        /// <summary>
        /// Multiplies a matrix with a scalar
        /// </summary>
        public static Matrix operator *(Matrix left, double right)
        {
            return right * left;
        }
    }
}
