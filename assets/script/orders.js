document.addEventListener('DOMContentLoaded', () => {
    fetchOrders('all'); // Tawagin ang database sa unang load

    document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            fetchOrders(this.dataset.filter); // Mag-fetch base sa filter
        });
    });
});

function fetchOrders(filterValue) {
    // Gamit ang Fetch API para tawagin ang WebMethod sa C#
    fetch('orders.aspx/GetOrders', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ filter: filterValue })
    })
        .then(response => response.json())
        .then(data => {
            renderOrders(data.d); // 'data.d' ang standard return ng ASP.NET WebMethods
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
                        <label>Total Amount</label>
                        <strong>₱${order.total.toLocaleString(undefined, { minimumFractionDigits: 2 })}</strong>
                    </div>
                    <div class="meta-item">
                        <label>Items</label>
                        <strong>${order.items} Qty</strong>
                    </div>
                </div>
            </div>
            <div class="order-card-actions">
                <button type="button" class="btn-outline">Invoice</button>
                <button type="button" class="btn-primary">Track</button>
            </div>
        </div>
    `).join('');
}