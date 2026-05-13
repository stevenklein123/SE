document.addEventListener('DOMContentLoaded', function () {
    // Kinukuha ang actual ID na ni-render ng ASP.NET
    const passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
    const loginBtn = document.getElementById('<%= btnLogin.ClientID %>');

    if (passwordInput && loginBtn) {
        // Submit on Enter key
        passwordInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault(); // Iwasan ang default form submission behavior
                loginBtn.click();
            }
        });
    }
});

// Utility function para sa error message
function showError(message) {
    const errorLabel = document.getElementById('<%= lblError.ClientID %>');
    const errorDiv = document.getElementById('errorMessage');

    if (errorLabel && errorDiv) {
        errorLabel.textContent = message;
        errorDiv.classList.add('show');

        // Shake effect para sa error (aesthetic/pro feel)
        errorDiv.style.animation = 'none';
        errorDiv.offsetHeight; // trigger reflow
        errorDiv.style.animation = 'shake 0.5s';

        setTimeout(() => {
            errorDiv.classList.remove('show');
        }, 5000);
    }
}

// Success message handler
function showSuccess(message) {
    const successDiv = document.getElementById('successMessage');
    if (successDiv) {
        successDiv.textContent = message;
        successDiv.classList.add('show');
        setTimeout(() => {
            successDiv.classList.remove('show');
        }, 3000);
    }
}

function showPassword() {
    // Dahil Static na ang mode, 'txtPassword' na ang ID sa browser
    const passwordField = document.getElementById('txtPassword');
    const toggleText = document.getElementById('togglePassword');

    if (passwordField && toggleText) {
        if (passwordField.type === "password") {
            passwordField.type = "text";
            toggleText.textContent = "Hide Password";
        } else {
            passwordField.type = "password";
            toggleText.textContent = "Show Password";
        }
    }
}