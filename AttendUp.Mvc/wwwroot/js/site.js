document.addEventListener('DOMContentLoaded', function () {
    var hamburger = document.getElementById('nav-hamburger');
    var navLinks = document.getElementById('nav-links');

    if (hamburger && navLinks) {
        hamburger.addEventListener('click', function (e) {
            e.stopPropagation();
            hamburger.classList.toggle('open');
            navLinks.classList.toggle('open');
        });

        document.addEventListener('click', function (e) {
            if (!navLinks.contains(e.target) && !hamburger.contains(e.target)) {
                hamburger.classList.remove('open');
                navLinks.classList.remove('open');
            }
        });
    }

    var aantalEl = document.getElementById('aantal');
    if (aantalEl) {
        aantalEl.addEventListener('change', function () {
            const aantal = parseInt(this.value);
            const section = document.getElementById('extra-persons-section');
            const p1 = document.getElementById('extra-person-1');
            const p2 = document.getElementById('extra-person-2');

            section.style.display = aantal > 1 ? 'block' : 'none';
            p1.style.display = aantal >= 2 ? 'block' : 'none';
            p2.style.display = aantal >= 3 ? 'block' : 'none';

            toggleRequired(p1, aantal >= 2);
            toggleRequired(p2, aantal >= 3);
        });
    }

    var sidebar = document.getElementById('sidebar');
    var toggle = document.querySelector('.sidebar-toggle');

    window.toggleSidebar = function () {
        if (sidebar) sidebar.classList.toggle('open');
    };

    document.addEventListener('click', function (e) {
        if (sidebar && toggle && !sidebar.contains(e.target) && !toggle.contains(e.target)) {
            sidebar.classList.remove('open');
        }
    });
});

function toggleRequired(container, isRequired) {
    container.querySelectorAll('input, select').forEach(input => {
        if (isRequired) {
            input.setAttribute('required', 'required');
        } else {
            input.removeAttribute('required');
        }
    });
}