using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.Hangfire
{
    public interface IBackgroundJobExecuter
    {
        Task ExecuteAsync(JobExecutionContext context);
    }
}
