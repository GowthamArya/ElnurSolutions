var currentPage = 1;
var pageSize = 8;

$(document).ready(function () {
    if (!queryParams?.category) {
        bindProducts();
    }
    bindCategoryDropDown();
    $("#btnSearchProducts").on("click",function () {
        bindProducts();
    });
    $("#categorySelect").on("change", function () {
        bindProducts();
    });
    $("#searchInput").on("keypress", function (e) {
        if (e.which === 13) {
            bindProducts();
        }
    });
    $('#btnReset').on('click', function () {
        $('#searchInput').val('');
        $('#categorySelect').prop('selectedIndex', 0);
        bindProducts();
    });
})

var bindCategoryDropDown = async function () {
    $.ajax({
        url: "/ProductCategories/GetProductCategories",
        success: function (data) {
            if (data.entity) {
                $("#categorySelect").html(`<option value="">All</option>`);
                data.entity.forEach(function (productCategory) {
                    $("#categorySelect").append(`<option value="${productCategory.id}">${productCategory.name}</option>`);
                });
                if (queryParams?.category) {
                    var category = data.entity.find(c => c.name == queryParams.category);
                    category && $("#categorySelect").val(category.id).trigger('change');
                }
            } else {
                $("#productsContainer").html("");
            }
        },
        error: function (data) {
            console.log(data);
            $("#productsContainer").html("  ");
        }
    })
};


var bindProducts = async function (page = 1) {
    $("#elnurThemeLoader").show();
    $.ajax({
        url: "/Products/GetProductsBySearchCriteria",
        data: {
            lookupText: $("#searchInput").val(),
            categoryId: $("#categorySelect").val(),
            page: page,
            pageSize: pageSize
        },
        success: function (data) {
            $("#elnurThemeLoader").hide();
            if (data.entity) {
                $("#productsContainer").html("");
                data.entity.forEach(function (product) {
                    $("#productsContainer").append(productCardTemp(product));
                });

                renderPagination(data.totalRecords, page);
            } else {
                $("#productsContainer").html("");
                $("#paginationContainer").html("");
            }
        },
        error: function (data) {
            $("#elnurThemeLoader").hide();
            console.log(data);
            $("#productsContainer").html("");
            $("#paginationContainer").html("");
        }
    });
};

var renderPagination = function (totalRecords, currentPage) {
    var totalPages = Math.ceil(totalRecords / pageSize);
    var paginationHtml = `<nav aria-label="Page navigation example"><ul class="pagination text-white justify-content-end mb-0 pb-3">`;

    // Previous button
    paginationHtml += `<li class="page-item ${currentPage === 1 ? 'disabled' : 'theme-blue'}">
        <a class="page-link" href="#" onclick="bindProducts(${currentPage - 1})">Previous</a>
    </li>`;

    for (var i = 1; i <= totalPages; i++) {
        paginationHtml += `<li class="page-item ${i === currentPage ? 'active' : ''}">
            <a class="page-link bg-blue" href="#" onclick="bindProducts(${i})">${i}</a>
        </li>`;
    }

    // Next button
    paginationHtml += `<li class="page-item ${currentPage === totalPages ? 'disabled' : 'theme-blue'}">
        <a class="page-link" href="#" onclick="bindProducts(${currentPage + 1})">Next</a>
    </li>`;

    paginationHtml += `</ul></nav>`;

    $("#paginationContainer").html(paginationHtml);
};


var bindProductDetails = async function (id) {
    $("#elnurThemeLoader").show();
    $.ajax({
        url: "/Products/GetDetails?id=" + id,
        success: function (data) {
            $("#elnurThemeLoader").hide();
            if (data.entity) {
                $("#productCatergoryName").html(data.entity.productCategory.name);
                $("#productCatergoryDescription").html(data.entity.productCategory.description);
                $(".productName").html(data.entity.name);
                $("#richTextDescription").closest(".col-12").hide();
                $("#richTextKeyFeatures").closest(".col-12").hide();
                if (data.entity.richTextArea) {
                    $("#richTextDescription").html(data.entity.richTextArea);
                    $("#richTextDescription").closest(".col-12").show();
                }
                if (data.entity.keyfeatures) {
                    $("#richTextKeyFeatures").html(data.entity.keyfeatures);
                    $("#richTextKeyFeatures").closest(".col-12").show();
                }
                $("#description").html(data.entity.description);    
                $("#imgProduct").attr("src", data.entity.imageGuid);
                $("#btnContactUs").attr("href", "/Contact?ProductName="+data.entity.name);
                $("#btnDownloadProductFile").hide();
                if (data.entity.fileUrl) {
                    $("#btnDownloadProductFile").show();
                    $("#btnDownloadProductFile").attr("href", data.entity.fileUrl);
                }
            }
        },
        error: function (data) {
            console.log(data);
            $("#elnurThemeLoader").hide();
            $("#productsContainer").html("");
        }
    })
};


var productCardTemp = function (data) {
    return `<div class="col rounded rounded-3">
        <div class="card w-100 shadow">
            <img src="${data.imageGuid || `/UploadedFiles/ElnurLogo.png`}" style="height: 27vh; object-fit: contain;" class="card-img-top border shadow-sm img-fluid d-block mx-auto img-contain" alt="${data.name}">
            <div class="card-body">
                <h5 class="card-title fs-6 theme-blue">${data.name}</h5>
                <p class="card-text text-truncate">${data.description}</p>
                <button class="btn btn-outline-success btn-sm" data-bs-toggle="modal" onclick="bindProductDetails('${data.id}')" data-id="${data.id}" data-bs-target="#exampleModal">View Details</button>
                ${data.fileUrl ? `<a href="${data.fileUrl}" target="_blank" class="btn bg-blue btn-sm" ><i class="fa-regular fa-file-lines me-2"></i>Download</a>` : ''}
            </div>
        </div>
    </div>`
}