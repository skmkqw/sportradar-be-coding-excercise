"use server";

import { revalidatePath } from "next/cache";
import type { CalendarEvent, EventDetails, EventsResponse } from "./types";

const DEFAULT_PAGE_SIZE = 10;
const API_BASE_URL = "http://localhost:5000";

function getMonthRange(year: number, month: number) {
	const start = new Date(Date.UTC(year, month, 1));
	const end = new Date(Date.UTC(year, month + 1, 0));

	return {
		startDate: start.toISOString().slice(0, 10),
		endDate: end.toISOString().slice(0, 10),
	};
}

function createApiUrl(pathname: string, searchParams?: Record<string, string>) {
	const url = new URL(pathname, API_BASE_URL);

	if (searchParams) {
		for (const [key, value] of Object.entries(searchParams)) {
			url.searchParams.set(key, value);
		}
	}

	return url;
}

async function fetchJson<T>(url: URL) {
	const response = await fetch(url, {
		method: "GET",
		headers: {
			"Content-Type": "application/json",
		},
		cache: "no-store",
	});

	if (!response.ok) {
		throw new Error(`Request failed with status ${response.status}`);
	}

	return (await response.json()) as T;
}

async function sendJson<T>(url: URL, body: unknown) {
	const response = await fetch(url, {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
		},
		body: JSON.stringify(body),
		cache: "no-store",
	});

	if (!response.ok) {
		const responseText = await response.text();
		throw new Error(responseText || `Request failed with status ${response.status}`);
	}

	if (response.status === 204) {
		return null as T;
	}

	return (await response.json()) as T;
}

function getStringValue(formData: FormData, key: string) {
	const value = formData.get(key);

	return typeof value === "string" ? value.trim() : "";
}

function getOptionalStringValue(formData: FormData, key: string) {
	const value = getStringValue(formData, key);

	return value.length > 0 ? value : undefined;
}

function getOptionalNumberValue(formData: FormData, key: string) {
	const value = getOptionalStringValue(formData, key);

	if (!value) {
		return undefined;
	}

	const parsed = Number(value);

	return Number.isNaN(parsed) ? undefined : parsed;
}

function buildTeamPayload(formData: FormData, side: "home" | "away") {
	const prefix = side === "home" ? "Home" : "Away";
	const existingId = getOptionalStringValue(formData, `existing${prefix}TeamId`);

	if (existingId) {
		return {
			[`existing${prefix}TeamId`]: existingId,
		};
	}

	const name = getOptionalStringValue(formData, `${side}TeamName`);
	const officialName = getOptionalStringValue(formData, `${side}TeamOfficialName`);
	const abbreviation = getOptionalStringValue(formData, `${side}TeamAbbreviation`);
	const countryCode = getOptionalStringValue(formData, `${side}TeamCountryCode`);
	const stagePosition = getOptionalNumberValue(formData, `${side}TeamStagePosition`);
	const sportId = getOptionalStringValue(formData, `${side}TeamSportId`);

	if (
		name &&
		officialName &&
		abbreviation &&
		countryCode &&
		stagePosition !== undefined &&
		sportId
	) {
		return {
			[`new${prefix}Team`]: {
				name,
				officialName,
				abbreviation,
				countryCode,
				stagePosition,
				sportId,
			},
		};
	}

	return {};
}

function buildStadiumPayload(formData: FormData) {
	const existingStadiumId = getOptionalStringValue(formData, "existingStadiumId");

	if (existingStadiumId) {
		return {
			existingStadiumId,
		};
	}

	const name = getOptionalStringValue(formData, "stadiumName");
	const countryCode = getOptionalStringValue(formData, "stadiumCountryCode");

	if (name && countryCode) {
		return {
			newStadium: {
				name,
				countryCode,
			},
		};
	}

	return {};
}

function buildStagePayload(formData: FormData) {
	const existingStageId = getOptionalStringValue(formData, "existingStageId");

	if (existingStageId) {
		return {
			existingStageId,
		};
	}

	const name = getOptionalStringValue(formData, "stageName");
	const ordering = getOptionalNumberValue(formData, "stageOrdering");

	if (name && ordering !== undefined) {
		return {
			newStage: {
				name,
				ordering,
			},
		};
	}

	return {};
}

function buildCompetitionPayload(formData: FormData) {
	const existingCompetitionId = getOptionalStringValue(formData, "existingCompetitionId");

	if (existingCompetitionId) {
		return {
			existingCompetitionId,
		};
	}

	const name = getOptionalStringValue(formData, "competitionName");
	const sportId = getOptionalStringValue(formData, "competitionSportId");

	if (name && sportId) {
		return {
			newCompetition: {
				name,
				sportId,
			},
		};
	}

	return {};
}

function buildResultPayload(formData: FormData) {
	const winnerSide = getOptionalStringValue(formData, "winnerSide");
	const message = getOptionalStringValue(formData, "resultMessage");
	const periodScores = [1, 2]
		.map((periodNumber) => {
			const homeScore = getOptionalNumberValue(formData, `period${periodNumber}HomeScore`);
			const awayScore = getOptionalNumberValue(formData, `period${periodNumber}AwayScore`);

			if (homeScore === undefined || awayScore === undefined) {
				return null;
			}

			return {
				periodNumber,
				homeScore,
				awayScore,
			};
		})
		.filter((score) => score !== null);
	const incidents = [1, 2, 3, 4]
		.map((index) => {
			const type = getOptionalStringValue(formData, `incident${index}Type`);
			const side = getOptionalStringValue(formData, `incident${index}Side`);
			const matchMinute = getOptionalNumberValue(formData, `incident${index}Minute`);

			if (!type || !side || matchMinute === undefined) {
				return null;
			}

			return {
				type,
				side,
				matchMinute,
			};
		})
		.filter((incident) => incident !== null);

	if (!winnerSide && !message && periodScores.length === 0 && incidents.length === 0) {
		return undefined;
	}

	return {
		winnerSide: winnerSide || undefined,
		message: message || undefined,
		periodScores,
		incidents,
	};
}

export async function fetchMonthEventsAction({
	month,
	year,
}: {
	month: number;
	year: number;
}) {
	const { startDate, endDate } = getMonthRange(year, month);
	let page = 1;
	let total = 0;
	const collectedEvents: CalendarEvent[] = [];

	do {
		const payload = await fetchJson<EventsResponse>(
			createApiUrl("/api/events", {
				page: String(page),
				pageSize: String(DEFAULT_PAGE_SIZE),
				startDate,
				endDate,
			}),
		);

		collectedEvents.push(...payload.events);
		total = payload.metadata.total;
		page += 1;
	} while (collectedEvents.length < total);

	return collectedEvents;
}

export async function fetchEventDetailsAction(eventId: string) {
	return fetchJson<EventDetails>(
		createApiUrl(`/api/events/${eventId}`),
	);
}

export type CreateEventActionState = {
	error: string | null;
	success: string | null;
};

export async function createEventAction(
	_previousState: CreateEventActionState,
	formData: FormData,
): Promise<CreateEventActionState> {
	try {
		const name = getStringValue(formData, "name");
		const season = Number(getStringValue(formData, "season"));
		const status = getStringValue(formData, "status");
		const description = getOptionalStringValue(formData, "description");
		const timeVenueUtc = getStringValue(formData, "timeVenueUtc");
		const dateVenueUtc = getStringValue(formData, "dateVenueUtc");

		if (!name || Number.isNaN(season) || !status || !timeVenueUtc || !dateVenueUtc) {
			return {
				error: "Please complete the required event fields before submitting.",
				success: null,
			};
		}

		const payload = {
			name,
			season,
			status,
			description: description ?? null,
			timeVenueUtc,
			dateVenueUtc: new Date(dateVenueUtc).toISOString(),
			...buildTeamPayload(formData, "home"),
			...buildTeamPayload(formData, "away"),
			...buildStadiumPayload(formData),
			...buildStagePayload(formData),
			...buildCompetitionPayload(formData),
			result: buildResultPayload(formData),
		};

		await sendJson(createApiUrl("/api/events"), payload);
		revalidatePath("/");

		return {
			error: null,
			success: "Event created successfully.",
		};
	} catch (error) {
		console.error("Failed to create event:", error);

		return {
			error: "Unable to create the event. Please check the form data and try again.",
			success: null,
		};
	}
}
