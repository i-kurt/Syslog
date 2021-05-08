using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLogServer
{
    /// <summary>
    /// Logger interface.
    /// </summary>
    public interface ILogger
    {
        public Task startService();

        public void stopService();

        public void Dispose();
    }
}
