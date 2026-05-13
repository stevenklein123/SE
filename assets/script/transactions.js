var currentReceiptId = null;

function escapeHtml(str) {
    if (!str) return '';
    return str.replace(/[&<>"'`]/g, function (s) {
        return ({
            '&': '&amp;','<': '&lt;','>': '&gt;','"': '&quot;',"'": '&#39;','`': '&#96;'
        })[s];
    });
}

function viewReceipt(id) {
    currentReceiptId = id;

    fetch("transactions.aspx/GetReceipt", {
        method: "POST",
        credentials: 'same-origin',
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id: id })
    })
        .then(res => res.json())
        .then(res => {
            var data = res && res.d ? res.d : [];
            var html = "<h3>Receipt</h3><hr>";
            var total = 0;

            if (data.length === 0) {
                html += '<p>No items found for this transaction.</p>';
            } else {
                html += '<table style="width:100%;border-collapse:collapse;margin-bottom:8px;">'
                    + '<thead><tr><th style="text-align:left">Product</th><th>Qty</th><th>Subtotal</th></tr></thead><tbody>';

                data.forEach(function (i) {
                    var name = escapeHtml(i.name);
                    var qty = parseInt(i.qty, 10) || 0;
                    var sub = parseFloat(i.subtotal) || 0;
                    total += sub;

                    html += `<tr><td style="padding:6px 4px">${name}</td><td style="text-align:center">${qty}</td><td style="text-align:right">₱${sub.toFixed(2)}</td></tr>`;
                });

                html += '</tbody></table>';
                html += `<div style="text-align:right;font-weight:bold">Total: ₱${total.toFixed(2)}</div>`;
            }

            var body = document.getElementById("receiptBody");
            if (body) body.innerHTML = html;

            var modal = document.getElementById("receiptModal");
            if (modal) modal.style.display = "block";
        })
        .catch(function (err) {
            console.error('View receipt error', err);
            var body = document.getElementById("receiptBody");
            if (body) body.innerHTML = '<p>Error loading receipt.</p>';
            var modal = document.getElementById("receiptModal");
            if (modal) modal.style.display = "block";
        });
}

function closeReceipt() {
    var modal = document.getElementById("receiptModal");
    if (modal) modal.style.display = "none";
    currentReceiptId = null;
}

window.addEventListener('click', function (e) {
    try {
        var modal = document.getElementById('receiptModal');
        if (!modal) return;
        if (modal.style.display !== 'block' && !modal.classList.contains('active')) return;
        if (e.target === modal) {
            closeReceipt();
        }
    } catch (ex) {
        console.error('receipt overlay click handler error', ex);
    }
});

function loadTransactions() {
    fetch('transactions.aspx/GetTransactions', {
        method: 'POST',
        credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json' },
        body: '{}'
    })
        .then(res => res.json())
        .then(res => {
            var list = res && res.d ? res.d : [];
            var container = document.getElementById('transactionsList');
            if (!container) return;

            if (list.length === 0) {
                container.innerHTML = '<div style="padding:20px;background:#fff;border-radius:8px;border:1px solid #f3d1dc">No transactions found.</div>';
                return;
            }

            var html = '';
            list.forEach(function (t) {
                var ref = escapeHtml(t.refcode || '');
                var total = parseFloat(t.total) || 0;
                var status = escapeHtml(t.status || '');
                var date = escapeHtml(t.date || '');

                var st = (status || '').toLowerCase();
                var statusClass = 'status-unknown';
                if (st.indexOf('pending') !== -1) statusClass = 'status-pending';
                else if (st.indexOf('completed') !== -1 || st.indexOf('done') !== -1) statusClass = 'status-completed';
                else if (st.indexOf('cancel') !== -1) statusClass = 'status-cancelled';
                else if (st.indexOf('processing') !== -1) statusClass = 'status-processing';

                html += `<div class="transaction-card product-card">
                    <div style="display:flex;justify-content:space-between;align-items:center;gap:12px">
                        <div>
                            <h3 style="margin:0">Ref: ${ref}</h3>
                            <div style="font-size:0.95rem;color:#555">Date: ${date}</div>
                        </div>
                        <div style="text-align:right">
                            <div class="status-badge ${statusClass}">${status || 'Unknown'}</div>
                            <div style="font-weight:700;margin-top:6px">₱${total.toFixed(2)}</div>
                        </div>
                    </div>
                    <div style="margin-top:10px;text-align:right">
                        <button type="button" onclick="(function(e){e.stopPropagation(); viewReceipt(${t.id});})(event)" class="btn-view-receipt">View Receipt</button>
                    </div>
                </div>`;
            });

            container.innerHTML = html;
        })
        .catch(err => {
            console.error('Load transactions error', err);
        });
}

// Auto-load transactions when this script runs
try { loadTransactions(); } catch (e) { }

