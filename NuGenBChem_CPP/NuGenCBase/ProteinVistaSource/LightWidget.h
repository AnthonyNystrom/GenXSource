#pragma once

#include "DXUTmisc.h"

class CLightWidget: public CDXUTDirectionWidget 
{
public:
	HRESULT Save(CFile &file);
	HRESULT Load(CFile &file);
};



