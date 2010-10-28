#include "stdafx.h"
#include "FPSCounter.h"

CFPSCounter::CFPSCounter(void) :
   m_eAveragingType  (FPS_FRAME),
   m_dAveragingValue (10),
   m_dFPS            (0.0),
   m_nFrames         (0)
{
	QueryPerformanceFrequency(&m_liFrequency);
	QueryPerformanceCounter(&m_liMark);
}

CFPSCounter::~CFPSCounter(void)
{
}

void CFPSCounter::SetAveraging(EFPSCounterAveraging eType, double dValue)
{
   m_eAveragingType  = eType;
   m_dAveragingValue = dValue;
}

void CFPSCounter::AddFrame(void)
{
   m_nFrames++;
   QueryPerformanceCounter(&m_liCounter);
   m_liTimeDiff.QuadPart=m_liCounter.QuadPart-m_liMark.QuadPart;
   double dTime=double(m_liTimeDiff.QuadPart)/double(m_liFrequency.QuadPart);

   if (   m_eAveragingType == FPS_FRAME && m_nFrames >= m_dAveragingValue
       || m_eAveragingType == FPS_TIME  &&     dTime >= m_dAveragingValue
      )
   {
      m_dFPS=m_nFrames/dTime;
      m_nFrames=0;
      m_liMark=m_liCounter;
   }
}

double CFPSCounter::GetFPS(void)
{
   return m_dFPS;
}


