export default function HomePage() {
	return (
		<section className="relative flex min-h-[30vh] items-center justify-center py-14 overflow-hidden">
			<div className="container relative z-10 px-4 text-center">
				<h1 className="text-6xl md:text-8xl font-black leading-none tracking-tighter uppercase italic">
					<span className="block text-white">SPORTS</span>
					<span className="block text-transparent stroke-text" style={{ WebkitTextStroke: '2px white' }}>
						CALENDAR
					</span>
				</h1>
				<p className="mt-4 text-sm font-bold tracking-[0.4em] text-white/60 uppercase">
					View & Edit Sports Event Data
				</p>
			</div>
		</section>
	);
}