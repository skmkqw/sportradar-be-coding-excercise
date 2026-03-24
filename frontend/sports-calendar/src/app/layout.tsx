import type { Metadata } from "next";
import { Geist_Mono } from "next/font/google";
import "./globals.css";
import Header from "@/components/layout/header";
import Footer from "@/components/layout/footer";


const geistMono = Geist_Mono({
	variable: "--font-geist-mono",
	subsets: ["latin"],
});

export const metadata: Metadata = {
	title: "Sports Calendar",
	description: "View and manage sports event data",
};

export default function RootLayout({
	children,
}: Readonly<{
	children: React.ReactNode;
}>) {
	return (
		<html lang="en">
			<body className="bg-brand-navy font-sans text-white antialiased">
				{/* Immersive Background Overlay */}
				<div className="fixed inset-0 z-[-1] opacity-20 overflow-hidden">
					{/* Placeholder for high-action sports imagery */}
					<div className="absolute inset-0 bg-[url('/stadium-bg.jpg')] bg-cover bg-center grayscale" />
					<div className="absolute inset-0 bg-linear-to-b from-brand-navy via-transparent to-brand-navy" />
				</div>

				<div className="flex min-h-screen flex-col">
					<Header />
					<main className="flex-1">
						{children}
					</main>
					<Footer />
				</div>
			</body>
		</html>
	);
}
