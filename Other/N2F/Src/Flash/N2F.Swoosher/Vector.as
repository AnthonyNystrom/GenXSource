package
{
	public class Vector
	{
		public var elements : Array;
	
		public function Vector()
		{
		}

		public function e( i )
		{
			return (i < 1 || i > this.elements.length) ? null : this.elements[i-1];
		}

		  // Returns the number of elements the vector has
		public function dimensions()
		{
			return this.elements.length;
		}

		public function modulus()
		{
			return Math.sqrt(this.dot(this));
		}

		// Returns true iff the vector is equal to the argument
		public function eql(vector)
		{
			var n = this.elements.length;
			var V = vector.elements || vector;
			if (n != V.length) { return false; }
			do {
			  if (Math.abs(this.elements[n-1] - V[n-1]) > Sylvester.precision) { return false; }
			} while (--n);
			return true;
		}

		// Returns a copy of the vector
		public function dup()
		{
			return Vector.create(this.elements);
		}

		// Maps the vector to another vector according to the given function
		public function map(fn)
		{
			var elements = [];
			this.each(function(x, i) {
			  elements.push(fn(x, i));
			});
			return Vector.create(elements);
		}

		// Calls the iterator for each element of the vector in turn
		public function each(fn)
		{
			var n = this.elements.length, k = n, i;
			do { i = k - n;
			  fn(this.elements[i], i+1);
			} while (--n);
		}

		// Returns a new vector created by normalizing the receiver
		public function toUnitVector()
		{
			var r = this.modulus();
			if (r === 0) { return this.dup(); }
			return this.map(function(x) { return x/r; });
		}

		// Returns the angle between the vector and the argument (also a vector)
		public function angleFrom(vector)
		{
			var V = vector.elements || vector;
			var n = this.elements.length, k = n, i;
			if (n != V.length) { return null; }
			var dot = 0, mod1 = 0, mod2 = 0;
			// Work things out in parallel to save time
			this.each(function(x, i) {
			  dot += x * V[i-1];
			  mod1 += x * x;
			  mod2 += V[i-1] * V[i-1];
			});
			mod1 = Math.sqrt(mod1); mod2 = Math.sqrt(mod2);
			if (mod1*mod2 === 0) { return null; }
			var theta = dot / (mod1*mod2);
			if (theta < -1) { theta = -1; }
			if (theta > 1) { theta = 1; }
			return Math.acos(theta);
		}

		// Returns true iff the vector is parallel to the argument
		public function isParallelTo(vector)
		{
			var angle = this.angleFrom(vector);
			return (angle === null) ? null : (angle <= Sylvester.precision);
		}

		// Returns true iff the vector is antiparallel to the argument
		public function isAntiparallelTo(vector)
		{
			var angle = this.angleFrom(vector);
			return (angle === null) ? null : (Math.abs(angle - Math.PI) <= Sylvester.precision);
		}

		// Returns true iff the vector is perpendicular to the argument
		public function isPerpendicularTo(vector)
		{
			var dot = this.dot(vector);
			return (dot === null) ? null : (Math.abs(dot) <= Sylvester.precision);
		}

		// Returns the result of adding the argument to the vector
		public function add(vector)
		{
			var V = vector.elements || vector;
			if (this.elements.length != V.length) { return null; }
			return this.map(function(x, i) { return x + V[i-1]; });
		}

		// Returns the result of subtracting the argument from the vector
		public function subtract(vector)
		{
			var V = vector.elements || vector;
			if (this.elements.length != V.length) { return null; }
			return this.map(function(x, i) { return x - V[i-1]; });
		}

		// Returns the result of multiplying the elements of the vector by the argument
		public function multiply(k)
		{
			return this.map(function(x) { return x*k; });
		}

		// Returns the result of multiplying the elements of the vector by the argument
		public function divide(k)
		{
			return this.map(function(x) { return x/k; });
		}

		public function x(k)
		{
			return this.multiply(k);
		}

		// Returns the scalar product of the vector with the argument
		// Both vectors must have equal dimensionality
		public function dot(vector)
		{
			var V = vector.elements || vector;
			var i, product = 0, n = this.elements.length;
			if (n != V.length) { return null; }
			do { product += this.elements[n-1] * V[n-1]; } while (--n);
			return product;
		}

		  // Returns the vector product of the vector with the argument
		  // Both vectors must have dimensionality 3
		public function cross(vector)
		{
			var B = vector.elements || vector;
			if (this.elements.length != 3 || B.length != 3) { return null; }
			var A = this.elements;
			return Vector.create([
			  (A[1] * B[2]) - (A[2] * B[1]),
			  (A[2] * B[0]) - (A[0] * B[2]),
			  (A[0] * B[1]) - (A[1] * B[0])
			]);
		}

		// Returns the (absolute) largest element of the vector
		public function max()
		{
			var m = 0, n = this.elements.length, k = n, i;
			do { i = k - n;
			  if (Math.abs(this.elements[i]) > Math.abs(m)) { m = this.elements[i]; }
			} while (--n);
			return m;
		}

		// Returns the index of the first match found
		public function indexOf(x)
		{
			var index = null, n = this.elements.length, k = n, i;
			do { i = k - n;
			  if (index === null && this.elements[i] == x) {
				index = i + 1;
			  }
			} while (--n);
			return index;
		}

		// Returns a diagonal matrix with the vector's elements as its diagonal elements
		public function toDiagonalMatrix()
		{
			return Matrix.diagonal(this.elements);
		}

		// Returns the result of rounding the elements of the vector
		public function round()
		{
			return this.map(function(x) { return Math.round(x); });
		}

		// Returns a copy of the vector with elements set to the given value if they
		// differ from it by less than Sylvester.precision
		public function snapTo(x)
		{
			return this.map(function(y) {
			  return (Math.abs(y - x) <= Sylvester.precision) ? x : y;
			});
		}

		// Returns the vector's distance from the argument, when considered as a point in space
		public function distanceFrom(obj)
		{
			if (obj.anchor) { return obj.distanceFrom(this); }
			var V = obj.elements || obj;
			if (V.length != this.elements.length) { return null; }
			var sum = 0, part;
			this.each(function(x, i) {
			  part = x - V[i-1];
			  sum += part * part;
			});
			return Math.sqrt(sum);
		}

		// Returns true if the vector is point on the given line
		public function liesOn(line)
		{
			return line.contains(this);
		}

		// Return true iff the vector is a point in the given plane
		public function liesIn(plane)
		{
			return plane.contains(this);
		}

		// Rotates the vector about the given object. The object should be a 
		// point if the vector is 2D, and a line if it is 3D. Be careful with line directions!
		public function rotate(t, obj)
		{
			var V, R, x, y, z;
			switch (this.elements.length) {
			  case 2:
				V = obj.elements || obj;
				if (V.length != 2) { return null; }
				R = Matrix.rotation(t).elements;
				x = this.elements[0] - V[0];
				y = this.elements[1] - V[1];
				return Vector.create([
				  V[0] + R[0][0] * x + R[0][1] * y,
				  V[1] + R[1][0] * x + R[1][1] * y
				]);
				break;
			  case 3:
				if (!obj.direction) { return null; }
				var C = obj.pointClosestTo(this).elements;
				R = Matrix.rotation(t, obj.direction).elements;
				x = this.elements[0] - C[0];
				y = this.elements[1] - C[1];
				z = this.elements[2] - C[2];
				return Vector.create([
				  C[0] + R[0][0] * x + R[0][1] * y + R[0][2] * z,
				  C[1] + R[1][0] * x + R[1][1] * y + R[1][2] * z,
				  C[2] + R[2][0] * x + R[2][1] * y + R[2][2] * z
				]);
				break;
			  default:
				return null;
			}
		}

		// Returns the result of reflecting the point in the given point, line or plane
		public function reflectionIn(obj)
		{
			if (obj.anchor) {
			  // obj is a plane or line
			  var P = this.elements.slice();
			  var C = obj.pointClosestTo(P).elements;
			  return Vector.create([C[0] + (C[0] - P[0]), C[1] + (C[1] - P[1]), C[2] + (C[2] - (P[2] || 0))]);
			} else {
			  // obj is a point
			  var Q = obj.elements || obj;
			  if (this.elements.length != Q.length) { return null; }
			  return this.map(function(x, i) { return Q[i-1] + (Q[i-1] - x); });
			}
		}

		// Utility to make sure vectors are 3D. If they are 2D, a zero z-component is added
		public function to3D()
		{
			var V = this.dup();
			switch (V.elements.length) {
			  case 3: break;
			  case 2: V.elements.push(0); break;
			  default: return null;
			}
			return V;
		}

		// Returns a string representation of the vector
		public function inspect()
		{
			return '[' + this.elements.join(', ') + ']';
		}

		// Set vector's elements from an array
		public function setElements(els)
		{
			this.elements = (els.elements || els).slice();
			return this;
		}

		public function length()
		{
			return Math.sqrt( ( ( this.elements[0] * this.elements[0] ) + ( this.elements[1] * this.elements[1] ) ) + ( this.elements[2] * this.elements[2] ) );
		}

		// Constructor function
		public static function create(elements)
		{
			var V = new Vector();
			return V.setElements(elements);
		};

		// i, j, k unit vectors
		public static var I : Vector = Vector.create([1,0,0]);
		public static var J : Vector = Vector.create([0,1,0]);
		public static var K : Vector = Vector.create([0,0,1]);
		
		// Random vector of size n
		public static function random(n)
		{
			var elements = [];
			do { elements.push(Math.random());
			} while (--n);
			return Vector.create(elements);
		}

		// Vector filled with zeros
		public static function zero(n)
		{
			var elements = [];
			do { elements.push(0);
			} while (--n);
			return Vector.create(elements);
		}

		public static function AngleBetween( vector1, vector2 )
		{
			var num;
			vector1 = vector1.toUnitVector();
			vector2 = vector2.toUnitVector();
			if ( vector1.dot( vector2 ) < 0 )
			{
				var vectord2 = Vector.create( [ 0, 0, 0 ] ).subtract( vector1 ).subtract( vector2 );
				num = 3.1415926535897931 - ( 2 * Math.asin( vectord2.modulus() / 2 ) );
			}
			else
			{
				var vectord = vector1.subtract( vector2 );
				num = 2 * Math.asin( vectord.modulus() / 2 );
			}
			return Quaternion.RadiansToDegrees(num);
		}
	}
}