using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCareAppointments.Migrations
{
    /// <inheritdoc />
    public partial class Appointmententity_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HealthCareProfessionalId = table.Column<int>(type: "int", nullable: false),
                    AppointmentStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_HealthCareProfessionals_HealthCareProfessionalId",
                        column: x => x.HealthCareProfessionalId,
                        principalTable: "HealthCareProfessionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_HealthCareProfessionalId",
                table: "Appointments",
                column: "HealthCareProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
