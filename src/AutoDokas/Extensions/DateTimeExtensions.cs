namespace AutoDokas.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo LithuanianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Vilnius");

    public static DateTime ToLithuanianTime(this DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, LithuanianTimeZone);
    }

    public static DateTime ToUtcFromLithuanian(this DateTime lithuanianDateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(lithuanianDateTime, LithuanianTimeZone);
    }
}
