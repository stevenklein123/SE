document.addEventListener('DOMContentLoaded', () => {
    initMenu();
    initCart();
    initSearch();
    updateCartCountFromDB();
    initPasswordToggles();
});

/* ======================
   MENU (FIXED)
====================== */
function initMenu() {
    const hamburger = document.getElementById('hamburger');
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');
    const closeBtn = document.getElementById('closeSidebar');
    const links = document.querySelectorAll('.nav-item');

    if (!hamburger || !sidebar) return;

    const openSidebar = () => {
        sidebar.classList.add('active');
        if (overlay) overlay.classList.add('active');
    };

    const closeSidebar = () => {
        sidebar.classList.remove('active');
        if (overlay) overlay.classList.remove('active');
    };

    hamburger.addEventListener('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        openSidebar();
    });

    if (overlay) overlay.addEventListener('click', closeSidebar);
    if (closeBtn) closeBtn.addEventListener('click', closeSidebar);

    // ACTIVE LINK HIGHLIGHT
    links.forEach(link => {
        if (link.href === window.location.href) {
            link.classList.add("active");
        }
    });
}
/* ======================
   PASSWORD TOGGLER (FIXED)
====================== */
function initPasswordToggles() {
    document.querySelectorAll('.toggle-pass').forEach(function (btn) {
        var targetId = btn.getAttribute('data-target');
        var input = targetId ? document.getElementById(targetId) : null;

        btn.addEventListener('click', function () {
            if (!input) return;

            if (input.type === 'password') {
                input.type = 'text';
                btn.innerText = 'Hide';
            } else {
                input.type = 'password';
                btn.innerText = 'Show';
            }
        });
    });
}

/* ======================
   CART INIT
====================== */
function initCart() {
    const buttons = document.querySelectorAll('.btn-add-cart');

    buttons.forEach(btn => {
        btn.addEventListener('click', () => addToCartDB(btn));
    });

    const cartIcon = document.getElementById('cartIcon');
    if (cartIcon) {
        cartIcon.addEventListener('click', () => {
            window.location.href = "cart.aspx";
        });
    }
}

/* ======================
   ADD TO CART (DB)
====================== */
function addToCartDB(btn) {
    const productId = parseInt(btn.getAttribute('data-id'));
    const productName = btn.getAttribute('data-name');

    if (!productId) {
        console.error("Product ID is missing!");
        showToast("Error: Product ID missing");
        return;
    }

    fetch('/dealers/dashboard.aspx/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({
            productId: productId,
            quantity: 1
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.d && data.d.includes("Success")) {
                animateBtn(btn);
                updateCartCount();
                showToast(productName + " added to cart!");
            } else if (data.d) {
                showToast(data.d);
            } else {
                showToast("Failed to add item to cart");
            }
        })
        .catch(err => {
            console.error("Fetch error:", err);
            showToast("Error connecting to server");
        });
}

/* ======================
   CART COUNT
====================== */
function updateCartCount() {
    fetch('/dealers/dashboard.aspx/GetCartCount', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' }
    })
        .then(res => res.json())
        .then(data => {
            const badge = document.getElementById('cartBadge');
            if (badge) badge.textContent = data.d;
        })
        .catch(err => console.error("Cart count error:", err));
}

function updateCartCountFromDB() {
    updateCartCount();
}

/* ======================
   SEARCH
====================== */
function initSearch() {
    const input = document.getElementById('searchInput');
    if (!input) return;

    input.addEventListener('keyup', () => {
        const value = input.value.toLowerCase();

        document.querySelectorAll('.product-card').forEach(card => {
            const name = card.querySelector('h3').innerText.toLowerCase();
            card.style.display = name.includes(value) ? "block" : "none";
        });
    });
}

/* ======================
   UI HELPERS
====================== */
function animateBtn(btn) {
    btn.innerText = "Added ✓";
    btn.disabled = true;

    setTimeout(() => {
        btn.innerText = "Add to Cart";
        btn.disabled = false;
    }, 1200);
}

function showToast(msg) {
    const t = document.createElement("div");
    t.innerText = msg;

    t.style = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        background: #e91e63;
        color: #fff;
        padding: 12px 16px;
        border-radius: 10px;
        z-index: 9999;
    `;

    document.body.appendChild(t);
    setTimeout(() => t.remove(), 2000);
}