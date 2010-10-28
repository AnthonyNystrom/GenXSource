using System;

namespace Genetibase.MathX.Core
{

	public enum Lexeme
	{
		// constants
		[TypeOfLexeme(LexemeType.ltConstant),LexemeCode("Math.E")]
		E,
		[TypeOfLexeme(LexemeType.ltConstant),LexemeCode("Math.PI")]
		Pi,

		[TypeOfLexeme(LexemeType.ltConstant)]
		Number,   
		[TypeOfLexeme(LexemeType.ltConstant)]
		Var,

		[LexemeName("(")]
		LeftBracket, 
		[LexemeName(")")]
		RightBracket, 
		
		Unknown,
		End,

		[LexemeName("+"),TypeOfLexeme(LexemeType.ltOperator)]
		Plus, 
		[LexemeName("-"),TypeOfLexeme(LexemeType.ltOperator)]
		Minus, 

		[TypeOfLexeme(LexemeType.ltOperator)]
		UMinus, 

		[LexemeName("*"),TypeOfLexeme(LexemeType.ltOperator)]
		Multiply, 
		[LexemeName("/"),TypeOfLexeme(LexemeType.ltOperator)]
		Divide,
		[LexemeName("%"),TypeOfLexeme(LexemeType.ltOperator)]
		Percent,
		[LexemeName("^"),TypeOfLexeme(LexemeType.ltOperator)]
		Power,    

		// Elementary functions

		[LexemeName("abs"),LexemeDiffMask("({0})/abs({0})"),LexemeCode("Math.Abs"),TypeOfLexeme(LexemeType.ltFunction)]
		Abs,
		[LexemeName("sqrt"),LexemeDiffMask("1/(2*sqrt({0}))"),LexemeCode("Math.Sqrt"),TypeOfLexeme(LexemeType.ltFunction)]
		Sqrt,     
      
		// Exponential functions

		[LexemeName("exp"),LexemeDiffMask("exp({0})"),LexemeCode("Math.Exp"),TypeOfLexeme(LexemeType.ltFunction)]
		Exp,

		// Logarithmic functions

		[LexemeName("log"),LexemeDiffMask("1/({0})"),LexemeCode("Math.Log"),TypeOfLexeme(LexemeType.ltFunction)]
		Log,
		[LexemeName("log10"),LexemeDiffMask("1/(({0})*log(10))"),LexemeCode("Math.Log10"),TypeOfLexeme(LexemeType.ltFunction)]
		Log10,

		// Trigonometric functions

		[LexemeName("sin"),LexemeDiffMask("cos({0})"),LexemeCode("Math.Sin"),TypeOfLexeme(LexemeType.ltFunction)]
		Sin,
		[LexemeName("cos"),LexemeDiffMask("-sin({0})"),LexemeCode("Math.Cos"),TypeOfLexeme(LexemeType.ltFunction)]
		Cos,
		[LexemeName("tan"),LexemeDiffMask("sec({0})^2"),LexemeCode("Math.Tan"),TypeOfLexeme(LexemeType.ltFunction)]
		Tan,
		[LexemeName("cot"),LexemeDiffMask("-csc({0})^2"),LexemeCode("Functions.Trigonometric.Cot"),TypeOfLexeme(LexemeType.ltFunction)]
		Cot,
		[LexemeName("sec"),LexemeDiffMask("tan({0})*sec({0})"),LexemeCode("Functions.Trigonometric.Sec"),TypeOfLexeme(LexemeType.ltFunction)]
		Sec,
		[LexemeName("csc"),LexemeDiffMask("-cot({0})*csc({0})"),LexemeCode("Functions.Trigonometric.Csc"),TypeOfLexeme(LexemeType.ltFunction)]		
		Csc,

		[LexemeName("asin"),LexemeDiffMask("1/sqrt(1-({0})^2)"),LexemeCode("Math.Asin"),TypeOfLexeme(LexemeType.ltFunction)]
		Asin,
		[LexemeName("acos"),LexemeDiffMask("-1/sqrt(1-({0})^2)"),LexemeCode("Math.Acos"),TypeOfLexeme(LexemeType.ltFunction)]
		Acos,
		[LexemeName("atan"),LexemeDiffMask("1/(1+({0})^2)"),LexemeCode("Math.Atan"),TypeOfLexeme(LexemeType.ltFunction)]
		Atan,    
		[LexemeName("acot"),LexemeDiffMask("-1/(1+({0})^2)"),LexemeCode("Functions.Trigonometric.Acot"),TypeOfLexeme(LexemeType.ltFunction)]
		Acot,    
		[LexemeName("asec"),LexemeDiffMask("1/(abs({0})*sqrt(({0})^2 - 1))"),LexemeCode("Functions.Trigonometric.Asec"),TypeOfLexeme(LexemeType.ltFunction)]
		Asec,    
		[LexemeName("acsc"),LexemeDiffMask("-1/(abs({0})*sqrt(({0})^2 - 1))"),LexemeCode("Functions.Trigonometric.Acsc"),TypeOfLexeme(LexemeType.ltFunction)]
		Acsc,    

		// Hyperbolic functions

		[LexemeName("sinh"),LexemeDiffMask("cosh({0})"),LexemeCode("Math.Sinh"),TypeOfLexeme(LexemeType.ltFunction)]
		Sinh,
		[LexemeName("cosh"),LexemeDiffMask("sinh({0})"),LexemeCode("Math.Cosh"),TypeOfLexeme(LexemeType.ltFunction)]
		Cosh,
		[LexemeName("tanh"),LexemeDiffMask("sech({0})^2"),LexemeCode("Math.Tanh"),TypeOfLexeme(LexemeType.ltFunction)]
		Tanh,      
		[LexemeName("coth"),LexemeDiffMask("-csch({0})^2"),LexemeCode("Functions.Hyperbolic.Coth"),TypeOfLexeme(LexemeType.ltFunction)]
		Coth,      
		[LexemeName("sech"),LexemeDiffMask("-tanh({0})*sech({0})"),LexemeCode("Functions.Hyperbolic.Sech"),TypeOfLexeme(LexemeType.ltFunction)]
		Sech,
		[LexemeName("csch"),LexemeDiffMask("-coth({0})*csch({0})"),LexemeCode("Functions.Hyperbolic.Csch"),TypeOfLexeme(LexemeType.ltFunction)]
		Csch,

		[LexemeName("asinh"),LexemeDiffMask("1/sqrt(({0})^2 + 1)"),LexemeCode("Functions.Hyperbolic.Asinh"),TypeOfLexeme(LexemeType.ltFunction)]
		Asinh,
		[LexemeName("acosh"),LexemeDiffMask("-1/sqrt(({0})^2 - 1)"),LexemeCode("Functions.Hyperbolic.Acosh"),TypeOfLexeme(LexemeType.ltFunction)]
		Acosh,
		[LexemeName("atanh"),LexemeDiffMask("1/(1 - ({0})^2)"),LexemeCode("Functions.Hyperbolic.Atanh"),TypeOfLexeme(LexemeType.ltFunction)]
		Atanh,		
		[LexemeName("acoth"),LexemeDiffMask("1/(1 - ({0})^2)"),LexemeCode("Functions.Hyperbolic.Acoth"),TypeOfLexeme(LexemeType.ltFunction)]
		Acoth,		
		[LexemeName("asech"),LexemeDiffMask("1/(({0})*sqrt(1 - ({0})^2))"),LexemeCode("Functions.Hyperbolic.Asech"),TypeOfLexeme(LexemeType.ltFunction)]
		Asech,
		[LexemeName("acsch"),LexemeDiffMask("-1/(abs({0})*sqrt(1 + ({0})^2))"),LexemeCode("Functions.Hyperbolic.Acsch"),TypeOfLexeme(LexemeType.ltFunction)]		
		Acsch		
	}
}
