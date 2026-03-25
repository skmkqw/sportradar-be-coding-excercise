"use server";

import { axiosInstance } from "@/lib/axios";
import type { CalendarEvent, EventsResponse } from "./types";

const DEFAULT_PAGE_SIZE = 10;

function getMonthRange(year: number, month: number) {
	const start = new Date(Date.UTC(year, month, 1));
	const end = new Date(Date.UTC(year, month + 1, 0));

	return {
		startDate: start.toISOString().slice(0, 10),
		endDate: end.toISOString().slice(0, 10),
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
		const { data: payload } = await axiosInstance.get<EventsResponse>("/api/events", {
			params: {
				page,
				pageSize: DEFAULT_PAGE_SIZE,
				startDate,
				endDate,
			},
		});

		collectedEvents.push(...payload.events);
		total = payload.metadata.total;
		page += 1;
	} while (collectedEvents.length < total);

	return collectedEvents;
}
