using MoooCart.lib.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Base
{
    public interface IDashboardService
    {
        Task<DtoDashboardStatistics> GetStatisticsAsync();
    }
}
