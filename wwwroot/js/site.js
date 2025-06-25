function bindddProductsCategories() {
    $.ajax({
        url: "/ProductCategories/GetProductCategories",
        success: function (data) {
            $("#ulproductsDropdown").empty();
            if (data.entity) {
                $("#ulproductsDropdown").html(`<li><a class="dropdown-item" href="/Products">All</a></li>`)
                data.entity.forEach(function (productCategory) {
                    $("#ulproductsDropdown").append(`<li><a class="dropdown-item" href="/Products?category=${productCategory.name}">${productCategory.name}</a></li>`);
                });
            }
        },
        error: function (data) {
            console.log(data);
            $("#ulproductsDropdown").empty();
        }
    })
}

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
    <a class="col px-2" href='/Products?category=${category}'>
        <div id="${carouselId}" class="carousel slide h-100" data-bs-ride="carousel" data-bs-interval="2000">
            <div class="carousel-inner bg-white overflow-hidden rounded-3 p-1" style="height: 250px !important;">
                ${carouselInner}
            </div>
            <p class="mb-0 text-center theme-blue fw-bold">${category}</p>
        </div>
    </a>`;
}



const navbar = document.getElementById('mainNavbar');
const toggler = document.querySelector('.navbar-toggler');

toggler.addEventListener('click', () => {
    navbar.classList.toggle('navbar-toggler-collapsed');
});

$(function () {
    $('[data-bs-toggle="dropdown"]').each(function () {
        var $this = $(this);
        var $parent = $this.parent();
        var bsDropdown = bootstrap.Dropdown.getOrCreateInstance(this);

        $parent.hover(
            function () {
                clearTimeout($parent.data('timeout'));
                bsDropdown.show();
            },
            function () {
                const timeout = setTimeout(() => {
                    bsDropdown.hide();
                }, 50);
                $parent.data('timeout', timeout);
            }
        );

    });

});

 function googleTranslateElementInit() {
     new google.translate.TranslateElement({pageLanguage: 'en'}, 'google_translate_element');
 }


function rich_file_upload_handler(file, callback, optionalIndex, optionalFiles) {
    var uploadhandlerpath = "/Account/Upload";

    console.log("upload", file, "to", uploadhandlerpath);

    function append(parent, tagname, csstext) {
        var tag = parent.ownerDocument.createElement(tagname);
        if (csstext) tag.style.cssText = csstext;
        parent.appendChild(tag);
        return tag;
    }

    var uploadcancelled = false;
    var xhr = new XMLHttpRequest();

    var dialogouter = append(document.body, "div", "display:flex;align-items:center;justify-content:center;z-index:999999;position:fixed;left:0px;top:0px;width:100%;height:100%;background-color:rgba(128,128,128,0.5)");
    var dialoginner = append(dialogouter, "div", "background-color:white;border:solid 1px gray;border-radius:15px;padding:15px;min-width:200px;box-shadow:2px 2px 6px #7777");

    var line1 = append(dialoginner, "div", "text-align:center;font-size:1.2em;margin:0.5em;");
    line1.innerText = "Uploading...";

    var totalsize = file.size;
    var sentsize = 0;

    if (optionalFiles && optionalFiles.length > 1) {
        totalsize = 0;
        for (var i = 0; i < optionalFiles.length; i++) {
            totalsize += optionalFiles[i].size;
            if (i < optionalIndex) sentsize = totalsize;
        }
        console.log(totalsize, optionalIndex, optionalFiles);
        line1.innerText = "Uploading..." + (optionalIndex + 1) + "/" + optionalFiles.length;
    }

    var line2 = append(dialoginner, "div", "text-align:center;font-size:1.0em;margin:0.5em;");
    line2.innerText = "0%";

    var progressbar = append(dialoginner, "div", "border:solid 1px gray;margin:0.5em;");
    var progressbg = append(progressbar, "div", "height:12px");

    var line3 = append(dialoginner, "div", "text-align:center;font-size:1.0em;margin:0.5em;");
    var btn = append(line3, "button");
    btn.className = "btn btn-primary";
    btn.innerText = "Cancel";
    btn.onclick = function () {
        uploadcancelled = true;
        xhr.abort();
    };

    var formData = new FormData();
    formData.append('file', file);

    xhr.open("POST", uploadhandlerpath, true);

    xhr.onload = xhr.onerror = xhr.onabort = function (event) {
        dialogouter.parentNode.removeChild(dialogouter);
        if (uploadcancelled) {
            console.log("Upload cancelled");
            callback(null, "cancelled");
        } else if (xhr.status === 200) {
            try {
                var data = JSON.parse(xhr.responseText);
                if (data && data.entity) {
                    callback(data.entity); // Success, return image URL
                } else {
                    callback(null, "invalid-response");
                }
            } catch (e) {
                callback(null, "parse-error");
            }
        } else {
            console.log("Upload failed", xhr);
            callback(null, "http-error-" + xhr.status);
        }
    };

    xhr.upload.onprogress = function (pe) {
        var percent = Math.floor(100 * (sentsize + pe.loaded) / totalsize);
        line2.innerText = percent + "%";
        progressbg.style.cssText = "background-color:green;width:" + (percent * progressbar.offsetWidth / 100) + "px;height:12px;";
    };

    xhr.send(formData);
}
