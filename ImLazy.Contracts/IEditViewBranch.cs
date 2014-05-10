using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImLazy.Contracts
{
    public interface IEditViewBranch : IEditView
    {
        event Action<IEditViewBranch, string> OnSelected;
        void Add(IEditView view);
    }
}
