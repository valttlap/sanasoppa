// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sanasoppa.API.Entities;

#nullable disable

namespace Sanasoppa.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:game_state", "not_started,waiting_dasher,giving_explanations,dasher_valuing_explanations,voting_explanations,calculating_points,game_ended");

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    game_state = table.Column<GameState>(type: "game_state", nullable: false, defaultValue: GameState.NotStarted)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    connection_id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    is_online = table.Column<bool>(type: "boolean", nullable: false),
                    is_host = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    total_points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    game_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_players", x => x.id);
                    table.ForeignKey(
                        name: "fk_players_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "rounds",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    is_current = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    dasher_id = table.Column<int>(type: "integer", nullable: false),
                    word = table.Column<string>(type: "text", nullable: false),
                    game_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rounds", x => x.id);
                    table.ForeignKey(
                        name: "fk_rounds_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rounds_players_dasher_id",
                        column: x => x.dasher_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "explanations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    round_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    is_right = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_explanations", x => x.id);
                    table.ForeignKey(
                        name: "fk_explanations_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_explanations_rounds_round_id",
                        column: x => x.round_id,
                        principalTable: "rounds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    round_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    explanation_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_votes", x => x.id);
                    table.ForeignKey(
                        name: "fk_votes_explanations_explanation_id",
                        column: x => x.explanation_id,
                        principalTable: "explanations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_votes_players_player_id",
                        column: x => x.player_id,
                        principalTable: "players",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_votes_rounds_round_id",
                        column: x => x.round_id,
                        principalTable: "rounds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_explanations_player_id",
                table: "explanations",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_explanations_round_id",
                table: "explanations",
                column: "round_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_name",
                table: "games",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_players_connection_id",
                table: "players",
                column: "connection_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_players_game_id",
                table: "players",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_rounds_dasher_id",
                table: "rounds",
                column: "dasher_id");

            migrationBuilder.CreateIndex(
                name: "ix_rounds_game_id",
                table: "rounds",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "ix_votes_explanation_id",
                table: "votes",
                column: "explanation_id");

            migrationBuilder.CreateIndex(
                name: "ix_votes_player_id",
                table: "votes",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "ix_votes_round_id",
                table: "votes",
                column: "round_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropTable(
                name: "explanations");

            migrationBuilder.DropTable(
                name: "rounds");

            migrationBuilder.DropTable(
                name: "players");

            migrationBuilder.DropTable(
                name: "games");
        }
    }
}
