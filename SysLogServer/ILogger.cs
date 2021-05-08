using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLogServer
{
    public interface ILogger
    {
        public Task startService();

        public void stopService();

        public void Dispose();
    }
}
