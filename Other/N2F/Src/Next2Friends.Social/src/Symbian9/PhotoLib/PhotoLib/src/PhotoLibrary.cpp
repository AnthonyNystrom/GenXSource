#include "PhotoLibrary.h"
#include "PhotoLibrarySis9.h"


PhotoLibrary * PhotoLibrary::Create()
{
	return new PhotoLibrarySis();
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

