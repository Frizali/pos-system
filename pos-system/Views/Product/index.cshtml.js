$(function () {
    $("#variant").hide();
    $("#hasVariant").change(function () {
        if ($(this).is(":checked")) {
            $("#variant").attr("class", "d-flex flex-column gap-2");
        } else {
            $("#variant").attr("class", "d-none");
        };
    });
});

//Handle Input Tag
$(document).ready(function () {
    let selectedTags = [];

    $(document).on("keypress", "#inputTag", function (e) {
        if (e.key === "Enter" && $(this).val().trim() !== "") {
            e.preventDefault();

            let tag = $(this).val().trim();
            let $tagContainer = $(this).closest(".col-8").find(".tagContainer");

            if (!$tagContainer.find(`.tag:contains(${tag})`).length) {
                $tagContainer.append(`<span class="tag">${tag} <span class="remove" role="button">&times;</span></span>`);
            }

            $(this).val("");

            let tagLists = [];
            $("#variant-container .tagContainer").each(function () {
                let tags = [];
                $(this).find(".tag").each(function () {
                    let tagText = $(this).text().trim().slice(0, -1);
                    tags.push(tagText);
                });
                tagLists.push(tags);
            });

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

            $("#table-body").empty();

            let combinations = tagLists.length > 0 ? tagLists[0] : [];

            for (let i = 1; i < tagLists.length; i++) {
                combinations = generateCombinations([combinations, tagLists[i]]);
            } 

            for (var i = 0; i < combinations.length; i++) {
                let isFound = false;

                $("#table-body tr").each(function () {
                    let firstTdText = $(this).find("td:first").text().trim();
                    if (firstTdText === combinations[i]) {
                        isFound = true;
                        return false;
                    }
                });

                if (isFound) continue;

                let rowData = `
                    <tr>
                        <td>${combinations[i]}</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                `;

                $("#table-body").append(rowData);
            }
        }
    });

    $(document).on("click", ".remove", function () {
        $(this).parent().remove();
    });
});


$(function () {
    let variantIndex = 0;

    $("#addVariant").click(function () {
        let variantHTML = `<div class="row">
            <div class="col-4">
                <label for="variantGroup${variantIndex}" class="form-label" name="VariantGroups[${variantIndex}].VariantName">Tipe Variant</label>
                <select class="form-select" aria-label="Default select example" id="variantGroup${variantIndex}">
                    <option selected>Open this select menu</option>
                    <option value="Bread">Bread</option>
                    <option value="Color">Color</option>
                    <option value="Size">Size</option>
                </select>
            </div>            
            <div class="col-8">
                <div>
                    <div class="form-group">
                        <label for="inputTag" class="form-label">Masukkan Item</label>
                        <input type="text" id="inputTag" class="form-control" placeholder="Ketik item lalu tekan Enter">
                    </div>
                    <div class="mt-3 border p-3 rounded tagContainer" style="min-height: 50px;">
                    </div>
                </div>
            </div>
        </div>`;

        $("#variant-container").append(variantHTML);
        variantIndex++;
    });
});