// ==========================================
// ADMIN LEAVE MANAGEMENT PAGE SCRIPTS
// ==========================================

// Elements
const searchBox = document.getElementById("searchBox");
const statusFilter = document.getElementById("statusFilter");
const leaveRows = document.querySelectorAll("#leaveTable tbody tr");

// Filter function
function filterLeaveTable() {
    const search = searchBox.value.toLowerCase();
    const status = statusFilter.value.toLowerCase();

    leaveRows.forEach(row => {
        const rowText = row.innerText.toLowerCase();
        const rowStatus = row.children[3].innerText.toLowerCase();

        const matchesSearch = rowText.includes(search);
        const matchesStatus = status === "" || rowStatus.includes(status);

        if (matchesSearch && matchesStatus)
            row.style.display = "";
        else
            row.style.display = "none";
    });
}

// Event listeners
if (searchBox) searchBox.addEventListener("input", filterLeaveTable);
if (statusFilter) statusFilter.addEventListener("change", filterLeaveTable);

// Run on load
filterLeaveTable();
