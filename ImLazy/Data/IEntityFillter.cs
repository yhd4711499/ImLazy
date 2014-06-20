using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImLazy.Data
{
    interface IEntityFillter<TEntity>
    {
        void FillEntity(TEntity entity);
    }
}
