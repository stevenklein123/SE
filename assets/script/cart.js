document.addEventListener("DOMContentLoaded", () => {
    loadCart();

    // Clear All should show a confirmation modal
    document.getElementById("btnClearCart")?.addEventListener("click", showClearModal);

    // shipping method buttons
    document.querySelectorAll('.ship-btn').forEach(b => {
        b.addEventListener('click', function () {
            document.querySelectorAll('.ship-btn').forEach(x => x.classList.remove('active'));
            this.classList.add('active');
            const method = this.getAttribute('data-method');
            const hidden = document.getElementById('shippingMethod');
            if (hidden) hidden.value = method;
            loadCart();
        });
    });
});

function loadCart() {
    fetch("cart.aspx/GetCart", {
        method: "POST",
        headers: { "Content-Type": "application/json" }
    })
        .then(res => res.json())
        .then(res => {
            const cart = res.d;
            const container = document.getElementById("cartItemsContainer");

            if (!cart || cart.length === 0) {
                container.innerHTML = ""; // IMPORTANT FIX
                document.getElementById("cartEmpty").style.display = "block";
                document.getElementById("cartSummary").style.display = "none";
                return;
            }

            document.getElementById("cartEmpty").style.display = "none";
            document.getElementById("cartSummary").style.display = "block";

            let html = "";
            let subtotal = 0;

            cart.forEach(item => {
                const price = parseFloat(item.price);
                const qty = parseInt(item.quantity, 10);
                subtotal += price * qty;

                const img = item.image && item.image.length ? item.image : '/assets/images/no-image.png';

                html += `
                <div class="cart-card" data-id="${item.id}">
                    <div class="cart-card-left">
                        <div class="cart-card-img"><img src="${img}" alt="${escapeHtml(item.name)}" /></div>
                    </div>
                    <div class="cart-card-body">
                        <div class="cart-item-name">${escapeHtml(item.name)}</div>
                        <div class="cart-item-price">₱${price.toFixed(2)}</div>

                        <div class="quantity-control">
                            <button type="button" class="quantity-btn minus" data-id="${item.id}" aria-label="Decrease">−</button>
                            <input class="quantity-input" data-id="${item.id}" value="${qty}" readonly />
                            <button type="button" class="quantity-btn plus" data-id="${item.id}" aria-label="Increase">+</button>
                        </div>
                    </div>
                    <div class="cart-card-actions">
                        <button type="button" class="remove-btn" data-id="${item.id}" title="Remove item">🗑 Remove</button>
                    </div>
                </div>`;
            });

            container.innerHTML = html;

            // attach listeners for quantity and remove
            container.querySelectorAll('.quantity-btn').forEach(btn => {
                btn.addEventListener('click', function () {
                    const id = parseInt(this.getAttribute('data-id'), 10);
                    const parent = this.closest('.cart-card');
                    const input = parent.querySelector('.quantity-input');
                    let current = parseInt(input.value, 10);

                    if (this.classList.contains('plus')) current += 1;
                    else current = Math.max(1, current - 1);

                    updateQty(id, current);
                });
            });

            container.querySelectorAll('.remove-btn').forEach(b => {
                b.addEventListener('click', function (ev) {
                    ev.preventDefault();
                    const id = parseInt(this.getAttribute('data-id'), 10);
                    showRemoveModal(id);
                });
            });

            updateTotal(subtotal);
        });
}

function escapeHtml(str) {
    if (!str) return '';
    return str.replace(/[&<>"'`]/g, function (s) {
        return ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":"&#39;","`":"&#96;"})[s];
    });
}

function updateTotal(subtotal) {

    const shipping =
        document.getElementById("shippingMethod").value;

    let fee =
        (shipping === "Delivery") ? 50 : 0;

    document.getElementById("subtotal").innerText =
        "₱" + subtotal.toFixed(2);

    document.getElementById("shipping").innerText =
        "₱" + fee.toFixed(2);

    document.getElementById("total").innerText =
        "₱" + (subtotal + fee).toFixed(2);
}

function checkout() {

    const shipping = document.getElementById("shippingMethod").value;
    const paymentMethod = document.getElementById("paymentMethod").value;

    fetch("cart.aspx/Checkout", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            shipping: shipping,
            paymentMethod: paymentMethod
        })
    })
        .then(res => res.json())
        .then(res => {

            const d = res.d;

            if (d && d.error) {
                alert(d.error);
                return;
            }

            alert(`Payment successful via ${paymentMethod}`);

            loadCart();

        })
}

function clearCart() {
    fetch("cart.aspx/ClearCart", {
        method: "POST",
        headers: { "Content-Type": "application/json" }
    })
        .then(() => {
            document.getElementById("cartItemsContainer").innerHTML = "";
            document.getElementById("cartEmpty").style.display = "block";
            document.getElementById("cartSummary").style.display = "none";
            hideClearModal();
        });
}

// Clear all confirmation modal
function showClearModal() {
    const m = document.getElementById('clearAllModal');
    if (!m) return clearCart();
    m.classList.add('active');
}

function hideClearModal() {
    const m = document.getElementById('clearAllModal');
    if (!m) return; m.classList.remove('active');
}

function confirmClear() {
    clearCart();
}

function removeItem(id) {
    fetch("cart.aspx/RemoveItem", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id: id })
    })
        .then(() => loadCart());
}

// update quantity on server
function updateQty(id, qty) {
    fetch("cart.aspx/UpdateQty", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id: id, qty: qty })
    })
        .then(res => res.json().catch(() => {}))
        .then(() => {
            // refresh cart to get server-validated prices/qty
            loadCart();
        })
        .catch(err => console.error('Update qty error', err));
}

// Remove confirmation modal logic
let _pendingRemoveId = null;
function showRemoveModal(id) {
    _pendingRemoveId = id;
    const m = document.getElementById('removeConfirmModal');
    if (!m) { removeItem(id); return; }
    m.classList.add('active');
}

function hideRemoveModal() {
    const m = document.getElementById('removeConfirmModal');
    if (!m) return; m.classList.remove('active');
    _pendingRemoveId = null;
}

function confirmRemove() {
    if (!_pendingRemoveId) return hideRemoveModal();
    removeItem(_pendingRemoveId);
    hideRemoveModal();
}

function addToCart(btn) {
    const productId = btn.getAttribute("data-id");

    fetch("dashboard.aspx/AddToCart", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            productId: parseInt(productId),
            quantity: 1
        })
    })
        .then(res => res.json())
        .then(() => {
            alert("Added to cart!");
            loadCartCount();
        });
}