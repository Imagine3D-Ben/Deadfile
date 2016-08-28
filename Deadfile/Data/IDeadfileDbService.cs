using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Data
{
    public interface IDeadfileDbService
    {
        void Connect(string connectionString);
    }
}
