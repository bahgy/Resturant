(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner(0);
    
    
    // Initiate the wowjs with faster animations
    new WOW({
        boxClass: 'wow',
        animateClass: 'animated',
        offset: 0,
        mobile: true,
        live: true
    }).init();
    
    
   // Back to top button
   $(window).scroll(function () {
    if ($(this).scrollTop() > 300) {
        $('.back-to-top').fadeIn('slow');
    } else {
        $('.back-to-top').fadeOut('slow');
    }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Modal Video
    $(document).ready(function () {
        var $videoSrc;
        $('.btn-play').click(function () {
            $videoSrc = $(this).data("src");
        });
        console.log($videoSrc);

        $('#videoModal').on('shown.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc + "?autoplay=1&amp;modestbranding=1&amp;showinfo=0");
        })

        $('#videoModal').on('hide.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc);
        })
    });


    // Facts counter
    $('[data-toggle="counter-up"]').counterUp({
        delay: 10,
        time: 2000
    });


    // Testimonial carousel
    $(".testimonial-carousel-1").owlCarousel({
        loop: true,
        dots: false,
        margin: 25,
        autoplay: true,
        slideTransition: 'linear',
        autoplayTimeout: 0,
        autoplaySpeed: 10000,
        autoplayHoverPause: false,
        responsive: {
            0:{
                items:1
            },
            575:{
                items:1
            },
            767:{
                items:2
            },
            991:{
                items:3
            }
        }
    });

    $(".testimonial-carousel-2").owlCarousel({
        loop: true,
        dots: false,
        rtl: true,
        margin: 25,
        autoplay: true,
        slideTransition: 'linear',
        autoplayTimeout: 0,
        autoplaySpeed: 10000,
        autoplayHoverPause: false,
        responsive: {
            0:{
                items:1
            },
            575:{
                items:1
            },
            767:{
                items:2
            },
            991:{
                items:3
            }
        }
    });


    // Enhanced Search Functionality
    $(document).ready(function() {
        const searchToggleBtn = $('#searchToggleBtn');
        const searchPanel = $('#searchPanel');
        const closeSearchBtn = $('#closeSearchBtn');
        const searchInput = $('#searchInput');
        const searchBtn = $('#searchBtn');
        const sortSelect = $('#sortSelect');

        // Toggle search panel
        searchToggleBtn.on('click', function(e) {
            e.preventDefault();
            searchPanel.toggleClass('show');
            if (searchPanel.hasClass('show')) {
                searchInput.focus();
            }
        });

        // Close search panel
        closeSearchBtn.on('click', function() {
            searchPanel.removeClass('show');
        });

        // Close search panel when clicking outside
        $(document).on('click', function(e) {
            if (!$(e.target).closest('.search-container').length) {
                searchPanel.removeClass('show');
            }
        });

        // Live search functionality
        let searchTimeout;
        searchInput.on('input', function() {
            clearTimeout(searchTimeout);
            const searchTerm = $(this).val().trim();
            
            if (searchTerm.length >= 2) {
                // Show loading indicator
                showSearchLoading();
                searchTimeout = setTimeout(() => {
                    performLiveSearch(searchTerm);
                }, 300); // 300ms delay for better performance
            } else if (searchTerm.length === 0) {
                clearSearchResults();
            }
        });

        // Search functionality
        searchBtn.on('click', function() {
            performSearch();
        });

        // Search on Enter key
        searchInput.on('keypress', function(e) {
            if (e.which === 13) {
                performSearch();
            }
        });

        // Sort functionality
        sortSelect.on('change', function() {
            const currentSearch = searchInput.val().trim();
            if (currentSearch.length >= 2) {
                performLiveSearch(currentSearch);
            }
        });

        function performLiveSearch(searchTerm) {
            const sortBy = sortSelect.val();
            
            // Sample data - in a real application, this would come from a database
            const sampleItems = [
                { id: 'grilled-chicken', name: 'Grilled Chicken', price: 25.99, category: 'Main Course' },
                { id: 'beef-steak', name: 'Beef Steak', price: 35.99, category: 'Main Course' },
                { id: 'caesar-salad', name: 'Caesar Salad', price: 12.99, category: 'Appetizer' },
                { id: 'chocolate-cake', name: 'Chocolate Cake', price: 8.99, category: 'Dessert' },
                { id: 'pasta-carbonara', name: 'Pasta Carbonara', price: 18.99, category: 'Main Course' },
                { id: 'fish-chips', name: 'Fish & Chips', price: 22.99, category: 'Main Course' },
                { id: 'tiramisu', name: 'Tiramisu', price: 9.99, category: 'Dessert' },
                { id: 'bruschetta', name: 'Bruschetta', price: 7.99, category: 'Appetizer' }
            ];

            // Filter by search term
            let filteredItems = sampleItems.filter(item => 
                item.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                item.category.toLowerCase().includes(searchTerm.toLowerCase())
            );

            // Sort items
            switch(sortBy) {
                case 'name':
                    filteredItems.sort((a, b) => a.name.localeCompare(b.name));
                    break;
                case 'min':
                    filteredItems.sort((a, b) => a.price - b.price);
                    break;
                case 'max':
                    filteredItems.sort((a, b) => b.price - a.price);
                    break;
            }

            // Display results
            displaySearchResults(filteredItems, searchTerm);
        }

        function performSearch() {
            const searchTerm = searchInput.val().trim();
            
            if (searchTerm === '') {
                alert('Please enter a search term');
                return;
            }

            performLiveSearch(searchTerm);
        }

        function displaySearchResults(items, searchTerm) {
            let resultsHtml = '';
            
            if (items.length === 0) {
                resultsHtml = `
                    <div class="alert alert-info">
                        <i class="fas fa-info-circle"></i> No items found for "${searchTerm}"
                    </div>
                `;
            } else {
                resultsHtml = `
                    <div class="alert alert-success">
                        <i class="fas fa-check-circle"></i> Found ${items.length} item(s) for "${searchTerm}"
                    </div>
                    <div class="search-results">
                        ${items.map(item => `
                            <div class="search-result-item p-2 border-bottom">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div class="flex-grow-1 me-3">
                                        <button class="btn btn-link text-decoration-none p-0 mb-1 product-name-btn" 
                                                data-product-id="${item.id}"
                                                data-product-name="${item.name}" 
                                                data-product-category="${item.category}"
                                                data-product-price="${item.price}">
                                            <h6 class="mb-1 text-start">${item.name}</h6>
                                        </button>
                                        <small class="text-muted d-block">${item.category}</small>
                                    </div>
                                    <div class="text-primary fw-bold">$${item.price}</div>
                                </div>
                            </div>
                        `).join('')}
                    </div>
                `;
            }

            // Create or update results container
            let resultsContainer = $('.search-results-container');
            if (resultsContainer.length === 0) {
                resultsContainer = $('<div class="search-results-container mt-3"></div>');
                searchPanel.find('.search-content').append(resultsContainer);
            }
            
            resultsContainer.html(resultsHtml);
            
            // Add click event listeners to product name buttons
            $('.product-name-btn').off('click').on('click', function() {
                const productId = $(this).data('product-id');
                const productName = $(this).data('product-name');
                const productCategory = $(this).data('product-category');
                const productPrice = $(this).data('product-price');
                
                // Navigate to product page with product details
                navigateToProductPage(productId, productName, productCategory, productPrice);
            });
        }
        
        function navigateToProductPage(productId, productName, productCategory, productPrice) {
            // Store product details in sessionStorage for the product page
            sessionStorage.setItem('selectedProduct', JSON.stringify({
                id: productId,
                name: productName,
                category: productCategory,
                price: productPrice
            }));
            
            // Also store in localStorage for consistency
            localStorage.setItem('selectedProduct', JSON.stringify({
                id: productId,
                name: productName,
                category: productCategory,
                price: productPrice
            }));
            
            // Navigate to product page
            window.location.href = 'product.html';
        }

        function showSearchLoading() {
            let resultsContainer = $('.search-results-container');
            if (resultsContainer.length === 0) {
                resultsContainer = $('<div class="search-results-container mt-3"></div>');
                searchPanel.find('.search-content').append(resultsContainer);
            }
            
            resultsContainer.html(`
                <div class="text-center py-3">
                    <div class="spinner-border spinner-border-sm text-primary me-2" role="status"></div>
                    <span class="text-muted">Searching...</span>
                </div>
            `);
        }

        function clearSearchResults() {
            $('.search-results-container').remove();
        }

        // Clear search results when search panel is closed
        searchPanel.on('hidden.bs.collapse', function() {
            clearSearchResults();
        });
    });

    // Cart functionality
    const CartManager = {
        items: JSON.parse(localStorage.getItem('cartItems')) || {},
        itemCount: 0,

        init() {
            this.updateItemCount();
            this.updateCartCountDisplay();
        },

        addItem(itemId, name, price, image = 'img/menu-1.jpg') {
            if (this.items[itemId]) {
                this.items[itemId].quantity += 1;
            } else {
                this.items[itemId] = {
                    name: name,
                    price: price,
                    quantity: 1,
                    image: image
                };
            }
            this.saveToStorage();
            this.updateItemCount();
            this.updateCartCountDisplay();
            this.showAddToCartNotification(name);
        },

        removeItem(itemId) {
            if (this.items[itemId]) {
                delete this.items[itemId];
                this.saveToStorage();
                this.updateItemCount();
                this.updateCartCountDisplay();
            }
        },

        updateQuantity(itemId, quantity) {
            if (this.items[itemId]) {
                if (quantity <= 0) {
                    this.removeItem(itemId);
                } else {
                    this.items[itemId].quantity = quantity;
                    this.saveToStorage();
                    this.updateItemCount();
                    this.updateCartCountDisplay();
                }
            }
        },

        getTotal() {
            let total = 0;
            Object.values(this.items).forEach(item => {
                total += item.price * item.quantity;
            });
            return total;
        },

        clearCart() {
            this.items = {};
            this.saveToStorage();
            this.updateItemCount();
            this.updateCartCountDisplay();
        },

        saveToStorage() {
            localStorage.setItem('cartItems', JSON.stringify(this.items));
        },

        updateItemCount() {
            this.itemCount = Object.values(this.items).reduce((total, item) => total + item.quantity, 0);
        },

        updateCartCountDisplay() {
            $('.cart-count, #cartCount').text(this.itemCount);
        },

        showAddToCartNotification(itemName) {
            // Create notification element
            const notification = $(`
                <div class="cart-notification" style="
                    position: fixed;
                    top: 20px;
                    right: 20px;
                    background: #28a745;
                    color: white;
                    padding: 15px 20px;
                    border-radius: 8px;
                    box-shadow: 0 4px 15px rgba(0,0,0,0.2);
                    z-index: 9999;
                    transform: translateX(100%);
                    transition: transform 0.3s ease;
                    max-width: 300px;
                ">
                    <i class="fas fa-check-circle me-2"></i>
                    <strong>${itemName}</strong> added to cart!
                </div>
            `);

            $('body').append(notification);

            // Animate in
            setTimeout(() => {
                notification.css('transform', 'translateX(0)');
            }, 100);

            // Animate out and remove
            setTimeout(() => {
                notification.css('transform', 'translateX(100%)');
                setTimeout(() => {
                    notification.remove();
                }, 300);
            }, 3000);
        }
    };

    // Initialize cart on page load
    $(document).ready(function() {
        CartManager.init();
        
        // Auto-add "Add to Cart" buttons to all menu items and fix existing ones
        addCartButtonsToMenuItems();
        // Make menu items clickable to open product details
        enableProductDetailNavigation();
        
        // Also fix buttons after a short delay to ensure all content is loaded
        setTimeout(function(){
            fixExistingCartButtons();
            enableProductDetailNavigation();
        }, 500);

        // Initialize customer service chat widget
        initCustomerServiceChat();

        // Ensure back-to-top arrow exists
        ensureBackToTopArrow();

        // Ensure floating feedback button exists
        ensureFeedbackButton();
    });

    // Function to automatically add "Add to Cart" buttons to menu items
    function addCartButtonsToMenuItems() {
        // Find all menu items that don't already have "Add to Cart" buttons
        $('.menu-item').each(function() {
            const $menuItem = $(this);
            const $content = $menuItem.find('.w-100.d-flex.flex-column.text-start.ps-4');
            
            // Check if "Add to Cart" button already exists
            if ($content.find('.btn-primary[onclick*="CartManager.addItem"]').length === 0) {
                const $title = $content.find('h4').first();
                const $price = $content.find('.text-primary');
                const $image = $menuItem.find('img');
                
                if ($title.length && $price.length && $image.length) {
                    const itemName = $title.text().trim();
                    const priceText = $price.text().trim();
                    const price = parseFloat(priceText.replace('$', ''));
                    const imageSrc = $image.attr('src');
                    const itemId = itemName.toLowerCase().replace(/\s+/g, '-');
                    
                    // Create the "Add to Cart" button
                    const $button = $(`
                        <button class="btn btn-primary btn-sm mt-2" onclick="CartManager.addItem('${itemId}', '${itemName}', ${price}, '${imageSrc}')">
                            <i class="fas fa-plus me-1"></i>Add to Cart
                        </button>
                    `);
                    
                    // Add the button after the description
                    $content.find('p.mb-0').after($button);
                }
            }
        });
        
        // Fix existing buttons with incorrect names
        fixExistingCartButtons();
    }
    
    // Function to fix existing "Add to Cart" buttons with correct item names
    function fixExistingCartButtons() {
        $('.menu-item').each(function() {
            const $menuItem = $(this);
            const $content = $menuItem.find('.w-100.d-flex.flex-column.text-start.ps-4');
            const $button = $content.find('.btn-primary[onclick*="CartManager.addItem"]');
            
            if ($button.length > 0) {
                const $title = $content.find('h4').first();
                const $price = $content.find('.text-primary');
                const $image = $menuItem.find('img');
                
                if ($title.length && $price.length && $image.length) {
                    const itemName = $title.text().trim();
                    const priceText = $price.text().trim();
                    const price = parseFloat(priceText.replace('$', ''));
                    const imageSrc = $image.attr('src');
                    const itemId = itemName.toLowerCase().replace(/\s+/g, '-');
                    
                    // Update the button's onclick attribute
                    $button.attr('onclick', `CartManager.addItem('${itemId}', '${itemName}', ${price}, '${imageSrc}')`);
                }
            }
        });
    }

    // Handle dynamic content loading (tabs, etc.)
    $(document).on('shown.bs.tab', function() {
        // Add cart buttons to newly shown content
        setTimeout(function(){
            addCartButtonsToMenuItems();
            enableProductDetailNavigation();
        }, 100);
    });

    // Also handle any dynamic content loading
    $(document).on('DOMNodeInserted', '.menu-item', function() {
        setTimeout(function(){
            addCartButtonsToMenuItems();
            enableProductDetailNavigation();
        }, 100);
    });

    // Ensure back-to-top arrow helper (uses existing scroll/show logic above)
    function ensureBackToTopArrow() {
        if ($('.back-to-top').length === 0) {
            const $btn = $(`
                <a href="#" class="back-to-top" aria-label="Back to top" style="display:none; right: 188px; bottom: 20px; z-index: 1045;">
                    <i class="fa fa-chevron-up"></i>
                </a>
            `);
            $('body').append($btn);
        }
    }

    // Ensure floating feedback button
    function ensureFeedbackButton() {
        if ($('#feedbackFab').length === 0) {
            const $fab = $(`
                <a id="feedbackFab" href="feedback.html" class="btn btn-primary btn-sm rounded-pill" aria-label="Leave feedback" title="Leave feedback" style="position: fixed; right: 86px; bottom: 20px; z-index: 1050; padding: 10px 14px; box-shadow: 0 8px 24px rgba(0,0,0,0.15);">
                    Leave Feedback
                </a>
            `);
            $('body').append($fab);
        }
    }

    // -----------------------------
    // Customer Service Chat Widget
    // -----------------------------
    function initCustomerServiceChat() {
        if (document.getElementById('chatWidgetButton')) {
            return;
        }

        // Inject minimal styles once
        const styleId = 'chat-widget-styles';
        if (!document.getElementById(styleId)) {
            const styleTag = document.createElement('style');
            styleTag.id = styleId;
            styleTag.textContent = `
                .chat-widget-button { position: fixed; right: 20px; bottom: 20px; z-index: 1050; width: 56px; height: 56px; border-radius: 50%; background: var(--bs-primary); color: #fff; display: flex; align-items: center; justify-content: center; cursor: pointer; box-shadow: 0 8px 24px rgba(0,0,0,0.15); }
                .chat-widget-button:hover { filter: brightness(1.05); }
                .chat-widget-panel { position: fixed; right: 20px; bottom: 86px; width: 320px; max-width: calc(100% - 40px); background: #fff; border: 1px solid rgba(0,0,0,0.08); border-radius: 12px; box-shadow: 0 16px 40px rgba(0,0,0,0.15); overflow: hidden; z-index: 1050; display: none; }
                .chat-widget-header { background: var(--bs-primary); color: #fff; padding: 12px 14px; display: flex; align-items: center; justify-content: space-between; }
                .chat-widget-title { font-weight: 600; font-size: 14px; }
                .chat-widget-body { background: #f8f9fa; height: 320px; display: flex; flex-direction: column; }
                .chat-messages { flex: 1; overflow-y: auto; padding: 12px; }
                .chat-input { display: flex; gap: 8px; padding: 10px; background: #fff; border-top: 1px solid rgba(0,0,0,0.08); }
                .chat-input input { flex: 1; border: 1px solid #dee2e6; border-radius: 20px; padding: 8px 12px; outline: none; }
                .chat-input button { border-radius: 20px; }
                .chat-bubble { max-width: 75%; padding: 8px 12px; margin: 6px 0; border-radius: 12px; font-size: 13px; line-height: 1.35; box-shadow: 0 2px 6px rgba(0,0,0,0.06); }
                .chat-bubble.user { margin-left: auto; background: #0d6efd; color: #fff; border-bottom-right-radius: 4px; }
                .chat-bubble.agent { margin-right: auto; background: #e9ecef; color: #212529; border-bottom-left-radius: 4px; }
                .chat-time { display: block; font-size: 10px; opacity: 0.6; margin-top: 4px; }
                .chat-widget-icon { font-size: 22px; }
                .chat-header-actions button { background: transparent; border: 0; color: #fff; }
            `;
            document.head.appendChild(styleTag);
        }

        // Build widget DOM
        const button = document.createElement('div');
        button.id = 'chatWidgetButton';
        button.className = 'chat-widget-button';
        button.innerHTML = '<i class="fas fa-comments chat-widget-icon"></i>';

        const panel = document.createElement('div');
        panel.id = 'chatWidgetPanel';
        panel.className = 'chat-widget-panel';
        panel.innerHTML = `
            <div class="chat-widget-header">
                <div class="chat-widget-title"><i class="fas fa-headset me-2"></i>Customer Service</div>
                <div class="chat-header-actions">
                    <button type="button" id="chatCloseBtn" title="Close"><i class="fas fa-times"></i></button>
                </div>
            </div>
            <div class="chat-widget-body">
                <div class="chat-messages" id="chatMessages"></div>
                <div class="chat-input">
                    <input id="chatInput" type="text" placeholder="Type your message..." />
                    <button id="chatSendBtn" class="btn btn-primary btn-sm"><i class="fas fa-paper-plane"></i></button>
                </div>
            </div>
        `;

        document.body.appendChild(button);
        document.body.appendChild(panel);

        const $panel = $(panel);
        const $messages = $('#chatMessages');
        const $input = $('#chatInput');

        // Load history
        function loadHistory() {
            const saved = localStorage.getItem('chatMessages');
            const history = saved ? JSON.parse(saved) : [];
            $messages.empty();
            history.forEach(renderMessage);
            scrollToBottom();
        }

        function saveMessage(role, text) {
            const saved = localStorage.getItem('chatMessages');
            const history = saved ? JSON.parse(saved) : [];
            history.push({ role: role, text: text, ts: Date.now() });
            localStorage.setItem('chatMessages', JSON.stringify(history));
        }

        function formatTime(ts) {
            const d = new Date(ts);
            return d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        }

        function renderMessage(msg) {
            const isUser = msg.role === 'user';
            const bubble = $(`<div class="chat-bubble ${isUser ? 'user' : 'agent'}"></div>`);
            bubble.text(msg.text);
            bubble.append(`<span class="chat-time">${formatTime(msg.ts || Date.now())}</span>`);
            $messages.append(bubble);
        }

        function scrollToBottom() {
            $messages.scrollTop($messages[0].scrollHeight);
        }

        function sendUserMessage() {
            const text = $input.val().trim();
            if (!text) return;
            const msg = { role: 'user', text: text, ts: Date.now() };
            renderMessage(msg);
            saveMessage(msg.role, msg.text);
            $input.val('');
            scrollToBottom();
            // Simulated agent reply
            setTimeout(function() {
                const replyText = generateAutoReply(text);
                const reply = { role: 'agent', text: replyText, ts: Date.now() };
                renderMessage(reply);
                saveMessage(reply.role, reply.text);
                scrollToBottom();
            }, 700);
        }

        function generateAutoReply(userText) {
            const lower = userText.toLowerCase();
            if (lower.includes('price') || lower.includes('cost')) {
                return 'Our menu prices are shown on each item. Can I help with a specific dish?';
            }
            if (lower.includes('book') || lower.includes('reservation')) {
                return 'You can book directly on the Book Now page. Need help filling the form?';
            }
            if (lower.includes('cart') || lower.includes('order')) {
                return 'Your cart is available via the cart icon. I can guide you through checkout.';
            }
            if (lower.includes('hello') || lower.includes('hi')) {
                return 'Hello! How can I assist you today?';
            }
            return 'Thanks for your message! A customer service agent will be with you shortly.';
        }

        // Events
        $('#chatSendBtn').on('click', sendUserMessage);
        $input.on('keypress', function(e) { if (e.which === 13) sendUserMessage(); });
        $('#chatCloseBtn').on('click', function() { $panel.hide(); });
        $('#chatWidgetButton').on('click', function() { $panel.toggle(); if ($panel.is(':visible')) { setTimeout(scrollToBottom, 50); $input.focus(); } });

        // Seed greeting once per session
        if (!localStorage.getItem('chatGreeted')) {
            const greet = { role: 'agent', text: "Hi! I'm here to help. Ask me anything.", ts: Date.now() };
            renderMessage(greet);
            saveMessage(greet.role, greet.text);
            localStorage.setItem('chatGreeted', '1');
        } else {
            loadHistory();
        }
    }

    // Make CartManager globally available
    window.CartManager = CartManager;

    // ---------------------------------------
    // Product Details Navigation and Caching
    // ---------------------------------------
    function collectMenuItemsData() {
        const items = [];
        $('.menu-item').each(function() {
            const $menuItem = $(this);
            const $content = $menuItem.find('.w-100.d-flex.flex-column.text-start.ps-4');
            const $title = $content.find('h4').first();
            const $price = $content.find('.text-primary').first();
            const $image = $menuItem.find('img').first();
            if ($title.length && $price.length && $image.length) {
                const name = $title.text().trim();
                const price = parseFloat($price.text().trim().replace('$','')) || 0;
                const image = $image.attr('src');
                const id = name.toLowerCase().replace(/\s+/g, '-');
                items.push({ id: id, name: name, price: price, image: image });
            }
        });
        return items;
    }

    function enableProductDetailNavigation() {
        // Build data cache once per page view
        const items = collectMenuItemsData();
        if (items.length) {
            try { localStorage.setItem('menuItemsCache', JSON.stringify(items)); } catch(e) {}
        }

        $('.menu-item').each(function() {
            const $menuItem = $(this);
            const $content = $menuItem.find('.w-100.d-flex.flex-column.text-start.ps-4');
            const $title = $content.find('h4').first();
            const $image = $menuItem.find('img').first();
            const titleText = $title.text().trim();
            if (!titleText) return;
            const id = titleText.toLowerCase().replace(/\s+/g, '-');

            function goToDetails(e){
                e.preventDefault();
                // Build selected product payload from DOM
                const priceText = $content.find('.text-primary').first().text().trim();
                const price = parseFloat(priceText.replace('$','')) || 0;
                const imageSrc = $image.attr('src');
                const selected = { id: id, name: titleText, price: price, image: imageSrc };
                try {
                    localStorage.setItem('selectedProductId', id);
                    localStorage.setItem('selectedProduct', JSON.stringify(selected));
                } catch(e) {}
                // Navigate with query param for robustness
                window.location.href = 'product.html?id=' + encodeURIComponent(id);
            }

            // Avoid double-binding
            $title.off('click.productNav').on('click.productNav', goToDetails).css('cursor','pointer');
            $image.off('click.productNav').on('click.productNav', goToDetails).css('cursor','pointer');
        });
    }

})(jQuery);

