// wwwroot/js/scroll-effects.js
document.addEventListener("DOMContentLoaded", function () {
    const revealEls = document.querySelectorAll(".reveal-on-scroll");
    if (!("IntersectionObserver" in window) || revealEls.length === 0) return;

    const observer = new IntersectionObserver(
        (entries, obs) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("is-visible");
                    obs.unobserve(entry.target);
                }
            });
        },
        {
            threshold: 0.15
        }
    );

    revealEls.forEach(el => observer.observe(el));
});

