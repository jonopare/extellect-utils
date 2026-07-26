namespace Extellect.Nuvo
{
    public static class FileSystem
    {
        public static string UserProfile(string relative)
        {
            return Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%"), relative);
        }
    }
}
