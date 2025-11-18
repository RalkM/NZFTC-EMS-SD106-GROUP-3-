document.addEventListener('DOMContentLoaded', () => {
    const createForm    = document.getElementById('create-form');
    const createStatus  = document.getElementById('create-status');
    const ticketBody    = document.getElementById('ticket-body');

    const detailEmpty   = document.getElementById('detail-empty');
    const detailBody    = document.getElementById('detail-body');
    const dtSubject     = document.getElementById('dt-subject');
    const dtStatus      = document.getElementById('dt-status');
    const dtPriority    = document.getElementById('dt-priority');
    const dtCreated     = document.getElementById('dt-created');
    const dtMessage     = document.getElementById('dt-message');
    const dtThread      = document.getElementById('dt-thread');

    const empReplyForm   = document.getElementById('emp-reply-form');
    const empReplyBody   = document.getElementById('emp-reply-body');
    const empReplyClosed = document.getElementById('emp-reply-closed');

    let currentTicketId = null;

    function badgeStatusClass(status) {
        switch (status) {
            case 'Open':        return 'text-bg-warning';
            case 'InProgress':  return 'text-bg-info';
            case 'Resolved':    return 'text-bg-success';
            case 'Closed':      return 'text-bg-secondary';
            default:            return 'text-bg-light';
        }
    }

    function badgePriorityClass(priority) {
        switch (priority) {
            case 'Low':    return 'text-bg-secondary';
            case 'Medium': return 'text-bg-primary';
            case 'High':   return 'text-bg-warning';
            case 'Urgent': return 'text-bg-danger';
            default:       return 'text-bg-light';
        }
    }

    function setDetailVisible(visible) {
        if (visible) {
            detailEmpty.classList.add('d-none');
            detailBody.classList.remove('d-none');
        } else {
            detailEmpty.classList.remove('d-none');
            detailBody.classList.add('d-none');
        }
    }

    async function loadRows() {
        ticketBody.innerHTML = `
            <tr>
                <td colspan="5" class="text-center text-muted py-3">
                    Loading...
                </td>
            </tr>`;

        try {
            const res = await fetch('/support/api/list');
            if (!res.ok) {
                ticketBody.innerHTML = `
                    <tr><td colspan="5" class="text-center text-danger py-3">
                        Failed to load tickets.
                    </td></tr>`;
                return;
            }

            const data = await res.json();
            ticketBody.innerHTML = '';

            if (!data || data.length === 0) {
                ticketBody.innerHTML = `
                    <tr>
                        <td colspan="5" class="text-center text-muted py-3">
                            You have no tickets yet.
                        </td>
                    </tr>`;
                setDetailVisible(false);
                currentTicketId = null;
                return;
            }

            data.forEach((t, index) => {
                const id       = t.id       ?? t.Id;
                const subject  = t.subject  ?? t.Subject  ?? '';
                const status   = t.status   ?? t.Status   ?? '';
                const priority = t.priority ?? t.Priority ?? '';
                const created  = t.createdAt?? t.CreatedAt;

                const tr = document.createElement('tr');
                tr.className = 'ticket-row';
                tr.dataset.id = id;

                const d = created ? new Date(created) : null;
                const createdText = d ? d.toLocaleString() : '';

                tr.innerHTML = `
                    <td>${index + 1}</td>
                    <td>${subject}</td>
                    <td>${status}</td>
                    <td>${priority}</td>
                    <td>${createdText}</td>
                `;
                ticketBody.appendChild(tr);
            });

            attachRowHandlers();
        } catch (e) {
            console.error(e);
            ticketBody.innerHTML = `
                <tr><td colspan="5" class="text-center text-danger py-3">
                    Error loading tickets.
                </td></tr>`;
        }
    }

    function attachRowHandlers() {
        document.querySelectorAll('tr.ticket-row').forEach(row => {
            row.addEventListener('click', () => {
                document.querySelectorAll('tr.ticket-row').forEach(r => r.classList.remove('table-active'));
                row.classList.add('table-active');

                const id = row.dataset.id;
                currentTicketId = id;
                if (id) {
                    loadDetail(id);
                }
            });
        });
    }

    function renderThread(messages) {
        dtThread.innerHTML = '';

        if (!messages || messages.length === 0) {
            dtThread.innerHTML = `<p class="text-muted small mb-0">No replies yet.</p>`;
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
            who.textContent = fromAdmin ? 'Admin' : 'You';

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
            dtThread.appendChild(wrapper);
        });
    }

    async function loadDetail(id) {
        try {
            const res = await fetch(`/support/api/${id}/detail`);
            if (!res.ok) {
                setDetailVisible(false);
                return;
            }

            const d = await res.json();

            const subject  = d.subject  ?? d.Subject  ?? '';
            const status   = d.status   ?? d.Status   ?? '';
            const priority = d.priority ?? d.Priority ?? '';
            const created  = d.createdAt?? d.CreatedAt;
            const message  = d.message  ?? d.Message  ?? '';

            dtSubject.textContent = subject;
            dtStatus.textContent  = status || '-';
            dtPriority.textContent = priority || '-';

            dtStatus.className   = 'badge ' + badgeStatusClass(status);
            dtPriority.className = 'badge ' + badgePriorityClass(priority);

            if (created) {
                const cd = new Date(created);
                dtCreated.textContent = cd.toLocaleString();
            } else {
                dtCreated.textContent = '';
            }

            dtMessage.textContent = message || '(no message)';

            const messages = d.messages ?? d.Messages ?? [];
            renderThread(messages);

            const isClosed = status === 'Closed';
            if (isClosed) {
                empReplyForm.classList.add('d-none');
                empReplyClosed.classList.remove('d-none');
            } else {
                empReplyForm.classList.remove('d-none');
                empReplyClosed.classList.add('d-none');
            }

            setDetailVisible(true);
        } catch (e) {
            console.error(e);
            setDetailVisible(false);
        }
    }

    // create ticket
    createForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        createStatus.textContent = 'Submitting...';

        const payload = {
            subject:  createForm.subject.value,
            message:  createForm.message.value,
            priority: createForm.priority.value
        };

        try {
            const res = await fetch('/support/api/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (!res.ok) {
                createStatus.textContent = 'Error submitting ticket.';
                return;
            }

            const json = await res.json();
            if (json.ok) {
                createStatus.textContent = 'Ticket submitted.';
                createForm.reset();
                createForm.priority.value = 'Medium';
                await loadRows();
            } else {
                createStatus.textContent = 'Something went wrong.';
            }
        } catch (e) {
            console.error(e);
            createStatus.textContent = 'Error submitting ticket.';
        }
    });

    // employee reply
    empReplyForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        if (!currentTicketId) return;

        const body = empReplyBody.value.trim();
        if (!body) return;

        try {
            const res = await fetch(`/support/api/${currentTicketId}/reply`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ body })
            });

            if (!res.ok) {
                const txt = await res.text();
                alert(txt || 'Failed to send reply.');
                return;
            }

            const json = await res.json();
            if (json.ok) {
                empReplyBody.value = '';
                await loadDetail(currentTicketId);
                await loadRows();
            }
        } catch (e) {
            console.error(e);
            alert('Failed to send reply.');
        }
    });

    // initial load
    loadRows();
});