import CalendarDayCard from "./calendar-day";
import MonthCalendarClient from "./month-calendar-client";
import { fetchMonthEventsAction } from "./actions";
import type { CalendarDay, CalendarEvent } from "./types";

const MONTH_FORMATTER = new Intl.DateTimeFormat("en-US", { month: "long" });

function getMonthRange(year: number, month: number) {
	const start = new Date(Date.UTC(year, month, 1));
	const end = new Date(Date.UTC(year, month + 1, 0));

	return {
		daysInMonth: end.getUTCDate(),
		offset: start.getUTCDay(),
	};
}

function buildCalendarDays(year: number, month: number): CalendarDay[] {
	const { daysInMonth, offset } = getMonthRange(year, month);
	const leadingEmptyDays = Array.from({ length: offset }, (_, index) => ({
		key: `empty-${index}`,
		dayNumber: null,
		isoDate: null,
	}));
	const visibleDays = Array.from({ length: daysInMonth }, (_, index) => {
		const dayNumber = index + 1;
		const isoDate = new Date(Date.UTC(year, month, dayNumber)).toISOString().slice(0, 10);

		return {
			key: isoDate,
			dayNumber,
			isoDate,
		};
	});

	return [...leadingEmptyDays, ...visibleDays];
}

type MonthCalendarProps = {
	selectedMonth: number;
	selectedYear: number;
};

export default async function MonthCalendar({
	selectedMonth,
	selectedYear,
}: MonthCalendarProps) {
	let events: CalendarEvent[] = [];
	let error: string | null = null;

	try {
		events = await fetchMonthEventsAction({
			month: selectedMonth,
			year: selectedYear,
		});
	} catch (loadError) {
		console.error("Failed to load events:", loadError);
		error = "Unable to load events for this month.";
	}

	const days = buildCalendarDays(selectedYear, selectedMonth);
	const eventsByDate = events.reduce<Record<string, CalendarEvent[]>>((groupedEvents, event) => {
		if (!groupedEvents[event.dateVenueUtc]) {
			groupedEvents[event.dateVenueUtc] = [];
		}

		groupedEvents[event.dateVenueUtc].push(event);
		return groupedEvents;
	}, {});
	const visibleMonthLabel = MONTH_FORMATTER.format(
		new Date(Date.UTC(selectedYear, selectedMonth, 1)),
	);

	return (
		<section className="mx-auto flex w-full max-w-7xl flex-col gap-8 px-4 py-8 text-slate-950 md:py-12">
			<div className="flex flex-col gap-6 rounded-4xl border border-slate-200 bg-white p-6 shadow-[0_24px_80px_rgba(15,23,42,0.08)] md:p-8">
				<div className="flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
					<div className="space-y-3">
						<p className="text-xs font-black uppercase tracking-[0.35em] text-red-600">
							Event Calendar
						</p>
						<div>
							<h1 className="text-4xl font-black uppercase tracking-tight md:text-6xl">
								{visibleMonthLabel} {selectedYear}
							</h1>
							<p className="mt-2 max-w-2xl text-sm font-medium text-slate-600 md:text-base">
								Each day shows the events scheduled at their venue date in UTC.
							</p>
						</div>
					</div>

					<MonthCalendarClient
						selectedMonth={selectedMonth}
						selectedYear={selectedYear}
					/>
				</div>

				{error ? (
					<div className="rounded-3xl border border-red-200 bg-red-50 px-4 py-6 text-sm font-semibold text-red-700">
						{error}
					</div>
				) : (
					<div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-7">
						{days.map((day) => (
							<CalendarDayCard
								key={day.key}
								day={day}
								events={day.isoDate ? eventsByDate[day.isoDate] ?? [] : []}
							/>
						))}
					</div>
				)}
			</div>
		</section>
	);
}
