using Microsoft.Extensions.Logging;
using MoooCart.lib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Services
{
    public class SerilogerAppAdapter<T>(ILogger<T> logger) : IAppLoger<T> where T : class
    {
        public void LogError (Exception ex , string messege)
        {
            logger.LogError(ex, messege);
        }

        public void LogInfo(string messege)
        {
            logger.LogWarning(messege);
        }

        public void LoginInformation(string messege)
        {
            logger.LogInformation(messege);
        }
    }


}