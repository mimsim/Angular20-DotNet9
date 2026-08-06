using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class FixMemberLikeForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes");

            migrationBuilder.AlterColumn<string>(
                name: "TargetMemberId",
                table: "Likes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "SourceMemberId",
                table: "Likes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Members_UserId",
                table: "Members",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes",
                column: "SourceMemberId",
                principalTable: "Members",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes",
                column: "TargetMemberId",
                principalTable: "Members",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Members_UserId",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "TargetMemberId",
                table: "Likes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "SourceMemberId",
                table: "Likes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_SourceMemberId",
                table: "Likes",
                column: "SourceMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Members_TargetMemberId",
                table: "Likes",
                column: "TargetMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
