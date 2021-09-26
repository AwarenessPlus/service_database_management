using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace database_Services.Migrations
{
    public partial class Services_Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    AuthenticationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.AuthenticationID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Medic",
                columns: table => new
                {
                    MedicID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicDataUserID = table.Column<int>(type: "int", nullable: true),
                    AuthenticationDataAuthenticationID = table.Column<int>(type: "int", nullable: true),
                    Rotation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medic", x => x.MedicID);
                    table.ForeignKey(
                        name: "FK_Medic_Authentication_AuthenticationDataAuthenticationID",
                        column: x => x.AuthenticationDataAuthenticationID,
                        principalTable: "Authentication",
                        principalColumn: "AuthenticationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medic_User_MedicDataUserID",
                        column: x => x.MedicDataUserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pacient",
                columns: table => new
                {
                    PacientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacientInfoUserID = table.Column<int>(type: "int", nullable: true),
                    Bloodgroup = table.Column<int>(type: "int", nullable: true),
                    Rh = table.Column<int>(type: "int", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacient", x => x.PacientID);
                    table.ForeignKey(
                        name: "FK_Pacient_User_PacientInfoUserID",
                        column: x => x.PacientInfoUserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Procedure",
                columns: table => new
                {
                    ProcedureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Asa = table.Column<int>(type: "int", nullable: false),
                    MedicID = table.Column<int>(type: "int", nullable: false),
                    PatientPacientID = table.Column<int>(type: "int", nullable: true),
                    VideoRecord = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedure", x => x.ProcedureID);
                    table.ForeignKey(
                        name: "FK_Procedure_Medic_MedicID",
                        column: x => x.MedicID,
                        principalTable: "Medic",
                        principalColumn: "MedicID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Procedure_Pacient_PatientPacientID",
                        column: x => x.PatientPacientID,
                        principalTable: "Pacient",
                        principalColumn: "PacientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medic_AuthenticationDataAuthenticationID",
                table: "Medic",
                column: "AuthenticationDataAuthenticationID");

            migrationBuilder.CreateIndex(
                name: "IX_Medic_MedicDataUserID",
                table: "Medic",
                column: "MedicDataUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Pacient_PacientInfoUserID",
                table: "Pacient",
                column: "PacientInfoUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_MedicID",
                table: "Procedure",
                column: "MedicID");

            migrationBuilder.CreateIndex(
                name: "IX_Procedure_PatientPacientID",
                table: "Procedure",
                column: "PatientPacientID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Procedure");

            migrationBuilder.DropTable(
                name: "Medic");

            migrationBuilder.DropTable(
                name: "Pacient");

            migrationBuilder.DropTable(
                name: "Authentication");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
