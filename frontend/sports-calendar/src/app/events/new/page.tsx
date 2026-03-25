import Link from "next/link";
import NewEventForm from "@/components/events/new-event-form";

export const metadata = {
	title: "Create Event | Sports Calendar",
	description: "Create a new sports event",
};

export default function NewEventPage() {
	return (
		<section className="mx-auto flex w-full max-w-7xl flex-col gap-6 px-4 py-8 text-slate-950 md:gap-8 md:py-12">
			<div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
				<div>
					<p className="text-xs font-black uppercase tracking-[0.35em] text-red-600">
						New Event
					</p>
					<h1 className="mt-3 text-3xl font-black uppercase tracking-tight md:text-5xl">
						Create a sports event
					</h1>
					<p className="mt-3 max-w-3xl text-sm text-slate-600 md:text-base">
						Provide either an existing child entity ID or the fields for a new entity.
						If both are filled, the existing ID will be used.
					</p>
				</div>

				<Link
					href="/"
					className="inline-flex w-fit items-center rounded-full border border-slate-400 bg-white px-4 py-2 text-sm font-bold uppercase tracking-[0.2em] text-slate-700 transition hover:border-red-500 hover:text-red-600"
				>
					Back to calendar
				</Link>
			</div>

			<div className="rounded-4xl border border-slate-300 bg-white p-5 shadow-[0_24px_80px_rgba(15,23,42,0.08)] md:p-8">
				<NewEventForm />
			</div>
		</section>
	);
}
