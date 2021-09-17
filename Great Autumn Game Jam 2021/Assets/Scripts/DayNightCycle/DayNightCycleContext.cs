namespace Assets.Scripts.DayNightCycle
{
    public static class DayNightCycleContext
    {
        /// <summary>
        /// Current Time of day
        /// </summary>
        private static int _timeOfDay = 6;

        /// <summary>
        /// Get or set the current time of day.
        /// </summary>
        public static int TimeOfDay { get => GetTimeOfDay(); set => SetTimeOfDay(value); }

        /// <summary>
        /// Get the current time of day
        /// </summary>
        /// <returns>Current time of day since last time update</returns>
        private static int GetTimeOfDay() => _timeOfDay;

        /// <summary>
        /// Set the current time of day.
        /// </summary>
        /// <param name="time">Time of day. Can be larger than 24</param>
        private static void SetTimeOfDay(int time)
        {
            if (time < 24)
            {
                _timeOfDay = time;
                return;
            }

            var quotient = time / 24;
            _timeOfDay = time - (quotient * 24);
        }
    }
}
