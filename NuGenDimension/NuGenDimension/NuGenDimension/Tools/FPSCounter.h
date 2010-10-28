#ifndef __FPSCOUNTER__
#define __FPSCOUNTER__

enum EFPSCounterAveraging
{
   FPS_FRAME = 0,
   FPS_TIME  = 1
};

class CFPSCounter
{
public:
            CFPSCounter (void);
   virtual ~CFPSCounter (void);

   void   SetAveraging (EFPSCounterAveraging eType, double dValue);
   void   AddFrame     (void);
   double GetFPS       (void);

protected:
   EFPSCounterAveraging m_eAveragingType;    // тип усреднения значения FPS
   double               m_dAveragingValue;   // значение для параметра усреднения (количество кадров или время)
   double               m_dFPS;              // текущее значение FPS
   int                  m_nFrames;           // количество кадров

   LARGE_INTEGER m_liFrequency; // частота таймера, тики в секунду
   LARGE_INTEGER m_liMark;      // последняя засечка таймера, тики
   LARGE_INTEGER m_liCounter;   // текущее значение таймера, тики
   LARGE_INTEGER m_liTimeDiff;  // длительность интервала, тики
};



#endif