using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
