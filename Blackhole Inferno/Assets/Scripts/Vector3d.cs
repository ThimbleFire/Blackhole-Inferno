public class Vector3d { 
    public decimal x, y, z;
    // M specifies type of decimal
    // eX specifies the 
	// Vector3d v = new Vector3d(2.24e13M, 1.24e11M, 1.16e10M);
	public Vector3d(decimal x, decimal y, decimal z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
    public decimal DistanceTo(Vector3d other) {
        decimal dx = x - other.x;
        decimal dy = y - other.y;
        decimal dz = z - other.z;
        return Sqrt(dx * dx + dy * dy + dz * dz);
    }
    public decimal DistanceToAU(Vector3d other) {
        decimal dx = x - other.x;
        decimal dy = y - other.y;
        decimal dz = z - other.z;
		decimal distance = Sqrt(dx * dx + dy * dy + dz * dz);
		decimal AU = distance / 149597870700;
        return AU;
    }
    private decimal Sqrt(decimal num, decimal precision = 0.00001M) {
        decimal sqrt = num / 2M;
        while (Abs(sqrt - num / sqrt) > precision) {
            sqrt = (sqrt + num / sqrt) / 2M;
        }
        return sqrt;
    }
    private decimal Abs(decimal value) {
        return value < 0 ? -value : value;
    }
}
