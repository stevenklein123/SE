function filterRank(rank) {
    let cards = document.querySelectorAll(".user-card");

    cards.forEach(card => {
        let userRank = card.getAttribute("data-rank");

        if (rank === "All" || userRank === rank) {
            card.style.display = "block";
        } else {
            card.style.display = "none";
        }
    });
}

function searchUser() {
    let input = document.getElementById("searchInput").value.toLowerCase();
    let cards = document.querySelectorAll(".user-card");

    cards.forEach(card => {
        let name = card.querySelector("h3").innerText.toLowerCase();

        card.style.display = name.includes(input) ? "block" : "none";
    });
}