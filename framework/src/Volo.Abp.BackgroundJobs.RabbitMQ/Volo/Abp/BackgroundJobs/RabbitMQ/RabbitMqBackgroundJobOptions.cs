﻿using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class RabbitMqBackgroundJobOptions
    {
        /// <summary>
        /// Key: Job Args Type
        /// </summary>
        public Dictionary<Type, JobQueueConfiguration> JobQueues { get; }

        /// <summary>
        /// Default value: "AbpBackgroundJobs.".
        /// </summary>
        public string DefaultQueueNamePrefix { get; set; }

        public RabbitMqBackgroundJobOptions()
        {
            JobQueues = new Dictionary<Type, JobQueueConfiguration>();
            DefaultQueueNamePrefix = "AbpBackgroundJobs.";
        }
    }
}
