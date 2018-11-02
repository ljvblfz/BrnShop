//增加商品数量
function addProuctCount() {
    var buyCountInput = $("#buyCount");
    var buyCount = buyCountInput.val();
    if (!isInt(buyCount)) {
        alert('请输入数字');
        return false;
    }
    buyCountInput.val(parseInt(buyCount) + 1);
}

//减少商品数量
function cutProductCount() {
    var buyCountInput = $("#buyCount");
    var buyCount = buyCountInput.val();
    if (!isInt(buyCount)) {
        alert('请输入数字');
        return false;
    }
    var count = parseInt(buyCount);
    if (count > 1) {
        buyCountInput.val(count - 1);
    }
}

//添加商品到收藏夹
function addProductToFavorite(pid) {
    if (pid < 1) {
        alert("请选择商品");
    }
    else if (uid < 1) {
        alert("请先登录");
    }
    else {
        $.get("/mob/ucenter/addproducttofavorite?pid=" + pid, addProductToFavoriteResponse)
    }
}

//处理添加商品到收藏夹的反馈信息
function addProductToFavoriteResponse(data) {
    var result = eval("(" + data + ")");
    alert(result.content);
}

//添加商品到购物车
function addProductToCart(pid, buyCount, type) {
    if (pid < 1) {
        alert("请选择商品");
    }
    else if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else if (buyCount < 1) {
        alert("请填写购买数量");
    }
    else {
        $.get("/mob/cart/addproduct?pid=" + pid + "&buyCount=" + buyCount, function (data) {
            addProductToCartResponse(type, data);
        });
    }
}

//处理添加商品到购物车的反馈信息
function addProductToCartResponse(type, data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        if (type == 0) {
            window.location.href = "/mob/cart/index";
        }
        else {
            $("#addResult1").show();
            $("#addResult2").show();
        }
    }
    else {
        alert(result.content);
    }
}

//添加套装到购物车
function addSuitToCart(pmId, buyCount, type) {
    if (pmId < 1) {
        alert("请选择套装");
    }
    else if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else if (buyCount < 1) {
        alert("请填写购买数量");
    }
    else {
        $.get("/mob/cart/addsuit?pmId=" + pmId + "&buyCount=" + buyCount, function (data) {
            addSuitToCartResponse(type, data);
        });
    }
}

//处理添加套装到购物车的反馈信息
function addSuitToCartResponse(type, data) {
    var result = eval("(" + data + ")");
    if (result.state != "stockout") {
        if (type == 0) {
            window.location.href = "/mob/cart/index";
        }
        else {
            $("#addResult1").show();
            $("#addResult2").show();
        }
    }
    else {
        alert("商品库存不足");
    }
}

//获得商品评价列表
function getProductReviewList(pid, reviewType, page) {
    $.get("/mob/catalog/ajaxproductreviewlist?pid=" + pid + "&reviewType=" + reviewType + "&page=" + page, getProductReviewListResponse)
}

//处理获得商品评价的反馈信息
function getProductReviewListResponse(data) {
    $("#productReviewList").html(data);
}

//获得商品咨询列表
function getProductConsultList(pid, consultTypeId, consultMessage, page) {
    $.get("/mob/catalog/ajaxproductconsultlist?pid=" + pid + "&consultTypeId=" + consultTypeId + "&consultMessage=" + encodeURIComponent(consultMessage) + "&page=" + page, getProductConsultListResponse)
}

//处理获得商品咨询的反馈信息
function getProductConsultListResponse(data) {
    $("#productConsultList").html(data);
}