document.addEventListener('DOMContentLoaded', () => {
    const rows      = document.querySelectorAll('tr.ticket-row');
    const emptyBox  = document.getElementById('preview-empty');
    const bodyBox   = document.getElementById('preview-body');

    const pvSubject  = document.getElementById('pv-subject');
    const pvEmployee = document.getElementById('pv-employee');
    const pvStatus   = document.getElementById('pv-status');
    const pvPriority = document.getElementById('pv-priority');
    const pvCreated  = document.getElementById('pv-created');
    const pvPreview  = document.getElementById('pv-preview');
    const pvThread   = document.getElementById('pv-thread');

    const actionsBox             = document.getElementById('preview-actions');
    const replyForm              = document.getElementById('replyForm');
    const replyBody              = document.getElementById('replyBody');
    const statusInvestigatingForm = document.getElementById('statusInvestigatingForm');
    const statusCloseForm        = document.getElementById('statusCloseForm');

    let currentTicketId = null;

    function clearActive() {
        rows.forEach(r => r.classList.remove('table-active'));
    }

    function renderThread(messages) {
        if (!pvThread) return;

        pvThread.innerHTML = '';

        if (!messages || messages.length === 0) {
            pvThread.innerHTML = '<span class="text-muted small">No replies yet.</span>';
            return;
        }

        messages.forEach(m => {
            const body      = m.body ?? m.Body ?? '';
            const fromAdmin = (m.fromAdmin ?? m.FromAdmin ?? m.senderIsAdmin ?? m.SenderIsAdmin) === true;
            const sentAt    = m.sentAt ?? m.SentAt;
            const d         = sentAt ? new Date(sentAt) : null;
            const timeText  = d ? d.toLocaleString() : '';

            const wrapper = document.createElement('div');
            wrapper.className = 'mb-2';

            const header = document.createElement('div');
            header.className = 'd-flex justify-content-between small mb-1';

            const who = document.createElement('span');
            who.className = fromAdmin ? 'fw-semibold text-primary' : 'fw-semibold';
            who.textContent = fromAdmin ? 'You (Admin)' : 'Employee';

            const time = document.createElement('span');
            time.className = 'text-muted';
            time.textContent = timeText;

            header.appendChild(who);
            header.appendChild(time);

            const bubble = document.createElement('div');
            bubble.className = 'border rounded px-2 py-1 bg-light small';
            bubble.style.whiteSpace = 'pre-wrap';
            bubble.textContent = body;

            wrapper.appendChild(header);
            wrapper.appendChild(bubble);
            pvThread.appendChild(wrapper);
        });
    }

    async function loadThread(ticketId) {
        if (!pvThread) return;

        pvThread.innerHTML = '<span class="text-muted small">Loading conversation…</span>';

        try {
            const res = await fetch(`/support_management/api/${ticketId}/thread`);
            if (!res.ok) {
                pvThread.innerHTML = '<span class="text-danger small">Failed to load conversation.</span>';
                return;
            }
            const data = await res.json();
            renderThread(data);
        } catch (e) {
            console.error(e);
            pvThread.innerHTML = '<span class="text-danger small">Error loading conversation.</span>';
        }
    }

    function statusClass(status) {
        switch (status) {
            case 'Open':        return 'text-bg-warning';
            case 'InProgress':  return 'text-bg-info';
            case 'Resolved':    return 'text-bg-success';
            case 'Closed':      return 'text-bg-secondary';
            default:            return 'text-bg-light';
        }
    }

    function priorityClass(priority) {
        switch (priority) {
            case 'Low':    return 'text-bg-secondary';
            case 'Medium': return 'text-bg-primary';
            case 'High':   return 'text-bg-warning';
            case 'Urgent': return 'text-bg-danger';
            default:       return 'text-bg-light';
        }
    }

    // If there are no rows, nothing to wire up
    if (!rows || rows.length === 0) return;

    rows.forEach(row => {
        row.addEventListener('click', () => {
            clearActive();
            row.classList.add('table-active');

            const id       = row.dataset.id;
            const subject  = row.dataset.subject || '';
            const employee = row.dataset.employee || '';
            const code     = row.dataset.code || '';
            const status   = row.dataset.status || '';
            const priority = row.dataset.priority || '';
            const created  = row.dataset.created || '';
            const preview  = row.dataset.preview || '';

            currentTicketId = id;

            if (pvSubject)  pvSubject.textContent = subject;

            if (pvEmployee) {
                if (employee && code) {
                    pvEmployee.textContent = `${employee} (${code})`;
                } else if (employee || code) {
                    pvEmployee.textContent = employee || code;
                } else {
                    pvEmployee.textContent = '-';
                }
            }

            if (pvStatus) {
                pvStatus.textContent = status || '-';
                pvStatus.className = 'badge ' + statusClass(status);
            }

            if (pvPriority) {
                pvPriority.textContent = priority || '-';
                pvPriority.className = 'badge ' + priorityClass(priority);
            }

            if (pvCreated) {
                if (created) {
                    try {
                        const d = new Date(created);
                        pvCreated.textContent = d.toLocaleString();
                    } catch {
                        pvCreated.textContent = created;
                    }
                } else {
                    pvCreated.textContent = '';
                }
            }

            if (pvPreview) {
                pvPreview.textContent = preview || 'No message content.';
            }

            // wire up forms to this ticket (null-safe)
            if (id) {
                const base = `/support_management/${id}`;
                if (replyForm)               replyForm.action = `${base}/reply`;
                if (statusInvestigatingForm) statusInvestigatingForm.action = `${base}/status`;
                if (statusCloseForm)         statusCloseForm.action = `${base}/status`;

                loadThread(id);
            }

            // reset reply textarea
            if (replyBody) replyBody.value = '';

            if (emptyBox)  emptyBox.classList.add('d-none');
            if (bodyBox)   bodyBox.classList.remove('d-none');
            if (actionsBox) actionsBox.classList.remove('d-none');
        });
    });
});
