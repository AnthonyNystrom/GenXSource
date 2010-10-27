//-----------------------------------------------------------------------------
// SlideShow.h : Interface of the StereoPlayer SlideShow class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#ifndef _GSPSLIDESHOW_H_
#define _GSPSLIDESHOW_H_

#include <strsafe.h>
#include <vector>
#include <string>

namespace glsp
{
class StereoPlayer;

// StereoPlayer SlideShow management class
class SlideShow
{
public:

    SlideShow();
    virtual ~SlideShow();

    void setPlayer(StereoPlayer* player);
    StereoPlayer* getPlayer();

    bool loadFile(const char* fileName, BOOL openFirstSlide=TRUE);
	const char* getFileName();

    void setPage(unsigned int index);
    unsigned int getPage();
    unsigned int getNumPages();

    void first();
    void last();
    void prev();
    void next();

protected:

	std::string m_fileName;
	std::string m_homeDir;
    std::vector<std::string> m_fileNames;
    unsigned int m_currentIndex;
    StereoPlayer* m_player;
};

}; // glsp

#endif // _GSPSLIDESHOW_H_
