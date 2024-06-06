document.addEventListener('DOMContentLoaded', function () {
    var sidebar = document.getElementById('default-sidebar').ej2_instances[0];
    var navbar = document.querySelector('.navbar');
    document.getElementById('toggle-btn').onclick = function () {
        sidebar.toggle();
        navbar.classList.toggle('shifted');
    }
});