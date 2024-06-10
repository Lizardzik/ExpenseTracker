document.addEventListener('DOMContentLoaded', function () {
    //Sidebar toggle 
    var sidebar = document.getElementById('default-sidebar').ej2_instances[0];
    var navbar = document.querySelector('.navbar');
    document.getElementById('toggle-btn').onclick = function () {
        sidebar.toggle();
        navbar.classList.toggle('shifted');
    }

    //Hide navbar on User's page
    if (window.location.pathname === '/Home/User') {
        navbar.style.display = 'none';
    }

    //loading gif on dashboard
    if (window.location.pathname === '/Dashboard' || window.location.pathname === '/Home/User') {   
        window.addEventListener('load', function () {

            setTimeout(function () {
                var loadingOverlay = document.getElementById('loading-overlay');
                if (loadingOverlay) {
                    loadingOverlay.classList.add('hidden');
                }

                var dashboardContent = document.getElementById('dashboard-content');
                if (dashboardContent) {
                    dashboardContent.style.display = 'block';
                }
            }, 500); 
        });
    } 
});