
/* BEGIN EXTERNAL SOURCE */

    function toggleSidebar() {
        document.getElementById("sidebar").classList.toggle("active");
        document.getElementById("overlay").classList.toggle("active");
    }

    var labels = /****************/;
    var data = /**************/;

    new Chart(document.getElementById("salesChart"), {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Sales',
                data: data,
                borderColor: '#e91e63',
                tension: 0.3
            }]
        }
    });

/* END EXTERNAL SOURCE */
