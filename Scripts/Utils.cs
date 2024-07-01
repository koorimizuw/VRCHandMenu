
using UdonSharp;
using System.Reflection;


namespace Yamadev.VRCHandMenu.Script
{
    public static class Utils
    {
        public static void SetVariable<T>(this UdonSharpBehaviour self, string symbolName, T value)
        {
            var type = self.GetType();
            var field = type.GetField(symbolName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(self, value);
        }
    }
}