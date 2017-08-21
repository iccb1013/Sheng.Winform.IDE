//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    /// <summary>
    /// Represents the extended format for the cache.
    /// </summary>    
    /// <remarks>
    /// Extended format syntax : <br/><br/>
    /// 
    /// Minute       - 0-59 <br/>
    /// Hour         - 0-23 <br/>
    /// Day of month - 1-31 <br/>
    /// Month        - 1-12 <br/>
    /// Day of week  - 0-6 (Sunday is 0) <br/>
    /// Wildcards    - * means run every <br/>
    /// Examples: <br/>
    /// * * * * *    - expires every minute<br/>
    /// 5 * * * *    - expire 5th minute of every hour <br/>
    /// * 21 * * *   - expire every minute of the 21st hour of every day <br/>
    /// 31 15 * * *  - expire 3:31 PM every day <br/>
    /// 7 4 * * 6    - expire Saturday 4:07 AM <br/>
    /// 15 21 4 7 *  - expire 9:15 PM on 4 July <br/>
    ///	Therefore 6 6 6 6 1 means:
    ///	•	have we crossed/entered the 6th minute AND
    ///	•	have we crossed/entered the 6th hour AND 
    ///	•	have we crossed/entered the 6th day AND
    ///	•	have we crossed/entered the 6th month AND
    ///	•	have we crossed/entered A MONDAY?
    ///
    ///	Therefore these cases should exhibit these behaviors:
    ///
    ///	getTime = DateTime.Parse( "02/20/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/07/2003 07:07:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 1", getTime, nowTime );
    ///	TRUE, ALL CROSSED/ENTERED
    ///			
    ///	getTime = DateTime.Parse( "02/20/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/07/2003 07:07:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 5", getTime, nowTime );
    ///	TRUE
    ///			
    ///	getTime = DateTime.Parse( "02/20/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/06/2003 06:06:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 *", getTime, nowTime );
    ///	TRUE
    ///	
    ///			
    ///	getTime = DateTime.Parse( "06/05/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/06/2003 06:06:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 5", getTime, nowTime );
    ///	TRUE
    ///						
    ///	getTime = DateTime.Parse( "06/05/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/06/2005 05:06:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 1", getTime, nowTime );
    ///	TRUE
    ///						
    ///	getTime = DateTime.Parse( "06/05/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/06/2003 05:06:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 1", getTime, nowTime );
    ///	FALSE:  we did not cross 6th hour, nor did we cross Monday
    ///						
    ///	getTime = DateTime.Parse( "06/05/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/06/2003 06:06:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 5", getTime, nowTime );
    ///	TRUE, we cross/enter Friday
    ///
    ///
    ///	getTime = DateTime.Parse( "06/05/2003 04:06:55 AM" );
    ///	nowTime = DateTime.Parse( "06/06/2003 06:06:00 AM" );
    ///	isExpired = ExtendedFormatHelper.IsExtendedExpired( "6 6 6 6 1", getTime, nowTime );
    ///	FALSE:  we don’t cross Monday but all other conditions satisfied
    /// </remarks>
    public class ExtendedFormat
    {
        private readonly string format;

        private static readonly char argumentDelimiter = Convert.ToChar(",", CultureInfo.CurrentUICulture);
        private static readonly char wildcardAll = Convert.ToChar("*", CultureInfo.CurrentUICulture);
        private static readonly char refreshDelimiter = Convert.ToChar(" ", CultureInfo.CurrentUICulture);

        private int[] minutes;
        private int[] hours;
        private int[] days;
        private int[] months;
        private int[] daysOfWeek;

		/// <summary>
		/// Validates the format.
		/// </summary>
		/// <param name="timeFormat">
		/// The format to validate.
		/// </param>
        public static void Validate(string timeFormat)
        {
            ExtendedFormat ef = new ExtendedFormat(timeFormat);
			ef.Initialize();
        }

		/// <summary>
		/// Initializes a new instnace of the <see cref="ExtendedFormat"/> class with a format.
		/// </summary>
		/// <param name="format">The extended format time.</param>
        public ExtendedFormat(string format)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            this.format = format;
			Initialize();
        }

		private void Initialize()
		{
			string[] parsedFormat = format.Trim().Split(refreshDelimiter);
			if (parsedFormat.Length != 5)
			{
				throw new ArgumentOutOfRangeException(Resources.ExceptionInvalidExtendedFormatArguments);
			}
			ParseMinutes(parsedFormat);
			ParseHours(parsedFormat);
			ParseDays(parsedFormat);
			ParseMonths(parsedFormat);
			ParseDaysOfWeek(parsedFormat);
		}

		/// <summary>
		/// Gets the exteneded format.
		/// </summary>
		/// <value>
		/// The extended format.
		/// </value>
        public string Format
        {
            get { return this.format; }
        }

		/// <summary>
		/// Gets the minutes to expire.
		/// </summary>
		/// <value>
		/// The minutes to expire.
		/// </value>
		/// <remarks>
		/// This returns a copy of the integer array of minutes to expire.
		/// </remarks>
        public int[] GetMinutes()
        {
            return (int[])minutes.Clone();
        }

		/// <summary>
		/// Gets the hours to expire.
		/// </summary>
		/// <value>
		/// The hours to expire.
		/// </value>
		/// <remarks>
		/// This returns a copy of the integer array of hours to expire.
		/// </remarks>
        public int[] GetHours()
        {
            return (int[])hours.Clone();
        }

		/// <summary>
		/// Gets the days to expire.
		/// </summary>
		/// <value>
		/// The days to expire.
		/// </value>
		/// <remarks>
		/// This returns a copy of the integer array of days to expire.
		/// </remarks>
        public int[] GetDays()
        {
            return (int[])days.Clone();
        }

		/// <summary>
		/// Gets the months of the year to expire.
		/// </summary>
		/// <value>
		/// The months of the year to expire.
		/// </value>
		/// <remarks>
		/// This returns a copy of the integer array of months to expire.
		/// </remarks>
		public int[] GetMonths()
        {
            return (int[])months.Clone();
        }

		/// <summary>
		/// Gets the days of the week to expire.
		/// </summary>
		/// <value>
		/// The days of the week to expire.
		/// </value>
		/// <remarks>
		/// This returns a copy of the integer array of the days of the week to expire.
		/// </remarks>
        public int[] GetDaysOfWeek()
        {
            return (int[])daysOfWeek.Clone();
        }

		/// <summary>
		/// Determines if should expire every minute.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if should expire every minute; otherwise, <see langword="false"/>.
		/// </value>
        public bool ExpireEveryMinute
        {
            get { return this.minutes[0] == -1; }
        }

		/// <summary>
		/// Determines if item should expire every day.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if should expire every day; otherwise, <see langword="false"/>.
		/// </value>
        public bool ExpireEveryDay
        {
            get { return this.days[0] == -1; }
        }

		/// <summary>
		/// Determines if should expire every hour.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if should expire every hour; otherwise, <see langword="false"/>.
		/// </value>
        public bool ExpireEveryHour
        {
            get { return this.hours[0] == -1; }
        }

		/// <summary>
		/// Determines if should expire every month.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if should expire every month; otherwise, <see langword="false"/>.
		/// </value>
        public bool ExpireEveryMonth
        {
            get { return this.months[0] == -1; }
        }

		/// <summary>
		/// Determines if should expire every day of the week.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if should expire every day of the week; otherwise, <see langword="false"/>.
		/// </value>
        public bool ExpireEveryDayOfWeek
        {
            get { return this.daysOfWeek[0] == -1; }
        }

        private void ParseMinutes(string[] parsedFormat)
        {
            this.minutes = ParseValueToInt(parsedFormat[0]);
            foreach (int minute in this.minutes)
            {
                if ((minute > 59) || (minute < -1))
                {
                    throw new ArgumentOutOfRangeException("format", Resources.ExceptionRangeMinute);
                }
            }
        }

        private void ParseHours(string[] parsedFormat)
        {
            this.hours = ParseValueToInt(parsedFormat[1]);
            foreach (int hour in this.hours)
            {
                if ((hour > 23) || (hour < -1))
                {
                    throw new ArgumentOutOfRangeException("format", Resources.ExceptionRangeHour);
                }
            }
        }

        private void ParseDays(string[] parsedFormat)
        {
            this.days = ParseValueToInt(parsedFormat[2]);
            foreach (int day in this.days)
            {
                if ((day > 31) || (day < -1))
                {
                    throw new ArgumentOutOfRangeException("format", Resources.ExceptionRangeDay);
                }
            }
        }

        private void ParseMonths(string[] parsedFormat)
        {
            this.months = ParseValueToInt(parsedFormat[3]);
            foreach (int month in this.months)
            {
                if ((month > 12) || (month < -1))
                {
                    throw new ArgumentOutOfRangeException("format", Resources.ExceptionRangeMonth);
                }
            }
        }

        private void ParseDaysOfWeek(string[] parsedFormat)
        {
            this.daysOfWeek = ParseValueToInt(parsedFormat[4]);
            foreach (int dayOfWeek in this.daysOfWeek)
            {
                if ((dayOfWeek > 6) || (dayOfWeek < -1))
                {
                    throw new ArgumentOutOfRangeException("format", Resources.ExceptionRangeDay);
                }
            }
        }

        private static int[] ParseValueToInt(string value)
        {
            int[] result;

            if (value.IndexOf(wildcardAll) != -1)
            {
                result = new int[1];
                result[0] = -1;
            }
            else
            {
                string[] values = value.Split(argumentDelimiter);
                result = new int[values.Length];
                for (int index = 0; index < values.Length; index++)
                {
                    result[index] = int.Parse(values[index], CultureInfo.CurrentUICulture);
                }
            }
            return result;
        }

		/// <summary>
		/// Determines if the time has expired.
		/// </summary>
		/// <param name="getTime">The time to compare.</param>
		/// <param name="nowTime">The current time.</param>
		/// <returns>
		/// <see langword="true"/> if the time is expired; otherwise, <see langword="false"/>.
		/// </returns>
        public bool IsExpired(DateTime getTime, DateTime nowTime)
        {
            // Removes the seconds to provide better precission on calculations
            getTime = getTime.AddSeconds(getTime.Second * -1);
            nowTime = nowTime.AddSeconds(nowTime.Second * -1);
            if (nowTime.Subtract(getTime).TotalMinutes < 1)
            {
                return false;
            }
            foreach (int minute in minutes)
            {
                foreach (int hour in hours)
                {
                    foreach (int day in days)
                    {
                        foreach (int month in months)
                        {
                            // Set the expiration date parts
                            int expirMinute = minute == -1 ? getTime.Minute : minute;
                            int expirHour = hour == -1 ? getTime.Hour : hour;
                            int expirDay = day == -1 ? getTime.Day : day;
                            int expirMonth = month == -1 ? getTime.Month : month;
                            int expirYear = getTime.Year;

                            // Adjust when wildcards are set
                            if ((minute == -1) && (hour != -1))
                            {
                                expirMinute = 0;
                            }
                            if ((hour == -1) && (day != -1))
                            {
                                expirHour = 0;
                            }
                            if ((minute == -1) && (day != -1))
                            {
                                expirMinute = 0;
                            }
                            if ((day == -1) && (month != -1))
                            {
                                expirDay = 1;
                            }
                            if ((hour == -1) && (month != -1))
                            {
                                expirHour = 0;
                            }
                            if ((minute == -1) && (month != -1))
                            {
                                expirMinute = 0;
                            }

                            if (DateTime.DaysInMonth(expirYear, expirMonth) < expirDay)
                            {
                                //								if (expirMonth == 12) 
                                //								{
                                //									expirMonth = 1;
                                //									expirYear++;
                                //								} 
                                //								else 
                                //								{
                                expirMonth++;
                                //								}
                                expirDay = 1;
                            }

                            // Create the date with the adjusted parts
                            DateTime expTime = new DateTime(
                                expirYear, expirMonth, expirDay,
                                expirHour, expirMinute, 0);

                            // Adjust when expTime is before getTime
                            if (expTime < getTime)
                            {
                                if ((month != -1) && (getTime.Month >= month))
                                {
                                    expTime = expTime.AddYears(1);
                                }
                                else if ((day != -1) && (getTime.Day >= day))
                                {
                                    expTime = expTime.AddMonths(1);
                                }
                                else if ((hour != -1) && (getTime.Hour >= hour))
                                {
                                    expTime = expTime.AddDays(1);
                                }
                                else if ((minute != -1) && (getTime.Minute >= minute))
                                {
                                    expTime = expTime.AddHours(1);
                                }
                            }

                            // Is Expired?
                            if (ExpireEveryDayOfWeek)
                            {
                                if (nowTime >= expTime)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                // Validate WeekDay								
                                foreach (int weekDay in daysOfWeek)
                                {
                                    DateTime tmpTime = getTime;
                                    tmpTime = tmpTime.AddHours(-1 * tmpTime.Hour);
                                    tmpTime = tmpTime.AddMinutes(-1 * tmpTime.Minute);
                                    while ((int)tmpTime.DayOfWeek != weekDay)
                                    {
                                        tmpTime = tmpTime.AddDays(1);
                                    }
                                    if ((nowTime >= tmpTime) && (nowTime >= expTime))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
