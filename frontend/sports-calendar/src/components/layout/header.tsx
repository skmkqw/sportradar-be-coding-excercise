"use client";

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { Menu, X, Activity } from 'lucide-react';
import Container from '../shared/container';
import { axiosInstance } from '@/lib/axios';

export default function Header() {
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isHealthy, setIsHealthy] = useState<boolean | null>(null);

    useEffect(() => {
        const checkHealth = async () => {
            try {
                const res = await axiosInstance.get('/health');
                setIsHealthy(res.data.Status === "Healthy");
            } catch (error) {
                console.error("Health check failed:", error);
                setIsHealthy(false);
            }
        };

        checkHealth();

        const intervalId = setInterval(checkHealth, 30000);

        return () => clearInterval(intervalId);
    }, []);

    const navLinks = [
        { name: 'CALENDAR', href: '/' },
        { name: 'SPORTS', href: '#' },
        { name: 'GITHUB', href: '#' },
    ];

    return (
        <header className="sticky top-0 z-50 border-b border-white/10 bg-brand-navy/90 backdrop-blur-md">
            <Container className="py-2 md:py-4">
                <div className="grid grid-cols-2 md:grid-cols-3">

                    {/* Brand Identity */}
                    <Link href="/" className="flex items-center gap-2">
                        <div className="bg-brand-red p-1">
                            <Activity size={22} className="text-white" />
                        </div>
                        <span className="text-lg font-black tracking-tighter uppercase italic">
                            SPORTSCALENDAR
                        </span>
                    </Link>

                    {/* Desktop Navigation */}
                    <nav className="hidden md:flex justify-self-center items-center gap-8">
                        {navLinks.map((link) => (
                            <Link
                                key={link.name}
                                href={link.href}
                                className="text-sm font-bold tracking-widest text-white/80 transition-colors hover:text-brand-red"
                            >
                                {link.name}
                            </Link>
                        ))}
                    </nav>

                    {/* Health Indicator */}
                    <div className="hidden md:flex justify-self-end items-center gap-2">
                        <div className={`h-2 w-2 rounded-full animate-pulse ${isHealthy ? 'bg-green-500' : 'bg-red-500'}`} />
                        <span className="text-sm font-bold tracking-tighter text-white/80 uppercase">
                            API: {isHealthy ? 'ONLINE' : 'OFFLINE'}
                        </span>
                    </div>

                    {/* Mobile Toggle */}
                    <button className="md:hidden justify-self-end text-white" onClick={() => setIsMenuOpen(!isMenuOpen)}>
                        {isMenuOpen ? <X /> : <Menu />}
                    </button>
                </div>
            </Container>

            {/* Mobile Menu Overlay */}
            {isMenuOpen && (
                <div className="md:hidden bg-brand-navy border-t border-white/10 px-4 space-y-2">
                    {navLinks.map((link) => (
                        <Link key={link.name} href={link.href} className="block text-lg font-black tracking-widest py-2 border-b border-white/5">
                            {link.name}
                        </Link>
                    ))}
                </div>
            )}
        </header>
    );
};
