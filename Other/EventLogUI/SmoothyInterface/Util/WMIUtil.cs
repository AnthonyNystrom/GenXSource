using System;
using System.Collections.Generic;
using System.Text;

namespace SmoothyInterface.Util
{
	public class WMIUtil
	{
		// Converts a given datetime in DMTF format to System.DateTime object.
		public static System.DateTime ToDateTime(string dmtfDate)
		{
			System.DateTime initializer = System.DateTime.MinValue;
			int year = initializer.Year;
			int month = initializer.Month;
			int day = initializer.Day;
			int hour = initializer.Hour;
			int minute = initializer.Minute;
			int second = initializer.Second;
			long ticks = 0;
			string dmtf = dmtfDate;
			System.DateTime datetime = System.DateTime.MinValue;
			string tempString = string.Empty;
			if ((dmtf == null))
			{
				throw new System.ArgumentOutOfRangeException();
			}
			if ((dmtf.Length == 0))
			{
				throw new System.ArgumentOutOfRangeException();
			}
			if ((dmtf.Length != 25))
			{
				throw new System.ArgumentOutOfRangeException();
			}
			try
			{
				tempString = dmtf.Substring(0, 4);
				if (("****" != tempString))
				{
					year = int.Parse(tempString);
				}
				tempString = dmtf.Substring(4, 2);
				if (("**" != tempString))
				{
					month = int.Parse(tempString);
				}
				tempString = dmtf.Substring(6, 2);
				if (("**" != tempString))
				{
					day = int.Parse(tempString);
				}
				tempString = dmtf.Substring(8, 2);
				if (("**" != tempString))
				{
					hour = int.Parse(tempString);
				}
				tempString = dmtf.Substring(10, 2);
				if (("**" != tempString))
				{
					minute = int.Parse(tempString);
				}
				tempString = dmtf.Substring(12, 2);
				if (("**" != tempString))
				{
					second = int.Parse(tempString);
				}
				tempString = dmtf.Substring(15, 6);
				if (("******" != tempString))
				{
					ticks = (long.Parse(tempString) * ((long)((System.TimeSpan.TicksPerMillisecond / 1000))));
				}
				if (((((((((year < 0)
							|| (month < 0))
							|| (day < 0))
							|| (hour < 0))
							|| (minute < 0))
							|| (minute < 0))
							|| (second < 0))
							|| (ticks < 0)))
				{
					throw new System.ArgumentOutOfRangeException();
				}
			}
			catch (System.Exception e)
			{
				throw new System.ArgumentOutOfRangeException(null, e.Message);
			}
			datetime = new System.DateTime(year, month, day, hour, minute, second, 0);
			datetime = datetime.AddTicks(ticks);
			System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
			int UTCOffset = 0;
			int OffsetToBeAdjusted = 0;
			long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
			tempString = dmtf.Substring(22, 3);
			if ((tempString != "******"))
			{
				tempString = dmtf.Substring(21, 4);
				try
				{
					UTCOffset = int.Parse(tempString);
				}
				catch (System.Exception e)
				{
					throw new System.ArgumentOutOfRangeException(null, e.Message);
				}
				OffsetToBeAdjusted = ((int)((OffsetMins - UTCOffset)));
				datetime = datetime.AddMinutes(((double)(OffsetToBeAdjusted)));
			}
			return datetime;
		}

		// Converts a given System.DateTime object to DMTF datetime format.
		public static string ToDmtfDateTime(System.DateTime date)
		{
			string utcString = string.Empty;
			System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(date);
			long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
			if ((System.Math.Abs(OffsetMins) > 999))
			{
				date = date.ToUniversalTime();
				utcString = "+000";
			}
			else
			{
				if ((tickOffset.Ticks >= 0))
				{
					utcString = string.Concat("+", ((System.Int64)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute))).ToString().PadLeft(3, '0'));
				}
				else
				{
					string strTemp = ((System.Int64)(OffsetMins)).ToString();
					utcString = string.Concat("-", strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
				}
			}
			string dmtfDateTime = ((System.Int32)(date.Year)).ToString().PadLeft(4, '0');
			dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Month)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Day)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Hour)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Minute)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Second)).ToString().PadLeft(2, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, ".");
			System.DateTime dtTemp = new System.DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
			long microsec = ((long)((((date.Ticks - dtTemp.Ticks)
						* 1000)
						/ System.TimeSpan.TicksPerMillisecond)));
			string strMicrosec = ((System.Int64)(microsec)).ToString();
			if ((strMicrosec.Length > 6))
			{
				strMicrosec = strMicrosec.Substring(0, 6);
			}
			dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
			dmtfDateTime = string.Concat(dmtfDateTime, utcString);
			return dmtfDateTime;
		}
	}
}
