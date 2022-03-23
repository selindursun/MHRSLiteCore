using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteEntityLayer.Models;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.QuartzWork
{
    [DisallowConcurrentExecution]
    public class DenemeJob : IJob
    {
        private readonly ILogger<DenemeJob> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public DenemeJob(ILogger<DenemeJob> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Deneme myDeneme = new Deneme()
                {
                    CreatedDate = DateTime.Now,
                    Mesaj = $"Mesaj eklendi. {DateTime.Now.ToShortDateString()}"
                };
                _unitOfWork.DenemeRepository.Add(myDeneme);
                _logger.LogInformation("DenemeJob worked");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {

                //log
                return Task.CompletedTask;
            }
        }
    }
}
