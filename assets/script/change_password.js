function _bySuffix(suffix) {
    return document.querySelector('[id$="' + suffix + '"]');
}

function validateChange() {
    const cur = _bySuffix('txtCurrent');
    const nw = _bySuffix('txtNew');
    const conf = _bySuffix('txtConfirm');

    if (!cur || !nw || !conf) return true; // fallback to server validation

    if (nw.value.length < 6) {
        showToast('New password must be at least 6 characters');
        nw.focus();
        return false;
    }

    if (nw.value !== conf.value) {
        showToast('New password and confirmation do not match');
        conf.focus();
        return false;
    }

    return true;
}

// expose for inline OnClientClick
window.validateChange = validateChange;
