#ifndef _REPORTCONTROLFACTORY_H_
#define _REPORTCONTROLFACTORY_H_

#include "DiagramEditor\DiagramEntity.h"

class CReportControlFactory {

public:
	static CDiagramEntity* CreateFromString( const CString& str );

};

#endif // _REPORTCONTROLFACTORY_H_