import { cn } from "@/lib/utils";

export default function Container({ children, className, id }: { children: React.ReactNode, className?: string, id?: string }) {
    return (
        <div className={cn("mx-auto max-w-7xl px-4 py-4", className)} id={id}>
            {children}
        </div>
    );
}