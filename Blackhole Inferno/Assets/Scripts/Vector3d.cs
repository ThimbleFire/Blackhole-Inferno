public class Vector3d {
    public decimal x, y, z;
    public decimal DistanceTo(Vector3d other) {
    decimal dx = x - other.x;
    decimal dy = y - other.y;
    decimal dz = z - other.z;
    return (decimal)Sqrt(dx * dx + dy * dy + dz * dz);
    }

    private decimal Sqrt(decimal number, decimal precision = 0.000001M) {
        if (number < 0)
            throw new ArgumentException("Number must be non-negative");

        decimal sqrt = number / 2M;

        while (Abs(sqrt - number / sqrt) > precision)
        {
            sqrt = (sqrt + number / sqrt) / 2M;
        }

        return sqrt;
    }
    public static decimal Abs(decimal value) {
        if (value < 0)
        {
            return -value;
        }
        else
        {
            return value;
        }
    }
}