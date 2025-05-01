//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//  by introducing yourself and telling us what you do with this community.
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{
  /// <summary>
  ///  Two dimensions immutable vector
  /// </summary>
  internal record Vector : IVector
  {
    #region IVector

    /// <summary>
    /// The newX component of the vector.
    /// </summary>
    public double x { get; init; }
    /// <summary>
    /// The newY component of the vector.
    /// </summary>
    public double y { get; init; }

    #endregion IVector

    /// <summary>
    /// Creates new instance of <seealso cref="Vector"/> and initialize all properties
    /// </summary>
    public Vector(double XComponent, double YComponent) {
      x = XComponent;
      y = YComponent;
    }

    public static Vector operator +(Vector a, Vector b) => new Vector(a.x + b.x, a.y + b.y);
    public static Vector operator -(Vector a, Vector b) => new Vector(a.x - b.x, a.y - b.y);
        public static Vector operator *(Vector a, double b) => new Vector(a.x * b, a.y * b);
        public static Vector operator *(double b, Vector a) => new Vector(a.x * b, a.y * b);
        public double Dot(Vector other) => x * other.x + y * other.y;
        public double Length => Math.Sqrt(x * x + y * y);
        public Vector Normalize() {
            double length = Length;
            if (length == 0) {
                throw new InvalidOperationException("Cannot normalize a zero-length vector.");
            }
            return new Vector(x / length, y / length);
        }
    }
}