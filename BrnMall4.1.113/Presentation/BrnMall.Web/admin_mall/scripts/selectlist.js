/*分类选择层开始*/
var openCategorySelectLayerBut = null;
var categorySelectLayerHtml = "<div class='selectBoxProgressBar'><p><img src='/admin_mall/images/progressbar.gif'/></p></div>";

function categoryTree(obj, layer) {
    var state = $(obj).prop("class");
    if (state == "open") {
        $(obj).parent().parent().nextAll().each(function (index) {
            var flag = parseInt($(this).attr("layer")) - layer;
            if (flag == 1) {
                $(this).show();
            }
            else if (flag == 0) {
                return false;
            }
        })
        $(obj).removeClass("open").addClass("close");
    }
    else if (state == "close") {
        $(obj).parent().parent().nextAll().each(function (index) {
            if (parseInt($(this).attr("layer")) > layer) {
                $(this).hide();
                $(this).find("th span").each(function (i) {
                    if (typeof ($(this).prop("class")) != "undefined" && $(this).prop("class") !== "") {
                        $(this).removeClass("close").addClass("open");
                    }
                })
            }
            else {
                return false;
            }
        })
        $(obj).removeClass("close").addClass("open");
    }
}

function setSelectedCategory(selectedCateId, selectedCateName) {
    $(openCategorySelectLayerBut).parent().find(".CateId").val(selectedCateId);
    $(openCategorySelectLayerBut).val(selectedCateName).parent().find(".CategoryName").val(selectedCateName);
    $.jBox.close('categorySelectLayer');
}

function ajaxCategorySelectList() {
    $.jBox.setContent(categorySelectLayerHtml);
    $.get("/cache/category/selectlist.htm?t=" + new Date(), function (data) {
        $.jBox.setContent(data);
    })
}

function openCategorySelectLayer(openLayerBut) {
    $.jBox('html:categorySelectLayer', {
        id: 'categorySelectLayer',
        width: 750,
        buttons: { '关闭': true },
        title: "选择类别"
    });
    openCategorySelectLayerBut = openLayerBut;
    ajaxCategorySelectList();
}
/*分类选择层结束*/

/*品牌选择层开始*/
var oldSearchBrandOfSelectList = "";
var openBrandSelectLayerBut = null;
var brandSelectLayerHtml = "<div class='selectBoxProgressBar'><p><img src='/admin_mall/images/progressbar.gif'/></p></div>";

function setSelectedBrand(item) {
    $(openBrandSelectLayerBut).parent().find(".BrandId").val($(item).attr("brandId"));
    var brandName = $(item).text();
    $(openBrandSelectLayerBut).val(brandName);
    $(openBrandSelectLayerBut).parent().find(".BrandName").val(brandName);
    $.jBox.close('brandSelectLayer');
}

function ajaxBrandSelectList(brandName, pageNumber) {
    $.jBox.setContent(brandSelectLayerHtml);
    $.get("/malladmin/brand/selectlist?t=" + new Date(), {
        'brandName': brandName,
        'pageNumber': pageNumber
    }, function (data) {
        var listObj = eval("(" + data + ")");
        var html = "<div id='selectBrandBox'><table width='100%' ><tr><td>品牌名称：<input type='text' id='searchBrandOfSelectList'  name='searchBrandOfSelectList' style='width:120px;height:18px;'> <input type='image' onclick='searchBrandSelectList()' src='/admin_mall/images/s.jpg' class='searchBut'></td></tr><tr><td><div id='selectBrandBoxCon'><ul>";
        for (var i = 0; i < listObj.items.length; i++) {
            html += "<li><a onclick='setSelectedBrand(this)' brandId='" + listObj.items[i].id + "'>" + listObj.items[i].name + "</a></li>";
        }
        html += "<div class='clear'></div></ul><div class='clear'></div></div></td></tr><tr><td><div class='page' style='position:static;'>";

        var pageNumber = parseInt(listObj.pageNumber);
        var totalPages = parseInt(listObj.totalPages);
        var startPageNumber = 1;
        var endPageNumber = 1;
        if (pageNumber == totalPages && totalPages >= 9) {
            startPageNumber = totalPages - 9;
        }
        else if (pageNumber - 4 >= 1) {
            startPageNumber = pageNumber - 4;
        }
        else {
            startPageNumber = 1;
        }
        if (pageNumber == 1 && totalPages >= 9) {
            endPageNumber = 9;
        }
        else if (pageNumber + 4 <= totalPages) {
            endPageNumber = pageNumber + 4;
        }
        else {
            endPageNumber = totalPages;
        }

        html += "<a href='javascript:;' class='bt' onclick='goBrandSelectListPage(this)' pageNumber='1'><<</a>";
        for (var j = startPageNumber; j <= endPageNumber; j++) {
            if (j != pageNumber) {
                html += "<a href='javascript:;' class='bt' onclick='goBrandSelectListPage(this)' pageNumber='" + j + "'>" + j + "</a>";
            }
            else {
                html += "<a href='javascript:;' class='bt hot'>" + j + "</a>";
            }
        }
        html += "<a href='javascript:;' class='bt' onclick='goBrandSelectListPage(this)' pageNumber='" + totalPages + "'>>></a>";

        html += "</div></td></tr></table></div>"
        $.jBox.setContent(html);
        $("#searchBrandOfSelectList").val(oldSearchBrandOfSelectList);
    })
}

function searchBrandSelectList() {
    oldSearchBrandOfSelectList = $("#searchBrandOfSelectList").val();
    ajaxBrandSelectList(oldSearchBrandOfSelectList);
}

function goBrandSelectListPage(pageObj) {
    oldSearchBrandOfSelectList = $("#searchBrandOfSelectList").val();
    ajaxBrandSelectList(oldSearchBrandOfSelectList, $(pageObj).attr("pageNumber"));
}

function openBrandSelectLayer(openLayerBut) {
    $.jBox('html:brandSelectLayer', {
        id: 'brandSelectLayer',
        width: 750,
        buttons: { '关闭': true },
        title: "选择品牌"
    });
    openBrandSelectLayerBut = openLayerBut;
    ajaxBrandSelectList();

}
/*品牌选择层结束*/

/*商品选择层开始*/
var oldSearchProductOfSelectList = "";
var openProductSelectLayerBut = null;
var productSelectLayerHtml = "<div class='selectBoxProgressBar'><p><img src='/admin_mall/images/progressbar.gif'/></p></div>";

function setSelectedProduct(item) {
    $(openProductSelectLayerBut).parent().find(".Pid").val($(item).attr("pid"));
    var productName = $(item).text();
    $(openProductSelectLayerBut).val(productName);
    $(openProductSelectLayerBut).parent().find(".ProductName").val(productName);
    $.jBox.close('productSelectLayer');
}

function ajaxProductSelectList(storeId, productName, pageNumber) {
    $.jBox.setContent(productSelectLayerHtml);
    $.get("/malladmin/product/productselectlist?t=" + new Date(), {
        'storeId': storeId,
        'productName': productName,
        'pageNumber': pageNumber
    }, function (data) {
        var listObj = eval("(" + data + ")");
        var html = "<div id='selectProductBox'><table width='100%' ><tr><td>商品名称：<input type='text' id='searchProductOfSelectList'  name='searchProductOfSelectList' style='width:120px;height:18px;'> <input type='image' onclick='searchProductSelectList(" + storeId + ")' src='/admin_mall/images/s.jpg' class='searchBut'></td></tr><tr><td><div id='selectProductBoxCon'><ul>";
        for (var i = 0; i < listObj.items.length; i++) {
            html += "<li><a onclick='setSelectedProduct(this)' pid='" + listObj.items[i].id + "'>" + listObj.items[i].name + "</a></li>";
        }
        html += "<div class='clear'></div></ul><div class='clear'></div></div></td></tr><tr><td><div class='page' style='position:static;'>";

        var pageNumber = parseInt(listObj.pageNumber);
        var totalPages = parseInt(listObj.totalPages);
        var startPageNumber = 1;
        var endPageNumber = 1;
        if (pageNumber == totalPages && totalPages >= 9) {
            startPageNumber = totalPages - 9;
        }
        else if (pageNumber - 4 >= 1) {
            startPageNumber = pageNumber - 4;
        }
        else {
            startPageNumber = 1;
        }
        if (pageNumber == 1 && totalPages >= 9) {
            endPageNumber = 9;
        }
        else if (pageNumber + 4 <= totalPages) {
            endPageNumber = pageNumber + 4;
        }
        else {
            endPageNumber = totalPages;
        }

        html += "<a href='javascript:;' class='bt' onclick='goProductSelectListPage(this," + storeId + ")' pageNumber='1'><<</a>";
        for (var j = startPageNumber; j <= endPageNumber; j++) {
            if (j != pageNumber) {
                html += "<a href='javascript:;' class='bt' onclick='goProductSelectListPage(this," + storeId + ")' pageNumber='" + j + "'>" + j + "</a>";
            }
            else {
                html += "<a href='javascript:;' class='bt hot'>" + j + "</a>";
            }
        }
        html += "<a href='javascript:;' class='bt' onclick='goProductSelectListPage(this," + storeId + ")' pageNumber='" + totalPages + "'>>></a>";

        html += "</div></td></tr></table></div>"
        $.jBox.setContent(html);
        $("#searchProductOfSelectList").val(oldSearchProductOfSelectList);
    })
}

function searchProductSelectList(storeId) {
    oldSearchProductOfSelectList = $("#searchProductOfSelectList").val();
    ajaxProductSelectList(storeId, oldSearchProductOfSelectList);
}

function goProductSelectListPage(pageObj, storeId) {
    oldSearchProductOfSelectList = $("#searchProductOfSelectList").val();
    ajaxProductSelectList(storeId, oldSearchProductOfSelectList, $(pageObj).attr("pageNumber"));
}

function openProductSelectLayer(openLayerBut, storeId) {
    $.jBox('html:productSelectLayer', {
        id: 'productSelectLayer',
        width: 950,
        buttons: { '关闭': true },
        title: "选择商品"
    });
    openProductSelectLayerBut = openLayerBut;
    ajaxProductSelectList(storeId);

}
/*商品选择层结束*/

/*店铺选择层开始*/
var oldSearchStoreOfSelectList = "";
var openStoreSelectLayerBut = null;
var storeSelectLayerHtml = "<div class='selectBoxProgressBar'><p><img src='/admin_mall/images/progressbar.gif'/></p></div>";

function setSelectedStore(item) {
    $(openStoreSelectLayerBut).parent().find(".StoreId").val($(item).attr("storeId"));
    var storeName = $(item).text();
    $(openStoreSelectLayerBut).val(storeName);
    $(openStoreSelectLayerBut).parent().find(".StoreName").val(storeName);
    $.jBox.close('storeSelectLayer');
}

function ajaxStoreSelectList(storeName, pageNumber) {
    $.jBox.setContent(storeSelectLayerHtml);
    $.get("/malladmin/store/storeselectlist?t=" + new Date(), {
        'storeName': storeName,
        'pageNumber': pageNumber
    }, function (data) {
        var listObj = eval("(" + data + ")");
        var html = "<div id='selectStoreBox'><table width='100%' ><tr><td>店铺名称：<input type='text' id='searchStoreOfSelectList'  name='searchStoreOfSelectList' style='width:120px;height:18px;'> <input type='image' onclick='searchStoreSelectList()' src='/admin_mall/images/s.jpg' class='searchBut'></td></tr><tr><td><div id='selectStoreBoxCon'><ul>";
        for (var i = 0; i < listObj.items.length; i++) {
            html += "<li><a onclick='setSelectedStore(this)' storeId='" + listObj.items[i].id + "'>" + listObj.items[i].name + "</a></li>";
        }
        html += "<div class='clear'></div></ul><div class='clear'></div></div></td></tr><tr><td><div class='page' style='position:static;'>";

        var pageNumber = parseInt(listObj.pageNumber);
        var totalPages = parseInt(listObj.totalPages);
        var startPageNumber = 1;
        var endPageNumber = 1;
        if (pageNumber == totalPages && totalPages >= 9) {
            startPageNumber = totalPages - 9;
        }
        else if (pageNumber - 4 >= 1) {
            startPageNumber = pageNumber - 4;
        }
        else {
            startPageNumber = 1;
        }
        if (pageNumber == 1 && totalPages >= 9) {
            endPageNumber = 9;
        }
        else if (pageNumber + 4 <= totalPages) {
            endPageNumber = pageNumber + 4;
        }
        else {
            endPageNumber = totalPages;
        }

        html += "<a href='javascript:;' class='bt' onclick='goStoreSelectListPage(this)' pageNumber='1'><<</a>";
        for (var j = startPageNumber; j <= endPageNumber; j++) {
            if (j != pageNumber) {
                html += "<a href='javascript:;' class='bt' onclick='goStoreSelectListPage(this)' pageNumber='" + j + "'>" + j + "</a>";
            }
            else {
                html += "<a href='javascript:;' class='bt hot'>" + j + "</a>";
            }
        }
        html += "<a href='javascript:;' class='bt' onclick='goStoreSelectListPage(this)' pageNumber='" + totalPages + "'>>></a>";

        html += "</div></td></tr></table></div>"
        $.jBox.setContent(html);
        $("#searchStoreOfSelectList").val(oldSearchStoreOfSelectList);
    })
}

function searchStoreSelectList() {
    oldSearchStoreOfSelectList = $("#searchStoreOfSelectList").val();
    ajaxStoreSelectList(oldSearchStoreOfSelectList);
}

function goStoreSelectListPage(pageObj) {
    oldSearchStoreOfSelectList = $("#searchStoreOfSelectList").val();
    ajaxStoreSelectList(oldSearchStoreOfSelectList, $(pageObj).attr("pageNumber"));
}

function openStoreSelectLayer(openLayerBut) {
    $.jBox('html:storeSelectLayer', {
        id: 'storeSelectLayer',
        width: 750,
        buttons: { '关闭': true },
        title: "选择店铺"
    });
    openStoreSelectLayerBut = openLayerBut;
    ajaxStoreSelectList();

}
/*店铺选择层结束*/