#pragma once
#include "sgCore\sg3D.h"
#include "Objects\3D\msgBRepPiece.h"

namespace sgCoreWrapper
{
	namespace Objects
	{
		public ref class msgBRep
		{
			msgBRepPiece^ GetPiece(unsigned int nmbr)
			{
				return gcnew msgBRepPiece(_sgBRep->GetPiece(nmbr));
			}

			unsigned int GetPiecesCount()
			{
				return _sgBRep->GetPiecesCount();
			}
		internal:
			msgBRep(sgCBRep* rep)
			{
				_sgBRep = rep;
			}
		private:
			sgCBRep* _sgBRep;
		};
	}
}