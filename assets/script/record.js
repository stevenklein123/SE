function toggleSidebar() {
    document.getElementById('sidebar').classList.toggle('active');
    document.getElementById('overlay').classList.toggle('active');
}
function toggleDropdown(link) {
    link.parentElement.classList.toggle('open');
}
function computeChange() {
    const due = parseFloat(document.getElementById('<%= txtAmountDue.ClientID %>').value.replace(/[PHP ,]/g, '')) || 0;
    const paid = parseFloat(document.getElementById('<%= txtPayment.ClientID %>').value) || 0;
    const change = paid - due;
    document.getElementById('changeDisplay').textContent = 'PHP ' + (change >= 0 ? change.toFixed(2) : '0.00');
    document.getElementById('<%= hdnChange.ClientID %>').value = change >= 0 ? change.toFixed(2) : '0';
}