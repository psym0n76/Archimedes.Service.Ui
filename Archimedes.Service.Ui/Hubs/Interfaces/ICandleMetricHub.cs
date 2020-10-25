﻿using System.Threading.Tasks;
using Archimedes.Library.Message.Dto;

namespace Archimedes.Service.Ui.Hubs
{
    public interface ICandleMetricHub
    {
        Task Add(CandleMetricDto value);
        Task Delete(CandleMetricDto value);
        Task Update(CandleMetricDto value); 
    }
}