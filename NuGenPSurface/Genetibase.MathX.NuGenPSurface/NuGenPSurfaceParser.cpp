// This is the main DLL file.

#include "NuGenPSurfaceParser.h"

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
//==============================
// Function parser v2.7 by Warp
//==============================

namespace Genetibase {
namespace NuGenPSurface
{
	namespace Parser 
    {
        ref struct FuncDefinition
        {
            String ^name;
            OPCODE opcode;
            unsigned params;

            FuncDefinition(String^ n, OPCODE opc, unsigned parms)
            {
                name = n;
                opcode = opc;
                params = parms;
            }

            static SortedList<String^, FuncDefinition^> ^Functions;

            static FuncDefinition()
            {
                Functions = gcnew SortedList<String^, FuncDefinition^>;

                Functions->Add("abs", gcnew FuncDefinition("abs", OPCODE::cAbs, 1));
                Functions->Add("acos", gcnew FuncDefinition("acos", OPCODE::cAcos, 1));

                Functions->Add("asin", gcnew FuncDefinition("asin", OPCODE::cAsin, 1));

                Functions->Add("atan", gcnew FuncDefinition("atan", OPCODE::cAtan, 1 ));
                Functions->Add("atan2", gcnew FuncDefinition("atan2", OPCODE::cAtan2, 2 ));

                Functions->Add("ceil", gcnew FuncDefinition("ceil", OPCODE::cCeil, 1 ));
                Functions->Add("cos", gcnew FuncDefinition("cos", OPCODE::cCos, 1 ));
                Functions->Add("cosh", gcnew FuncDefinition("cosh", OPCODE::cCosh, 1 ));
                Functions->Add("cot", gcnew FuncDefinition("cot", OPCODE::cCot, 1 ));
                Functions->Add("csc", gcnew FuncDefinition("csc", OPCODE::cCsc, 1 ));

                Functions->Add("eval", gcnew FuncDefinition("eval", OPCODE::cEval, 0 ));

                Functions->Add("exp", gcnew FuncDefinition("exp", OPCODE::cExp, 1 ));
                Functions->Add("floor", gcnew FuncDefinition("floor", OPCODE::cFloor, 1 ));
                Functions->Add("if", gcnew FuncDefinition("if", OPCODE::cIf, 0 ));
                Functions->Add("int", gcnew FuncDefinition("int", OPCODE::cInt, 1 ));
                Functions->Add("log", gcnew FuncDefinition("log", OPCODE::cLog, 1 ));
                Functions->Add("log10", gcnew FuncDefinition("log10", OPCODE::cLog10, 1 ));
                Functions->Add("max", gcnew FuncDefinition("max", OPCODE::cMax, 2 ));
                Functions->Add("min", gcnew FuncDefinition("min", OPCODE::cMin, 2 ));
                Functions->Add("sec", gcnew FuncDefinition("sec", OPCODE::cSec, 1 ));
                Functions->Add("sin", gcnew FuncDefinition("sin", OPCODE::cSin, 1 ));
                Functions->Add("sinh", gcnew FuncDefinition("sinh", OPCODE::cSinh, 1 ));
                Functions->Add("sqrt", gcnew FuncDefinition("sqrt", OPCODE::cSqrt, 1 ));
                Functions->Add("tan", gcnew FuncDefinition("tan", OPCODE::cTan, 1 ));
                Functions->Add("tanh", gcnew FuncDefinition("tanh", OPCODE::cTanh, 1));
            }

            public:
            static FuncDefinition^ FindFunction (String ^f)
            {
				unsigned ind = 0;
                while (ind < (unsigned int)f->Length && (Char::IsLetterOrDigit(f[ind])))
                    ++ind;

				f = f->Substring(0, ind);

                if (Functions->ContainsKey (f))
                    return Functions[f];

                return nullptr; 
            }
        };

            // Parse variables
    bool ParseVars(String^ Vars, Dictionary<String^, unsigned>^ dest)
    {
        unsigned varNumber = (unsigned) OPCODE::VarBegin;
        int ind1 = 0, ind2;

        while(ind1 < Vars->Length)
        {
            if(!Char::IsLetter(Vars[ind1]) && Vars[ind1]!='_') 
                return false;

            for(ind2=ind1+1; ind2 < Vars->Length && Vars[ind2]!=','; ++ind2)
                if(!Char::IsLetterOrDigit(Vars[ind2]) && Vars[ind2]!='_') 
                    return false;

            String^ varName = Vars->Substring(ind1, ind2-ind1);

            if (dest->ContainsKey (varName))
                return false;

//            System::Console::WriteLine("{0} : {1}", varName, varNumber);
            dest[varName]  = varNumber++; 
                
            ind1 = ind2+1;
        }
        
        return true;
    }

        // Is given char an operator?
    inline bool IsOperator(int c)
    {
        switch (c)
        {
        case '+':
        case '-':
        case '*':
        case '/':
        case '%':
        case '^':
        case '=':
        case '<':
        case '>':
        case '&':
        case '|':
        case '"':
            return true;

        default:
            return false; 
        }
    }

    // skip whitespace
    inline void sws(String^ F, int& Ind)
    {
        while (Ind < F->Length && Char::IsWhiteSpace(F[Ind]))
            ++Ind;
    }

    inline int doubleToInt(double d)
    {
        return d<0 ? -int((-d)+.5) : int(d+.5);
    }

    inline double Min(double d1, double d2)
    {
        return d1<d2 ? d1 : d2;
    }
    inline double Max(double d1, double d2)
    {
        return d1>d2 ? d1 : d2;
    }


    inline double DegreesToRadians(double degrees)
    {
        return degrees*(Math::PI / 180.0);
    }
    inline double RadiansToDegrees(double radians)
    {
        return radians*(180.0/Math::PI);
    }

        inline void FunctionParser::copyOnWrite()
        {
            if(data->referenceCounter > 1)
            {
                Data^ oldData = data;
                data = gcnew Data(oldData);
                --(oldData->referenceCounter);
                data->referenceCounter = 1;
            }
        }

        FunctionParser::FunctionParser():
            parseErrorType(FunctionParser::ParseErrorType::FP_NO_ERROR), evalErrorType(0),
            data(gcnew Data)
        {
            data->referenceCounter = 1;
        }

        FunctionParser::~FunctionParser()
        {
            if(--(data->referenceCounter) == 0)
            {
                delete data;
            }
        }

        FunctionParser::FunctionParser(const FunctionParser^ cpy):
            parseErrorType(cpy->parseErrorType),
            evalErrorType(cpy->evalErrorType),
            data(cpy->data)
        {
            ++(data->referenceCounter);
        }

        FunctionParser^ FunctionParser::operator=(const FunctionParser^ cpy)
        {
            if(data != cpy->data)
            {
                if(--(data->referenceCounter) == 0) delete data;

                parseErrorType = cpy->parseErrorType;
                evalErrorType = cpy->evalErrorType;
                data = cpy->data;

                ++(data->referenceCounter);
            }

            return this;
        }


        FunctionParser::Data::Data():
            useDegreeConversion(false),
            ByteCode(nullptr), ByteCodeSize(0),
            Immed(nullptr), ImmedSize(0),
            Stack(nullptr), StackSize(0)
        {
            Variables = gcnew Dictionary<String^, unsigned>;
            Constants = gcnew Dictionary<String^, double>;
            FuncPtrNames = gcnew Dictionary<String^, unsigned>;

            FuncPtrs = gcnew List<FuncPtrData^>;
            FuncParserNames = gcnew Dictionary<String^, unsigned>;
            FuncParsers = gcnew List<FunctionParser^>; 
        }

        FunctionParser::Data::~Data()
        {
        }

        FunctionParser::Data::Data(const Data^ cpy):
            varAmount(cpy->varAmount), useDegreeConversion(cpy->useDegreeConversion),
            Variables(cpy->Variables), Constants(cpy->Constants),
            FuncPtrNames(cpy->FuncPtrNames), FuncPtrs(cpy->FuncPtrs),
            FuncParserNames(cpy->FuncParserNames), FuncParsers(cpy->FuncParsers),
            ByteCode(nullptr), ByteCodeSize(cpy->ByteCodeSize),
            Immed(nullptr), ImmedSize(cpy->ImmedSize),
            Stack(nullptr), StackSize(cpy->StackSize)
        {
            if(ByteCodeSize)
                ByteCode = (array<unsigned>^) cpy->ByteCode->Clone (); 

            if(ImmedSize)
                Immed = (array<double>^) cpy->Immed->Clone ();

            if(StackSize)
                Stack = gcnew array<double> (StackSize);
        }

        bool FunctionParser::isValidName(String^ name) 
        {
			if (name == nullptr || name->Length < 1 || 
				( (! Char::IsLetter(name[0])) && name[0] != '_'))
			{
				return false;
			}

            for (int i=0; i<name->Length; ++i)
                if (! Char::IsLetterOrDigit(name[i]) && name[i] != '_') 
                    return false;

            if (FuncDefinition::FindFunction(name))
                return false;

            return true;
        }


        // Constants:
        bool FunctionParser::AddConstant(String^ name, double value)
        {
            if(isValidName(name))
            {
                if(FindVariable(name, data->FuncParserNames) != nullptr ||
                   FindVariable(name, data->FuncPtrNames) != nullptr)
                {
                    return false;
                }

                copyOnWrite();

                data->Constants[name] = value;
                return true;
            }
            return false;
        }

        // Function pointers
        bool FunctionParser::AddFunction(String^ name,
                                         FunctionPtr^ func, unsigned paramsAmount)
        {
            if(paramsAmount == 0) return false; // Currently must be at least one

            if(isValidName(name))
            {
                if(FindVariable(name, data->FuncParserNames) != nullptr ||
                   FindConstant(name) != nullptr)
                {
                    return false;
                }

                copyOnWrite();

                data->FuncPtrNames[name] = data->FuncPtrs->Count;
                data->FuncPtrs->Add(gcnew Data::FuncPtrData(func, paramsAmount));
                return true;
            }
            return false;
        }

        bool FunctionParser::checkRecursiveLinking(FunctionParser^ fp)
        {
            if(fp == this) 
                return true;

            for(int i=0; i < fp->data->FuncParsers->Count; ++i)
                if(checkRecursiveLinking(fp->data->FuncParsers[i])) 
                    return true;
            return false;
        }

        bool FunctionParser::AddFunction(String^ name, FunctionParser^ parser)
        {
            if(parser->data->varAmount == 0) // Currently must be at least one
                return false;

            if(isValidName(name))
            {
                if (FindVariable(name, data->FuncPtrNames) != nullptr ||
                    FindConstant(name) != nullptr)
                {
                    return false;
                }

                if(checkRecursiveLinking(parser)) 
                    return false;

                copyOnWrite();

                data->FuncParserNames[name] = data->FuncParsers->Count;
                data->FuncParsers->Add(parser);
                return true;
            }
            return false;
        }



        // Main parsing function
        // ---------------------
        int FunctionParser::Parse(String^ Function, String^ Vars, bool useDegrees)
        {
            copyOnWrite();

            data->Variables->Clear();

            if(!ParseVars(Vars, data->Variables))
            {
                parseErrorType = ParseErrorType::INVALID_VARS;
                return Function->Length;
            }
            data->varAmount = data->Variables->Count; // this is for Eval()

            parseErrorType = ParseErrorType::FP_NO_ERROR;

            int Result = CheckSyntax(Function);
            if(Result>=0) return Result;

            data->useDegreeConversion = useDegrees;
            if(!Compile(Function))
                return Function->Length;

            data->Variables->Clear();

            parseErrorType = ParseErrorType::FP_NO_ERROR;
            return -1;
        }

        String^ FunctionParser::FindVariable(String^ F, Dictionary<String^, unsigned>^ vars) 
        {
            if(vars->Count > 0 )
            {
                unsigned ind = 0;
                while (ind < (unsigned int)F->Length && (Char::IsLetterOrDigit(F[ind]) || F[ind] == '_')) 
                    ++ind;

                if(ind)
                {
                    String^ name = F->Substring(0, ind);
                    if (! vars->ContainsKey (name))
                        return nullptr; 

                    return name;
                }
            }

            return nullptr;
        }

        String^ FunctionParser::FindConstant(String^ F)
        {
            if(data->Constants->Count > 0)
            {
                unsigned ind = 0;
                while(ind < (unsigned int)F->Length && (Char::IsLetterOrDigit(F[ind]) || F[ind] == '_'))
                    ++ind;

                if(ind)
                {
                    String ^s = F->Substring(0, ind);
                    if (! data->Constants->ContainsKey (s))
                        return nullptr; 

                    return s; 
                }
            }

            return nullptr;
        }

        //---------------------------------------------------------------------------
        // Check function string syntax
        // ----------------------------
        int FunctionParser::CheckSyntax(String^ Function)
        {
            //    const Data::VarMap_t& Variables = data->Variables;
            //    const Data::ConstMap_t& Constants = data->Constants;
            //    const Data::VarMap_t& FuncPtrNames = data->FuncPtrNames;
            //    const Data::VarMap_t& FuncParserNames = data->FuncParserNames;

            List<int>^ functionParenthDepth = gcnew List<int>;

            int Ind=0, ParenthCnt=0;
            //    char* Ptr;

try
{
            while(true)
            {
                sws(Function, Ind);
                
                if(Ind >= Function->Length) { parseErrorType=ParseErrorType::PREMATURE_EOS; return Ind; }

                int c = Function[Ind];

                // Check for valid operand (must appear)

                // Check for leading -
                if(c=='-') { sws(Function, ++Ind); c=Function[Ind]; }

                // Check for math function
                bool foundFunc = false;
                FuncDefinition^ fptr = FuncDefinition::FindFunction(Function->Substring (Ind));

                if(fptr != nullptr)
                {
                    Ind += fptr->name->Length;
                    foundFunc = true;
                }
                else
                {
                    // Check for user-defined function
                    String^ fIter = FindVariable(Function->Substring (Ind), data->FuncPtrNames);
                    if(fIter != nullptr)
                    {
                        Ind += fIter->Length;
                        foundFunc = true;
                    }
                    else
                    {
                        String ^pIter = FindVariable(Function->Substring (Ind), data->FuncParserNames);
                        if(pIter != nullptr)
                        {
                            Ind += pIter->Length;
                            foundFunc = true;
                        }
                    }
                }

                if(foundFunc)
                {
                    sws(Function, Ind);
                    c = Function[Ind];
                    if(c!='(') { parseErrorType=ParseErrorType::EXPECT_PARENTH_FUNC; return Ind; }
                    functionParenthDepth->Add(ParenthCnt+1);
                }

                // Check for opening parenthesis
                if(c=='(')
                {
                    ++ParenthCnt;
                    sws(Function, ++Ind);
                    if(Function[Ind]==')') { parseErrorType=ParseErrorType::EMPTY_PARENTH; return Ind;}
                    continue;
                }

                // Check for number
                if(Char::IsDigit(c) || (c=='.' && Char::IsDigit(Function[Ind+1])))
                {
                    // skip digits
                    while (Ind < Function->Length && Char::IsDigit (Function[Ind]))
                        Ind++;

                    // check for fraction
                    if (Ind < Function->Length - 1 && Function[Ind] == '.')
                    {
                        Ind ++; 
                        while (Ind < Function->Length && Char::IsDigit (Function[Ind]))
                            Ind++;
                    }

                    sws(Function, Ind);
                    c = Function[Ind];
                }
                else
                {
                    // Check for variable
                    String^ vIter = FindVariable(Function->Substring (Ind), data->Variables);
                    if(vIter != nullptr)
                        Ind += vIter->Length;
                    else
                    {
                        // Check for constant
                        String^ cIter = FindConstant(Function->Substring (Ind));
                        if(cIter != nullptr)
                            Ind += cIter->Length;
                        else
                        {
                            parseErrorType = ParseErrorType::SYNTAX_ERROR;
                            return Ind;
                        }
                    }
                    sws(Function, Ind);
                    c = Function[Ind];
                }

                // Check for closing parenthesis
                while(c==')')
                {
                    if (functionParenthDepth->Count > 0 &&
                        functionParenthDepth[functionParenthDepth->Count-1] == ParenthCnt)
                    {
                        functionParenthDepth->RemoveAt (functionParenthDepth->Count - 1); 
                    }

                    if ((--ParenthCnt) < 0)
                    {
                        parseErrorType = ParseErrorType::MISM_PARENTH;
                        return Ind;
                    }

                    sws(Function, ++Ind);
                    c = Function[Ind];
                }

                // If we get here, we have a legal operand and now a legal operator or
                // end of string must follow

                // Check for EOS
                if (Ind >= Function->Length)
                    break; // The only way to end the checking loop without error

                // Check for operator
                if(!IsOperator(c) && (c != ',' || functionParenthDepth->Count > 0 ||
                                      functionParenthDepth[functionParenthDepth->Count-1] != ParenthCnt))
                {
                    parseErrorType = ParseErrorType::EXPECT_OPERATOR;
                    return Ind;
                }

                // If we get here, we have an operand and an operator; the next loop will
                // check for another operand (must appear)
                ++Ind;
            } // while
} catch (...) {} // catching string out of bounds access

            // Check that all opened parentheses are also closed
            if(ParenthCnt>0)
            {
                parseErrorType = ParseErrorType::MISSING_PARENTH;
                return Ind;
            }

            // The string is ok
            parseErrorType = ParseErrorType::FP_NO_ERROR;
            return -1;
        }


        // Compile function string to bytecode
        // -----------------------------------
        bool FunctionParser::Compile(String^ Function)
        {
            data->ByteCode = nullptr; 
            data->Immed    = nullptr; 
            data->Stack    = nullptr; 

            tempByteCode = gcnew List<unsigned> (1024);
            tempImmed    = gcnew List<double> (1024);

            data->StackSize = StackPtr = 0;

            CompileExpression(Function, 0, false);
            if(parseErrorType != ParseErrorType::FP_NO_ERROR)
                return false;

            data -> ByteCodeSize = tempByteCode -> Count;
            data -> ImmedSize    = tempImmed    -> Count;

            if(data -> ByteCodeSize > 0)
            {
                data -> ByteCode = tempByteCode -> ToArray (); 
            }

            if(data->ImmedSize)
            {
                data->Immed = tempImmed -> ToArray (); 
            }

            if(data->StackSize)
            {
                data->Stack = gcnew array<double> (data->StackSize);
            }

            return true;
        }

        inline void FunctionParser::AddCompiledByte(unsigned c)
        {
            tempByteCode->Add(c);
        }

        inline void FunctionParser::AddImmediate(double i)
        {
            tempImmed->Add(i);
        }

        inline void FunctionParser::AddFunctionOpcode(OPCODE opcode)
        {
            if(data->useDegreeConversion)
                switch(opcode)
                {
                case OPCODE::cCos:
                case OPCODE::cCosh:
                case OPCODE::cCot:
                case OPCODE::cCsc:
                case OPCODE::cSec:
                case OPCODE::cSin:
                case OPCODE::cSinh:
                case OPCODE::cTan:
                case OPCODE::cTanh:
                    AddCompiledByte((unsigned)OPCODE::cRad);
                }

            AddCompiledByte((unsigned) opcode);

            if(data->useDegreeConversion)
                switch(opcode)
                {
                case OPCODE::cAcos:

                case OPCODE::cAsin:
                case OPCODE::cAtan:
                case OPCODE::cAtan2:
                    AddCompiledByte((unsigned)OPCODE::cDeg);
                }
        }

        inline void FunctionParser::incStackPtr()
        {
            if(++StackPtr > data->StackSize) ++(data->StackSize);
        }


        // Compile if()
        int FunctionParser::CompileIf(String^ F, int ind)
        {
            int ind2 = CompileExpression(F, ind, true); // condition
            sws(F, ind2);
            if(F[ind2] != ',') { parseErrorType=ParseErrorType::ILL_PARAMS_AMOUNT; return ind2; }
            AddCompiledByte((unsigned)OPCODE::cIf);
            unsigned curByteCodeSize = tempByteCode->Count;
            AddCompiledByte( 0); // Jump index; to be set later
            AddCompiledByte( 0); // Immed jump index; to be set later

            --StackPtr;

            ind2 = CompileExpression(F, ind2+1, true); // then
            sws(F, ind2);
            if(F[ind2] != ',') { parseErrorType=ParseErrorType::ILL_PARAMS_AMOUNT; return ind2; }
            AddCompiledByte((unsigned)OPCODE::cJump);
            unsigned curByteCodeSize2 = tempByteCode->Count;
            unsigned curImmedSize2 = tempImmed->Count;
            AddCompiledByte( 0); // Jump index; to be set later
            AddCompiledByte( 0); // Immed jump index; to be set later

            --StackPtr;

            ind2 = CompileExpression(F, ind2+1, true); // else
            sws(F, ind2);
            if(F[ind2] != ')') { parseErrorType=ParseErrorType::ILL_PARAMS_AMOUNT; return ind2; }

            // Set jump indices
            tempByteCode[curByteCodeSize] = (curByteCodeSize2+1);
            tempByteCode[curByteCodeSize+1] = curImmedSize2;
            tempByteCode[curByteCodeSize2] =  (tempByteCode->Count - 1);
            tempByteCode[curByteCodeSize2+1] = tempImmed->Count;

            return ind2+1;
        }

        int FunctionParser::CompileFunctionParams(String^ F, int ind, unsigned requiredParams)
        {
            unsigned curStackPtr = StackPtr;
            int ind2 = CompileExpression(F, ind, false);

            if(StackPtr != curStackPtr+requiredParams)
            { parseErrorType=ParseErrorType::ILL_PARAMS_AMOUNT; return ind; }

            StackPtr -= requiredParams - 1;
            sws(F, ind2);
            return ind2+1; // F[ind2] is ')'
        }

        // Compiles element
        int FunctionParser::CompileElement(String^ F, int ind)
        {
            sws(F, ind);
            int c = F[ind];

            if(c == '(')
            {
                ind = CompileExpression(F, ind+1, false);
                sws(F, ind);
                return ind+1; // F[ind] is ')'
            }

            if(Char::IsDigit(c) || c == '.' /*|| c=='-'*/) // Number
            {
                //        const char* startPtr = &F[ind];
                //        char* endPtr;
                //        double val = strtod(startPtr, &endPtr);

                int start = ind; 
                // skip digits
                while (ind < F->Length && Char::IsDigit (F[ind]))
                    ind++;

                // check for fraction
                if (ind < F->Length - 1 && F[ind] == '.')
                {
                    ind ++; 
                    while (ind < F->Length && Char::IsDigit (F[ind]))
                        ind++;
                }

                double val = 0;
                try
                {
                    val = Double::Parse (F->Substring (start, ind - start));
                }
                catch (...)
                {
                    val = 0;
                }

                AddImmediate(val);
                AddCompiledByte((unsigned)OPCODE::cImmed);
                incStackPtr();

                return ind;
            }

            if(Char::IsLetter(c) || c == '_') // Function, variable or constant
            {
                FuncDefinition^ func = FuncDefinition::FindFunction(F->Substring (ind));
                if (func != nullptr) // is function
                {
                    int ind2 = ind + func->name->Length;
                    sws(F, ind2); // F[ind2] is '('
                    if(func->name == "if") // "if" is a special case
                    {
                        return CompileIf(F, ind2+1);
                    }

                    unsigned requiredParams = func->name == "eval" ?
                        data->Variables->Count :
                        func->params;

                    ind2 = CompileFunctionParams(F, ind2+1, requiredParams);
                    AddFunctionOpcode(func->opcode);
                    return ind2; // F[ind2-1] is ')'
                }

                String^ vIter = FindVariable(F->Substring(ind), data->Variables);
                if(vIter != nullptr) // is variable
                {
//                    System::Console::WriteLine("{0} {1}", vIter, data->Variables[vIter]);
                    AddCompiledByte( data->Variables[vIter]);
                    incStackPtr();
                    return ind + vIter->Length;
                }

                String^ cIter = FindConstant(F->Substring(ind));
                if(cIter != nullptr) // is constant
                {
                    AddImmediate(data->Constants[cIter]);
                    AddCompiledByte((unsigned)OPCODE::cImmed);
                    incStackPtr();
                    return ind + cIter->Length;
                }

                String^ fIter = FindVariable(F->Substring(ind), data->FuncPtrNames);
                if(fIter != nullptr) // is user-defined func pointer
                {
                    unsigned index = data->FuncPtrNames[fIter];

                    int ind2 = ind + fIter->Length;
                    sws(F, ind2); // F[ind2] is '('

                    ind2 = CompileFunctionParams(F, ind2+1, data->FuncPtrs[index]->params);

                    AddCompiledByte((unsigned)OPCODE::cFCall);
                    AddCompiledByte( index);
                    return ind2;
                }

                String^ pIter = FindVariable(F->Substring(ind), data->FuncParserNames);
                if(pIter != nullptr) // is user-defined func parser
                {
                    unsigned index = data->FuncParserNames[pIter];

                    int ind2 = ind + pIter->Length;
                    sws(F, ind2); // F[ind2] is '('

                    ind2 = CompileFunctionParams (F, ind2+1, data->FuncParsers[index]->data->varAmount);

                    AddCompiledByte((unsigned)OPCODE::cPCall);
                    AddCompiledByte( index);
                    return ind2;
                }
            }

            parseErrorType = ParseErrorType::UNEXPECTED_ERROR;
            return ind;
        }

        // Compiles '^'
        int FunctionParser::CompilePow(String^ F, int ind)
        {
            int ind2 = CompileElement(F, ind);
            sws(F, ind2);

            while(ind2 < F->Length && F[ind2] == '^')
            {
                ind2 = CompileUnaryMinus(F, ind2+1);
                sws(F, ind2);
                AddCompiledByte((unsigned)OPCODE::cPow);
                --StackPtr;
            }

            return ind2;
        }

        // Compiles unary '-'
        int FunctionParser::CompileUnaryMinus(String^ F, int ind)
        {
            sws(F, ind);
            if(F[ind] == '-')
            {
                int ind2 = ind+1;
                sws(F, ind2);
                ind2 = CompilePow(F, ind2);
                sws(F, ind2);

                // if we are negating a constant, negate the constant itself:
                if(tempByteCode[tempByteCode->Count - 1] == (unsigned)OPCODE::cImmed)
                    tempImmed[tempImmed->Count-1] = -tempImmed[tempImmed->Count-1];

                // if we are negating a negation, we can remove both:
                else if(tempByteCode[tempByteCode->Count-1] == (unsigned) OPCODE::cNeg)
                    tempByteCode->RemoveAt (tempByteCode->Count-1);

                else
                    AddCompiledByte((unsigned)OPCODE::cNeg);

                return ind2;
            }

            int ind2 = CompilePow(F, ind);
            sws(F, ind2);
            return ind2;
        }

        // Compiles '*', '/' and '%'
        int FunctionParser::CompileMult(String^ F, int ind)
        {
            int ind2 = CompileUnaryMinus(F, ind);
            sws(F, ind2);
            int op;

            while(ind2 < F->Length && ((op = F[ind2]) == '*' || op == '/' || op == '%'))
            {
                ind2 = CompileUnaryMinus(F, ind2+1);
                sws(F, ind2);
                switch(op)
                {
                case '*': AddCompiledByte((unsigned)OPCODE::cMul); break;
                case '/': AddCompiledByte((unsigned)OPCODE::cDiv); break;
                case '%': AddCompiledByte((unsigned)OPCODE::cMod); break;
                }
                --StackPtr;
            }

            return ind2;
        }

        // Compiles '+' and '-'
        int FunctionParser::CompileAddition(String^ F, int ind)
        {
            int ind2 = CompileMult(F, ind);
            sws(F, ind2);
            int op;

            while(ind2 < F->Length && ((op = F[ind2]) == '+' || op == '-'))
            {
                ind2 = CompileMult(F, ind2+1);
                sws(F, ind2);
                AddCompiledByte((unsigned)(op=='+' ? OPCODE::cAdd : OPCODE::cSub));
                --StackPtr;
            }

            return ind2;
        }

        // Compiles '=', '<' and '>'
        int FunctionParser::CompileComparison(String^ F, int ind)
        {
            int ind2 = CompileAddition(F, ind);
            sws(F, ind2);
            int op;

            while(ind2 < F->Length && ((op = F[ind2]) == '=' || op == '<' || op == '>'))
            {
                ind2 = CompileAddition(F, ind2+1);
                sws(F, ind2);
                switch(op)
                {
                case '=': AddCompiledByte((unsigned)OPCODE::cEqual); break;
                case '<': AddCompiledByte((unsigned)OPCODE::cLess); break;
                case '>': AddCompiledByte((unsigned)OPCODE::cGreater); break;
                }
                --StackPtr;
            }

            return ind2;
        }

        // Compiles '&'
        int FunctionParser::CompileAnd(String^ F, int ind)
        {
            int ind2 = CompileComparison(F, ind);
            sws(F, ind2);

            while(ind2 < F->Length && F[ind2] == '&')
            {
                ind2 = CompileComparison(F, ind2+1);
                sws(F, ind2);
                AddCompiledByte((unsigned)OPCODE::cAnd);
                --StackPtr;
            }

            return ind2;
        }

        // Compiles '|'
        int FunctionParser::CompileOr(String^ F, int ind)
        {
            int ind2 = CompileAnd(F, ind);
            sws(F, ind2);

            while(ind2 < F->Length && F[ind2] == '|')
            {
                ind2 = CompileAnd(F, ind2+1);
                sws(F, ind2);
                AddCompiledByte((unsigned)OPCODE::cOr);
                --StackPtr;
            }

            return ind2;
        }

        // Compiles ','
        int FunctionParser::CompileExpression(String^ F, int ind, bool stopAtComma)
        {
            int ind2 = CompileOr(F, ind);
            sws(F, ind2);

            if(stopAtComma) return ind2;

            while(ind2 < F->Length && F[ind2] == ',')
            {
                ind2 = CompileOr(F, ind2+1);
                sws(F, ind2);
            }

            return ind2;
        }


        // Return parse error message
        // --------------------------
        String^ FunctionParser::ErrorMsg() 
        {
            if(parseErrorType != ParseErrorType::FP_NO_ERROR) return ParseErrorMessage[ (int) parseErrorType];
            return "";
        }

        double FunctionParser::Eval(array<double>^ Vars)
        {
            array<unsigned>^ ByteCode = data->ByteCode;
            array<double>^ Immed = data->Immed;
            array<double>^ Stack = data->Stack;

            const unsigned ByteCodeSize = data->ByteCodeSize;
            unsigned IP, DP=0;
            int SP=-1;

            for(IP=0; IP<ByteCodeSize; ++IP)
            {
                switch(ByteCode[IP])
                {
                    // Functions:
                case OPCODE::cAbs: Stack[SP] = Math::Abs(Stack[SP]); break;
                case OPCODE::cAcos: if(Stack[SP] < -1 || Stack[SP] > 1)
                    { evalErrorType=4; return 0; }
                    Stack[SP] = Math::Acos(Stack[SP]); break;

                case OPCODE::cAsin: if(Stack[SP] < -1 || Stack[SP] > 1)
                    { evalErrorType=4; return 0; }
                    Stack[SP] = Math::Asin(Stack[SP]); break;

                case OPCODE::cAtan: Stack[SP] = Math::Atan(Stack[SP]); break;
                case OPCODE::cAtan2: Stack[SP-1] = Math::Atan2(Stack[SP-1], Stack[SP]);
                    --SP; break;

                case OPCODE::cCeil: Stack[SP] = Math::Ceiling(Stack[SP]); break;
                case OPCODE::cCos: Stack[SP] = Math::Cos(Stack[SP]); break;
                case OPCODE::cCosh: Stack[SP] = Math::Cosh(Stack[SP]); break;

                case OPCODE::cCot:
                    {
                        double t = Math::Tan(Stack[SP]);
                        if(t == 0) { evalErrorType=1; return 0; }
                        Stack[SP] = 1/t; break;
                    }
                case   OPCODE::cCsc:
                    {
                        double s = Math::Sin(Stack[SP]);
                        if(s == 0) { evalErrorType=1; return 0; }
                        Stack[SP] = 1/s; break;
                    }

                case  OPCODE::cEval:
                    {
                        data->Stack = gcnew array<double> (data->StackSize);

                        int new_length = Stack->Length - (SP - data->varAmount + 1);
                        array<double>^ varsStack = gcnew array<double> (new_length); 
                        Array::Copy (Stack, SP - data->varAmount + 1, varsStack, 0, new_length); 

                        double retVal = Eval(varsStack);
                        data->Stack = Stack;
                        SP -= data->varAmount-1;
                        Stack[SP] = retVal;
                        break;
                    }

                case OPCODE::cExp: Stack[SP] = Math::Exp(Stack[SP]); break;
                case OPCODE::cFloor: Stack[SP] = Math::Floor(Stack[SP]); break;

                case OPCODE::cIf:
                    {
                        unsigned jumpAddr = (unsigned) ByteCode[++IP];
                        unsigned immedAddr = (unsigned) ByteCode[++IP];
                        if(doubleToInt(Stack[SP]) == 0)
                        {
                            IP = jumpAddr;
                            DP = immedAddr;
                        }
                        --SP; break;
                    }

                case OPCODE::cInt: Stack[SP] = Math::Floor(Stack[SP]+.5); break;
                case OPCODE::cLog: if(Stack[SP] <= 0) { evalErrorType=3; return 0; }
                    Stack[SP] = Math::Log(Stack[SP]); break;
                case OPCODE::cLog10: if(Stack[SP] <= 0) { evalErrorType=3; return 0; }
                    Stack[SP] = Math::Log10(Stack[SP]); break;
                case OPCODE::cMax: Stack[SP-1] = Max(Stack[SP-1], Stack[SP]);
                    --SP; break;
                case OPCODE::cMin: Stack[SP-1] = Min(Stack[SP-1], Stack[SP]);
                    --SP; break;
                case OPCODE::cSec:
                    {
                        double c = Math::Cos(Stack[SP]);
                        if(c == 0) { evalErrorType=1; return 0; }
                        Stack[SP] = 1/c; break;
                    }
                case OPCODE::cSin: Stack[SP] = Math::Sin(Stack[SP]); break;
                case OPCODE:: cSinh: Stack[SP] = Math::Sinh(Stack[SP]); break;
                case OPCODE:: cSqrt: if(Stack[SP] < 0) { evalErrorType=2; return 0; }
                    Stack[SP] = Math::Sqrt(Stack[SP]); break;
                case OPCODE::  cTan: Stack[SP] = Math::Tan(Stack[SP]); break;
                case OPCODE:: cTanh: Stack[SP] = Math::Tanh(Stack[SP]); break;


                    // Misc:
                case OPCODE::cImmed: Stack[++SP] = Immed[DP++]; break;
                case OPCODE:: cJump: DP = (unsigned) ByteCode[IP+2];
                    IP = (unsigned) ByteCode[IP+1];
                    break;

                    // Operators:
                case OPCODE::  cNeg: Stack[SP] = -Stack[SP]; break;
                case OPCODE::  cAdd: Stack[SP-1] += Stack[SP]; --SP; break;
                case OPCODE::  cSub: Stack[SP-1] -= Stack[SP]; --SP; break;
                case OPCODE::  cMul: Stack[SP-1] *= Stack[SP]; --SP; break;
                case OPCODE::  cDiv: if(Stack[SP] == 0) { evalErrorType=1; return 0; }
                    Stack[SP-1] /= Stack[SP]; --SP; break;
                case OPCODE::  cMod: if(Stack[SP] == 0) { evalErrorType=1; return 0; }
                    Stack[SP-1] = Math::IEEERemainder (Stack[SP-1],Stack[SP]);
                    --SP; break;
                case OPCODE::  cPow: Stack[SP-1] = Math::Pow(Stack[SP-1], Stack[SP]);
                    --SP; break;

                case OPCODE::cEqual: Stack[SP-1] = (Stack[SP-1] == Stack[SP]);
                    --SP; break;
                case OPCODE:: cLess: Stack[SP-1] = (Stack[SP-1] < Stack[SP]);
                    --SP; break;
                case OPCODE::cGreater: Stack[SP-1] = (Stack[SP-1] > Stack[SP]);
                    --SP; break;
                case OPCODE::  cAnd: Stack[SP-1] =
                        (doubleToInt(Stack[SP-1]) &&
                         doubleToInt(Stack[SP]));
                    --SP; break;
                case OPCODE::   cOr: Stack[SP-1] =
                        (doubleToInt(Stack[SP-1]) ||
                         doubleToInt(Stack[SP]));
                    --SP; break;

                    // Degrees-radians conversion:
                case OPCODE::  cDeg: Stack[SP] = RadiansToDegrees(Stack[SP]); break;
                case OPCODE::  cRad: Stack[SP] = DegreesToRadians(Stack[SP]); break;

                    // User-defined function calls:
                case OPCODE::cFCall:
                    {
                        unsigned index = (unsigned) ByteCode[++IP];
                        unsigned params = data->FuncPtrs[index]->params;

                        int new_len = Stack->Length - (SP-params+1);
                        array<double>^ new_arr = gcnew array<double> (new_len);
                        Array::Copy (Stack, SP - params + 1, new_arr, 0, new_len); 
                        double retVal = data->FuncPtrs[index]->ptr(new_arr);
                  
                        SP -= params-1;
                        Stack[SP] = retVal;
                        break;
                    }

                case OPCODE::cPCall:
                    {
                        unsigned index = (unsigned) ByteCode[++IP];
                        unsigned params = data->FuncParsers[index]->data->varAmount;

                        int new_len = Stack->Length - (SP - params + 1);
                        array<double>^ new_arr = gcnew array<double> (new_len);
                        Array::Copy (Stack, SP - params + 1, new_arr, 0, new_len); 
                        double retVal = data->FuncParsers[index]->Eval(new_arr);
                        SP -= params-1;
                        Stack[SP] = retVal;
                        break;
                    }


                case OPCODE::  cVar: break; // Paranoia. These should never exist
                case OPCODE::  cDup: Stack[SP+1] = Stack[SP]; ++SP; break;
                case OPCODE::  cInv:
                    if(Stack[SP] == 0.0) { evalErrorType=1; return 0; }
                    Stack[SP] = 1.0/Stack[SP];
                    break;

                    // Variables:
                default:
                    {
                        unsigned index = (unsigned) ByteCode[IP] - (unsigned) OPCODE::VarBegin;
//                        System::Console::WriteLine("{0} {1}", ByteCode[IP], index);
                    Stack[++SP] = Vars[ index];
                    }
                }
            }

            evalErrorType=0;
            return Stack[SP];
        }
    }
}
}