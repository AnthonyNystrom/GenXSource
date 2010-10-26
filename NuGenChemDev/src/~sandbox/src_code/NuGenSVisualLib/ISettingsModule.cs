using System;
using System.Collections.Generic;
using System.Text;

namespace NuGenSVisualLib.Settings
{
    public interface ISettingsModule
    {
        void LoadModuleSettings(HashTableSettings settings);
        string Name { get; }
    }
}
