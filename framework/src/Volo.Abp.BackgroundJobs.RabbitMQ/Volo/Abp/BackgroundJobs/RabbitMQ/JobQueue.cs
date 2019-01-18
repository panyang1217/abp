<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class JobQueue<TArgs> : IJobQueue<TArgs>
    {
        private const string ChannelPrefix = "JobQueue.";

        protected BackgroundJobConfiguration JobConfiguration { get; }
        protected JobQueueConfiguration QueueConfiguration { get; }
        protected IChannelAccessor ChannelAccessor { get; private set; }
        protected EventingBasicConsumer Consumer { get; private set; }
        
        public ILogger<JobQueue<TArgs>> Logger { get; set; }

        protected BackgroundJobOptions BackgroundJobOptions { get; }
        protected RabbitMqBackgroundJobOptions RabbitMqBackgroundJobOptions { get; }
        protected IChannelPool ChannelPool { get; }
        protected IRabbitMqSerializer Serializer { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }

        protected AsyncLock SyncObj = new AsyncLock();
        protected bool IsDiposed { get; private set; }

        public JobQueue(
            IOptions<BackgroundJobOptions> backgroundJobOptions,
            IOptions<RabbitMqBackgroundJobOptions> rabbitMqBackgroundJobOptions,
            IChannelPool channelPool,
            IRabbitMqSerializer serializer,
            IBackgroundJobExecuter jobExecuter)
        {
            BackgroundJobOptions = backgroundJobOptions.Value;
            RabbitMqBackgroundJobOptions = rabbitMqBackgroundJobOptions.Value;
            Serializer = serializer;
            JobExecuter = jobExecuter;
            ChannelPool = channelPool;

            JobConfiguration = BackgroundJobOptions.GetJob(typeof(TArgs));
            QueueConfiguration = GetOrCreateJobQueueConfiguration();

            Logger = NullLogger<JobQueue<TArgs>>.Instance;
        }

        protected virtual JobQueueConfiguration GetOrCreateJobQueueConfiguration()
        {
            return RabbitMqBackgroundJobOptions.JobQueues.GetOrDefault(typeof(TArgs)) ??
                   new JobQueueConfiguration(
                       typeof(TArgs),
                       RabbitMqBackgroundJobOptions.DefaultQueueNamePrefix + JobConfiguration.JobName
                   );
        }

        public virtual async Task<string> EnqueueAsync(
            TArgs args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            CheckDisposed();

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();

                await PublishAsync(args, priority, delay);

                return null;
            }
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken = default)
        {
            CheckDisposed();

            if (!BackgroundJobOptions.IsJobExecutionEnabled)
            {
                return;
            }

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();
            }
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            Dispose();
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            if (IsDiposed)
            {
                return;
            }

            IsDiposed = true;

            ChannelAccessor?.Dispose();
        }

        protected virtual Task EnsureInitializedAsync()
        {
            if (ChannelAccessor != null)
            {
                return Task.CompletedTask;
            }

            ChannelAccessor = ChannelPool.Acquire(
                ChannelPrefix + QueueConfiguration.QueueName,
                QueueConfiguration.ConnectionName
            );

            var result = QueueConfiguration.Declare(ChannelAccessor.Channel);
            Logger.LogDebug($"RabbitMQ Queue '{QueueConfiguration.QueueName}' has {result.MessageCount} messages and {result.ConsumerCount} consumers.");

            if (BackgroundJobOptions.IsJobExecutionEnabled)
            {
                Consumer = new EventingBasicConsumer(ChannelAccessor.Channel);
                Consumer.Received += MessageReceived;

                //TODO: What BasicConsume returns?
                ChannelAccessor.Channel.BasicConsume(
                    queue: QueueConfiguration.QueueName,
                    autoAck: false,
                    consumer: Consumer
                );
            }

            return Task.CompletedTask;
        }

        protected virtual Task PublishAsync(
            TArgs args, 
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            //TODO: How to handle priority & delay?

            ChannelAccessor.Channel.BasicPublish(
                exchange: "",
                routingKey: QueueConfiguration.QueueName,
                basicProperties: CreateBasicPropertiesToPublish(),
                body: Serializer.Serialize(args)
            );

            return Task.CompletedTask;
        }

        protected virtual IBasicProperties CreateBasicPropertiesToPublish()
        {
            var properties = ChannelAccessor.Channel.CreateBasicProperties();
            properties.Persistent = true;
            return properties;
        }

        protected virtual void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            var context = new JobExecutionContext(
                JobConfiguration.JobType,
                Serializer.Deserialize(ea.Body, typeof(TArgs))
            );

            try
            {
                JobExecuter.Execute(context);
                ChannelAccessor.Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (BackgroundJobExecutionException)
            {
                //TODO: Reject like that?
                ChannelAccessor.Channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
            }
            catch (Exception)
            {
                //TODO: Reject like that?
                ChannelAccessor.Channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);
            }
        }

        protected void CheckDisposed()
        {
            if (IsDiposed)
            {
                throw new AbpException("This object is disposed!");
            }
        }
    }
=======
﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class JobQueue<TArgs> : IJobQueue<TArgs>
    {
        private const string ChannelPrefix = "JobQueue.";

        protected BackgroundJobConfiguration JobConfiguration { get; }
        protected JobQueueConfiguration QueueConfiguration { get; }
        protected IChannelAccessor ChannelAccessor { get; private set; }
        protected EventingBasicConsumer Consumer { get; private set; }
        
        public ILogger<JobQueue<TArgs>> Logger { get; set; }

        protected BackgroundJobOptions BackgroundJobOptions { get; }
        protected RabbitMqBackgroundJobOptions RabbitMqBackgroundJobOptions { get; }
        protected IChannelPool ChannelPool { get; }
        protected IRabbitMqSerializer Serializer { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        protected AsyncLock SyncObj = new AsyncLock();
        protected bool IsDiposed { get; private set; }

        public JobQueue(
            IOptions<BackgroundJobOptions> backgroundJobOptions,
            IOptions<RabbitMqBackgroundJobOptions> rabbitMqBackgroundJobOptions,
            IChannelPool channelPool,
            IRabbitMqSerializer serializer,
            IBackgroundJobExecuter jobExecuter,
            IServiceScopeFactory serviceScopeFactory)
        {
            BackgroundJobOptions = backgroundJobOptions.Value;
            RabbitMqBackgroundJobOptions = rabbitMqBackgroundJobOptions.Value;
            Serializer = serializer;
            JobExecuter = jobExecuter;
            ServiceScopeFactory = serviceScopeFactory;
            ChannelPool = channelPool;

            JobConfiguration = BackgroundJobOptions.GetJob(typeof(TArgs));
            QueueConfiguration = GetOrCreateJobQueueConfiguration();

            Logger = NullLogger<JobQueue<TArgs>>.Instance;
        }

        protected virtual JobQueueConfiguration GetOrCreateJobQueueConfiguration()
        {
            return RabbitMqBackgroundJobOptions.JobQueues.GetOrDefault(typeof(TArgs)) ??
                   new JobQueueConfiguration(
                       typeof(TArgs),
                       RabbitMqBackgroundJobOptions.DefaultQueueNamePrefix + JobConfiguration.JobName
                   );
        }

        public virtual async Task<string> EnqueueAsync(
            TArgs args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            CheckDisposed();

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();

                await PublishAsync(args, priority, delay);

                return null;
            }
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken = default)
        {
            CheckDisposed();

            if (!BackgroundJobOptions.IsJobExecutionEnabled)
            {
                return;
            }

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();
            }
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            Dispose();
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            if (IsDiposed)
            {
                return;
            }

            IsDiposed = true;

            ChannelAccessor?.Dispose();
        }

        protected virtual Task EnsureInitializedAsync()
        {
            if (ChannelAccessor != null)
            {
                return Task.CompletedTask;
            }

            ChannelAccessor = ChannelPool.Acquire(
                ChannelPrefix + QueueConfiguration.QueueName,
                QueueConfiguration.ConnectionName
            );

            var result = QueueConfiguration.Declare(ChannelAccessor.Channel);
            Logger.LogDebug($"RabbitMQ Queue '{QueueConfiguration.QueueName}' has {result.MessageCount} messages and {result.ConsumerCount} consumers.");

            if (BackgroundJobOptions.IsJobExecutionEnabled)
            {
                Consumer = new EventingBasicConsumer(ChannelAccessor.Channel);
                Consumer.Received += MessageReceived;

                //TODO: What BasicConsume returns?
                ChannelAccessor.Channel.BasicConsume(
                    queue: QueueConfiguration.QueueName,
                    autoAck: false,
                    consumer: Consumer
                );
            }

            return Task.CompletedTask;
        }

        protected virtual Task PublishAsync(
            TArgs args, 
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            //TODO: How to handle priority & delay?

            ChannelAccessor.Channel.BasicPublish(
                exchange: "",
                routingKey: QueueConfiguration.QueueName,
                basicProperties: CreateBasicPropertiesToPublish(),
                body: Serializer.Serialize(args)
            );

            return Task.CompletedTask;
        }

        protected virtual IBasicProperties CreateBasicPropertiesToPublish()
        {
            var properties = ChannelAccessor.Channel.CreateBasicProperties();
            properties.Persistent = true;
            return properties;
        }

        protected virtual void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = new JobExecutionContext(
                    scope.ServiceProvider,
                    JobConfiguration.JobType,
                    Serializer.Deserialize(ea.Body, typeof(TArgs))
                );

                try
                {
                    JobExecuter.Execute(context);
                    ChannelAccessor.Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (BackgroundJobExecutionException)
                {
                    //TODO: Reject like that?
                    ChannelAccessor.Channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
                catch (Exception)
                {
                    //TODO: Reject like that?
                    ChannelAccessor.Channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);
                }
            }
        }

        protected void CheckDisposed()
        {
            if (IsDiposed)
            {
                throw new AbpException("This object is disposed!");
            }
        }
    }
>>>>>>> upstream/master
}