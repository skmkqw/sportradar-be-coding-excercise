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
