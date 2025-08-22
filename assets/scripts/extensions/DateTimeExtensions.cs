using System;

namespace SpaceInvaders.Assets.Scripts.Extensions;

public static class DateTimeExtensions
{
    public static long ToUnixMilliseconds(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
    }
}
