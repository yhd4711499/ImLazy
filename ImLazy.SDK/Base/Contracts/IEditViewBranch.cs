using System;

namespace ImLazy.SDK.Base.Contracts
{
    public interface IEditViewBranch : SDK.Base.Contracts.IEditView
    {
        event Action<IEditViewBranch, string> OnSelected;
        void Add(SDK.Base.Contracts.IEditView view);
    }
}
