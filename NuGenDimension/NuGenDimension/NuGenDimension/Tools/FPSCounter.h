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
   EFPSCounterAveraging m_eAveragingType;    // ⨯ ��।����� ���祭�� FPS
   double               m_dAveragingValue;   // ���祭�� ��� ��ࠬ��� ��।����� (������⢮ ���஢ ��� �६�)
   double               m_dFPS;              // ⥪�饥 ���祭�� FPS
   int                  m_nFrames;           // ������⢮ ���஢

   LARGE_INTEGER m_liFrequency; // ���� ⠩���, ⨪� � ᥪ㭤�
   LARGE_INTEGER m_liMark;      // ��᫥���� ���窠 ⠩���, ⨪�
   LARGE_INTEGER m_liCounter;   // ⥪�饥 ���祭�� ⠩���, ⨪�
   LARGE_INTEGER m_liTimeDiff;  // ���⥫쭮��� ���ࢠ��, ⨪�
};



#endif