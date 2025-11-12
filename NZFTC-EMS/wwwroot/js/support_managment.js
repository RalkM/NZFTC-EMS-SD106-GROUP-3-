 const grid   = document.getElementById('grid');
  const dEmp   = document.getElementById('d-emp');
  const dSub   = document.getElementById('d-sub');
  const dDate  = document.getElementById('d-date');
  const dMsg   = document.getElementById('d-msg');
  const respId = document.getElementById('resp-id');
  const resId  = document.getElementById('res-id');
  const respTx = document.getElementById('resp-text');

  function clearActive(){ grid.querySelectorAll('tbody tr').forEach(tr => tr.classList.remove('active')); }

  async function loadDetails(id, row){
    const r = await fetch('@Url.Action("Details","Grievances")/' + id);
    if(!r.ok) return;
    const g = await r.json(); // snake_case keys

    dEmp.textContent  = g.employee_full_name;
    dSub.textContent  = g.subject;
    dDate.textContent = g.date_submitted;
    dMsg.value        = g.message ?? "";
    respTx.value      = g.admin_response ?? "";

    respId.value = id;
    resId.value  = id;

    clearActive(); row.classList.add('active');
  }

  grid.addEventListener('click', (e) => {
    const row = e.target.closest('tr[data-id]');
    if(!row) return;
    loadDetails(row.dataset.id, row);
  });

  const first = grid.querySelector('tbody tr[data-id]');
  if (first) loadDetails(first.dataset.id, first);

  (async () => {
  try {
    const res = await fetch('/support/api/list', { credentials: 'same-origin' });
    if (!res.ok) return;
    const rows = await res.json();
    const openCount = rows.filter(r => r.status === 'Open' || r.status === 'InProgress').length;
    const badge = document.getElementById('support-badge');
    if (openCount > 0) {
      badge.textContent = openCount;
      badge.style.display = 'inline-block';
    }
  } catch { /* ignore */ }
})();