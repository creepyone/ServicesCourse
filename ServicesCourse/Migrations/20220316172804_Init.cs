using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicesCourse.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SexName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subsection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscetionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subsection_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ActivityStatus = table.Column<bool>(type: "bit", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Login);
                    table.ForeignKey(
                        name: "FK_User_UserType_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboutService = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubsectionId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Subsection_SubsectionId",
                        column: x => x.SubsectionId,
                        principalTable: "Subsection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SexId = table.Column<int>(type: "int", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Login);
                    table.ForeignKey(
                        name: "FK_UserProfile_Sex_SexId",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfile_User_Login",
                        column: x => x.Login,
                        principalTable: "User",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => new { x.Login, x.AccessTime });
                    table.ForeignKey(
                        name: "FK_History_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_History_User_Login",
                        column: x => x.Login,
                        principalTable: "User",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Section",
                columns: new[] { "Id", "SectionName" },
                values: new object[] { 1, "Общее" });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "SexName" },
                values: new object[,]
                {
                    { 1, "Мужской" },
                    { 2, "Женский" }
                });

            migrationBuilder.InsertData(
                table: "UserType",
                columns: new[] { "Id", "UserTypeName" },
                values: new object[,]
                {
                    { 1, "Администратор" },
                    { 2, "Пользователь" }
                });

            migrationBuilder.InsertData(
                table: "Subsection",
                columns: new[] { "Id", "SectionId", "SubscetionName" },
                values: new object[] { 1, 1, "Общее" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Login", "ActivityStatus", "Password", "UserTypeId" },
                values: new object[] { "admin", true, "admin", 1 });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Login", "ActivityStatus", "Password", "UserTypeId" },
                values: new object[] { "user", true, "user", 2 });

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[] { "Login", "BirthDate", "Email", "Name", "Patronymic", "PhoneNumber", "SexId", "Surname" },
                values: new object[] { "admin", null, null, null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[] { "Login", "BirthDate", "Email", "Name", "Patronymic", "PhoneNumber", "SexId", "Surname" },
                values: new object[] { "user", null, null, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_History_ServiceId",
                table: "History",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_SubsectionId",
                table: "Service",
                column: "SubsectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subsection_SectionId",
                table: "Subsection",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserTypeId",
                table: "User",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_SexId",
                table: "UserProfile",
                column: "SexId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Sex");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Subsection");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropTable(
                name: "Section");
        }
    }
}
