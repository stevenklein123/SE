document.addEventListener("DOMContentLoaded", function () {

    // GO BACK BUTTON
    const backBtn = document.getElementById("backBtn");

    if (backBtn) {
        backBtn.addEventListener("click", function () {

            // if galing sa ibang page
            if (document.referrer !== "") {
                window.history.back();
            } else {
                window.location.href = "/auth_pages/personal.aspx";
            }

        });
    }

    // FAQ TOGGLE
    const faqQuestions = document.querySelectorAll(".faq-question");

    faqQuestions.forEach(function (question) {
        question.addEventListener("click", function () {

            this.classList.toggle("active");

            const answer = this.nextElementSibling;

            if (answer.style.maxHeight) {
                answer.style.maxHeight = null;
            } else {
                answer.style.maxHeight = answer.scrollHeight + "px";
            }

        });
    });

});