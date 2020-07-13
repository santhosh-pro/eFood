using System;
using System.Threading;
using System.Threading.Tasks;
using eFood.Common.MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace eFood.Common.OutboxPattern
{
    public sealed class BackgroundMessagePublisher : IHostedService
    {
        private readonly ILogger<BackgroundMessagePublisher> _logger;
        private readonly OutboxMessageConfiguration _outboxConfig;
        private readonly IOutboxMessageAccessor _outboxMessageAccessor;
        private readonly IBusPublisher _publisher;
        private readonly TimeSpan _interval;
        private Timer _timer;

        public BackgroundMessagePublisher(ILogger<BackgroundMessagePublisher> logger,
            OutboxMessageConfiguration outboxConfig,
            IOutboxMessageAccessor outboxMessageAccessor,
            IBusPublisher publisher)
        {
            _logger = logger;
            _interval = TimeSpan.FromMilliseconds(outboxConfig.PollingIntervalMilliseconds);
            _outboxConfig = outboxConfig;
            _outboxMessageAccessor = outboxMessageAccessor;
            _publisher = publisher;
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
                OutboxMessage pendingMessage;
                while ((pendingMessage = await _outboxMessageAccessor.GetPendingMessage()) != null)
                {
                    var messageType = Type.GetType(pendingMessage.TypeFullName);
                    var messageBody = JsonConvert.DeserializeObject(pendingMessage.SerializedMessage, messageType);
                    await _publisher.Publish(messageBody, messageType);
                    await _outboxMessageAccessor.MarkAsProcessedAsync(pendingMessage);
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
