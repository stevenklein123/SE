(function () {

    function loadChatbase() {

        const script = document.createElement("script");
        script.src = "https://www.chatbase.co/embed.min.js";
        script.id = "bebNRvwQS_WvHNUnRo4sS";
        script.domain = "www.chatbase.co";

        script.onload = function () {

            if (window.chatbaseConfig) {

                window.chatbase("identify", {
                    userId: window.chatbaseConfig.userId,
                    userName: window.chatbaseConfig.userName,
                    userRole: window.chatbaseConfig.userRole
                });

            }

        };

        document.body.appendChild(script);
    }

    if (document.readyState === "complete") {
        loadChatbase();
    } else {
        window.addEventListener("load", loadChatbase);
    }

})();