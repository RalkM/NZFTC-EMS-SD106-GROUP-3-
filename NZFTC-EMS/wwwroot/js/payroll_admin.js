// ============================================================
// PAYROLL ADMIN JAVASCRIPT
// ============================================================

// ================================
// Utility: Format currency (NZD)
// ================================
function formatNZD(value) {
    return "$" + Number(value).toFixed(2);
}

// ================================
// Highlight table rows on hover
// ================================
document.querySelectorAll("table.table tbody tr").forEach(row => {
    row.addEventListener("mouseover", () => row.classList.add("table-active"));
    row.addEventListener("mouseout", () => row.classList.remove("table-active"));
});

// ================================
// Payroll Summary Search + Filter
// ================================
const payrollSearchBox = document.getElementById("payrollSearchBox");
const payrollStatusFilter = document.getElementById("payrollStatusFilter");

if (payrollSearchBox || payrollStatusFilter) {
    const rows = document.querySelectorAll("#payrollSummaryTable tbody tr");

    function filterPayrollSummary() {
        const search = payrollSearchBox ? payrollSearchBox.value.toLowerCase() : "";
        const status = payrollStatusFilter ? payrollStatusFilter.value.toLowerCase() : "";

        rows.forEach(row => {
            const text = row.innerText.toLowerCase();
            const statusText = row.children[row.children.length - 2].innerText.toLowerCase();

            const matchesSearch = text.includes(search);
            const matchesStatus = status === "" || statusText.includes(status);

            row.style.display = (matchesSearch && matchesStatus) ? "" : "none";
        });
    }

    if (payrollSearchBox) payrollSearchBox.addEventListener("input", filterPayrollSummary);
    if (payrollStatusFilter) payrollStatusFilter.addEventListener("change", filterPayrollSummary);

    // run on load
    filterPayrollSummary();
}

// ================================
// Table Sorting (by clicking header)
// ================================
document.querySelectorAll("th.sortable").forEach(header => {
    header.style.cursor = "pointer";

    header.addEventListener("click", () => {
        const table = header.closest("table");
        const tbody = table.querySelector("tbody");
        const index = Array.from(header.parentNode.children).indexOf(header);

        const rows = Array.from(tbody.querySelectorAll("tr"));

        const direction = header.dataset.sortDirection === "asc" ? "desc" : "asc";
        header.dataset.sortDirection = direction;

        rows.sort((a, b) => {
            const A = a.children[index].innerText.replace("$", "").trim();
            const B = b.children[index].innerText.replace("$", "").trim();

            const numA = parseFloat(A) || A;
            const numB = parseFloat(B) || B;

            if (direction === "asc") {
                return numA > numB ? 1 : -1;
            } else {
                return numA < numB ? 1 : -1;
            }
        });

        rows.forEach(r => tbody.appendChild(r));
    });
});

// ================================
// Confirm Buttons for Admin Actions
// ================================
document.querySelectorAll("[data-confirm]").forEach(btn => {
    btn.addEventListener("click", e => {
        const message = btn.getAttribute("data-confirm");
        if (!confirm(message)) e.preventDefault();
    });
});
