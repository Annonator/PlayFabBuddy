using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabBuddy.PlayFabHelpers.Util.Repository
{
    internal interface IRepository<T>
    {
        public void Save(List<T> toSave);
        public List<T> Get();
    }
}
