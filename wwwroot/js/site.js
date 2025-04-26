const navbar = document.getElementById('mainNavbar');
const toggler = document.querySelector('.navbar-toggler');

toggler.addEventListener('click', () => {
    navbar.classList.toggle('navbar-toggler-collapsed');
});

$(function () {
    var $dropdown = $('#aboutDropdown');
    var toggle = $dropdown.find('[data-bs-toggle="dropdown"]')[0];
    var bsDropdown = bootstrap.Dropdown.getOrCreateInstance(toggle);

    $dropdown.hover(
        function () {
            bsDropdown.show();
        },
        function () {
            bsDropdown.hide();
            // Simulate "mouseleave" by manually hiding the dropdown
            $(toggle).dropdown('hide');
        }
    );
});

 function googleTranslateElementInit() {
     new google.translate.TranslateElement({pageLanguage: 'en'}, 'google_translate_element');
 }
