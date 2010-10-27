#include "PhotoLibrary.h"
#include "PhotoLibraryWM.h"


PhotoLibrary * PhotoLibrary::Create()
{
	return new PhotoLibraryWM();
}


PhotoLibrary::PhotoLibrary()
{
	Refresh();
	photoCounter = 0;
}

PhotoLibrary::~PhotoLibrary()
{
}

void PhotoLibrary::Refresh()
{
	needRefresh[EPST_PHONE] = true;
	needRefresh[EPST_CARD] = true;
}

