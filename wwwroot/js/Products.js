$(document).ready(function () {
    bindProducts();
    bindCategoryDropDown();
    $("#btnSearchProducts").on("click",function () {
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
            } else {
                $("#productsContainer").html("");
            }
        },
        error: function (data) {
            console.log(data);
            $("#productsContainer").html("");
        }
    })
};
var bindProducts = async function () {
    $.ajax({
        url: "/Products/GetProductsBySearchCriteria",
        data: {
            lookupText: $("#searchInput").val(),
            categoryId: $("#categorySelect").val()
        },
        success: function (data) {
            if (data.entity) {
                $("#productsContainer").html("");
                data.entity.forEach(function (product) {
                    $("#productsContainer").append(productCardTemp(product));
                });
            } else {
                $("#productsContainer").html("");
            }
        },
        error: function (data) {
            console.log(data);
            $("#productsContainer").html("");
        }
    })
};

var bindProductDetails = async function (id) {
    $.ajax({
        url: "/Products/GetDetails?id=" + id,
        success: function (data) {
            if (data.entity) {
                $("#productCatergoryName").html(data.entity.productCategory.name);
                $("#productCatergoryDescription").html(data.entity.productCategory.description);
                $(".productName").html(data.entity.name);
                $("#richTextDescription").html(data.entity.richTextArea);
                $("#description").html(data.entity.description);
                $("#imgProduct").attr("src", data.entity.imageGuid);
            }
        },
        error: function (data) {
            console.log(data);
            $("#productsContainer").html("");
        }
    })
};


var productCardTemp = function (data) {
    return `<div class="col rounded rounded-3">
        <div class="card w-100 shadow">
            <img src="${data.imageGuid}" style="object-fit:cover;object-position:center;max-height:200px" class="card-img-top bg-light" alt="...">
            <div class="card-body">
                <h5 class="card-title fs-6 theme-blue">${data.name}</h5>
                <p class="card-text text-truncate">${data.description}</p>
                <button class="btn bg-blue btn-sm" data-bs-toggle="modal" onclick="bindProductDetails('${data.id}')" data-id="${data.id}" data-bs-target="#exampleModal">View Details</button>
                <a href="#" class="btn btn-outline-success btn-sm"><i class="fa-regular fa-file-lines me-2"></i>Download</a>
            </div>
        </div>
    </div>`
}