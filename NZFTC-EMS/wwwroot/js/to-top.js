// wwwroot/js/to-top.js
document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById("scrollTopBtn");
    if (!btn) return;

    function toggleVisibility() {
        if (window.scrollY > 300) {
            btn.classList.add("show");
        } else {
            btn.classList.remove("show");
        }
    }

    window.addEventListener("scroll", toggleVisibility);
    toggleVisibility();

    btn.addEventListener("click", function (e) {
        e.preventDefault();
        window.scrollTo({ top: 0, behavior: "smooth" });
    });
});

