document.addEventListener("DOMContentLoaded", function () {
    getAllProducts();
});

function getAllProducts() {
    //let productsTable = document.querySelector("#tblProductData");
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
            data: products,
            columns: [
                { data: "title", "width": "15%" },
                { data: "author", "width": "15%" },
                { data: "coverType.name", "width": "15%" },
                { data: "category.name", "width": "15%" },
                { data: "price", "width": "15%" },
                { data: "isbn", "width": "15%" }
            ]
        })
    })
    .catch(error => {
        console.error("Error fetching products", error);
    });



}