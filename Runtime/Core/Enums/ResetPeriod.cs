namespace Glitch9
{
    /// <summary>
    /// The numbers mean the number of days to reset
    /// </summary>
    public enum ResetPeriod : int
    {
        /// <summary>
        /// None can be achievements, character missions, or anything that does not reset
        /// </summary>
        Never = 0,
        Daily = 1,
        Weekly = 7,
        Monthly = 30
    }

}