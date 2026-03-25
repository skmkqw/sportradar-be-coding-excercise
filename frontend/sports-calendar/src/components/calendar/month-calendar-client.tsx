"use client";

import { ChevronLeft, ChevronRight } from "lucide-react";
import { useRouter } from "next/navigation";

const MONTH_FORMATTER = new Intl.DateTimeFormat("en-US", { month: "long" });
const MONTH_OPTIONS = Array.from({ length: 12 }, (_, monthIndex) => ({
	value: monthIndex,
	label: MONTH_FORMATTER.format(new Date(Date.UTC(2026, monthIndex, 1))),
}));

type MonthCalendarClientProps = {
	selectedMonth: number;
	selectedYear: number;
};

export default function MonthCalendarClient({
	selectedMonth,
	selectedYear,
}: MonthCalendarClientProps) {
	const router = useRouter();

	const updateCalendar = (month: number, year: number) => {
		const searchParams = new URLSearchParams({
			month: String(month + 1),
			year: String(year),
		});

		router.push(`/?${searchParams.toString()}`, { scroll: false });
	};

	const shiftMonth = (direction: number) => {
		const nextDate = new Date(Date.UTC(selectedYear, selectedMonth + direction, 1));
		updateCalendar(nextDate.getUTCMonth(), nextDate.getUTCFullYear());
	};

	return (
		<div className="flex flex-col gap-3 sm:flex-row sm:items-center">
			<div className="flex items-center rounded-full border border-slate-200 bg-slate-50 p-1">
				<button
					type="button"
					onClick={() => shiftMonth(-1)}
					className="rounded-full p-2 text-slate-700 transition hover:bg-white hover:text-slate-950"
					aria-label="Previous month"
				>
					<ChevronLeft size={18} />
				</button>
				<button
					type="button"
					onClick={() => shiftMonth(1)}
					className="rounded-full p-2 text-slate-700 transition hover:bg-white hover:text-slate-950"
					aria-label="Next month"
				>
					<ChevronRight size={18} />
				</button>
			</div>

			<label className="flex flex-col gap-2 text-xs font-black uppercase tracking-[0.25em] text-slate-500">
				Month
				<select
					value={selectedMonth}
					onChange={(event) => updateCalendar(Number(event.target.value), selectedYear)}
					className="rounded-2xl border border-slate-200 bg-white px-4 py-3 text-sm font-bold tracking-wide text-slate-950 outline-none transition focus:border-red-500"
				>
					{MONTH_OPTIONS.map((monthOption) => (
						<option key={monthOption.value} value={monthOption.value}>
							{monthOption.label}
						</option>
					))}
				</select>
			</label>

			<label className="flex flex-col gap-2 text-xs font-black uppercase tracking-[0.25em] text-slate-500">
				Year
				<input
					type="number"
					inputMode="numeric"
					min="2000"
					max="2100"
					value={selectedYear}
					onChange={(event) =>
						updateCalendar(selectedMonth, Number(event.target.value) || new Date().getFullYear())
					}
					className="rounded-2xl border border-slate-200 bg-white px-4 py-3 text-sm font-bold tracking-wide text-slate-950 outline-none transition focus:border-red-500"
				/>
			</label>
		</div>
	);
}
