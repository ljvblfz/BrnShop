var isLoading = false;
var favoriteProductListEnd = false;
var favoriteStoreListEnd = false;
var browseProductListEnd = false;

//获得收藏夹商品列表
var fpListNextPageNumber = 2;
function getFavoriteProductList(page) {
    if (favoriteProductListEnd) {
        $("#lastPagePrompt").show();
    }
    else {
        isLoading = true;
        $("#loadPrompt").show();
        $.get("/mob/ucenter/ajaxfavoriteproductlist?page=" + page, getFavoriteProductListResponse)
    }
}

//处理获得收藏夹商品列表的反馈信息
function getFavoriteProductListResponse(data) {
    try {
        var result = eval("(" + data + ")");
        for (var i = 0; i < result.ProductList.length; i++) {
            var element = document.createElement("li");
            element.id = "favoriteProduct" + result.ProductList[i].pid;
            var innerHTML = "";
            innerHTML += "<a href='/mob/" + result.ProductList[i].pid + "'.html>";
            innerHTML += "<img src='/upload/store/" + result.ProductList[i].storeid + "/product/show/thumb60_60/" + result.ProductList[i].showimg + "' /></a>";
            innerHTML += "<div class='description'>";
            innerHTML += "<a href='/mob/" + result.ProductList[i].pid + "'.html>";
            innerHTML += "<span class='title'>" + result.ProductList[i].name + "</span><span class='price'>¥" + result.ProductList[i].shopprice + "</span></a>";
            innerHTML += "<a href='javascript:delFavoriteProduct(" + result.ProductList[i].pid + ")' class='del delFavorite'></a>";
            innerHTML += "</div></li>";
            element.innerHTML = innerHTML;
            document.getElementById("favoriteProductListBlock").appendChild(element);
        }
        $("#loadPrompt").hide();
        if (result.PageModel.HasNextPage) {
            fpListNextPageNumber += 1;
        }
        else {
            favoriteProductListEnd = true;
            $("#lastPagePrompt").show();
        }
        isLoading = false;
    }
    catch (ex) {
        alert("加载错误");
    }
}

//删除收藏夹中的商品
function delFavoriteProduct(pid) {
    $.get("/mob/ucenter/delfavoriteproduct?pid=" + pid, delFavoriteProductResponse)
}

//处理删除收藏夹中的商品的反馈信息
function delFavoriteProductResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        $("#favoriteProduct" + result.content).remove();
        alert("删除成功");
    }
    else {
        alert(result.content);
    }
}

//获得收藏夹店铺列表
var fsListNextPageNumber = 1;
function getFavoriteStoreList(page) {
    if (favoriteStoreListEnd) {
        $("#lastPagePrompt").show();
    }
    else {
        isLoading = true;
        $("#loadPrompt").show();
        $.get("/mob/ucenter/ajaxfavoritestorelist?page=" + page, getFavoriteStoreListResponse)
    }
}

//处理获得收藏夹店铺列表的反馈信息
function getFavoriteStoreListResponse(data) {
    try {
        var result = eval("(" + data + ")");
        for (var i = 0; i < result.StoreList.length; i++) {
            var element = document.createElement("li");
            element.id = "favoriteStore" + result.StoreList[i].storeid;
            var innerHTML = "";
            innerHTML += "<li><a href='/store/" + result.StoreList[i].storeid + ".html'>";
            innerHTML += "<img src='/upload/store/" + result.StoreList[i].storeid + "/logo/thumb50_50/" + result.StoreList[i].logo + "' />";
            innerHTML += "<div class='favDPinfo'><span class='title'>" + result.StoreList[i].name + "</span>";
            innerHTML += "<div class='heart'><span class='new-mu-heartv' style='width:95.32%'></span></div>";
            innerHTML += "</div></a><a href='javascript:delFavoriteStore(" + result.StoreList[i].storeid + ")' class='del delFavorite'></a></li>";
            element.innerHTML = innerHTML;
            document.getElementById("favoriteStoreListBlock").appendChild(element);
        }
        $("#loadPrompt").hide();
        if (result.PageModel.HasNextPage) {
            fsListNextPageNumber += 1;
        }
        else {
            favoriteStoreListEnd = true;
            $("#lastPagePrompt").show();
        }
        isLoading = false;
    }
    catch (ex) {
        alert("加载错误");
    }
}

//删除收藏夹中的店铺
function delFavoriteStore(storeId) {
    $.get("/mob/ucenter/delfavoritestore?storeId=" + storeId, delFavoriteStoreResponse)
}

//处理删除收藏夹中的店铺的反馈信息
function delFavoriteStoreResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        $("#favoriteStore" + result.content).remove();
        alert("删除成功");
    }
    else {
        alert(result.content);
    }
}

//获得浏览商品列表
var bpListNextPageNumber = 1;
function getBrowseProductList(page) {
    if (browseProductListEnd) {
        $("#lastPagePrompt").show();
    }
    else {
        isLoading = true;
        $("#loadPrompt").show();
        $.get("/mob/ucenter/ajaxbrowseproductlist?page=" + page, getBrowseProductListResponse)
    }
}

//处理获得浏览商品列表的反馈信息
function getBrowseProductListResponse(data) {
    try {
        var result = eval("(" + data + ")");
        for (var i = 0; i < result.ProductList.length; i++) {
            var element = document.createElement("li");
            var innerHTML = "";
            innerHTML += "<a href='/mob/" + result.ProductList[i].Pid + "'.html>";
            innerHTML += "<img src='/upload/store/" + result.ProductList[i].StoreId + "/product/show/thumb60_60/" + result.ProductList[i].ShowImg + "' /></a>";
            innerHTML += "<div class='description'>";
            innerHTML += "<a href='/mob/" + result.ProductList[i].Pid + "'.html>";
            innerHTML += "<span class='title'>" + result.ProductList[i].Name + "</span><span class='price'>¥" + result.ProductList[i].ShopPrice + "</span></a>";
            innerHTML += "</div></li>";
            element.innerHTML = innerHTML;
            document.getElementById("browseProductListBlock").appendChild(element);
        }
        $("#loadPrompt").hide();
        if (result.PageModel.HasNextPage) {
            bpListNextPageNumber += 1;
        }
        else {
            browseProductListEnd = true;
            $("#lastPagePrompt").show();
        }
        isLoading = false;
    }
    catch (ex) {
        alert("加载错误");
    }
}

//删除配送地址
function delShipAddress(saId) {
    $.get("/mob/ucenter/delshipaddress?saId=" + saId, delShipAddressResponse)
}

//处理删除配送地址的反馈信息
function delShipAddressResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        $("#shipAddress" + result.content).remove();
        alert("删除成功");
    }
    else {
        alert(result.content);
    }
}

//设置默认配送地址
function setDefaultShipAddress(saId, obj) {
    $.get("/mob/ucenter/setdefaultshipaddress?saId=" + saId, function (data) {
        setDefaultShipAddressResponse(data, obj);
    })
}

//处理设置默认配送地址的反馈信息
function setDefaultShipAddressResponse(data, obj) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        var defaultShipAddress = document.getElementById("defaultShipAddress");
        if (defaultShipAddress != undefined) {
            defaultShipAddress.checked = "";
            defaultShipAddress.id = "";
        }
        obj.checked = "checked";
        obj.id = "defaultShipAddress";
    }
    else {
        alert(result.content);
    }
}

//添加配送地址
function addShipAddress() {
    var shipAddressForm = document.forms["shipAddressForm"];

    var consignee = shipAddressForm.elements["consignee"].value;
    var mobile = shipAddressForm.elements["mobile"].value;
    var regionId = $(shipAddressForm.elements["regionId"]).find("option:selected").val();
    var address = shipAddressForm.elements["address"].value;

    if (!verifyShipAddress(consignee, mobile, regionId, address)) {
        return;
    }

    $.post("/mob/ucenter/addshipaddress",
            { 'consignee': consignee, 'mobile': mobile, 'regionId': regionId, 'address': address },
            addShipAddressResponse)
}

//处理添加配送地址的反馈信息
function addShipAddressResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        window.location.href = "/mob/ucenter/shipaddresslist";
    }
    else if (result.state == "full") {
        alert("配送地址的数量已经达到系统所允许的最大值")
    }
    else if (result.state == "error") {
        var msg = "";
        for (var i = 0; i < result.content.length; i++) {
            msg += result.content[i].msg + "\n";
        }
        alert(msg)
    }
}

//编辑配送地址
function editShipAddress() {
    var shipAddressForm = document.forms["shipAddressForm"];

    var saId = shipAddressForm.elements["saId"].value;
    var consignee = shipAddressForm.elements["consignee"].value;
    var mobile = shipAddressForm.elements["mobile"].value;
    var regionId = $(shipAddressForm.elements["regionId"]).find("option:selected").val();
    var address = shipAddressForm.elements["address"].value;

    if (saId < 1) {
        alert("请选择配送地址");
        return;
    }
    if (!verifyShipAddress(consignee, mobile, regionId, address)) {
        return;
    }

    $.post("/mob/ucenter/editshipaddress?saId=" + saId,
            { 'consignee': consignee, 'mobile': mobile, 'regionId': regionId, 'address': address },
            editShipAddressResponse)
}

//处理编辑配送地址的反馈信息
function editShipAddressResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        window.location.href = "/mob/ucenter/shipaddresslist";
    }
    else if (result.state == "noexist") {
        alert("配送地址不存在");
    }
    else if (result.state == "error") {
        var msg = "";
        for (var i = 0; i < result.content.length; i++) {
            msg += result.content[i].msg + "\n";
        }
        alert(msg)
    }
}

//验证配送地址
function verifyShipAddress(consignee, mobile, regionId, address) {
    if (consignee == "") {
        alert("请填写收货人");
        return false;
    }
    if (mobile == "") {
        alert("请填写手机号");
        return false;
    }
    if (parseInt(regionId) < 1) {
        alert("请选择区域");
        return false;
    }
    if (address == "") {
        alert("请填写详细地址");
        return false;
    }
    return true;
}