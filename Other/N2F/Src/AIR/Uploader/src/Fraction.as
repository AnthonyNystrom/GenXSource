package {
	public class Fraction {
		private var numerator:int = 0;
		private var denominator:int = 0;
		
		public function Fraction( numerator:int = 0, denominator:int = 0 ) {
			setNumerator( numerator );
			setDenominator( denominator );	
		}
		
		public function setNumerator( numerator:int ):void {
			this.numerator = numerator;	
		}
		
		public function getNumerator():int {
			return numerator;	
		}
		
		public function setDenominator( denominator:int ):void {
			this.denominator = denominator;	
		}
		
		public function getDenominator():int {
			return denominator;	
		}		
	}
}