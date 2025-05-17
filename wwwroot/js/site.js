

function bindImgsInFindYourWay() {
    $("#elnurThemeLoader").show();
    $.ajax({
        url: "/Products/GetProductImages",
        success: function (data) {
            $("#elnurThemeLoader").hide();
            if (data.entity) {
                $("#findYourWay").html("");
                const grouped = {};

                data.entity.forEach(item => {
                    const category = item.productCategory?.name || 'Unknown';
                    if (!grouped[category]) {
                        grouped[category] = [];
                    }
                    grouped[category].push(item.imageGuid);
                });
                for (const category in grouped) {
                    $('#findYourWay').append(tempCardInFindYourWay(category, grouped));
                }
            } else {
                
            }
        },
        error: function (data) {
            $("#elnurThemeLoader").hide();
            console.log(data);
            $("#findYourWay").html("");
        }
    })
};
function tempCardInFindYourWay(category, grouped) {
    const images = grouped[category];
    const carouselId = category.toLowerCase().replace(/\s+/g, '') + "Lighting";

    let carouselInner = '';
    images.forEach((img, index) => {
        carouselInner += `
        <div class="carousel-item h-100 ${index === 0 ? 'active' : ''}">
            <img src="${img}" class="w-100 h-100 d-block" style="object-fit:contain !important" alt="${category} image">
        </div>`;
    });

    return `
    <div class="col px-2">
        <div id="${carouselId}" class="carousel slide h-100" data-bs-ride="carousel" data-bs-interval="2000">
            <div class="carousel-inner bg-white overflow-hidden rounded-3 p-1" style="height: 250px !important;">
                ${carouselInner}
            </div>
            <p class="mb-0 text-center theme-blue fw-bold">${category}</p>
        </div>
    </div>`;
}



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
