using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace eFood.Common.OutboxPattern.EntityFramework
{
    public class DapperOutboxAccessor : IOutboxMessageAccessor
    {
        private readonly string _connectionString;

        public DapperOutboxAccessor(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DbContext");
        }

        public async Task<OutboxMessage> GetPendingMessage()
        {
            return await GetEventFromQueue();
        }

        public Task MarkAsProcessedAsync(OutboxMessage outboxMessage)
        {
            outboxMessage.State = MessageState.Published;
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteAsync(
                    "update OutboxMessages set State = 2, ProcessedAt = GETUTCDATE() where Id = @id",
                    new {id = outboxMessage.Id});

            }
        }

        private async Task<OutboxMessage> GetEventFromQueue()
        {
            var sql = @"DECLARE @NextId uniqueidentifier
                        BEGIN TRANSACTION

                        --Find next available item available
                        SELECT TOP 1 @NextId = ID
                        FROM OutboxMessages WITH(UPDLOCK, READPAST)
                        WHERE State = 0 OR ( State = 1 and DATEDIFF(second, ProcessingStart, GETUTCDATE()) > 60)
                        ORDER BY ID ASC

                        --If found, flag it to prevent being picked up again
                        IF(@NextId IS NOT NULL)
                            BEGIN
                                UPDATE OutboxMessages
                                SET State = 1, ProcessingStart = GETUTCDATE()

                                WHERE ID = @NextId
                            END

                        COMMIT TRANSACTION

                        -- Now return the queue item, if we have one
                        IF(@NextId IS NOT NULL)
                            SELECT* FROM OutboxMessages WHERE ID = @NextId";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<OutboxMessage>(sql);
            }
        }
    }
}