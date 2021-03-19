namespace System.Drawing
{
    public static partial class ColorExtensions
    {
        // UNTESTED: https://stackoverflow.com/a/16245278
        public static Color Invert(this Color color) =>
            Color.FromArgb(color.ToArgb() ^ 0xFFFFFF);
    }
}