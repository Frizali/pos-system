$(document).ready(function () {
    $("#variant, #addons").hide();

    $("#hasVariant").on("change", function () {
        if (this.checked) {
            $("#variant").attr("class", "d-flex flex-column gap-2");
            $("#product-management").attr("class", "d-none");
        } else {
            $("#variant").attr("class", "d-none");
            $("#product-management").attr("class", "card");
        }
    });

    $("#hasAddons").on("change", function () {
        $("#addons").attr("class", this.checked ? "d-flex flex-column gap-2" : "d-none");
    });

    let variantIndex = 0;
    $("#newVariant").on("click", function () {
        const variantHTML = `
            <div class="row">
                <div class="col-4">
                    <label for="variantGroups[${variantIndex}]" class="form-label">Tipe Variant</label>
                    <select class="form-select variantGroup" id="variantGroups[${variantIndex}]" name="VariantGroups[${variantIndex}].VariantName">
                        <option selected>Open this select variant</option>
                        <option value="Variant">Variant</option>
                        <option value="Ukuran">Ukuran</option>
                        <option value="Rasa">Rasa</option>
                        <option value="Tingkat Kepedasan">Tingkat Kepedasan</option>
                        <option value="Saus">Saus</option>
                        <option value="Topping">Topping</option>
                        <option value="Level Gula">Level Gula</option>
                        <option value="Suhu">Suhu</option>
                    </select>
                </div>
                <div class="col-7">
                    <div class="form-group">
                        <label for="inputTag" class="form-label">Masukkan Item</label>
                        <input type="text" id="inputTag" class="form-control" placeholder="Ketik item lalu tekan Enter">
                    </div>
                    <div class="mt-3 border p-3 rounded tagContainer" style="min-height: 50px;"></div>
                </div>
                <div class="col-1 remove-variant-group" role="button">
                    <span>&times;</span>
                </div>
            </div>`;
        $("#variant-container").append(variantHTML);
        variantIndex++;
    });

    $("#newAddon").on("click", function () {
        const rowCount = $("#table-addons-body tr").length;
        const rowData = `
            <tr>
                <td><input type="text" class="form-control addon-name" name="ProductAddons[${rowCount}].AddonName"></td>
                <td>
                    <div class="input-group">
                        <div class="input-group-prepend"><span class="input-group-text">Rp</span></div>
                        <input type="text" class="form-control addon-price" name="ProductAddons[${rowCount}].AddonPrice">
                    </div>
                </td>
                <td><input type="number" class="form-control addon-stock" id="addon-stock" name="ProductAddons[${rowCount}].AddonStock"></td>
                <td><input class="form-check-input islimited" type="checkbox" value="true" name="ProductAddons[${rowCount}].IsLimited" checked></td>
                <td><input class="form-check-input isavailable" type="checkbox" value="true" name="ProductAddons[${rowCount}].IsAvailable" checked></td>
                <td id="data-action"><div class="remove-add-on" role="button"><span>&times;</span></div></td>
            </tr>`;
        $("#table-addons-body").append(rowData);
    });

    $(document)
        .on("click", ".remove-variant-group", function () {
            $(this).parent().remove();
            generateRowCombinations();
        })
        .on("click", ".remove-add-on", function () {
            $(this).closest("tr").remove();
            resetRowIndexes();
        })
        .on("keypress", "#inputTag", function (e) {
            if (e.key === "Enter" && this.value.trim()) {
                e.preventDefault();
                const tag = this.value.trim();
                const $tagContainer = $(this).closest(".col-7").find(".tagContainer");
                if (!$tagContainer.find(`.tag:contains(${tag})`).length) {
                    $tagContainer.append(`<span class="tag">${tag} <span class="remove" role="button">&times;</span></span>`);
                }
                this.value = "";
                generateRowCombinations();
            }
        })
        .on("click", ".remove", function () {
            $(this).parent().remove();
            generateRowCombinations();
        })
        .on("change", ".islimited", function () {
            $(this).closest("tr").find(".variant-stock").prop("disabled", !this.checked);
        })
        .on("change", ".isavailable", function () {
            const $row = $(this).closest("tr");
            const isAvailable = this.checked;
            $row.css("background-color", isAvailable ? "transparent" : "#e0e0e0");
            $row.find(".variant-stock, .variant-price, .islimited").prop("disabled", !isAvailable);
        });

    function resetRowIndexes() {
        $("#table-addons-body tr").each(function (index) {
            $(this).find("input").each(function () {
                const name = $(this).attr("name");
                if (name) {
                    const newName = name.replace(/\[\d+\]/, `[${index}]`);
                    $(this).attr("name", newName);
                }
            });
        });
    }

    function generateCombinations(arrays) {
        return arrays.reduce((acc, curr) => {
            const result = [];
            acc.forEach(a => {
                curr.forEach(c => result.push(`${a.trim()}-${c}`));
            });
            return result;
        });
    }

    function generateRowCombinations() {
        const tagLists = [];

        $("#variant-container .tagContainer").each(function () {
            const tags = $(this).find(".tag").map(function () {
                return $(this).text().trim().slice(0, -1);
            }).get();
            tagLists.push(tags);
        });

        let combinations = tagLists.length ? tagLists[0] : [];
        for (let i = 1; i < tagLists.length; i++) {
            combinations = generateCombinations([combinations, tagLists[i]]);
        }

        $("#table-variant-body").empty();
        combinations.forEach((combo, i) => {
            const exists = $("#table-variant-body tr").toArray().some(tr => $(tr).find("td:first").text().trim() === combo);
            if (exists) return;

            const rowData = `
                <tr>
                    <td>
                        ${combo}
                        <input type="text" class="form-control d-none variant-sku" value="${combo}" name="ProductVariants[${i}].Sku">
                    </td>
                    <td>
                        <div class="input-group">
                            <div class="input-group-prepend"><span class="input-group-text">Rp</span></div>
                            <input type="text" class="form-control variant-price" name="ProductVariants[${i}].VariantPrice">
                        </div>
                    </td>
                    <td><input type="number" class="form-control variant-stock" name="ProductVariants[${i}].VariantStock"></td>
                    <td>
                        <input type="hidden" name="ProductVariants[${i}].IsLimitedStock" value="false">
                        <input class="form-check-input islimited" type="checkbox" value="true" name="ProductVariants[${i}].IsLimitedStock" checked>
                    </td>
                    <td>
                        <input type="hidden" name="ProductVariants[${i}].IsAvailable" value="false">
                        <input class="form-check-input isavailable" type="checkbox" value="true" name="ProductVariants[${i}].IsAvailable" checked>
                    </td>
                </tr>`;
            $("#table-variant-body").append(rowData);
        });
    }
});
