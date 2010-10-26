// NuGenPSurfaceParser.h

/***************************************************************************
 *   Copyright (C) 2005 by Warp                                            *
 *                                                                         *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA            *
 ***************************************************************************/
/***************************************************************************\
|* Function parser v2.7 by Warp                                            *|
|* ----------------------------                                            *|
|* Parses and evaluates the given function with the given variable values. *|
|*                                                                         *|
\***************************************************************************/

#ifndef ONCE_FPARSER_H_
#define ONCE_FPARSER_H_

#using <system.dll>

using namespace System;
using namespace System::Collections::Generic;

namespace Genetibase {
namespace NuGenPSurface
{
	namespace Parser 
    {
        // The functions must be in alphabetical order:
        enum class OPCODE : unsigned
        {
            cAbs, cAcos,

                cAsin,

                cAtan,
                cAtan2,

                cCeil, cCos, cCosh, cCot, cCsc,

                cEval,

                cExp, cFloor, cIf, cInt, cLog, cLog10, cMax, cMin,
                cSec, cSin, cSinh, cSqrt, cTan, cTanh,

                // These do not need any ordering:
                cImmed, cJump,
                cNeg, cAdd, cSub, cMul, cDiv, cMod, cPow,
                cEqual, cLess, cGreater, cAnd, cOr,

                cDeg, cRad,

                cFCall, cPCall,


                cVar, cDup, cInv,

                VarBegin
                };

        public delegate double FunctionPtr(array<double>^);

        public ref class FunctionParser
        {
        public:
            enum class ParseErrorType
            {
                SYNTAX_ERROR=0, MISM_PARENTH, MISSING_PARENTH, EMPTY_PARENTH,
                    EXPECT_OPERATOR, OUT_OF_MEMORY, UNEXPECTED_ERROR, INVALID_VARS,
                    ILL_PARAMS_AMOUNT, PREMATURE_EOS, EXPECT_PARENTH_FUNC,
                    FP_NO_ERROR
                    };

            int Parse(String^ Function, String^ Vars, bool useDegrees);
            String^ ErrorMsg();
            inline ParseErrorType GetParseErrorType()  { return parseErrorType; }

            double Eval(array<double>^ Vars);
            inline int EvalError()  { return evalErrorType; }

            bool AddConstant(String^ name, double value);

            bool AddFunction(String^ name, FunctionPtr^ , unsigned paramsAmount);
            bool AddFunction(String^ name, FunctionParser^);

            FunctionParser();
            ~FunctionParser();

            // Copy constructor and assignment operator (implemented using the
            // copy-on-write technique for efficiency):
            FunctionParser(const FunctionParser^);
            FunctionParser^ operator=(const FunctionParser^);

        private:

            // Private data:
            // ------------
            ParseErrorType parseErrorType;
            int evalErrorType;

            static array<String^>^ ParseErrorMessage = {
                "Syntax error",                             // 0
                "Mismatched parenthesis",                   // 1
                "Missing ')'",                              // 2
                "Empty parentheses",                        // 3
                "Syntax error: Operator expected",          // 4
                "Not enough memory",                        // 5
                "An unexpected error ocurred. Please make a full bug report "
                "to warp@iki.fi",                           // 6
                "Syntax error in parameter 'Vars' given to "
                "FunctionParser::Parse()",                  // 7
                "Illegal number of parameters to function", // 8
                "Syntax error: Premature end of string",    // 9
                "Syntax error: Expecting ( after function", // 10
                ""
            };
    
            ref struct Data
            {
                unsigned referenceCounter;

                int varAmount;
                bool useDegreeConversion;

                Dictionary<String^, unsigned>^ Variables; 
                Dictionary<String^, double>^   Constants;
                Dictionary<String^, unsigned>^ FuncPtrNames;

                ref struct FuncPtrData
                {
                    FunctionPtr^ ptr; unsigned params;
                    FuncPtrData(FunctionPtr^ p, unsigned par): ptr(p), params(par) {}
                };

                List<FuncPtrData^>^ FuncPtrs;

                Dictionary<String^, unsigned> ^ FuncParserNames;
                List<FunctionParser^>^ FuncParsers;

                array<unsigned>^ ByteCode;
                unsigned ByteCodeSize;

                array<double>^ Immed;
                unsigned ImmedSize;
        
                array<double>^ Stack;
                unsigned StackSize;

                Data();
                ~Data();
                Data(const Data^);
            };

            Data^ data;

            // Temp data needed in Compile():
            unsigned StackPtr;
            List<unsigned>^ tempByteCode;
            List<double>^ tempImmed;

            inline void copyOnWrite();

            bool checkRecursiveLinking(FunctionParser^);

            bool isValidName(String^) ;
            String^ FindVariable(String^ , Dictionary<String^, unsigned>^) ;
            String^ FindConstant(String^) ;
            int CheckSyntax(String^ );
            bool Compile(String ^);

            void AddCompiledByte(unsigned);
            void AddImmediate(double);
            void AddFunctionOpcode(OPCODE);
            inline void incStackPtr();
            int CompileIf(String^, int);
            int CompileFunctionParams(String^, int, unsigned);
            int CompileElement(String^, int);
            int CompilePow(String^, int);
            int CompileUnaryMinus(String^, int);
            int CompileMult(String^, int);
            int CompileAddition(String^, int);
            int CompileComparison(String^, int);
            int CompileAnd(String^, int);
            int CompileOr(String^, int);
            int CompileExpression(String^, int, bool);
        };
    }
}
}
#endif
