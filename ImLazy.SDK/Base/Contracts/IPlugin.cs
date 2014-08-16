using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImLazy.SDK.Base.Contracts
{
    /// <summary>
    /// 功能插件的基本接口
    /// </summary>
    interface IPlugin : IAddin
    {
        void OnActivated();
    }
}
