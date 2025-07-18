﻿$(document).ready(function () {
    const $scrollContainer = $('#scrollable');
    let isDown = false;
    let startX;
    let scrollLeft;

    $scrollContainer.on('mousedown', function (e) {
        isDown = true;
        $scrollContainer.addClass('dragging');
        startX = e.pageX - $scrollContainer.offset().left;
        scrollLeft = $scrollContainer.scrollLeft();
    });

    $(document).on('mouseup', function () {
        isDown = false;
        $scrollContainer.removeClass('dragging');
    });

    $scrollContainer.on('mousemove', function (e) {
        if (!isDown) return;
        e.preventDefault();
        const x = e.pageX - $scrollContainer.offset().left;
        const walk = (x - startX) * 1;
        $scrollContainer.scrollLeft(scrollLeft - walk);
    });


    $('#order-detail-btn').click(function () {
        $('#orderDetailModal').modal('show');
    });

    $('.clickable-card').click(function () {
        var productId = $(this).data('id');
        $('#productDetailModal').modal('show');
        $('.btn-add-cart').prop("disabled", true);

        $.get('/Product/ProductDetailByID', { id: productId }, function (data) {
            $('#productModalBody').html(data);
            $(".btn-add-cart").attr('data-prodid', productId);

            const totalGroups = $('.body-variant-group').length;

            if (totalGroups == 0) {
                $('.btn-add-cart').removeAttr("disabled");
            }
        }).fail(function () {
            $('#productModalBody').html('<div class="alert alert-danger">Failed to load product details</div>');
        });

        var product = products.find(p => p.id == productId);

        if (product.price && product.price > 0) {
            menuPrice = product.price;
            $('#priceContainer').text(' (Rp' + menuPrice + ')');
        }
        else
            menuPrice = 0;
    });

    $('#product-detail-close').click(function () {
        $('#productDetailModal').modal('hide');
        $('#priceContainer').text('');
        var input = $("input[name='Quantity']");
        input.val(1).trigger('change');
        selected = {};
        menuPrice = 0;

        const btnAddCart = $('#add-cart');
        btnAddCart.prop('disabled', true);
    })

    $('#order-detail-close').click(function () {
        $('#orderDetailModal').modal('hide');
    });
});

$(document).ready(function () {
    $('#preorder-active').hide();
    $('#detail-po').hide();

    $('#preorder-inactive').on('click', function () {
        $('#btn-create-order').text('Pre Order');
        $('#OrderType').val('PreOrder')
        $('#detail-po').slideDown();
        $('#preorder-inactive').hide();
        $('#preorder-active').show();
    });

    $('#preorder-active').on('click', function () {
        $('#btn-create-order').text('Order Now');
        $('#OrderType').val('Online')
        $('#detail-po').slideUp();
        $('#preorder-active').hide();
        $('#preorder-inactive').show();
    });
});

$(document).ready(function () {
    $('.input-number').each(function () {
        updateButtonState($(this));
    });
});

$(document).on('click', '.btn-number', function (e) {
    e.preventDefault();

    var fieldName = $(this).data('field');
    var type = $(this).data('type');
    var input = $("input[name='" + fieldName + "']");
    var currentVal = parseInt(input.val()) || 0;
    var minValue = parseInt(input.attr('min')) || 0;
    var maxValue = parseInt(input.attr('max')) || Infinity;

    if (type === 'minus') {
        if (currentVal > minValue) {
            input.val(currentVal - 1).trigger('change');
        }
    } else if (type === 'plus') {
        if (currentVal < maxValue) {
            input.val(currentVal + 1).trigger('change');
        }
    }

    updateButtonState(input);
});

function updateButtonState(input) {
    var currentVal = parseInt(input.val()) || 0;
    var minValue = parseInt(input.attr('min')) || 0;
    var maxValue = parseInt(input.attr('max')) || Infinity;

    var minusBtn = $('.btn-number[data-type="minus"]');
    var plusBtn = $('.btn-number[data-type="plus"]');

    if (currentVal <= minValue)
        minusBtn.prop('disabled', true);
    else
        minusBtn.removeAttr("disabled");

    if (currentVal >= maxValue)
        plusBtn.prop('disabled', true);
    else
        plusBtn.removeAttr("disabled");

    var totalPrice = currentVal * menuPrice;
    $('#priceContainer').text(' (Rp' + totalPrice + ')');
}

$('.input-number').on('change', function () {
    var valueCurrent = parseInt($(this).val()) || 0;
    var minValue = parseInt($(this).attr('min')) || 0;
    var maxValue = parseInt($(this).attr('max')) || Infinity;

    if (valueCurrent < minValue) {
        $(this).val(minValue);
    } else if (valueCurrent > maxValue) {
        $(this).val(maxValue);
    }

    updateButtonState($(this));
});


$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const selectedCategoryId = urlParams.get('category');

    console.log(selectedCategoryId);
    if (selectedCategoryId) {
        $('.category-btn').each(function () {
            console.log($(this).val())
            if ($(this).val() === selectedCategoryId) {
                $(this).find('.card-wrapper').addClass('border border-primary rounded');
                $(this).find('.card-title').addClass('text-primary');
                $(this).find('.circle-wrapper').removeClass('bg-light').addClass('bg-primary');
                $(this).find('.icon-category').removeClass('text-muted').addClass('text-white');
            }
        });
    }
});

// Untuk tag yang baru muncul setelah mendapatkan data dari database - Fahrizal
//$(document).on('click', '.variant-option-tag', function () {
//    $(this).closest('.body-variant-group').find('.variant-option-tag').removeClass('option-selected');

//    $(this).addClass('option-selected');
//});
