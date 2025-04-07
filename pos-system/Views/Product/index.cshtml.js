$(function () {
    $("#variant").hide();
    $("#hasVariant").change(function () {
        if ($(this).is(":checked")) {
            $("#variant").attr("class", "d-flex flex-column gap-2");
            $("#product-management").attr("class", "d-none");
        } else {
            $("#variant").attr("class", "d-none");
            $("#product-management").attr("class", "card");
        };
    });
});

$(function () {
    $("#addons").hide();
    $("#hasAddons").change(function () {
        if ($(this).is(":checked")) {
            $("#addons").attr("class", "d-flex flex-column gap-2");
        } else {
            $("#addons").attr("class", "d-none");
        };
    });
});

$(document).on("click", ".remove-variant-group", function () {
    $(this).parent().remove();
    generateRowCombinations();
})

$(document).on("click", ".remove-add-on", function () {
    $(this).closest("#data-action").parent().remove();
    resetRowIndexes();
})

function resetRowIndexes() {
    $("#table-addons-body tr").each(function (index) {
        $(this).find("input").each(function () {
            let name = $(this).attr("name");
            if (name) {
                let newName = name.replace(/\[\d+\]/, `[${index}]`);
                $(this).attr("name", newName);
            }
        });
    });
}

function generateRowCombinations() {
    let tagLists = [];
    $("#variant-container .tagContainer").each(function () {
        let tags = [];
        $(this).find(".tag").each(function () {
            let tagText = $(this).text().trim().slice(0, -1);
            tags.push(tagText);
        });
        tagLists.push(tags);
    });

    $("#table-variant-body").empty();

    let combinations = tagLists.length > 0 ? tagLists[0] : [];

    for (let i = 1; i < tagLists.length; i++) {
        combinations = generateCombinations([combinations, tagLists[i]]);
    }

    for (var i = 0; i < combinations.length; i++) {
        let isFound = false;

        $("#table-variant-body tr").each(function () {
            let firstTdText = $(this).find("td:first").text().trim();
            if (firstTdText === combinations[i]) {
                isFound = true;
                return false;
            }
        });

        if (isFound) continue;

        let rowData = `
                    <tr>
                        <td>
                            ${combinations[i]}
                            <input type="text" class="form-control d-none variant-sku" value="${combinations[i]}" name="ProductVariants[${i}].Sku">
                        </td>
                        <td>
                            <div class="input-group">
                              <div class="input-group-prepend">
                                <span class="input-group-text">Rp</span>
                              </div>
                              <input type="text" class="form-control variant-price" id="variant-price" name="ProductVariants[${i}].VariantPrice">
                            </div>
                        </td>
                        <td>
                            <input type="number" class="form-control variant-stock" id="variant-stock" name="ProductVariants[${i}].VariantStock">
                        </td>
                        <td>
                            <div class="form-check">
                                <input class="d-none" name="ProductVariants[${i}].IsLimitedStock" value="false">
                                <input class="form-check-input islimited" type="checkbox" value="true" id="isLimited" checked name="ProductVariants[${i}].IsLimitedStock">
                            </div>
                        </td>
                        <td>
                            <div class="form-check">
                                <input class="d-none" name="ProductVariants[${i}].IsAvailable" value="false">
                                <input class="form-check-input isavailable" type="checkbox" value="true" id="isAvailable" checked name="ProductVariants[${i}].IsAvailable">
                            </div>
                        </td>
                    </tr>
                `;

        $("#table-variant-body").append(rowData);
    }
}

function generateCombinations(arr) {
    let result = arr.reduce((acc, current) => {
        let combinations = [];
        acc.forEach(a => {
            current.forEach(c => {
                combinations.push(`${a.trim()}-${c}`);
            });
        });
        return combinations;
    });
    return result;
}

$(document).ready(function () {
    let selectedTags = [];

    $(document).on("keypress", "#inputTag", function (e) {
        if (e.key === "Enter" && $(this).val().trim() !== "") {
            e.preventDefault();

            let tag = $(this).val().trim();
            let $tagContainer = $(this).closest(".col-7").find(".tagContainer");
            
            if (!$tagContainer.find(`.tag:contains(${tag})`).length) {
                $tagContainer.append(`<span class="tag">${tag} <span class="remove" role="button">&times;</span></span>`);
            }

            $(this).val("");

            generateRowCombinations();
        }
    });

    $(document).on("click", ".remove", function () {
        $(this).parent().remove();
        generateRowCombinations();
    });

    $(document).on("change", ".islimited", function () {
        $(this).closest("tr").find(".variant-stock").prop("disabled", !$(this).is(":checked"));
    });

    $(document).on("change", ".isavailable", function () {
        if ($(this).is(":checked")) {
            $(this).closest("tr").css("background-color", "transparent");
            $(this).closest("tr").find(".variant-stock").prop("disabled", false);
            $(this).closest("tr").find(".variant-price").prop("disabled", false);
            $(this).closest("tr").find(".islimited").prop("disabled", false);
        } else {
            $(this).closest("tr").css("background-color", "#e0e0e0");
            $(this).closest("tr").find(".variant-stock").prop("disabled", true);
            $(this).closest("tr").find(".variant-price").prop("disabled", true);
            $(this).closest("tr").find(".islimited").prop("disabled", true);
        }
    });

    $("#newAddon").click(function () {
        let rowCount = $("#table-addons-body tr").length;

        let rowData = `
               <tr>
                    <td>
                        <input type="text" class="form-control variant-price">
                    </td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-prepend">
                            <span class="input-group-text">Rp</span>
                            </div>
                            <input type="text" class="form-control variant-price" aria-label="Amount (to the nearest dollar)">
                        </div>
                    </td>
                    <td>
                        <input type="number" class="form-control variant-stock" id="name" name="Product.ProductStock">
                    </td>
                    <td>
                        <div class="form-check">
                            <input class="form-check-input islimited" type="checkbox" value="" id="isLimited" checked>
                        </div>
                    </td>
                    <td>
                        <div class="form-check">
                            <input class="form-check-input isavailable" type="checkbox" value="" id="isAvailable" checked>
                        </div>
                    </td>
                    <td id="data-action">
                        <div class="remove-add-on" role="button">
                            <span>&times;</span>
                        </div>
                    </td>
               </tr>
            `

        $("#table-addons-body").append(rowData)
    })
});


$(function () {
    let variantIndex = 0;

    $("#newVariant").click(function () {
        let variantHTML =
        `<div class="row">
            <div class="col-4">
                <label for="variantGroups[${variantIndex}]" class="form-label">Tipe Variant</label>
                <select class="form-select variantGroup" id="variantGroups[${variantIndex}]" name="VariantGroups[${variantIndex}].VariantName">
                    <option selected>Open this select menu</option>
                    <option value="Bread">Bread</option>
                    <option value="Color">Color</option>
                    <option value="Size">Size</option>
                </select>
            </div>            
            <div class="col-7">
                <div>
                    <div class="form-group">
                        <label for="inputTag" class="form-label">Masukkan Item</label>
                        <input type="text" id="inputTag" class="form-control" placeholder="Ketik item lalu tekan Enter">
                    </div>
                    <div class="mt-3 border p-3 rounded tagContainer" style="min-height: 50px;">
                    </div>
                </div>
            </div>
            <div class="col-1 remove-variant-group" role="button"> 
                <span>&times;</span>
            </div>
        </div>`;

        $("#variant-container").append(variantHTML);
        variantIndex++;
    });
});
