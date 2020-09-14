using System.Diagnostics.CodeAnalysis;

public class Position :  System.IEquatable<Position>

{
    public int X { get; set; }
    public int Y { get; set; }

    public bool Equals([AllowNull] Position other)
    {
        return this.X.Equals(other.X) && this.Y.Equals(other.Y);
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }
}
