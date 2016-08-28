using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Services
{
    internal sealed class ExitService : IExitService
    {
        private readonly Action exit;
        public ExitService(Action exit)
        {
            this.exit = exit;
        }

        public void Exit()
        {
            exit();
        }
    }
}
