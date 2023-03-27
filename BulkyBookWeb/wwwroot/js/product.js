document.addEventListener("DOMContentLoaded", function () {
    getAllProducts();
});

function getAllProducts() {
    let products;

    fetch("Product/GetAll")
    .then(response => {
        if (!response.ok) {
            throw new Error(`Http Error. Status: ${response.status}`);
        }

        return response.json();
    })
    .then(json => {
        products = json.data;
        let table = new DataTable("#tblProductData", {
            responsive: true,
            initComplete: function () {
                document.querySelector("#tblProductData .loading-container").style.display = "none";
            },
            data: products,
            columns: [
                { data: "title", "width": "15%" },
                { data: "author", "width": "15%" },
                { data: "coverType.name", "width": "15%" },
                { data: "category.name", "width": "15%" },
                { data: "price", "width": "15%" },
                { data: "isbn", "width": "15%" },
                {
                    data: "id",
                    "render": function (currentId) {
                        return `
                            <div class="w-75 btn-group" role="group">
                                <a href="/Admin/Product/Upsert?id=${currentId}" class="btn btn-primary mx-2">
                                    <i class="bi-pencil-square"></i>&nbsp; Edit
                                </a>
                                <a href="/Admin/Product/Delete?id=${currentId}" class="btn btn-danger mx-2">
                                    <i class="bi-trash"></i>&nbsp; Delete
                                </a>
                            </div>
                        `
                    },
                    "width": "15%"
                }
            ]
        })
    })
    .catch(error => {
        console.error("Error fetching products", error);
    });
}