document.addEventListener("DOMContentLoaded", function () {
    const searchToggleBtn = document.getElementById("searchToggleBtn");
    const searchPanel = document.getElementById("searchPanel");
    const searchInput = document.getElementById("searchInput");
    const sortSelect = document.getElementById("sortSelect");
    const products = Array.from(document.querySelectorAll(".product-item"));
    const searchResults = document.getElementById("searchResults");

    // Toggle search panel
    searchToggleBtn.addEventListener("click", function () {
        searchPanel.classList.toggle("active");
        if (searchPanel.classList.contains("active")) {
            searchInput.focus();
        }
    });

    // Close when clicking outside
    document.addEventListener("click", function (e) {
        if (!searchPanel.contains(e.target) && !searchToggleBtn.contains(e.target)) {
            searchPanel.classList.remove("active");
        }
    });

    // Keep original order index
    products.forEach((p, i) => p.dataset.index = i);

    let debounceTimer;
    function debounce(func, delay = 200) {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(func, delay);
    }

    function filterAndSort() {
        const searchValue = searchInput.value.toLowerCase().trim();
        const sortValue = sortSelect.value;

        let filtered = products.filter(p =>
            p.dataset.name.toLowerCase().includes(searchValue) ||
            p.dataset.description.toLowerCase().includes(searchValue)
        );

        // Sorting
        if (sortValue === "name") {
            filtered.sort((a, b) => a.dataset.name.localeCompare(b.dataset.name));
        } else if (sortValue === "min") {
            filtered.sort((a, b) => parseFloat(a.dataset.price) - parseFloat(b.dataset.price));
        } else if (sortValue === "max") {
            filtered.sort((a, b) => parseFloat(b.dataset.price) - parseFloat(a.dataset.price));
        } else {
            filtered.sort((a, b) => parseInt(a.dataset.index) - parseInt(b.dataset.index));
        }

        // Render results inside panel
        searchResults.innerHTML = "";
        if (filtered.length > 0) {
            filtered.forEach(p => {
                let imageUrl = p.dataset.image.replace("~", ""); // ASP.NET ~ fix

                const item = document.createElement("div");
                item.className = "search-result-item";
                item.innerHTML = `
                <img src="${imageUrl}" alt="${p.dataset.name}">
                <div class="flex-grow-1">
                    <div><strong>${p.dataset.name}</strong></div>
                    <div class="text-muted small">$${p.dataset.price}</div>
                </div>
                <button class="btn btn-sm btn-primary add-to-cart"
                        data-product-id="${p.dataset.id}"
                        data-quantity="1">
                    <i class="fas fa-shopping-cart"></i>
                </button>
            `;

                // Click image → go to product page
                item.querySelector("img").addEventListener("click", () => {
                    window.location.href = p.dataset.url || "#";
                });

                // Add to Cart button
                item.querySelector(".add-to-cart").addEventListener("click", (e) => {
                    e.stopPropagation();
                    const productId = e.currentTarget.dataset.productId;
                    const qty = e.currentTarget.dataset.quantity;

                    fetch(`/Cart/AddToCart?productId=${productId}&quantity=${qty}`, {
                        method: "POST"
                    })
                        .then(res => res.json())
                        .then(data => {
                            if (data.success) {
                                let countEl = document.getElementById("cartCount");
                                if (countEl) countEl.textContent = data.cartCount;
                            } else {
                                alert(data.message);
                            }
                        });
                });

                searchResults.appendChild(item);
            });
        } else {
            searchResults.innerHTML = `<div class="text-center text-muted">No results found</div>`;
        }
    }


    // Attach events
    searchInput.addEventListener("keyup", () => debounce(filterAndSort));
    sortSelect.addEventListener("change", filterAndSort);
});
