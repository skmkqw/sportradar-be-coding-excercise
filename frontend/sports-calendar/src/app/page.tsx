import MonthCalendar from "@/components/calendar/month-calendar";

type HomePageProps = {
	searchParams: Promise<{
		month?: string;
		year?: string;
	}>;
};

function getSelectedDate(month?: string, year?: string) {
	const today = new Date();
	const parsedMonth = Number(month);
	const parsedYear = Number(year);
	const selectedMonth =
		Number.isInteger(parsedMonth) && parsedMonth >= 1 && parsedMonth <= 12
			? parsedMonth - 1
			: today.getMonth();
	const selectedYear =
		Number.isInteger(parsedYear) && parsedYear >= 2000 && parsedYear <= 2100
			? parsedYear
			: today.getFullYear();

	return { selectedMonth, selectedYear };
}

export default async function HomePage({ searchParams }: HomePageProps) {
	const resolvedSearchParams = await searchParams;
	const { selectedMonth, selectedYear } = getSelectedDate(
		resolvedSearchParams.month,
		resolvedSearchParams.year,
	);

	return <MonthCalendar selectedMonth={selectedMonth} selectedYear={selectedYear} />;
}
