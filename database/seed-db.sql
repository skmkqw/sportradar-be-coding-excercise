USE SportsCalendar;

-- 1. Clear existing data in dependency order
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

-- 2. Sports
DECLARE @FootballId UNIQUEIDENTIFIER = NEWID();
DECLARE @TennisId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Sports (Id, Name) VALUES (@FootballId, 'Football');
INSERT INTO Sports (Id, Name) VALUES (@TennisId, 'Tennis');

-- 3. Countries
INSERT INTO Countries (Code, Name) VALUES ('GBR', 'United Kingdom');
INSERT INTO Countries (Code, Name) VALUES ('ESP', 'Spain');
INSERT INTO Countries (Code, Name) VALUES ('ITA', 'Italy');
INSERT INTO Countries (Code, Name) VALUES ('DEU', 'Germany');
INSERT INTO Countries (Code, Name) VALUES ('FRA', 'France');
INSERT INTO Countries (Code, Name) VALUES ('SRB', 'Serbia');

-- 4. Competitions
DECLARE @PremierLeagueId UNIQUEIDENTIFIER = NEWID();
DECLARE @ContinentalCupId UNIQUEIDENTIFIER = NEWID();
DECLARE @SpringMastersId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Competitions (Id, Name, Slug, SportId)
VALUES (@PremierLeagueId, 'Premier League', 'premier-league', @FootballId);

INSERT INTO Competitions (Id, Name, Slug, SportId)
VALUES (@ContinentalCupId, 'Continental Cup', 'continental-cup', @FootballId);

INSERT INTO Competitions (Id, Name, Slug, SportId)
VALUES (@SpringMastersId, 'Spring Masters', 'spring-masters', @TennisId);

-- 5. Stages
DECLARE @Matchweek29Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Matchweek33Id UNIQUEIDENTIFIER = NEWID();
DECLARE @QuarterFinalId UNIQUEIDENTIFIER = NEWID();
DECLARE @SemiFinalId UNIQUEIDENTIFIER = NEWID();
DECLARE @FinalId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Stages (Id, Name, Ordering) VALUES (@Matchweek29Id, 'Matchweek 29', 29);
INSERT INTO Stages (Id, Name, Ordering) VALUES (@Matchweek33Id, 'Matchweek 33', 33);
INSERT INTO Stages (Id, Name, Ordering) VALUES (@QuarterFinalId, 'Quarter Final', 40);
INSERT INTO Stages (Id, Name, Ordering) VALUES (@SemiFinalId, 'Semi Final', 50);
INSERT INTO Stages (Id, Name, Ordering) VALUES (@FinalId, 'Final', 60);

-- 6. Stadiums
DECLARE @OldTraffordId UNIQUEIDENTIFIER = NEWID();
DECLARE @AllianzArenaId UNIQUEIDENTIFIER = NEWID();
DECLARE @ParcDesPrincesId UNIQUEIDENTIFIER = NEWID();
DECLARE @SanSiroId UNIQUEIDENTIFIER = NEWID();
DECLARE @RodLaverArenaId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Stadiums (Id, Name, CountryCode) VALUES (@OldTraffordId, 'Old Trafford', 'GBR');
INSERT INTO Stadiums (Id, Name, CountryCode) VALUES (@AllianzArenaId, 'Allianz Arena', 'DEU');
INSERT INTO Stadiums (Id, Name, CountryCode) VALUES (@ParcDesPrincesId, 'Parc des Princes', 'FRA');
INSERT INTO Stadiums (Id, Name, CountryCode) VALUES (@SanSiroId, 'San Siro', 'ITA');
INSERT INTO Stadiums (Id, Name, CountryCode) VALUES (@RodLaverArenaId, 'Rod Laver Arena', 'GBR');

-- 7. Teams
DECLARE @ArsenalId UNIQUEIDENTIFIER = NEWID();
DECLARE @ChelseaId UNIQUEIDENTIFIER = NEWID();
DECLARE @BayernId UNIQUEIDENTIFIER = NEWID();
DECLARE @JuventusId UNIQUEIDENTIFIER = NEWID();
DECLARE @PsgId UNIQUEIDENTIFIER = NEWID();
DECLARE @InterId UNIQUEIDENTIFIER = NEWID();
DECLARE @DjokovicId UNIQUEIDENTIFIER = NEWID();
DECLARE @AlcarazId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@ArsenalId, 'Arsenal', 'Arsenal Football Club', 'arsenal', 'ARS', 'GBR', @FootballId, 1);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@ChelseaId, 'Chelsea', 'Chelsea Football Club', 'chelsea', 'CHE', 'GBR', @FootballId, 4);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@BayernId, 'Bayern Munich', 'FC Bayern Munich', 'bayern-munich', 'BAY', 'DEU', @FootballId, 2);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@JuventusId, 'Juventus', 'Juventus Football Club', 'juventus', 'JUV', 'ITA', @FootballId, 3);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@PsgId, 'PSG', 'Paris Saint-Germain', 'psg', 'PSG', 'FRA', @FootballId, 1);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId, StagePosition)
VALUES (@InterId, 'Inter Milan', 'Football Club Internazionale Milano', 'inter-milan', 'INT', 'ITA', @FootballId, 2);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId)
VALUES (@DjokovicId, 'N. Djokovic', 'Novak Djokovic', 'novak-djokovic', 'DJO', 'SRB', @TennisId);

INSERT INTO Teams (Id, Name, OfficialName, Slug, Abbreviation, CountryCode, SportId)
VALUES (@AlcarazId, 'C. Alcaraz', 'Carlos Alcaraz', 'carlos-alcaraz', 'ALC', 'ESP', @TennisId);

-- 8. Events in March / April / May 2026
DECLARE @Event1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Event2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Event3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Event4Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Event5Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Event6Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Event7Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Events (Id, Name, Description, Season, Status, TimeVenueUtc, DateVenueUtc, HomeTeamId, AwayTeamId, StadiumId, StageId, CompetitionId)
VALUES
(@Event1Id, 'Arsenal vs Chelsea', 'London derby with title implications.', 2026, 'played', '15:00:00', '2026-03-26', @ArsenalId, @ChelseaId, @OldTraffordId, @Matchweek29Id, @PremierLeagueId),
(@Event2Id, 'Bayern Munich vs Juventus', 'Quarter-final first leg between two heavyweights.', 2026, 'played', '19:30:00', '2026-03-30', @BayernId, @JuventusId, @AllianzArenaId, @QuarterFinalId, @ContinentalCupId),
(@Event3Id, 'PSG vs Inter Milan', 'Attacking showdown in Paris.', 2026, 'played', '20:00:00', '2026-04-08', @PsgId, @InterId, @ParcDesPrincesId, @QuarterFinalId, @ContinentalCupId),
(@Event4Id, 'Chelsea vs Arsenal', 'Reverse fixture as the run-in heats up.', 2026, 'playing', '17:30:00', '2026-04-19', @ChelseaId, @ArsenalId, @OldTraffordId, @Matchweek33Id, @PremierLeagueId),
(@Event5Id, 'Juventus vs Bayern Munich', 'Second leg under the lights in Milan.', 2026, 'scheduled', '19:45:00', '2026-04-28', @JuventusId, @BayernId, @SanSiroId, @SemiFinalId, @ContinentalCupId),
(@Event6Id, 'N. Djokovic vs C. Alcaraz', 'Spring Masters final between two elite players.', 2026, 'played', '13:00:00', '2026-05-03', @DjokovicId, @AlcarazId, @RodLaverArenaId, @FinalId, @SpringMastersId),
(@Event7Id, 'Inter Milan vs PSG', 'Continental Cup semifinal decider.', 2026, 'scheduled', '20:00:00', '2026-05-12', @InterId, @PsgId, @SanSiroId, @SemiFinalId, @ContinentalCupId);

-- 9. Results
DECLARE @Result1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Result2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Result3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Result4Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Result6Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Results (Id, EventId, WinnerId, Message)
VALUES
(@Result1Id, @Event1Id, @ArsenalId, 'Arsenal controlled the second half and took all three points.'),
(@Result2Id, @Event2Id, @BayernId, 'Bayern earned a narrow advantage ahead of the return leg.'),
(@Result3Id, @Event3Id, @PsgId, 'PSG edged a fast-paced match with a late winner.'),
(@Result4Id, @Event4Id, NULL, 'The match is still underway with both sides pushing for control.'),
(@Result6Id, @Event6Id, @DjokovicId, 'Djokovic held firm in the key moments to lift the trophy.');

-- 10. Period scores
INSERT INTO PeriodScores (Id, ResultId, PeriodNumber, HomeScore, AwayScore) VALUES
(NEWID(), @Result1Id, 1, 1, 0),
(NEWID(), @Result1Id, 2, 1, 1),
(NEWID(), @Result2Id, 1, 1, 0),
(NEWID(), @Result2Id, 2, 1, 1),
(NEWID(), @Result3Id, 1, 1, 1),
(NEWID(), @Result3Id, 2, 1, 0),
(NEWID(), @Result4Id, 1, 0, 1),
(NEWID(), @Result4Id, 2, 1, 0),
(NEWID(), @Result6Id, 1, 6, 4),
(NEWID(), @Result6Id, 2, 7, 5);

-- 11. Match incidents
INSERT INTO MatchIncidents (Id, ResultId, TeamId, IncidentType, MatchMinute) VALUES
(NEWID(), @Result1Id, @ArsenalId, 'goal', 18),
(NEWID(), @Result1Id, @ChelseaId, 'yellowCard', 39),
(NEWID(), @Result1Id, @ChelseaId, 'goal', 62),
(NEWID(), @Result1Id, @ArsenalId, 'goal', 84),

(NEWID(), @Result2Id, @BayernId, 'goal', 27),
(NEWID(), @Result2Id, @JuventusId, 'yellowCard', 56),
(NEWID(), @Result2Id, @BayernId, 'goal', 79),

(NEWID(), @Result3Id, @InterId, 'goal', 11),
(NEWID(), @Result3Id, @PsgId, 'goal', 35),
(NEWID(), @Result3Id, @PsgId, 'goal', 88),

(NEWID(), @Result4Id, @ArsenalId, 'goal', 14),
(NEWID(), @Result4Id, @ChelseaId, 'yellowCard', 33),
(NEWID(), @Result4Id, @ChelseaId, 'goal', 57),

(NEWID(), @Result6Id, @DjokovicId, 'goal', 1),
(NEWID(), @Result6Id, @AlcarazId, 'yellowCard', 2);
