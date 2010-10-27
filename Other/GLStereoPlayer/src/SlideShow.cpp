//-----------------------------------------------------------------------------
// SlideShow.cpp : Implementation of the StereoPlayer SlideShow class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"

#include "StereoPlayer.h"
#include "StereoPlayerXML.h"

#include "SlideShow.h"

using namespace glsp;

SlideShow::SlideShow()
{
    m_player = NULL;
    m_currentIndex = 0;
}

SlideShow::~SlideShow()
{
}

void SlideShow::setPlayer(StereoPlayer* player)
{
    m_player = player;
}

StereoPlayer* SlideShow::getPlayer()
{
    return m_player;
}

bool SlideShow::loadFile(const char* fileName, BOOL openFirstSlide)
{
	if (!fileName) return FALSE;

    m_fileNames.clear();

	m_fileName = fileName;

    char drive[_MAX_DRIVE], dir[_MAX_DIR];
    char fname[_MAX_FNAME], ext[_MAX_EXT];
	char homeDir[_MAX_PATH];
    _splitpath(fileName, drive, dir, fname, ext);
    StringCchPrintf(homeDir, _MAX_PATH, "%s%s", drive, dir);
	m_homeDir = homeDir;

    FILE* fp = fopen(fileName, "r");
    if (fp) {
        char line[1024];
        while(fgets(line, 1024, fp)) {
            if (strlen(line) > 0) {
                if (line[strlen(line)-1] == '\n')
                    line[strlen(line)-1] = '\0';
                m_fileNames.push_back(line);
            }
        }
        fclose(fp);

		if (openFirstSlide)
	        first();

        return TRUE;
    }

    return FALSE;
}

const char* SlideShow::getFileName()
{
	return m_fileName.c_str();
}

void SlideShow::setPage(unsigned int index)
{
    if (index < m_fileNames.size())
        m_currentIndex = index;
    if (m_player)
        LoadSettingFile(m_player, m_homeDir.c_str(), m_fileNames[m_currentIndex].c_str());
}

unsigned int SlideShow::getPage()
{
    return m_currentIndex;
}

unsigned int SlideShow::getNumPages()
{
    return m_fileNames.size();
}

void SlideShow::first()
{
    m_currentIndex = 0;
    if (m_player && m_fileNames.size() > 0)
        LoadSettingFile(m_player, m_homeDir.c_str(), m_fileNames[m_currentIndex].c_str());
}

void SlideShow::last()
{
    if (m_fileNames.size() > 0)
        m_currentIndex = m_fileNames.size() - 1;
    if (m_player && m_fileNames.size() > 0)
        LoadSettingFile(m_player, m_homeDir.c_str(), m_fileNames[m_currentIndex].c_str());
}

void SlideShow::prev()
{
    if (m_fileNames.size()) {
        if (m_currentIndex > 0)
            m_currentIndex--;
        if (m_player)
            LoadSettingFile(m_player, m_homeDir.c_str(), m_fileNames[m_currentIndex].c_str());
    }
}

void SlideShow::next()
{
    if (m_fileNames.size() > 0) {
        m_currentIndex++;
        if (m_currentIndex >= m_fileNames.size())
            m_currentIndex = m_fileNames.size() - 1;
        if (m_player)
            LoadSettingFile(m_player, m_homeDir.c_str(), m_fileNames[m_currentIndex].c_str());
    }
}
