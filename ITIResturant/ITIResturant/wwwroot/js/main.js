

(function ($) {
    "use strict";

    // -----------------------------
    // Spinner
    // -----------------------------
    const initSpinner = () => {
        setTimeout(() => $('#spinner').removeClass('show'), 1);
    };

    // -----------------------------
    // WOW Animations
    // -----------------------------
    const initWow = () => {
        new WOW({
            boxClass: 'wow',
            animateClass: 'animated',
            offset: 0,
            mobile: true,
            live: true
        }).init();
    };

    // -----------------------------
    // Modal Video
    // -----------------------------
    const initModalVideo = () => {
        let videoSrc = "";

        $(document).on('click', '.btn-play', function () {
            videoSrc = $(this).data("src");
        });

        $('#videoModal').on('shown.bs.modal', () => {
            $("#video").attr('src', `${videoSrc}?autoplay=1&modestbranding=1&showinfo=0`);
        });

        $('#videoModal').on('hide.bs.modal', () => {
            $("#video").attr('src', videoSrc);
        });
    };

    // -----------------------------
    // Owl Carousels (Testimonials)
    // -----------------------------
    const initCarousels = () => {
        const carouselSettings = {
            loop: true,
            dots: false,
            margin: 25,
            autoplay: true,
            slideTransition: 'linear',
            autoplayTimeout: 0,
            autoplaySpeed: 10000,
            autoplayHoverPause: false,
            responsive: {
                0: { items: 1 },
                575: { items: 1 },
                767: { items: 2 },
                991: { items: 3 }
            }
        };

        $(".testimonial-carousel-1").owlCarousel(carouselSettings);
        $(".testimonial-carousel-2").owlCarousel({ ...carouselSettings, rtl: true });
    };

    // -----------------------------
    // Cart Manager
    // -----------------------------
    const CartManager = {
        items: JSON.parse(localStorage.getItem('cartItems')) || {},

        init() {

            this.updateDisplay();
            this.bindEvents();
        },

        bindEvents() {
            // Attach event handler for all add-to-cart buttons
            $(document).on("click", ".add-to-cart", (e) => {
                e.preventDefault();

                const btn = $(e.currentTarget);
                const productId = btn.data("product-id") || btn.data("id");
                const quantity = btn.data("quantity") || 1;

                this.addItem(productId, quantity);
            });
        },

        addItem(productId, quantity = 1) {
            $.ajax({
                url: "/Cart/AddToCart",
                type: "POST",
                data: { productId: productId, quantity: quantity },
                success: (response) => {
                    if (response.success) {
                        toastr.success(response.message || "Added to cart!");
                        $("#cartCount").text(response.cartCount);
                    } else {
                        toastr.error(response.message || "You must login first!");
                    }
                },
                error: () => {
                    toastr.error("You must login first!");
                }
            });
        },

        refreshCartCount() {
            $.get("/Cart/GetCartCount", (count) => {
                $(".cart-count, #cartCount").text(count);
            }).fail(() => {
                console.warn("Could not update cart count from server.");
            });
        },

        removeItem(id) {
            delete this.items[id];
            this.save();
        },
        success: (response) => {
            if (response.success) {
                toastr.success(response.message || "Added to cart!");

                //  update count
                $("#cartCount").text(response.cartCount);

                if (response.product) {
                    console.log("Added product:", response.product);
                    toastr.info(`<img src="${response.product.imageUrl}" 
                          style="width:40px;height:40px;object-fit:cover;margin-right:8px;">
                          ${response.product.productName}`,
                        "Item Added",
                        { timeOut: 2000, escapeHtml: false });
                }
            } else {
                toastr.error(response.message || "Something went wrong!");
            }
        },

        save() {
            localStorage.setItem("cartItems", JSON.stringify(this.items));
            this.updateDisplay();
        },

        updateDisplay() {
            const count = Object.values(this.items).reduce((t, i) => t + i.quantity, 0);
            $(".cart-count, #cartCount").text(count);
        }
    };
    // -----------------------------
    // Extra Events (Update Qty + Remove Item)
    // -----------------------------
    $(document).on("click", ".update-qty", function () {
        const productId = $(this).data("product-id");
        const change = parseInt($(this).data("change"));
        const qtyElem = $(this).siblings(".quantity");
        let newQty = parseInt(qtyElem.text()) + change;

        if (newQty < 1) newQty = 1;

        $.post("/Cart/UpdateQuantity", { productId: productId, quantity: newQty }, function (response) {
            if (response.success) {

                // If cart is now empty after update
                if (response.cart.itemsCount === 0) {
                    location.reload(); // or inject emptyCart div
                    return;
                }

                // update quantity
                qtyElem.text(newQty);

                // update item total
                const cartItem = qtyElem.closest(".cart-item");
                const updatedItem = response.cart.items.find(i => i.productId === productId);
                if (updatedItem) {
                    cartItem.find(".fw-bold").text(updatedItem.totalPriceFormatted);
                }

                // update summary
                $(".cart-summary").find("span:contains('Subtotal')").next().text(response.cart.subtotalFormatted);
                $(".cart-summary").find("span:contains('Tax')").next().text(response.cart.taxFormatted);
                $(".cart-summary").find("span:contains('Delivery Fee')").next().text(response.cart.deliveryFeeFormatted);
                $(".cart-summary h5.text-primary").text(response.cart.grandTotalFormatted);

                // update header cart count
                $("#cartCount, .cart-count").text(response.cart.itemsCount);

                toastr.success(response.message);
            } else {
                toastr.error(response.message);
            }
        });
    });


    $(document).on("click", ".remove-item", function () {
        const productId = $(this).data("product-id");
        const cartItem = $(this).closest(".cart-item");

        $.post("/Cart/RemoveItem", { productId: productId }, function (response) {
            if (response.success) {
                // remove the item from DOM
                cartItem.remove();

                if (response.cart.itemsCount === 0) {
                    // empty cart fallback
                    location.reload(); // or inject emptyCart div
                    return;
                } else {
                    // update summary
                    $(".cart-summary").find("span:contains('Subtotal')").next().text(response.cart.subtotalFormatted);
                    $(".cart-summary").find("span:contains('Tax')").next().text(response.cart.taxFormatted);
                    $(".cart-summary").find("span:contains('Delivery Fee')").next().text(response.cart.deliveryFeeFormatted);
                    $(".cart-summary h5.text-primary").text(response.cart.grandTotalFormatted);

                    // update header cart count
                    $("#cartCount, .cart-count").text(response.cart.itemsCount);
                }

                toastr.success(response.message);
            } else {
                toastr.error(response.message);
            }
        });
    });


    // -----------------------------
    // Init All
    // -----------------------------
    $(document).ready(() => {
        initSpinner();
        initWow();
        initModalVideo();
        initCarousels();
        CartManager.init();
        CartManager.refreshCartCount();
    });

})(jQuery);
