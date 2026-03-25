"use server";

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
