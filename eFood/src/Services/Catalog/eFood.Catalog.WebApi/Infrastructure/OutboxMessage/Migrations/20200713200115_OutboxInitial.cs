using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eFood.Catalog.WebApi.Infrastructure.OutboxMessage.Migrations
{
    public partial class OutboxInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CorrelationId = table.Column<Guid>(nullable: false),
                    TypeFullName = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    SerializedMessage = table.Column<string>(nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    ProcessingStart = table.Column<DateTime>(nullable: true),
                    ProcessedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages");
        }
    }
}
