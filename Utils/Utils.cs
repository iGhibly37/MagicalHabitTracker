using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.utils
{
    public class Utils
    {

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

            return TimeZoneInfo.ConvertTimeFromUtc(localReminder, tz);
        }

        public static DateTime NextDaily(DateTime localNow, TimeOnly pref)
        {
            var candidate = localNow.Date + pref.ToTimeSpan();

            if (candidate <= localNow)
            {
                candidate = candidate.AddDays(1);
            }
            return candidate;
        }

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
