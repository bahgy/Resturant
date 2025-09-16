$(document).ready(function () {
    // Initialize profile page functionality
    initProfilePage();

    // Load user data from localStorage if available
    loadUserData();
});

function initProfilePage() {
    // Sidebar navigation
    $('.sidebar-item').on('click', function () {
        if ($(this).attr('id') === 'logoutBtn') {
            showToast('Successfully logged out', 'success');
            setTimeout(() => {
                window.location.href = 'index.html';
            }, 1500);
            return;
        }

        $('.sidebar-item').removeClass('active');
        $(this).addClass('active');

        const sectionId = $(this).data('section') + '-section';
        $('.profile-section').removeClass('active');
        $('#' + sectionId).addClass('active');
    });

    // Edit profile toggle
    let isEditing = false;
    $('#editProfileBtn').on('click', function () {
        isEditing = !isEditing;

        if (isEditing) {
            $('#profileName, #profilePhone, #profileEmail, #profileAddress').prop('disabled', false);
            $(this).html('<i class="fas fa-save me-1"></i> Save Changes');
            $(this).removeClass('btn-outline-primary').addClass('btn-primary');
        } else {
            // Save changes
            $('#profileName, #profilePhone, #profileEmail, #profileAddress').prop('disabled', true);
            $(this).html('<i class="fas fa-edit me-1"></i> Edit Profile');
            $(this).removeClass('btn-primary').addClass('btn-outline-primary');

            // Save to localStorage
            saveUserData();
            showToast('Profile updated successfully', 'success');
        }
    });

    // Update password
    $('#updatePasswordBtn').on('click', function () {
        const currentPassword = $('#currentPassword').val();
        const newPassword = $('#newPassword').val();
        const confirmPassword = $('#confirmPassword').val();

        if (!currentPassword || !newPassword || !confirmPassword) {
            showToast('Please fill in all password fields', 'danger');
            return;
        }

        if (newPassword !== confirmPassword) {
            showToast('New passwords do not match', 'danger');
            return;
        }

        if (newPassword.length < 6) {
            showToast('Password must be at least 6 characters', 'danger');
            return;
        }

        // In a real app, you would verify the current password with the server
        // For this demo, we'll just simulate success
        $('#currentPassword, #newPassword, #confirmPassword').val('');
        showToast('Password updated successfully', 'success');
    });

    // View order details
    $('.view-order-details').on('click', function () {
        const orderId = $(this).data('order-id');
        showOrderDetails(orderId);
    });

    // Edit booking
    $('.edit-booking').on('click', function () {
        const bookingId = $(this).data('booking-id');
        showToast('Edit booking functionality would open for ID: ' + bookingId, 'info');
    });

    // Cancel booking
    $('.cancel-booking').on('click', function () {
        const bookingId = $(this).data('booking-id');
        if (confirm('Are you sure you want to cancel this booking?')) {
            $(this).closest('.booking-card').fadeOut(300, function () {
                $(this).remove();
                showToast('Booking cancelled successfully', 'success');
            });
        }
    });
}

function loadUserData() {
    // In a real app, you would load this from your backend or localStorage
    const userData = JSON.parse(localStorage.getItem('userData')) || {
        name: 'John Doe',
        email: 'john.doe@example.com',
        phone: '+1 (555) 123-4567',
        address: '123 Main St, New York, NY 10001'
    };

    $('#userName').text(userData.name);
    $('#userEmail').text(userData.email);
    $('#profileName').val(userData.name);
    $('#profileEmail').val(userData.email);
    $('#profilePhone').val(userData.phone);
    $('#profileAddress').val(userData.address);
}

function saveUserData() {
    const userData = {
        name: $('#profileName').val(),
        email: $('#profileEmail').val(),
        phone: $('#profilePhone').val(),
        address: $('#profileAddress').val()
    };

    localStorage.setItem('userData', JSON.stringify(userData));

    // Update the sidebar display
    $('#userName').text(userData.name);
    $('#userEmail').text(userData.email);
}

function showOrderDetails(orderId) {
    // Sample order data - in a real app, this would come from your backend
    const orderData = {
        '12345': {
            date: 'Jan 15, 2023 at 2:30 PM',
            status: 'Completed',
            paymentMethod: 'Credit Card',
            deliveryAddress: '123 Main St, New York, NY 10001',
            contactPhone: '+1 (555) 123-4567',
            items: [
                { name: 'Grilled Chicken', quantity: 1, price: 25.99 },
                { name: 'Caesar Salad', quantity: 2, price: 12.99 },
                { name: 'Chocolate Cake', quantity: 1, price: 8.99 }
            ],
            subtotal: 60.96,
            tax: 5.49,
            deliveryFee: 4.99,
            total: 71.44
        },
        '12344': {
            date: 'Jan 10, 2023 at 6:45 PM',
            status: 'Completed',
            paymentMethod: 'PayPal',
            deliveryAddress: '123 Main St, New York, NY 10001',
            contactPhone: '+1 (555) 123-4567',
            items: [
                { name: 'Beef Steak', quantity: 2, price: 35.99 },
                { name: 'Pasta Carbonara', quantity: 1, price: 18.99 },
                { name: 'Tiramisu', quantity: 1, price: 9.99 }
            ],
            subtotal: 100.96,
            tax: 9.09,
            deliveryFee: 0,
            total: 110.05
        },
        '12343': {
            date: 'Jan 5, 2023 at 12:15 PM',
            status: 'Completed',
            paymentMethod: 'Credit Card',
            deliveryAddress: '123 Main St, New York, NY 10001',
            contactPhone: '+1 (555) 123-4567',
            items: [
                { name: 'Fish & Chips', quantity: 1, price: 22.99 },
                { name: 'Bruschetta', quantity: 1, price: 7.99 },
                { name: 'Iced Tea', quantity: 2, price: 3.99 }
            ],
            subtotal: 38.96,
            tax: 3.51,
            deliveryFee: 4.99,
            total: 47.46
        }
    };

    const order = orderData[orderId];
    if (!order) return;

    // Populate modal with order details
    $('#modalOrderId').text(orderId);
    $('#modalOrderDate').text(order.date);
    $('#modalOrderStatus').text(order.status);
    $('#modalPaymentMethod').text(order.paymentMethod);
    $('#modalDeliveryAddress').text(order.deliveryAddress);
    $('#modalContactPhone').text(order.contactPhone);

    // Clear and populate order items
    $('#modalOrderItems').empty();
    order.items.forEach(item => {
        $('#modalOrderItems').append(`
                    <tr>
                        <td>${item.name}</td>
                        <td>${item.quantity}</td>
                        <td class="text-end">$${item.price.toFixed(2)}</td>
                    </tr>
                `);
    });

    // Set totals
    $('#modalSubtotal').text('$' + order.subtotal.toFixed(2));
    $('#modalTax').text('$' + order.tax.toFixed(2));
    $('#modalDeliveryFee').text('$' + order.deliveryFee.toFixed(2));
    $('#modalTotal').text('$' + order.total.toFixed(2));

    // Show the modal
    $('#orderDetailsModal').modal('show');
}

function showToast(message, type) {
    const toast = $('#toastNotification');
    const toastMessage = $('#toastMessage');

    // Set message and alert type
    toastMessage.text(message);
    toast.find('.alert')
        .removeClass('alert-success alert-danger alert-info alert-warning')
        .addClass('alert-' + type);

    // Show toast
    toast.addClass('show');

    // Auto hide after 3 seconds
    setTimeout(() => {
        toast.removeClass('show');
    }, 3000);
}