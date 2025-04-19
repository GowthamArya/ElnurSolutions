$(document).ready(function () {
    console.log("test");

    var productCardTemp = function () {
        return `<div class="col rounded rounded-3">
            <div class="card w-100 shadow">
                <img src="~/Images/Products/DownLightSeriesIndoor.png" style="object-fit:cover;object-position:center;max-height:200px" class="card-img-top bg-light" alt="...">
                <div class="card-body">
                    <h5 class="card-title fs-6 theme-blue">Down Light Series</h5>
                    <p class="card-text">Driver type : Fixed Constant Current 3W to 30W.</p>
                    <button class="btn bg-blue btn-sm" data-bs-toggle="modal" data-bs-target="#exampleModal">View Details</button>
                    <a href="#" class="btn btn-outline-success btn-sm"><i class="fa-regular fa-file-lines me-2"></i>Download</a>
                </div>
            </div>
        </div>`
    }
    $.ajax({
        url: "/Products/GetProductsBySearchCriteria",
        success: function (data) {
            console.log(data);
        },
        error: function (data) {
            console.log(data);
        }
    })
})