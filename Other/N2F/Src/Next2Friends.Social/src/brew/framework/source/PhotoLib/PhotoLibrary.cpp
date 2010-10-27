#include "PhotoLibrary.h"
#include "PhotoLibraryBrew.h"


PhotoLibrary * PhotoLibrary::Create()
{
	return new PhotoLibraryBrew();
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

