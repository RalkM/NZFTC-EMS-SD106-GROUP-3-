// ===============================
// APPLY LEAVE PAGE SCRIPTS
// ===============================

// ELEMENTS
const reasonBox = document.getElementById("reasonBox");
const charCount = document.getElementById("charCount");
const startDate = document.getElementById("startDate");
const endDate = document.getElementById("endDate");
const daysCount = document.getElementById("daysCount");
const halfDay = document.getElementById("IsHalfDay");

// -------------------------------
// CHARACTER COUNTER
// -------------------------------
if (reasonBox && charCount) {
    reasonBox.addEventListener("input", () => {
        charCount.textContent = `${reasonBox.value.length}/300`;
    });
}

// -------------------------------
// AUTO CALCULATE DAYS
// -------------------------------
function calculateDays() {
    if (!startDate.value || !endDate.value) {
        daysCount.value = "";
        return;
    }

    const start = new Date(startDate.value);
    const end = new Date(endDate.value);

    if (start > end) {
        daysCount.value = "0";
        return;
    }

    // NORMAL DAY CALCULATION
    let diff = (end - start) / (1000 * 3600 * 24) + 1;

    // HALF DAY OVERRIDE
    if (halfDay && halfDay.checked) {
        daysCount.value = "0.5";
    } else {
        daysCount.value = diff;
    }
}

if (startDate) startDate.addEventListener("change", calculateDays);
if (endDate) endDate.addEventListener("change", calculateDays);
if (halfDay) halfDay.addEventListener("change", calculateDays);

// Run once on load
calculateDays();
