package
{
	public class CPlane
	{
		var n : Vector;
		var d : Number;

		public function CPlane()
		{
		}

		public function setPointNormal( point, normal )
		{
			this.setNormalDistance( normal, -normal.dot( point ) );
		}

		public function setNormalDistance( normal, d )
		{
			this.n = normal;
			this.d = d;
		}

		public function distance( point )
		{
			return this.n.elements[0] * point.elements[0] + this.n.elements[1] * point.elements[1] + this.n.elements[2] * point.elements[2] + this.d;
		}

		public function normalize()
		{
			var mag = Math.sqrt( this.n.elements[0] * this.n.elements[0] + this.n.elements[1] * this.n.elements[1] + this.n.elements[2] * this.n.elements[2] );
			return CPlane.create( this.n.divide( mag ), this.d / mag );
		}

		public static function create( normal, d )
		{
			var p = new CPlane();
			p.setNormalDistance( normal, d );
			return p;
		}

		public static function createAnchor( point, normal )
		{
			var p = new CPlane();
			p.setPointNormal( point, normal );
			return p;
		}
	}
}