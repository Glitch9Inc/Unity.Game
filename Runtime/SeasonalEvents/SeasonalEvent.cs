
using Glitch9.Apis.Google.Sheets;

namespace Glitch9.Game
{

    [Sheets(typeof(SeasonalEvent))]
    public enum SeasonalEvent
    {
        Unset = -1,

        /// <summary>
        /// No Event
        /// </summary>
        None = 0,

        /// <summary>
        /// 1st January - 신년
        /// </summary>
        NewYearsDay,

        /// <summary>
        /// 14th February - 발렌타인데이
        /// </summary>
        ValentinesDay,

        /// <summary>
        /// March 17th - St. Patrick's Day
        /// </summary>
        StPatricksDay,

        /// <summary>
        /// Typically falls in March or April - 부활절
        /// </summary>
        Easter,

        /// <summary>
        /// 1st April - 만우절
        /// </summary>
        AprilFoolsDay,

        /// <summary>
        /// Late January to February, date varies - 설날 (Korean Lunar New Year)
        /// </summary>
        LunarNewYear,

        /// <summary>
        /// May - Embrace the warmth and new beginnings
        /// </summary>
        SpringFestival,

        /// <summary>
        /// 4th July - Fireworks and freedom celebrations
        /// </summary>
        FourthOfJuly,

        /// <summary>
        /// July - Sun, sand, and beach fun
        /// </summary>
        SummerBeachParty,

        /// <summary>
        /// October 31st - Spooky and fun-filled night
        /// </summary>
        Halloween,

        /// <summary>
        /// 4th Thursday of November - Feast and gratitude
        /// </summary>
        Thanksgiving,

        /// <summary>
        /// 25th December - Festive season's joy
        /// </summary>
        Christmas,

        /// <summary>
        /// 킥스타터 투자자를 위한 이벤트
        /// </summary>
        Kickstarter,
    }
}