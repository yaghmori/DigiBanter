namespace DigiBanter.Shared.Extensions;
public static class TimeZoneExtensions
{
    #region Utc handled

    public static DateTime ToTimeZoneDateTime(this DateTimeOffset dateTimeOffset, TimeZoneInfo timeZone)
    {

        DateTimeOffset dto = dateTimeOffset;

        // Convert the DateTimeOffset to the specified time zone
        DateTimeOffset targetTimeZoneOffset = TimeZoneInfo.ConvertTime(dto, timeZone);
        // Return the DateTime part of the DateTimeOffset
        return targetTimeZoneOffset.DateTime;
    }
    public static DateTime? ToTimeZoneDateTime(this DateTimeOffset? dateTimeOffset, TimeZoneInfo timeZone)
    {
        if (dateTimeOffset == null)
        {
            return null; // or throw new ArgumentNullException(nameof(dateTimeOffset));
        }

        DateTimeOffset dto = dateTimeOffset.Value;

        // Convert the DateTimeOffset to the specified time zone
        DateTimeOffset targetTimeZoneOffset = TimeZoneInfo.ConvertTime(dto, timeZone);
        // Return the DateTime part of the DateTimeOffset
        return targetTimeZoneOffset.DateTime;
    }

    #endregion
    /// <summary>
    /// Converts a DateTime in a specified time zone to a DateTimeOffset in UTC.
    /// </summary>
    /// <param name="dateTime">The DateTime to convert.</param>
    /// <param name="timeZone">The TimeZoneInfo representing the source time zone.</param>
    /// <returns>A DateTimeOffset representing the input DateTime in UTC.</returns>
    public static DateTimeOffset ToUtcDateTimeOffset(this DateTime dateTime, TimeZoneInfo timeZone)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
        {
            // If the DateTime is already in UTC, return as DateTimeOffset in UTC
            return new DateTimeOffset(dateTime, TimeSpan.Zero);
        }

        if (dateTime.Kind == DateTimeKind.Local)
        {
            throw new ArgumentException("The DateTime must not be of Local kind. Use Unspecified or Utc.", nameof(dateTime));
        }

        // Get the offset for the time zone at the specified time
        TimeSpan offset = timeZone.GetUtcOffset(dateTime);

        // Create a DateTimeOffset in the specified time zone
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, offset);

        // Convert to UTC
        return dateTimeOffset.ToUniversalTime();
    }

    /// <summary>
    /// Converts a nullable DateTime in a specified time zone to a nullable DateTimeOffset in UTC.
    /// </summary>
    /// <param name="dateTime">The nullable DateTime to convert.</param>
    /// <param name="timeZone">The TimeZoneInfo representing the source time zone.</param>
    /// <returns>A nullable DateTimeOffset representing the input DateTime in UTC, or null if the input is null.</returns>
    public static DateTimeOffset? ToUtcDateTimeOffset(this DateTime? dateTime, TimeZoneInfo timeZone)
    {
        if (dateTime == null)
        {
            return null;
        }

        // Use the non-nullable method for conversion
        return dateTime.Value.ToUtcDateTimeOffset(timeZone);
    }


}
