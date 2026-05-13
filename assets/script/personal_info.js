document.addEventListener('DOMContentLoaded', () => {
    initProfileView();
});

function initProfileView() {
    loadProfileView();

    const editBtn = document.getElementById('btnEdit');
    const saveBtn = document.getElementById('btnSave');
    const cancelBtn = document.getElementById('btnCancel');

    if (editBtn) editBtn.addEventListener('click', enterEditMode);
    if (saveBtn) saveBtn.addEventListener('click', saveProfileView);
    if (cancelBtn) cancelBtn.addEventListener('click', exitEditMode);

    // Ensure inputs are disabled by default (read-only view). JS will enable on edit.
    document.querySelectorAll('.edit-input').forEach(i => {
        try { i.style.display = i.style.display || 'none'; } catch (e) {}
        i.disabled = true;
    });

    // store original values so Cancel can revert changes
    window._profileOriginal = {};
}

function loadProfileView() {
    fetch('/view/personal_info.aspx/GetProfile', {
        method: 'POST',
        credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json' },
        body: '{}'
    })
        .then(r => r.json())
        .then(res => {
            const data = res && res.d ? res.d : res;
            if (!data || data.error) {
                document.getElementById('lblMsg').innerText = data && data.error ? data.error : 'Unable to load profile.';
                return;
            }

            // populate spans and inputs
            setText('spanFirst', data.first_name);
            setVal('inputFirst', data.first_name);
            setText('spanMiddle', data.middle_name);
            setVal('inputMiddle', data.middle_name);
            setText('spanLast', data.last_name);
            setVal('inputLast', data.last_name);
            setText('spanEmail', data.email);
            setVal('inputEmail', data.email);
            setText('spanBirthday', data.birthday);
            setVal('inputBirthday', data.birthday);
            setText('spanContact', data.contact_number);
            setVal('inputContact', data.contact_number);
            setText('spanMobile', data.mobile_number);
            setVal('inputMobile', data.mobile_number);
            setText('spanAddress', data.address);
            setVal('inputAddress', data.address);
            setText('spanZip', data.zip_code);
            setVal('inputZip', data.zip_code);
        })
        .catch(err => console.error('Load profile view error', err));
}

function setText(id, v) { const e = document.getElementById(id); if (e) e.innerText = v || ''; }
function setVal(id, v) { const e = document.getElementById(id); if (e) e.value = v || ''; }

function enterEditMode() {
    // cache original values
    document.querySelectorAll('[id^="span"]').forEach(s => {
        const id = s.id.replace('span', '');
        window._profileOriginal[id] = s.innerText;
    });

    document.querySelectorAll('.edit-input').forEach(i => { i.style.display = 'block'; i.disabled = false; });
    document.querySelectorAll('[id^="span"]').forEach(s => s.style.display = 'none');
    document.getElementById('btnEdit').style.display = 'none';
    document.getElementById('btnSave').style.display = 'inline-block';
    document.getElementById('btnCancel').style.display = 'inline-block';
}

function exitEditMode() {
    // revert inputs to original values when cancelling
    document.querySelectorAll('.edit-input').forEach(i => {
        const id = i.id.replace('input', '');
        if (window._profileOriginal && window._profileOriginal[id] !== undefined) {
            if (i.tagName.toLowerCase() === 'textarea') i.value = window._profileOriginal[id];
            else i.value = window._profileOriginal[id];
        }
        i.style.display = 'none';
        i.disabled = true;
    });
    document.querySelectorAll('[id^="span"]').forEach(s => s.style.display = 'inline');
    document.getElementById('btnEdit').style.display = 'inline-block';
    document.getElementById('btnSave').style.display = 'none';
    document.getElementById('btnCancel').style.display = 'none';
}

function saveProfileView() {
    const saveBtn = document.getElementById('btnSave');
    if (saveBtn) { saveBtn.disabled = true; saveBtn.innerText = 'Saving...'; }

    const data = {
        firstName: document.getElementById('inputFirst').value.trim(),
        middleName: document.getElementById('inputMiddle').value.trim(),
        lastName: document.getElementById('inputLast').value.trim(),
        birthday: document.getElementById('inputBirthday').value,
        contactNumber: document.getElementById('inputContact').value.trim(),
        mobileNumber: document.getElementById('inputMobile').value.trim(),
        address: document.getElementById('inputAddress').value.trim(),
        zipCode: document.getElementById('inputZip').value.trim()
    };

    fetch('/view/personal_info.aspx/UpdateProfile', {
        method: 'POST',
        credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(res => {
            const d = res && res.d ? res.d : res;
            if (d && d.success) {
                // update spans
                setText('spanFirst', data.firstName);
                setText('spanMiddle', data.middleName);
                setText('spanLast', data.lastName);
                setText('spanBirthday', data.birthday);
                setText('spanContact', data.contactNumber);
                setText('spanMobile', data.mobileNumber);
                setText('spanAddress', data.address);
                setText('spanZip', data.zipCode);

                exitEditMode();
                const msg = document.getElementById('lblMsg'); if (msg) { msg.style.color = 'green'; msg.innerText = 'Profile saved.'; }
            } else {
                const msg = document.getElementById('lblMsg');
                if (msg) { msg.style.color = '#b00020'; msg.innerText = (d && d.message) || 'Error saving profile.'; }
                console.error('UpdateProfile returned error', d);
            }
        })
        .catch(err => {
            console.error('Save profile view error', err);
            const msg = document.getElementById('lblMsg'); if (msg) { msg.style.color = '#b00020'; msg.innerText = 'Error saving profile.'; }
        })
        .finally(() => {
            if (saveBtn) { saveBtn.disabled = false; saveBtn.innerText = 'Save'; }
        });
}
