#include "stdafx.h"
#include "datetimeconverter.h"

DateTimeConverter::DateTimeConverter()
{
	int ar1[13] = { 0, 0x1f, 0x3b,
					90, 120, 0x97,
					0xb5, 0xd4, 0xf3,
					0x111, 0x130, 0x14e,
					0x16d };

	int ar2[13] = { 0, 0x1f, 60,
					0x5b, 0x79, 0x98,
					0xb6, 0xd5, 0xf4,
					0x112, 0x131, 0x14f,
					0x16e };

	memcpy(iDaysToMonth365, ar1, sizeof(ar1));
	memcpy(iDaysToMonth366, ar2, sizeof(ar2));
}

DateTimeConverter::~DateTimeConverter()
{

}

bool DateTimeConverter::IsLeapYear( int year )
{
	if ( year < 1 || year > 0x270f )
		return false;

	if ( year % 4 != 0 )
		return false;

	if ( year % 100 == 0 )
		return (year % 400 == 0);

	return true;
}

INT64 DateTimeConverter::DateToTicks( int year, int month, int day )
{
	INT64 result = 0;

	if (((year >= 1) && (year <= 0x270f)) && ((month >= 1) && (month <= 12)))
	{
		int *numArray = this->IsLeapYear(year)? iDaysToMonth366: iDaysToMonth365;
		if ((day >= 1) && (day <= (numArray[month] - numArray[month - 1])))
		{
			int num = year - 1;
			int num2 = ((((((num * 0x16d) + (num / 4)) - (num / 100)) + (num / 400)) + numArray[month - 1]) + day) - 1;
			result = (num2 * 0xc92a69c000L);
		}
	}

	return result;
}

INT64 DateTimeConverter::TimeToTicks( int hour, int minute, int second )
{
	INT64 result = 0;

	if ((((hour < 0) || (hour >= 0x18)) ||
		((minute < 0) || (minute >= 60))) ||
		((second < 0) || (second >= 60)))
		return result;

	INT64 num = ((hour * 0xe10L) + (minute * 60L)) + second;
	if ((num > 0xd6bf94d5e5L) || (num < -922337203685L))
		return result;

	result = num * 0x989680L;

	return result;
}

INT64 DateTimeConverter::GetTicks( int year, int month, int day, int hour, int minute, int second )
{
	INT64 result = this->DateToTicks(year, month, day) + this->TimeToTicks(hour, minute, second);

	return result;
}

INT64 DateTimeConverter::GetTicks( SYSTEMTIME &st )
{
	return this->GetTicks(st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond);
}
