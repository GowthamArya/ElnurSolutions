﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.addEventListener('scroll', function () {
    const navbar = document.getElementById('mainNavbar');
    if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
    } else {
        navbar.classList.remove('scrolled');
    }
});
const navbar = document.getElementById('mainNavbar');
const toggler = document.querySelector('.navbar-toggler');

toggler.addEventListener('click', () => {
    navbar.classList.toggle('navbar-toggler-collapsed');
});

$("document").ready(function () {
    var $dropdown = $('#aboutDropdown');
    $dropdown.hover(
        function () {
            var toggle = $(this).find('[data-bs-toggle="dropdown"]')[0];
            var bsDropdown = bootstrap.Dropdown.getOrCreateInstance(toggle);
            bsDropdown.show();
        },
        function () {
            var toggle = $(this).find('[data-bs-toggle="dropdown"]')[0];
            var bsDropdown = bootstrap.Dropdown.getOrCreateInstance(toggle);
            bsDropdown.hide();
        }
    );
})
 function googleTranslateElementInit() {
     new google.translate.TranslateElement({pageLanguage: 'en'}, 'google_translate_element');
 }
