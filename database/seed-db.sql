USE SportsCalendar;

-- 1. Clear existing data in correct dependency order
DELETE FROM MatchIncidents;
DELETE FROM PeriodScores;
DELETE FROM Results;
DELETE FROM Events;
DELETE FROM Teams;
DELETE FROM Stadiums;
DELETE FROM Stages;
DELETE FROM Competitions;
DELETE FROM Sports;
DELETE FROM Countries;

-- 2. Seed Sports
DECLARE @FootballId UNIQUEIDENTIFIER = NEWID();
DECLARE @TennisId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Sports (Id, Name) VALUES (@FootballId, 'Football');
INSERT INTO Sports (Id, Name) VALUES (@TennisId, 'Tennis');

-- 3. Seed Countries
INSERT INTO Countries (Code, Name) VALUES ('GBR', 'United Kingdom');
INSERT INTO Countries (Code, Name) VALUES ('ESP', 'Spain');
INSERT INTO Countries (Code, Name) VALUES ('USA', 'United States');
INSERT INTO Countries (Code, Name) VALUES ('FRA', 'France');

-- 4. Seed Competitions
DECLARE @PremierLeagueId UNIQUEIDENTIFIER = NEWID();
DECLARE @WimbledonId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Competitions (Id, Name, Slug, SportId) 
VALUES (@PremierLeagueId, 'Premier League', 'premier-league', @FootballId);

INSERT INTO Competitions (Id, Name, Slug, SportId) 
VALUES (@WimbledonId, 'Wimbledon', 'wimbledon', @TennisId);

-- 5. Seed Stages
DECLARE @GroupStageId UNIQUEIDENTIFIER = NEWID();
DECLARE @FinalId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Stages (Id, Name, Ordering) VALUES (@GroupStageId, 'Regular Season', 1);
INSERT INTO Stages (Id, Name, Ordering) VALUES (@FinalId, 'Final', 10);

-- 6. Seed Stadiums
DECLARE @AnfieldId UNIQUEIDENTIFIER = NEWID();
DECLARE @CenterCourtId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Stadiums (Id, Name, CountryCode) VALUES (@AnfieldId, 'Anfield', 'GBR');
INSERT INTO Stadiums (Id, Name, CountryCode) VALUES (@CenterCourtId, 'Centre Court', 'GBR');

-- 7. Seed Teams
DECLARE @LIV UNIQUEIDENTIFIER = NEWID();
DECLARE @MCI UNIQUEIDENTIFIER = NEWID();
DECLARE @NADAL UNIQUEIDENTIFIER = NEWID();
DECLARE @FEDERER UNIQUEIDENTIFIER = NEWID();

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@LIV, 'Liverpool', 'Liverpool FC', 'liverpool', 'LIV', 'GBR', @FootballId, 1);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@MCI, 'Man City', 'Manchester City FC', 'man-city', 'MCI', 'GBR', @FootballId, 2);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId)
VALUES (@NADAL, 'R. Nadal', 'Rafael Nadal', 'rafael-nadal', 'NAD', 'ESP', @TennisId);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId)
VALUES (@FEDERER, 'R. Federer', 'Roger Federer', 'roger-federer', 'FED', 'USA', @TennisId);

-- 8. Seed Events (Matches)
DECLARE @EventFootballId UNIQUEIDENTIFIER = NEWID();
DECLARE @EventTennisId UNIQUEIDENTIFIER = NEWID();
DECLARE @EventFutureId UNIQUEIDENTIFIER = NEWID();

-- Played Football Match
INSERT INTO Events (Id, Name, Season, Status, TimeVenueUtc, DateVenueUtc, HomeTeamId, AwayTeamId, StadiumId, StageId, CompetitionId)
VALUES (@EventFootballId, 'Liverpool vs Man City', 2024, 'played', '15:00:00', '2024-03-10', @LIV, @MCI, @AnfieldId, @GroupStageId, @PremierLeagueId);

-- Played Tennis Match
INSERT INTO Events (Id, Name, Season, Status, TimeVenueUtc, DateVenueUtc, HomeTeamId, AwayTeamId, StadiumId, StageId, CompetitionId)
VALUES (@EventTennisId, 'Nadal vs Federer', 2024, 'played', '14:00:00', '2024-07-14', @NADAL, @FEDERER, @CenterCourtId, @FinalId, @WimbledonId);

-- Scheduled Future Match
INSERT INTO Events (Id, Name, Season, Status, TimeVenueUtc, DateVenueUtc, HomeTeamId, AwayTeamId, StadiumId, StageId, CompetitionId)
VALUES (@EventFutureId, 'Man City vs Liverpool', 2024, 'scheduled', '19:00:00', '2026-05-01', @MCI, @LIV, NULL, @GroupStageId, @PremierLeagueId);

-- 9. Seed Results
DECLARE @ResultFootballId UNIQUEIDENTIFIER = NEWID();
DECLARE @ResultTennisId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Results (Id, EventId, WinnerId, Message)
VALUES (@ResultFootballId, @EventFootballId, @LIV, 'Liverpool won a tight game');

INSERT INTO Results (Id, EventId, WinnerId, Message)
VALUES (@ResultTennisId, @EventTennisId, @NADAL, 'Nadal wins in straight sets');

-- 10. Seed PeriodScores
-- Football: 1st Half 1-0, 2nd Half 1-1 (Total 2-1)
INSERT INTO PeriodScores (Id, ResultId, PeriodNumber, HomeScore, AwayScore) VALUES (NEWID(), @ResultFootballId, 1, 1, 0);
INSERT INTO PeriodScores (Id, ResultId, PeriodNumber, HomeScore, AwayScore) VALUES (NEWID(), @ResultFootballId, 2, 1, 1);

-- Tennis: Set 1 (6-4), Set 2 (7-6)
INSERT INTO PeriodScores (Id, ResultId, PeriodNumber, HomeScore, AwayScore) VALUES (NEWID(), @ResultTennisId, 1, 6, 4);
INSERT INTO PeriodScores (Id, ResultId, PeriodNumber, HomeScore, AwayScore) VALUES (NEWID(), @ResultTennisId, 2, 7, 6);

-- 11. Seed MatchIncidents
INSERT INTO MatchIncidents (Id, ResultId, TeamId, IncidentType, MatchMinute) 
VALUES (NEWID(), @ResultFootballId, @LIV, 'goal', 23);

INSERT INTO MatchIncidents (Id, ResultId, TeamId, IncidentType, MatchMinute) 
VALUES (NEWID(), @ResultFootballId, @MCI, 'yellowCard', 45);

INSERT INTO MatchIncidents (Id, ResultId, TeamId, IncidentType, MatchMinute) 
VALUES (NEWID(), @ResultFootballId, @LIV, 'goal', 88);
