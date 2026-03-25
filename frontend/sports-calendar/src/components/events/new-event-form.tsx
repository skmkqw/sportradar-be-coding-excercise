"use client";

import { useActionState, useRef } from "react";
import { useFormStatus } from "react-dom";
import { createEventAction, type CreateEventActionState } from "@/components/calendar/actions";

const initialState: CreateEventActionState = {
	error: null,
	success: null,
};

const PLACEHOLDER_VALUES: Record<string, string> = {
	name: "Barcelona vs Real Madrid",
	season: "2026",
	status: "Played",
	timeVenueUtc: "15:00:00",
	dateVenueUtc: "2026-03-23T20:00:00Z",
	description: "Mid-season title decider at the Emirates.",

	existingHomeTeamId: "",
	homeTeamName: "Barcelona",
	homeTeamOfficialName: "Barcelona Football Club",
	homeTeamAbbreviation: "BRC",
	homeTeamCountryCode: "GBR",
	homeTeamStagePosition: "1",
	homeTeamSportId: "390F334A-5BE0-44E8-B853-18A25712E378",

	existingAwayTeamId: "",
	awayTeamName: "Real Madrid",
	awayTeamOfficialName: "Real Madrid Football Club",
	awayTeamAbbreviation: "RMD",
	awayTeamCountryCode: "GBR",
	awayTeamStagePosition: "2",
	awayTeamSportId: "390F334A-5BE0-44E8-B853-18A25712E378",

	existingCompetitionId: "",
	competitionName: "Legue of Champions",
	competitionSportId: "390F334A-5BE0-44E8-B853-18A25712E378",

	existingStageId: "",
	stageName: "Regular Season",
	stageOrdering: "1",
	
	existingStadiumId: "",
	stadiumName: "Emirates Stadium",
	stadiumCountryCode: "GBR",
	
	winnerSide: "Home",
	resultMessage: "Arsenal secures a vital 3 points.",
	
	period1HomeScore: "1",
	period1AwayScore: "0",
	period2HomeScore: "1",
	period2AwayScore: "1",
	
	incident1Type: "Goal",
	incident1Side: "Home",
	incident1Minute: "23",
	incident2Type: "YellowCard",
	incident2Side: "Away",
	incident2Minute: "41",
	incident3Type: "Goal",
	incident3Side: "Away",
	incident3Minute: "65",
	incident4Type: "Goal",
	incident4Side: "Home",
	incident4Minute: "89",
};

function SubmitButton() {
	const { pending } = useFormStatus();

	return (
		<button
			type="submit"
			disabled={pending}
			className="inline-flex items-center justify-center rounded-full border border-slate-500 bg-slate-950 px-6 py-3 text-sm font-black uppercase tracking-[0.2em] text-white transition hover:border-red-500 hover:bg-red-600 disabled:cursor-not-allowed disabled:opacity-60"
		>
			{pending ? "Creating..." : "Create Event"}
		</button>
	);
}

function SectionTitle({ title, description }: { title: string; description: string }) {
	return (
		<div className="mb-4">
			<h2 className="text-lg font-black uppercase tracking-[0.15em] text-slate-950">{title}</h2>
			<p className="mt-1 text-sm text-slate-600">{description}</p>
		</div>
	);
}

function Field({
	label,
	name,
	type = "text",
	required = false,
	placeholder,
}: {
	label: string;
	name: string;
	type?: string;
	required?: boolean;
	placeholder?: string;
}) {
	return (
		<label className="flex flex-col gap-2 text-sm font-bold text-slate-800">
			<span>{label}</span>
			<input
				name={name}
				type={type}
				required={required}
				placeholder={placeholder}
				className="rounded-2xl border border-slate-400 bg-white px-4 py-3 text-sm font-medium text-slate-950 outline-none transition focus:border-red-500"
			/>
		</label>
	);
}

function TextArea({
	label,
	name,
	placeholder,
}: {
	label: string;
	name: string;
	placeholder?: string;
}) {
	return (
		<label className="flex flex-col gap-2 text-sm font-bold text-slate-800">
			<span>{label}</span>
			<textarea
				name={name}
				rows={4}
				placeholder={placeholder}
				className="rounded-2xl border border-slate-400 bg-white px-4 py-3 text-sm font-medium text-slate-950 outline-none transition focus:border-red-500"
			/>
		</label>
	);
}

export default function NewEventForm() {
	const [state, formAction] = useActionState(createEventAction, initialState);
	const formRef = useRef<HTMLFormElement>(null);

	const populateWithPlaceholderData = () => {
		const form = formRef.current;

		if (!form) {
			return;
		}

		for (const [fieldName, fieldValue] of Object.entries(PLACEHOLDER_VALUES)) {
			const field = form.elements.namedItem(fieldName);

			if (
				field instanceof HTMLInputElement ||
				field instanceof HTMLTextAreaElement ||
				field instanceof HTMLSelectElement
			) {
				field.value = fieldValue;
				field.dispatchEvent(new Event("input", { bubbles: true }));
				field.dispatchEvent(new Event("change", { bubbles: true }));
			}
		}
	};

	return (
		<form ref={formRef} action={formAction} className="space-y-6">
			{state.error ? (
				<div className="rounded-3xl border border-red-300 bg-red-50 px-4 py-4 text-sm font-semibold text-red-700">
					{state.error}
				</div>
			) : null}

			{state.success ? (
				<div className="rounded-3xl border border-emerald-300 bg-emerald-50 px-4 py-4 text-sm font-semibold text-emerald-700">
					{state.success}
				</div>
			) : null}

			<section className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
				<SectionTitle
					title="Core Event"
					description="These fields are required for every new event."
				/>
				<div className="grid gap-4 md:grid-cols-2">
					<Field label="Event name" name="name" required placeholder="Barcelona vs Real Madrid" />
					<Field label="Season" name="season" type="number" required placeholder="2026" />
					<Field label="Status" name="status" required placeholder="Played" />
					<Field label="Venue time UTC" name="timeVenueUtc" required placeholder="15:00:00" />
					<Field
						label="Venue date & time UTC"
						name="dateVenueUtc"
						required
						placeholder="2026-03-23T20:00:00Z"
					/>
					<div className="md:col-span-2">
						<TextArea
							label="Description"
							name="description"
							placeholder="Mid-season title decider at the Emirates."
						/>
					</div>
				</div>
			</section>

			<section className="grid gap-6 xl:grid-cols-2">
				<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
					<SectionTitle
						title="Home Team"
						description="Use an existing ID or fill in the new team fields."
					/>
					<div className="grid gap-4">
						<Field label="Existing home team ID" name="existingHomeTeamId" />
						<Field label="New team name" name="homeTeamName" />
						<Field label="Official name" name="homeTeamOfficialName" />
						<div className="grid gap-4 sm:grid-cols-2">
							<Field label="Abbreviation" name="homeTeamAbbreviation" />
							<Field label="Country code" name="homeTeamCountryCode" placeholder="GBR" />
						</div>
						<div className="grid gap-4 sm:grid-cols-2">
							<Field label="Stage position" name="homeTeamStagePosition" type="number" />
							<Field label="Sport ID" name="homeTeamSportId" />
						</div>
					</div>
				</div>

				<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
					<SectionTitle
						title="Away Team"
						description="Use an existing ID or fill in the new team fields."
					/>
					<div className="grid gap-4">
						<Field label="Existing away team ID" name="existingAwayTeamId" />
						<Field label="New team name" name="awayTeamName" />
						<Field label="Official name" name="awayTeamOfficialName" />
						<div className="grid gap-4 sm:grid-cols-2">
							<Field label="Abbreviation" name="awayTeamAbbreviation" />
							<Field label="Country code" name="awayTeamCountryCode" placeholder="GBR" />
						</div>
						<div className="grid gap-4 sm:grid-cols-2">
							<Field label="Stage position" name="awayTeamStagePosition" type="number" />
							<Field label="Sport ID" name="awayTeamSportId" />
						</div>
					</div>
				</div>
			</section>

			<section className="grid gap-6 xl:grid-cols-3">
				<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
					<SectionTitle
						title="Competition"
						description="Required. Existing ID takes priority over new details."
					/>
					<div className="grid gap-4">
						<Field label="Existing competition ID" name="existingCompetitionId" />
						<Field label="New competition name" name="competitionName" />
						<Field label="Competition sport ID" name="competitionSportId" />
					</div>
				</div>

				<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
					<SectionTitle
						title="Stage"
						description="Optional. Use an ID or provide a new stage."
					/>
					<div className="grid gap-4">
						<Field label="Existing stage ID" name="existingStageId" />
						<Field label="New stage name" name="stageName" />
						<Field label="Stage ordering" name="stageOrdering" type="number" />
					</div>
				</div>

				<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
					<SectionTitle
						title="Stadium"
						description="Optional. Use an ID or provide a new stadium."
					/>
					<div className="grid gap-4">
						<Field label="Existing stadium ID" name="existingStadiumId" />
						<Field label="New stadium name" name="stadiumName" />
						<Field label="Country code" name="stadiumCountryCode" placeholder="GBR" />
					</div>
				</div>
			</section>

			<section className="grid gap-6 xl:grid-cols-[1.1fr_0.9fr]">
				<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
					<SectionTitle
						title="Result"
						description="Optional. Leave this section empty if the event has no result yet."
					/>
					<div className="grid gap-4 md:grid-cols-2">
						<Field label="Winner side" name="winnerSide" placeholder="Home" />
						<Field label="Result message" name="resultMessage" placeholder="Arsenal secures a vital 3 points." />
					</div>

					<div className="mt-5 grid gap-4 md:grid-cols-2">
						<div className="rounded-2xl border border-slate-300 bg-slate-50 p-4">
							<p className="text-sm font-black uppercase tracking-[0.2em] text-slate-950">Period 1</p>
							<div className="mt-3 grid gap-4 sm:grid-cols-2">
								<Field label="Home score" name="period1HomeScore" type="number" />
								<Field label="Away score" name="period1AwayScore" type="number" />
							</div>
						</div>
						<div className="rounded-2xl border border-slate-300 bg-slate-50 p-4">
							<p className="text-sm font-black uppercase tracking-[0.2em] text-slate-950">Period 2</p>
							<div className="mt-3 grid gap-4 sm:grid-cols-2">
								<Field label="Home score" name="period2HomeScore" type="number" />
								<Field label="Away score" name="period2AwayScore" type="number" />
							</div>
						</div>
					</div>
				</div>

				<div className="rounded-[1.75rem] border border-slate-300 bg-white p-5 md:p-6">
					<SectionTitle
						title="Incidents"
						description="Optional. Add up to four incidents for now."
					/>
					<div className="space-y-4">
						{[1, 2, 3, 4].map((index) => (
							<div key={index} className="rounded-2xl border border-slate-300 bg-slate-50 p-4">
								<p className="text-sm font-black uppercase tracking-[0.2em] text-slate-950">
									Incident {index}
								</p>
								<div className="mt-3 grid gap-4">
									<Field label="Type" name={`incident${index}Type`} placeholder="Goal" />
									<div className="grid gap-4 sm:grid-cols-2">
										<Field label="Side" name={`incident${index}Side`} placeholder="Home" />
										<Field label="Minute" name={`incident${index}Minute`} type="number" />
									</div>
								</div>
							</div>
						))}
					</div>
				</div>
			</section>

			<div className="flex flex-col gap-3 sm:flex-row sm:justify-end">
				<button
					type="button"
					onClick={populateWithPlaceholderData}
					className="inline-flex items-center justify-center rounded-full border border-slate-400 bg-white px-6 py-3 text-sm font-black uppercase tracking-[0.2em] text-slate-700 transition hover:border-red-500 hover:text-red-600"
				>
					Populate with placeholder data
				</button>
				<SubmitButton />
			</div>
		</form>
	);
}
