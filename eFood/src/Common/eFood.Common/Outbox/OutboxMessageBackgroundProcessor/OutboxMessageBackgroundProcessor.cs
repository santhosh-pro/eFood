using System;
using System.Threading;
using System.Threading.Tasks;
using eFood.Common.MassTransit;
using eFood.Common.Outbox.MessageAccessors;
using eFood.Common.Outbox.ServiceCollections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace eFood.Common.Outbox.OutboxMessageBackgroundProcessor
{
    public sealed class OutboxMessageBackgroundProcessor : IHostedService
    {
        private readonly ILogger<OutboxMessageBackgroundProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBusPublisher _publisher;
        private Timer _timer;
        private readonly TimeSpan _interval;

        public OutboxMessageBackgroundProcessor(ILogger<OutboxMessageBackgroundProcessor> logger,
            IServiceScopeFactory serviceScopeFactory, IBusPublisher publisher, OutboxMessageConfiguration outboxConfig)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _publisher = publisher;
            _interval = TimeSpan.FromMilliseconds(outboxConfig.PollingDbIntervalMilliseconds);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SendOutboxMessages, null, TimeSpan.Zero, _interval);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void SendOutboxMessages(object state)
        {
            _ = SendOutboxMessagesAsync();
        }

        private async Task SendOutboxMessagesAsync()
        {
            var jobId = Guid.NewGuid();

            _logger.LogDebug($"Started processing outbox messages with generated Id: {jobId}");

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var outboxAccessor = scope.ServiceProvider.GetRequiredService<IOutboxAccessor>();
                OutboxMessage pendingMessage;
                while ((pendingMessage = await outboxAccessor.GetPendingMessage()) != null)
                {
                    var messageType = Type.GetType(pendingMessage.TypeFullName);
                    var messageBody = JsonConvert.DeserializeObject(pendingMessage.SerializedMessage, messageType);
                    await _publisher.Publish(messageBody, messageType);
                    await outboxAccessor.MarkAsProcessedAsync(pendingMessage);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Error whe processing outbox messages with generated Id: {jobId}, message: {e.Message}");
            }


            _logger.LogDebug($"End processing outbox messages with generated Id: {jobId}");
        }
    }
}
