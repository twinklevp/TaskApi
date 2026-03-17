document.querySelectorAll('.filter-select').forEach(el => {
    el.addEventListener('change', () => el.closest('form').submit());
});

document.querySelectorAll('.bar-fill').forEach(bar => {
    const target = bar.style.width;
    bar.style.width = '0';
    requestAnimationFrame(() => {
        bar.style.transition = 'width .6s cubic-bezier(.4,0,.2,1)';
        bar.style.width = target;
    });
});

// Button to call API and display result
const apiBtn = document.getElementById('call-api-btn');
const apiResult = document.getElementById('api-result');
if (apiBtn && apiResult) {
    const escapeHtml = (str) => str.replace(/[&<>"'`]/g, s => ({
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#39;',
        '`': '&#96;'
    }[s]));

    apiBtn.addEventListener('click', async () => {
        apiBtn.disabled = true;
        const originalText = apiBtn.textContent;
        apiBtn.textContent = 'Calling...';
        apiResult.textContent = '';
        try {
            const res = await fetch('/api/tasks');
            if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
            const data = await res.json();
            apiResult.innerHTML = `<div>Fetched <strong>${data.length}</strong> task(s).</div>` +
                `<pre style="white-space:pre-wrap;margin-top:8px">${escapeHtml(JSON.stringify(data, null, 2))}</pre>`;
        } catch (err) {
            apiResult.textContent = `API error: ${err.message}`;
        } finally {
            apiBtn.disabled = false;
            apiBtn.textContent = originalText;
        }
    });
}
