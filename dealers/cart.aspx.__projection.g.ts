
/* BEGIN EXTERNAL SOURCE */


    // CLEAR OLD CHAT CACHE
    localStorage.removeItem("chatbase-session");

    sessionStorage.removeItem("chatbase-session");

    // CHATBASE CONFIG
    window.chatbaseConfig = {
        chatbotId: "tARzRJPxLzrHZlYqWgswv",
        domain: "www.chatbase.co"
    };

    (function () {

        var script =
            document.createElement("script");

        script.src =
            "https://www.chatbase.co/embed.min.js";

        script.defer = true;

        script.onload = function () {

            setTimeout(function () {

                // IDENTIFY CURRENT USER
                window.chatbase("identify", {
                    token: '/**************************/'
                });

                // FORCE NEW CONVERSATION
                window.chatbase("newConversation");

            }, 1500);

        };

        document.body.appendChild(script);

    })();


/* END EXTERNAL SOURCE */
