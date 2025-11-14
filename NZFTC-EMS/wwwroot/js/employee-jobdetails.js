// wwwroot/js/employee-jobdetails.js

document.addEventListener('DOMContentLoaded', function () {
    const deptSelect    = document.getElementById('Department');
    const jobSelect     = document.getElementById('JobTitle');
    const payFreqSelect = document.getElementById('PayFrequency');
    const payGradeInput = document.getElementById('PayGradeDisplay');
    const basicPayInput = document.getElementById('BasicPayDisplay');
    

    // Only run on pages that have these fields (employee create/edit)
    if (!deptSelect || !jobSelect) return;

    // URL for JobMeta endpoint – set from Razor
    const jobMetaUrl = window.employeeJobMetaUrl || '/employee_management/job-meta';

    let jobMeta = [];

    // 1. Load job + pay data from server
    fetch(jobMetaUrl)
        .then(r => {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        })
        .then(data => {
            jobMeta = Array.isArray(data) ? data : [];
            populateJobsForDept(deptSelect.value);
            applyJobSelection();
        })
        .catch(err => console.error('JobMeta load failed', err));

    // 2. Department change -> filter job titles
    deptSelect.addEventListener('change', function () {
        populateJobsForDept(this.value);
        applyJobSelection();
    });

    // 3. Job title change -> update pay grade, basic pay, frequency
    jobSelect.addEventListener('change', function () {
        applyJobSelection();
    });

    function populateJobsForDept(dept) {
        const current = jobSelect.value;
        jobSelect.innerHTML = '';

        const jobsForDept = jobMeta.filter(j => j.department === dept);

        if (jobsForDept.length === 0) {
            const opt = document.createElement('option');
            opt.value = '';
            opt.textContent = '-- No jobs for this department --';
            jobSelect.appendChild(opt);
            return;
        }

        const placeholder = document.createElement('option');
        placeholder.value = '';
        placeholder.textContent = '-- Select job title --';
        jobSelect.appendChild(placeholder);

        jobsForDept.forEach(j => {
            const opt = document.createElement('option');
            opt.value = j.jobTitle; // binds to Employee.JobTitle
            opt.textContent = j.jobTitle;
            if (j.jobTitle === current) {
                opt.selected = true;
            }
            jobSelect.appendChild(opt);
        });
    }

    function applyJobSelection() {
        const dept  = deptSelect.value;
        const title = jobSelect.value;

        const job = jobMeta.find(j => j.department === dept && j.jobTitle === title);

        if (!job) {
            if (payGradeInput) payGradeInput.value = '';
            if (basicPayInput) basicPayInput.value = '';
            return;
        }

        // Pay Grade name
        if (payGradeInput) {
            payGradeInput.value = job.payGradeName || '';
        }

        // Basic Pay (BaseRate)
        if (basicPayInput) {
            const rate = job.baseRate != null ? parseFloat(job.baseRate) : NaN;
            if (!isNaN(rate)) {
                basicPayInput.value = rate.toFixed(2);
            } else {
                basicPayInput.value = '';
            }
        }

        // Auto-select Pay Frequency based on RateType
        if (payFreqSelect && job.rateType) {
            let freq = '';
            if (job.rateType === 'Hourly') {
                freq = 'Weekly';      // tweak if you want different mapping
            } else if (job.rateType === 'Salary') {
                freq = 'Monthly';
            }

            if (freq) {
                payFreqSelect.value = freq;
            }
        }
    }
});
