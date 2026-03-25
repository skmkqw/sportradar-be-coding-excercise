import Link from "next/link";
import type { CalendarDay, CalendarEvent } from "./types";

type CalendarDayProps = {
	day: CalendarDay;
	events: CalendarEvent[];
};

export default function CalendarDayCard({ day, events }: CalendarDayProps) {
	if (!day.isoDate) {
		return (
			<div
				aria-hidden="true"
				className="hidden min-h-44 rounded-3xl border border-transparent bg-transparent lg:block"
			/>
		);
	}

	return (
		<article className="flex min-h-44 flex-col rounded-3xl border border-slate-300 bg-white p-4 shadow-[0_0_0_1px_rgba(148,163,184,0.12)]">
			<div className="mb-4 flex items-center justify-between">
				<span className="text-2xl font-black tracking-tight text-slate-950">{day.dayNumber}</span>
				<span className="rounded-full bg-slate-950 px-2.5 py-1 text-[10px] font-black uppercase tracking-[0.2em] text-white">
					{events.length} event{events.length === 1 ? "" : "s"}
				</span>
			</div>

			<div className="space-y-2">
				{events.length > 0 ? (
					events.map((event) => (
						<Link
							key={event.id}
							href={`/events/${event.id}`}
							className="block rounded-2xl border border-red-200 bg-red-50/40 px-3 py-2 transition hover:border-red-400 hover:bg-red-50"
						>
							<p className="text-sm font-bold leading-snug text-slate-950">{event.name}</p>
							<p className="mt-1 text-[11px] font-black uppercase tracking-[0.2em] text-red-600">
								{event.timeVenueUtc}
							</p>
						</Link>
					))
				) : (
					<p className="text-sm font-medium text-slate-400">No events scheduled.</p>
				)}
			</div>
		</article>
	);
}
