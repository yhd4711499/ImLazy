using System.IO;

namespace ImLazy.ControlPanel.Converters
{
    public class GetFolderConverter : StringConverterBase
    {
        protected override object Convert(string value)
        {
            return Path.GetDirectoryName(value);
        }
    }
}
