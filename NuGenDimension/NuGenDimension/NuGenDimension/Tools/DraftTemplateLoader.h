#ifndef __DRAFT_TEMPLATE_LOADER__
#define __DRAFT_TEMPLATE_LOADER__


#include "..//ReportEditor/DiagramEditor/DiagramEntityContainer.h"

class CDraftTemplateLoader
{
public:

private:

public:

  static  CxImage*  GetThumbnailFromFile(const char* filePath);
  static  bool  SaveDraftTemplateInFile(const char* thumbnailFile,
										CDiagramEntityContainer* obj_container,
										const char* targetFile);
  static  CDiagramEntityContainer*  LoadDraftTemplate(const char* filePath);

};
#endif