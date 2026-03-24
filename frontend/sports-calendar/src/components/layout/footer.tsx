import Link from "next/link";
import Container from "../shared/container";

export default function Footer() {
    return (
        <footer className="sticky top-0 z-50 border-b border-white/10 bg-brand-navy/90 backdrop-blur-md">
            <Container className="flex justify-center">
                <div className="max-w-2xl text-center flex flex-col items-center">
                    <Link
                        href="https://github.com/skmkqw/sportradar-be-coding-excercise"
                        className="text-sm font-bold tracking-tighter text-white/50 uppercase underline"
                    >
                        Repo Link
                    </Link>
                </div>
            </Container>
        </footer>
    );
}
