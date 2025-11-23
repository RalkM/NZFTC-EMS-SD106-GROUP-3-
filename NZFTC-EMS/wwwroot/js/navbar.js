// wwwroot/js/navbar.js
document.addEventListener("DOMContentLoaded", function () {
    const navbar = document.querySelector(".navbar");
    const navbarCollapse = document.getElementById("mainNavbar");

    // --- Shrink navbar on scroll ---
    function handleScroll() {
        if (!navbar) return;
        navbar.classList.toggle("navbar-shrink", window.scrollY > 40);
    }

    window.addEventListener("scroll", handleScroll);
    handleScroll(); // run on load

    // --- Close mobile menu on nav click ---
    if (!navbarCollapse) return;

    const navLinks = navbarCollapse.querySelectorAll(".nav-link");
    const loginBtn = navbarCollapse.querySelector(".login-outline-btn");

    function closeMobileMenu() {
        if (!navbarCollapse.classList.contains("show")) return;

        // bootstrap.Collapse is available because you include bootstrap.bundle
        let bsCollapse = bootstrap.Collapse.getInstance(navbarCollapse);
        if (!bsCollapse) {
            bsCollapse = new bootstrap.Collapse(navbarCollapse, { toggle: false });
        }
        bsCollapse.hide();
    }

    const clickableItems = [...navLinks];
    if (loginBtn) clickableItems.push(loginBtn);

    clickableItems.forEach(el => {
        el.addEventListener("click", () => {
            if (window.innerWidth < 992) {
                closeMobileMenu();
            }
        });
    });
});
