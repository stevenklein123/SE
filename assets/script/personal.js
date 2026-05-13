document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('continueBtn').addEventListener('click', saveTempProfile);
});

function saveTempProfile() {

    const msg = document.getElementById("profileMsg");

    const data = {
        firstName: firstName.value,
        middleName: middleName.value,
        lastName: lastName.value,
        email: email.value,
        birthday: birthday.value,
        contactNumber: contactNumber.value,
        mobileNumber: mobileNumber.value,
        address: address.value,
        zipCode: zipCode.value,
        termsAccepted: document.getElementById("termsAccepted").checked ? 1 : 0
    };

    fetch("personal.aspx/SaveTempProfile", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    })
        .then(res => res.json())
        .then(res => {

            if (res.d.success) {
                window.location.href = res.d.redirect;
            } else {
                msg.innerText = res.d.message;
                msg.style.color = "red";
            }

        })
        .catch(() => {
            msg.innerText = "Server error";
        });
}