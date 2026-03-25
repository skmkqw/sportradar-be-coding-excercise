type EventPageProps = {
	params: Promise<{
		eventId: string;
	}>;
};

export default async function EventPage({ params }: EventPageProps) {
	const { eventId } = await params;

	return (
		<section className="mx-auto flex min-h-[50vh] w-full max-w-4xl items-center justify-center px-4 py-12">
			<div className="w-full rounded-4xl border border-slate-200 bg-white p-10 text-center text-slate-950 shadow-[0_24px_80px_rgba(15,23,42,0.08)]">
				<p className="text-xs font-black uppercase tracking-[0.35em] text-red-600">
					Event Page
				</p>
				<h1 className="mt-4 text-4xl font-black uppercase tracking-tight">Coming Soon</h1>
				<p className="mt-4 text-sm font-medium text-slate-600 md:text-base">
					Placeholder page for event <span className="font-bold text-slate-950">{eventId}</span>.
				</p>
			</div>
		</section>
	);
}
