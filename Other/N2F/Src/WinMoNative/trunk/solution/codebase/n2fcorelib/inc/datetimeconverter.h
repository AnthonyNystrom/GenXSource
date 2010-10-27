#pragma once

class DateTimeConverter
{
public:

	DateTimeConverter();

	virtual	~DateTimeConverter();

	bool		IsLeapYear(int year);
	INT64		DateToTicks(int year, int month, int day);
	INT64		TimeToTicks(int hour, int minute, int second);

	INT64		GetTicks(int year, int month, int day, int hour, int minute, int second);
	INT64		GetTicks(SYSTEMTIME &st);


protected:
	
private:
	int		iDaysToMonth365[13];
	int		iDaysToMonth366[13];
};