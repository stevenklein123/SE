document.addEventListener('DOMContentLoaded', function () {
    // Selectors gamit ang IDs na ni-render ng ASP.NET (suffix-based search)
    const passwordInput = document.querySelector('input[id*="password"]');
    const confirmInput = document.querySelector('input[id*="confirmPassword"]');

    if (passwordInput) {
        passwordInput.addEventListener('input', function () {
            validatePassword(this.value);
        });
    }

    if (confirmInput) {
        confirmInput.addEventListener('input', validatePasswordMatch);
    }
});

function validatePassword(password) {
    const rules = {
        'req-length': password.length >= 8,
        'req-upper': /[A-Z]/.test(password),
        'req-number': /[0-9]/.test(password),
        'req-special': /[!@#$%^&*]/.test(password)
    };

    for (const [id, isMet] of Object.entries(rules)) {
        const el = document.getElementById(id);
        if (el) {
            isMet ? el.classList.add('met') : el.classList.remove('met');
        }
    }
}

function validatePasswordMatch() {
    const password = document.querySelector('input[id*="password"]').value;
    const confirm = document.querySelector('input[id*="confirmPassword"]').value;
    const errorDiv = document.getElementById('errorMessage');

    if (confirm && password !== confirm) {
        errorDiv.textContent = "Passwords do not match";
        errorDiv.style.display = "block";
    } else {
        errorDiv.style.display = "none";
    }
}

function showPassword(inputId, toggleEl) {
    const input = document.querySelector(`input[id*="${inputId}"]`);
    if (input.type === "password") {
        input.type = "text";
        toggleEl.textContent = "Hide";
    } else {
        input.type = "password";
        toggleEl.textContent = "Show";
    }
}