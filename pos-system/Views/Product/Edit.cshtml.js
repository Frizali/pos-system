//Variant
function renderVariantRow(variant, index) {
    return `
    <tr>
        <input type="hidden" name="ProductVariants[${index}].VariantId" value="${variant.VariantId}" />
        <input type="hidden" class="form-control" name="ProductVariants[${index}].Sku" value="${variant.Sku ?? ''}" />
        <td><input type="text" disabled class="form-control" name="ProductVariants[${index}].Sku" value="${variant.Sku ?? ''}" /></td>
        <td><input type="number" class="form-control" name="ProductVariants[${index}].VariantPrice" value="${variant.VariantPrice ?? 0}" /></td>
        <td><input type="number" class="form-control" name="ProductVariants[${index}].VariantStock" value="${variant.VariantStock ?? 0}" /></td>
        <td class="text-center">
            <input class="form-check-input" type="checkbox" name="ProductVariants[${index}].IsLimitedStock" ${variant.IsLimitedStock ? 'checked' : ''} />
        </td>
        <td class="text-center">
            <input class="form-check-input" type="checkbox" name="ProductVariants[${index}].IsAvailable" ${variant.IsAvailable ? 'checked' : ''} />
        </td>
        <td>
            <button type="button" class="btn btn-delete btn-custom btn-action-status remove-variant">
                <i class="bi bi-x"></i>
            </button>
        </td>
    </tr>`;
}


function addNewVariant() {
    const index = $('#table-variant-body tr').length;
    const newVariant = {
        Sku: '',
        VariantPrice: 0,
        VariantStock: 0,
        IsLimitedStock: false,
        IsAvailable: true
    };
    $('#table-variant-body').append(renderVariantRow(newVariant, index));
}

$(document).on('click', '#newVariant', function () {
    addNewVariant();
});

function renumberVariants() {
    $('#table-variant-body tr').each(function (index) {
        $(this).find('input').each(function () {
            const name = $(this).attr('name');
            if (name) {
                const newName = name.replace(/ProductVariants\[\d+\]/, `ProductVariants[${index}]`);
                $(this).attr('name', newName);
            }
        });
    });
}

$(document).on('click', '.remove-variant', function () {
    $(this).closest('tr').remove();
    renumberVariants();
});

function populateVariants(data) {
    const $body = $('#table-variant-body');
    $body.empty();
    data.forEach((variant, index) => {
        $body.append(renderVariantRow(variant, index));
    });
}

function toggleVariantSection(show) {
    if (show) {
        $('#variant').show();
        $('#product-management').hide();
    } else {
        $('#variant').hide();
        $('#product-management').show();
    }
}

//Addon
function renderAddonRow(addon, index) {
    return `
    <tr>
        <input type="hidden" name="ProductAddons[${index}].AddonId" value="${addon.AddonId}" />
        <td><input type="text" class="form-control" name="ProductAddons[${index}].AddonName" value="${addon.AddonName ?? ''}" /></td>
        <td><input type="number" class="form-control" name="ProductAddons[${index}].AddonPrice" value="${addon.AddonPrice ?? 0}" /></td>
        <td><input type="number" class="form-control" name="ProductAddons[${index}].AddonStock" value="${addon.AddonStock ?? 0}" /></td>
        <td class="text-center">
            <input type="checkbox" name="ProductAddons[${index}].IsLimitedStock" ${addon.IsLimitedStock ? 'checked' : ''} />
        </td>
        <td class="text-center">
            <input type="checkbox" name="ProductAddons[${index}].IsAvailable" ${addon.IsAvailable ? 'checked' : ''} />
        </td>
        <td>
            <button type="button" class="btn btn-danger btn-sm remove-addon">Remove</button>
        </td>
    </tr>`;
}

function addNewAddon() {
    const index = $('#table-addons-body tr').length;
    const newAddon = {
        AddonName: '',
        AddonPrice: 0,
        AddonStock: 0,
        IsLimitedStock: false,
        IsAvailable: true
    };
    $('#table-addons-body').append(renderAddonRow(newAddon, index));
}

$(document).on('click', '#newAddon', function () {
    addNewAddon();
});

function renumberAddons() {
    $('#table-addons-body tr').each(function (index) {
        $(this).find('input, select, textarea').each(function () {
            const name = $(this).attr('name');
            if (name) {
                const newName = name.replace(/ProductAddons\[\d+\]/, `ProductAddons[${index}]`);
                $(this).attr('name', newName);
            }
        });
    });
}

$(document).on('click', '.remove-addon', function () {
    $(this).closest('tr').remove();
    renumberAddons();
});

function populateAddons(data) {
    const $body = $('#table-addons-body');
    $body.empty();
    data.forEach((addon, index) => {
        $body.append(renderAddonRow(addon, index));
    });
}

function toggleAddonSection(show) {
    if (show) {
        $('#addons').show();
    } else {
        $('#addons').hide();
    }
}

$(document).ready(function () {
    $('#hasVariant').on('change', function () {
        toggleVariantSection($(this).is(':checked'));
    });

    $('#hasAddons').on('change', function () {
        toggleAddonSection($(this).is(':checked'));
    });

    toggleVariantSection($('#hasVariant').is(':checked'));
    toggleAddonSection($('#hasAddons').is(':checked'));
});

$(document).ready(function () {
    if (Array.isArray(productVariants) && productVariants.length > 0) {
        $('#hasVariant').prop('checked', true).trigger('change');
        populateVariants(productVariants);
    }

    if (Array.isArray(productAddons) && productAddons.length > 0) {
        $('#hasAddons').prop('checked', true).trigger('change');
        populateAddons(productAddons);
    }

    $(document).on('click', '.remove-addon', function () {
        $(this).closest('tr').remove();
        renumberAddons();
    });
});