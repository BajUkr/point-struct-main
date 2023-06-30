using System.Text.RegularExpressions;

namespace PointStruct
{
    /// <summary>
    /// Represents a point in the Cartesian coordinate system.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure with the specified <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x">An x-coordinate of this <see cref="Point"/> structure.</param>
        /// <param name="y">An y-coordinate of this <see cref="Point"/> structure.</param>
        public Point(long x, long y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> structure with the specified <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x">An x-coordinate of this <see cref="Point"/> structure.</param>
        /// <param name="y">An y-coordinate of this <see cref="Point"/> structure.</param>
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the x-coordinate of this <see cref="Point"/> structure.
        /// </summary>
        public long X { get; private set; }

        /// <summary>
        /// Gets the y-coordinate of this <see cref="Point"/> structure.
        /// </summary>
        public long Y { get; private set; }

        /// <summary>
        /// Compares the <paramref name="left"/> and <paramref name="right"/> objects. Returns true if the left <see cref="Point"/> is equal to the right <see cref="Point"/>; otherwise, false.
        /// </summary>
        /// <param name="left">A left <see cref="Point"/>.</param>
        /// <param name="right">A right <see cref="Point"/>.</param>
        /// <returns>true if the left <see cref="Point"/> is equal to the right <see cref="Point"/>; otherwise, false.</returns>
        public static bool operator ==(Point left, Point right) => left.Equals(right);

        /// <summary>
        /// Compares the <paramref name="left"/> and <paramref name="right"/> objects. Returns true if the left <see cref="Point"/> is not equal to the right <see cref="Point"/>; otherwise, false.
        /// </summary>
        /// <param name="left">A left <see cref="Point"/>.</param>
        /// <param name="right">A right <see cref="Point"/>.</param>
        /// <returns>true if the left <see cref="Point"/> is not equal to the right <see cref="Point"/>; otherwise, false.</returns>
        public static bool operator !=(Point left, Point right) => !(left == right);

        /// <summary>
        /// Converts the string representation of a point to its <see cref="Point"/> equivalent.
        /// </summary>
        /// <param name="pointString">A string containing a point to convert.</param>
        /// <returns>A <see cref="Point"/> equivalent to the point contained in <paramref name="pointString"/>.</returns>
        public static Point Parse(string pointString)
        {
            if (TryParse(pointString, out var point))
            {
                return point;
            }
            else
            {
                throw new ArgumentException($"Invalid input format.{pointString}", nameof(pointString));
            }
        }

        /// <summary>
        /// Converts the string representation of a point to its <see cref="Point"/> equivalent. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="pointString">A string containing a point to convert.</param>
        /// <param name="point">A <see cref="Point"/> equivalent to the point contained in <paramref name="pointString"/>.</param>
        /// <returns>true if <paramref name="rgbString"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string pointString, out Point point)
        {
            var pattern = @"^\s*(?<x>-?\d+)\s*,\s*(?<y>-?\d+)\s*$";
            var match = Regex.Match(pointString, pattern);

            if (match.Success)
            {
#pragma warning disable CA1305
                var x = long.Parse(match.Groups["x"].Value);
                var y = long.Parse(match.Groups["y"].Value);
                point = new Point(x, y);
                return true;
            }
            else
            {
                point = default;
                return false;
            }
        }

        /// <summary>
        /// Returns the number of points that have exact same coordinates as the <see cref="Point"/>.
        /// </summary>
        /// <param name="points">A collection of points.</param>
        /// <returns>The number of points that have exact same coordinates as the <see cref="Point"/>.</returns>
        public int CountPointsInExactSameLocation(IEnumerable<Point> points)
        {
            var count = 0;
            foreach (var point in points)
            {
                if (this == point)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Returns a string with points that are collinear to the <see cref="Point"/>.
        /// </summary>
        /// <param name="points">A collection of points.</param>
        /// <returns>A string with points that are collinear to the <see cref="Point"/>.</returns>
        public string GetCollinearPointCoordinates(IEnumerable<Point> points)
        {
            var collinearPoints = new List<string>();

            foreach (var point in points)
            {
                if (point == this)
                {
                    collinearPoints.Add($"({point.X},{point.Y},\"SAME\")");
                }
                else if (point.X == this.X)
                {
                    collinearPoints.Add($"({point.X},{point.Y},\"X\")");
                }
                else if (point.Y == this.Y)
                {
                    collinearPoints.Add($"({point.X},{point.Y},\"Y\")");
                }
            }

            return string.Join(",", collinearPoints);
        }

        /// <summary>
        /// Returns a collection of points that are distance-neighbors for the <see cref="Point"/>.
        /// </summary>
        /// <param name="distance">Distance around a given point.</param>
        /// <param name="points">A list of points.</param>
        /// <returns>A collection of points that are distance-neighbors.</returns>
        public ICollection<Point> GetNeighbors(int distance, IEnumerable<Point> points)
        {
            if (distance <= 0)
            {
                throw new ArgumentException("Distance must be greater than 0.", nameof(distance));
            }

            var neighbors = new List<Point>();

            foreach (var candidate in points)
            {
                if (this.IsNeighbor(distance, candidate))
                {
                    neighbors.Add(candidate);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="other">The <see cref="Point"/> to compare with the current <see cref="Point"/>.</param>
        /// <returns>true if the specified <see cref="Point"/> is equal to the current <see cref="Point"/>; otherwise, false.</returns>
        public bool Equals(Point other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Point"/> is equal to the current <see cref="Point"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="Point"/>.</param>
        /// <returns>true if the specified <see cref="Point"/> is equal to the current <see cref="Point"/>; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Point other && this.Equals(other);
        }

        /// <summary>
        /// Returns a string that represents the current <see cref="Point"/>.
        /// </summary>
        /// <returns>A string that represents the current <see cref="Point"/>.</returns>
        public override int GetHashCode()
        {
            var xHash = (int)(this.X ^ this.X >> 32);
            var yHash = (int)(this.Y ^ this.Y >> 32);
            return xHash ^ yHash;
        }

        /// <summary>
        /// Gets a hash code of the current <see cref="Point"/>.
        /// </summary>
        /// <returns>A hash code of the current <see cref="Point"/>.</returns>
        public override string ToString()
        {
            return $"{this.X},{this.Y}";
        }

        private bool IsNeighbor(long distance, Point candidate)
        {
            var deltaX = Math.Abs(candidate.X - this.X);
            var deltaY = Math.Abs(candidate.Y - this.Y);

            return deltaX <= distance && deltaY <= distance && (deltaX > 0 || deltaY > 0);
        }
    }
}
