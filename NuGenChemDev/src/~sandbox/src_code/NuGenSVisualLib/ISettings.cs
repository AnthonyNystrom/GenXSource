namespace NuGenSVisualLib.Settings
{
    public interface ISettings
    {
        object GetSettingObj(string token);
        int GetSettingInt32(string token);
        string GetSettingString(string token);
        object this[string key] { get; }
        bool TryGetValue(string key, out object value);
    }
}