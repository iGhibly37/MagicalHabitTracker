using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.utils
{
    public class Utils
    {



        /// <summary>
        /// A function to calculate the next due date in UTC based on the periodicity, schedule, timezone, and current UTC time.
        /// </summary>
        /// <param name="periodicity"></param>
        /// <param name="schedule"></param>
        /// <param name="tz"></param>
        /// <param name="utcNow"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static DateTime? CalculateNextDueUtc(Periodicity periodicity, HabitSchedule schedule, TimeZoneInfo tz, DateTime utcNow)
        {
            DateTime localNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tz);

            DateTime localNext = periodicity switch
            {
                Periodicity.Daily => NextDaily(localNow, schedule.PreferredLocalTime),
                Periodicity.Weekly => NextWeekly(localNow, schedule.PreferredLocalTime, schedule.WeeklyDaysMask),
                _ => throw new NotSupportedException($"Periodcity '{periodicity}' not supported here.")
            };

            var localReminder = localNext.AddMinutes(-schedule.ReminderOffsetsMinutes);

            if (localReminder <= localNow)
            {
                localNext = periodicity switch
                {
                    Periodicity.Daily => NextDaily(localNow.AddDays(1), schedule.PreferredLocalTime),
                    Periodicity.Weekly => NextWeekly(localNow.AddDays(1), schedule.PreferredLocalTime, schedule.WeeklyDaysMask),
                    _ => localNext
                };
                localReminder = localNext.AddMinutes(-schedule.ReminderOffsetsMinutes);
            }

            return TimeZoneInfo.ConvertTimeToUtc(localReminder, tz);
        }



        /// <summary>
        /// A function to calculate the next daily occurrence of a preferred time.
        /// </summary>
        /// <param name="localNow"></param>
        /// <param name="pref"></param>
        /// <returns></returns>
        public static DateTime NextDaily(DateTime localNow, TimeOnly pref)
        {
            var candidate = localNow.Date + pref.ToTimeSpan();

            if (candidate <= localNow)
            {
                candidate = candidate.AddDays(1);
            }
            return candidate;
        }



        /// <summary>
        /// A function to calculate the next weekly occurrence of a preferred time based on a days mask.
        /// </summary>
        /// <param name="localNow"></param>
        /// <param name="pref"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static DateTime NextWeekly(DateTime localNow, TimeOnly pref, WeeklyDaysMask mask)
        {
            for (int i = 0; i < 7; i++)
            {
                var day = localNow.Date.AddDays(1);
                var dayOfWeek = localNow.DayOfWeek;
                if (IsInMask(dayOfWeek, mask))
                {
                    var candidate = day + pref.ToTimeSpan();
                    if ((candidate > localNow))
                    {
                        return candidate;
                    }
                }
            }

            return localNow.Date.AddDays(1) + pref.ToTimeSpan();
        }


        /// <summary>
        /// A function to check if a DayOfWeek is included in a WeeklyDaysMask.
        /// </summary>
        /// <param name="dow"></param>
        /// <param name="mask"></param>
        /// <returns>A boolean valor to indicate if that day is present inside the mask</returns>
        public static bool IsInMask(DayOfWeek dow, WeeklyDaysMask mask) => dow switch
        {
            DayOfWeek.Sunday => mask.HasFlag(WeeklyDaysMask.Sunday),
            DayOfWeek.Monday => mask.HasFlag(WeeklyDaysMask.Monday),
            DayOfWeek.Tuesday => mask.HasFlag(WeeklyDaysMask.Tuesday),
            DayOfWeek.Wednesday => mask.HasFlag(WeeklyDaysMask.Wednesday),
            DayOfWeek.Thursday => mask.HasFlag(WeeklyDaysMask.Thursday),
            DayOfWeek.Friday => mask.HasFlag(WeeklyDaysMask.Friday),
            DayOfWeek.Saturday => mask.HasFlag(WeeklyDaysMask.Saturday),
            _ => false
        };


        /// <summary>
        /// A function to get TimeZoneInfo from a timezone ID, throwing ArgumentException if invalid.
        /// </summary>
        /// <param name="timeZoneId"></param>
        /// <returns>Returns that specific TimeZoneInfo related to a timezon ID such as Europe/Rome</returns>
        /// <exception cref="ArgumentException"></exception>
        public static TimeZoneInfo GetTimeZoneInfo(string timeZoneId)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch
            {
                throw new ArgumentException($"TimeZoneId Invalid.{timeZoneId}", nameof(timeZoneId));
            }
        }
    }
}
