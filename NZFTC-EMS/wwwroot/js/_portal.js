document.addEventListener('DOMContentLoaded', function () {
    const btn = document.getElementById('navToggle');
    if (!btn) return;
    
    btn.addEventListener('click', function () {
        document.body.classList.toggle('sidebar-open');
    });
});