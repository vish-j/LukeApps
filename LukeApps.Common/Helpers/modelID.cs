namespace LukeApps.Common.Helpers
{
    public static class modelID
    {
        /// <summary>
        /// Checks if inputted ID is an unusable long number. maxValue can be passed to the function.
        /// Default maxValue = 99999
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="maxValue"></param>
        /// <returns>true if its a bad number</returns>
        public static bool check(long? ID, long maxValue = long.MaxValue)
        {
            if (ID == null || ID < 1 || ID > maxValue)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if inputted ID is an unusable byte number. maxValue can be passed to the function.
        /// Default maxValue = 255
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="maxValue"></param>
        /// <returns>true if its a bad number</returns>
        public static bool check(byte? ID, long maxValue = 255)
        {
            if (ID == null || ID < 1 || ID > maxValue)
            {
                return true;
            }
            return false;
        }
    }
}