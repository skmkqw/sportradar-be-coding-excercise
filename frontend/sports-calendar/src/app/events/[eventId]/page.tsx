import Link from "next/link";
import { fetchEventDetailsAction } from "@/components/calendar/actions";
import type { EventMatchIncident } from "@/components/calendar/types";

type EventPageProps = {
	params: Promise<{
		eventId: string;
	}>;
};

function formatStatus(status: string) {
	return status.replace(/_/g, " ");
}

function formatDate(date: string) {
	return new Intl.DateTimeFormat("en-US", {
		month: "long",
		day: "numeric",
		year: "numeric",
		timeZone: "UTC",
	}).format(new Date(`${date}T00:00:00Z`));
}

function formatIncidentTeamName(teamId: string, homeTeamId: string, awayTeamId: string) {
	if (teamId === homeTeamId) {
		return "Home";
	}

	if (teamId === awayTeamId) {
		return "Away";
	}

	return "Team";
}

function IncidentItem({
	incident,
	homeTeamId,
	awayTeamId,
}: {
	incident: EventMatchIncident;
	homeTeamId: string;
	awayTeamId: string;
}) {
	return (
		<div className="rounded-2xl border border-slate-300 bg-slate-50 px-4 py-3">
			<div className="flex items-center justify-between gap-3">
				<p className="text-sm font-bold uppercase tracking-wide text-slate-950">
					{incident.type}
				</p>
				<p className="text-sm font-black text-red-600">{incident.matchMinute}&apos;</p>
			</div>
			<p className="mt-2 text-sm text-slate-600">
				{formatIncidentTeamName(incident.teamId, homeTeamId, awayTeamId)} side
			</p>
		</div>
	);
}

export default async function EventPage({ params }: EventPageProps) {
	const { eventId } = await params;

	try {
		const event = await fetchEventDetailsAction(eventId);
		const incidents = event.result?.matchIncidents ?? [];
		const periodScores = event.result?.periodScores ?? [];

		return (
			<section className="mx-auto flex w-full max-w-7xl flex-col gap-6 px-4 py-8 text-slate-950 md:gap-8 md:py-12">
				<Link
					href="/"
					className="inline-flex w-fit items-center rounded-full border border-slate-400 bg-white px-4 py-2 text-sm font-bold uppercase tracking-[0.2em] text-slate-700 transition hover:border-red-500 hover:text-red-600"
				>
					Back to calendar
				</Link>

				<div className="overflow-hidden rounded-4xl border border-slate-300 bg-white shadow-[0_24px_80px_rgba(15,23,42,0.08)]">
					<div className="bg-slate-950 px-6 py-8 text-white md:px-8 md:py-10">
						<p className="text-xs font-black uppercase tracking-[0.35em] text-red-400">
							{event.competition?.name ?? "Competition"} • {formatStatus(event.status)}
						</p>
						<h1 className="mt-4 text-3xl font-black uppercase tracking-tight md:text-5xl">
							{event.name}
						</h1>
						<p className="mt-4 max-w-3xl text-sm text-white/75 md:text-base">
							{event.description ?? "No event description available yet."}
						</p>

						<div className="mt-8 grid gap-4 rounded-[1.75rem] border border-white/20 bg-white/5 p-4 md:grid-cols-3 md:p-6">
							<div className="rounded-3xl border border-white/20 bg-black/10 p-4 text-center">
								<p className="text-xs font-black uppercase tracking-[0.25em] text-white/55">
									Home
								</p>
								<p className="mt-3 text-2xl font-black md:text-3xl">{event.homeTeam.name}</p>
							</div>
							<div className="rounded-3xl border border-red-400/50 bg-red-500/10 p-4 text-center">
								<p className="text-xs font-black uppercase tracking-[0.3em] text-red-200">
									Score
								</p>
								<p className="mt-3 text-4xl font-black tracking-tight md:text-5xl">
									{event.result ? `${event.result.homeScore} : ${event.result.awayScore}` : "vs"}
								</p>
								<p className="mt-2 text-xs font-bold uppercase tracking-[0.25em] text-white/65">
									{formatDate(event.dateVenueUtc)} • {event.timeVenueUtc} UTC
								</p>
							</div>
							<div className="rounded-3xl border border-white/20 bg-black/10 p-4 text-center">
								<p className="text-xs font-black uppercase tracking-[0.25em] text-white/55">
									Away
								</p>
								<p className="mt-3 text-2xl font-black md:text-3xl">{event.awayTeam.name}</p>
							</div>
						</div>
					</div>

					<div className="grid gap-6 px-6 py-6 md:px-8 md:py-8 xl:grid-cols-[1.6fr_1fr]">
						<div className="space-y-6">
							<div className="grid gap-4 md:grid-cols-2">
								<div className="rounded-[1.75rem] border border-slate-300 bg-slate-50 p-5">
									<p className="text-xs font-black uppercase tracking-[0.3em] text-red-600">
										Match Info
									</p>
									<div className="mt-4 space-y-3 text-sm text-slate-600">
										<p>
											<span className="font-bold text-slate-950">Stage:</span>{" "}
											{event.stage?.name ?? "TBD"}
										</p>
										<p>
											<span className="font-bold text-slate-950">Venue:</span>{" "}
											{event.stadium?.name ?? "TBD"}
										</p>
										<p>
											<span className="font-bold text-slate-950">Season:</span> {event.season}
										</p>
										<p>
											<span className="font-bold text-slate-950">Updated:</span>{" "}
											{new Date(event.updatedAtUtc).toLocaleString("en-US", {
												timeZone: "UTC",
												dateStyle: "medium",
												timeStyle: "short",
											})}{" "}
											UTC
										</p>
									</div>
								</div>

								<div className="rounded-[1.75rem] border border-slate-300 bg-slate-50 p-5">
									<p className="text-xs font-black uppercase tracking-[0.3em] text-red-600">
										Teams
									</p>
									<div className="mt-4 space-y-4 text-sm text-slate-600">
										<div>
											<p className="font-bold text-slate-950">{event.homeTeam.officialName}</p>
											<p>{event.homeTeam.abbreviation.trim()} • {event.homeTeam.countryCode}</p>
										</div>
										<div>
											<p className="font-bold text-slate-950">{event.awayTeam.officialName}</p>
											<p>{event.awayTeam.abbreviation.trim()} • {event.awayTeam.countryCode}</p>
										</div>
									</div>
								</div>
							</div>

							<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5">
								<p className="text-xs font-black uppercase tracking-[0.3em] text-red-600">
									Period Scores
								</p>
								{periodScores.length > 0 ? (
									<div className="mt-4 grid gap-3 sm:grid-cols-2">
										{periodScores.map((periodScore) => (
											<div
												key={periodScore.id}
												className="rounded-2xl border border-slate-300 bg-slate-50 px-4 py-3"
											>
												<p className="text-sm font-bold text-slate-950">
													Period {periodScore.periodNumber}
												</p>
												<p className="mt-2 text-2xl font-black tracking-tight text-slate-950">
													{periodScore.homeScore} : {periodScore.awayScore}
												</p>
											</div>
										))}
									</div>
								) : (
									<p className="mt-4 text-sm text-slate-500">No period scores available.</p>
								)}
							</div>

							<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5">
								<p className="text-xs font-black uppercase tracking-[0.3em] text-red-600">
									Match Summary
								</p>
								<p className="mt-4 text-sm leading-7 text-slate-600">
									{event.result?.message ?? "No summary message available for this event."}
								</p>
							</div>
						</div>

						<div className="rounded-[1.75rem] border border-slate-300 bg-slate-50 p-5">
							<p className="text-xs font-black uppercase tracking-[0.3em] text-red-600">
								Match Incidents
							</p>
							{incidents.length > 0 ? (
								<div className="mt-4 space-y-3">
									{incidents
										.slice()
										.sort((left, right) => left.matchMinute - right.matchMinute)
										.map((incident) => (
											<IncidentItem
												key={incident.id}
												incident={incident}
												homeTeamId={event.homeTeam.id}
												awayTeamId={event.awayTeam.id}
											/>
										))}
								</div>
							) : (
								<p className="mt-4 text-sm text-slate-500">No incidents recorded.</p>
							)}
						</div>
					</div>
				</div>
			</section>
		);
	} catch (error) {
		console.error("Failed to load event details:", error);

		return (
			<section className="mx-auto flex min-h-[50vh] w-full max-w-4xl items-center justify-center px-4 py-12">
				<div className="w-full rounded-4xl border border-red-300 bg-white p-10 text-center text-slate-950 shadow-[0_24px_80px_rgba(15,23,42,0.08)]">
					<p className="text-xs font-black uppercase tracking-[0.35em] text-red-600">
						Event Details
					</p>
					<h1 className="mt-4 text-3xl font-black uppercase tracking-tight md:text-4xl">
						Unable to load event
					</h1>
					<p className="mt-4 text-sm font-medium text-slate-600 md:text-base">
						We couldn&apos;t load details for event{" "}
						<span className="font-bold text-slate-950">{eventId}</span>.
					</p>
					<Link
						href="/"
						className="mt-6 inline-flex items-center rounded-full border border-slate-400 px-4 py-2 text-sm font-bold uppercase tracking-[0.2em] text-slate-700 transition hover:border-red-500 hover:text-red-600"
					>
						Return to calendar
					</Link>
				</div>
			</section>
		);
	}
}
