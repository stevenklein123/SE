document.addEventListener('DOMContentLoaded', () => {
    fetchOrders('all');

    document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            fetchOrders(this.dataset.filter);
        });
    });
});

function fetchOrders(filterValue) {
    fetch('orders.aspx/GetOrders', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ status: filterValue })
    })
        .then(response => response.json())
        .then(data => {
            renderOrders(data.d);
        })
        .catch(err => console.error('Error fetching orders:', err));
}

function renderOrders(orders) {
    const list = document.getElementById('ordersList');
    const empty = document.getElementById('ordersEmpty');

    if (!orders || orders.length === 0) {
        empty.style.display = 'block';
        list.innerHTML = '';
        return;
    }

    empty.style.display = 'none';
    list.innerHTML = orders.map(order => `
        <div class="order-card shadow-sm">
            <div class="order-card-header">
                <div class="order-info-main">
                    <span class="order-number">#${order.id}</span>
                    <span class="order-date">${order.date}</span>
                </div>
                <span class="status-pill status-${order.status}">${order.status}</span>
            </div>
            <div class="order-card-body">
                <div class="order-meta">
                    <div class="meta-item">
                        <label>Reference No</label>
                        <strong>${order.refcode ?? 'N/A'}</strong>
                    </div>
                    <div class="meta-item">
                        <label>Total Amount</label>
                        <strong>₱${parseFloat(order.total).toLocaleString(undefined, { minimumFractionDigits: 2 })}</strong>
                    </div>
                    <div class="meta-item">
                        <label>Items</label>
                        <strong>${order.itemCount} Qty</strong>
                    </div>
                    <div class="meta-item">
                        <label>Shipping</label>
                        <strong>${order.shipping}</strong>
                    </div>
                </div>
            </div>
        </div>
    `).join('');
}