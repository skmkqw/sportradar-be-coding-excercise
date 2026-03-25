export type CalendarEvent = {
	id: string;
	name: string;
	dateVenueUtc: string;
	timeVenueUtc: string;
};

export type CalendarDay = {
	key: string;
	dayNumber: number | null;
	isoDate: string | null;
};

export type EventsResponse = {
	events: CalendarEvent[];
	metadata: {
		page: number;
		pageSize: number;
		total: number;
	};
};

export type EventTeam = {
	id: string;
	name: string;
	officialName: string;
	slug: string;
	abbreviation: string;
	countryCode: string;
	sportId: string;
	stagePosition: number;
	createdAtUtc: string;
	updatedAtUtc: string;
};

export type EventStadium = {
	id: string;
	name: string;
	countryCode: string;
	createdAtUtc: string;
};

export type EventStage = {
	id: string;
	name: string;
	ordering: number;
	createdAtUtc: string;
};

export type EventCompetition = {
	id: string;
	name: string;
	slug: string;
	sportId: string;
	createdAtUtc: string;
	updatedAtUtc: string;
};

export type EventPeriodScore = {
	id: string;
	periodNumber: number;
	homeScore: number;
	awayScore: number;
	createdAtUtc: string;
};

export type EventMatchIncident = {
	id: string;
	periodNumber: number;
	teamId: string;
	type: string;
	matchMinute: number;
	createdAtUtc: string;
};

export type EventResult = {
	id: string;
	homeScore: number;
	awayScore: number;
	periodScores: EventPeriodScore[];
	matchIncidents: EventMatchIncident[];
	winnerId: string;
	message: string | null;
	createdAtUtc: string;
	updatedAtUtc: string;
};

export type EventDetails = {
	id: string;
	name: string;
	description: string | null;
	season: number;
	status: string;
	timeVenueUtc: string;
	dateVenueUtc: string;
	homeTeam: EventTeam;
	awayTeam: EventTeam;
	stadium: EventStadium | null;
	stage: EventStage | null;
	competition: EventCompetition | null;
	result: EventResult | null;
	createdAtUtc: string;
	updatedAtUtc: string;
};
