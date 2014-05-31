using System;

namespace ImLazy.SDK.Base.Contracts
{
    public interface IEditViewBranch : IEditView
    {
        event Action<IEditViewBranch, string> OnSelected;
        void Add(IEditView view);
    }
}
