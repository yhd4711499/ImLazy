using System.Linq;

namespace ImLazy.ControlPanel.Converters
{
    public class ShortPathConverter : StringConverterBase
    {
        protected override object Convert(string value)
        {
            var splits = value.Split('\\');
            return splits.Length > 2 ? splits.Last() : value;
        }
    }
}
