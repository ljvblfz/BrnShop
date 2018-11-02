var uid = -1; //用户id
var isGuestSC = 0; //是否允许游客使用购物车(0代表不可以，1代表可以)
var scSubmitType = 0; //购物车的提交方式(0代表跳转到提示页面，1代表跳转到列表页面，2代表ajax提交)

$.ajaxSetup({
    cache: false //关闭AJAX缓存
});

//判断是否是数字
function isNumber(val) {
    var regex = /^[\d|\.]+$/;
    return regex.test(val);
}

//判断是否为整数
function isInt(val) {
    var regex = /^\d+$/;
    return regex.test(val);
}

//判断是否为邮箱
function isEmail(val) {
    var regex = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return regex.test(val);
}

//判断是否为手机号
function isMobile(val) {
    var regex = /^[1][0-9][0-9]{9}$/;
    return regex.test(val);
}

//搜索
function search(word) {
    if (word == undefined || word == null || word.length < 1) {
        alert("请输入关键词");
    }
    else {
        window.location.href = "/catalog/search?word=" + encodeURIComponent(word);
    }
}

//获得购物车快照
function getCartSnap() {
    if (isGuestSC == 0 && uid < 1) {
        return;
    }
    $("#cartSnap").show();
    $.get("/cart/snap", function (data) {
        getCartSnapResponse(data);
    })
}

//处理获得购物车快照的反馈信息
function getCartSnapResponse(data) {
    try {
        var result = eval("(" + data + ")");
        alert(result.content);
    }
    catch (ex) {
        $("#cartSnap").html(data);
        $("#cartSnapProudctCount").html($("#csProudctCount").html());
    }
}

//关闭购物车快照
function closeCartSnap() {
    $("#cartSnap").hide();
}

//添加商品到收藏夹
function addToFavorite(pid) {
    if (pid < 1) {
        alert("请选择商品");
    }
    else if (uid < 1) {
        alert("请先登录");
    }
    else {
        $.get("/ucenter/addtofavorite?pid=" + pid, addToFavoriteResponse)
    }
}

//处理添加商品到收藏夹的反馈信息
function addToFavoriteResponse(data) {
    var result = eval("(" + data + ")");
    alert(result.content);
}

//添加商品到购物车
function addProductToCart(pid, buyCount) {
    if (pid < 1) {
        alert("请选择商品");
    }
    else if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else if (buyCount < 1) {
        alert("请填写购买数量");
    }
    else if (scSubmitType != 2) {
        window.location.href = "/cart/addproduct?pid=" + pid + "&buyCount=" + buyCount;
    }
    else {
        $.get("/cart/addproduct?pid=" + pid + "&buyCount=" + buyCount, addProductToCartResponse)
    }
}

//处理添加商品到购物车的反馈信息
function addProductToCartResponse(data) {
    var result = eval("(" + data + ")");
    alert(result.content);
}

//购买商品
function buyProduct(pid, buyCount) {
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
        $.get("/cart/buyproduct?pid=" + pid + "&buyCount=" + buyCount, buyProductResponse)
    }
}

//处理购买商品的反馈信息
function buyProductResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        window.location.href = result.content;
    }
    else {
        alert(result.content);
    }
}

//添加套装到购物车
function addSuitToCart(pmId, buyCount) {
    if (pmId < 1) {
        alert("请选择套装");
    }
    else if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else if (buyCount < 1) {
        alert("请填写购买数量");
    }
    else if (scSubmitType != 2) {
        window.location.href = "/cart/addsuit?pmId=" + pmId + "&buyCount=" + buyCount;
    }
    else {
        $.get("/cart/addsuit?pmId=" + pmId + "&buyCount=" + buyCount, addSuitToCartResponse)
    }
}

//处理添加套装到购物车的反馈信息
function addSuitToCartResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state != "stockout") {
        alert(result.content);
    }
    else {
        alert("商品库存不足");
    }
}

//购买套装
function buySuit(pmId, buyCount) {
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
        $.get("/cart/buysuit?pmId=" + pmId + "&buyCount=" + buyCount, buySuitResponse)
    }
}

//处理购买套装的反馈信息
function buySuitResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        window.location.href = result.content;
    }
    else {
        alert(result.content);
    }
}

//获得选中的购物车项键列表
function getSelectedCartItemKeyList() {
    var valueList = new Array();
    $("#cartBody input[type=checkbox][name=cartItemCheckbox]:checked").each(function () {
        valueList.push($(this).val());
    })

    if (valueList.length < 1) {
        //当取消全部商品时,添加一个字符防止商品全部选中
        return "_";
    }
    else {
        return valueList.join(',');
    }
}

//设置选择全部购物车项复选框
function setSelectAllCartItemCheckbox() {
    var flag = true;
    $("#cartBody input[type=checkbox][name=cartItemCheckbox]:not(:checked)").each(function () {
        flag = false;
        return false;
    })

    if (flag) {
        $("#selectAllBut_top").prop("checked", true);
        $("#selectAllBut_bottom").prop("checked", true);
    }
    else {
        $("#selectAllBut_top").prop("checked", false);
        $("#selectAllBut_bottom").prop("checked", false);
    }
}

//删除购物车中商品
function delCartProduct(pid, pos) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    if (pos == 0) {
        $.get("/cart/delproduct?pid=" + pid + "&pos=" + pos + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
            try {
                alert(val("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
    }
    else {
        $.get("/cart/delproduct?pid=" + pid + "&pos=" + pos, function (data) {
            try {
                alert(val("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartSnap").html(data);
                $("#cartSnapProudctCount").html($("#csProudctCount").html());
            }
        })
    }
}

//删除购物车中套装
function delCartSuit(pmId, pos) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    if (pos == 0) {
        $.get("/cart/delsuit?pmId=" + pmId + "&pos=" + pos + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
    }
    else {
        $.get("/cart/delsuit?pmId=" + pmId + "&pos=" + pos, function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartSnap").html(data);
                $("#cartSnapProudctCount").html($("#csProudctCount").html());
            }
        })
    }
}

//删除购物车中满赠
function delCartFullSend(pmId, pos) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    if (pos == 0) {
        $.get("/cart/delfullsend?pmId=" + pmId + "&pos=" + pos + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
                setBatchSelectCartItemCheckbox();
            }
        })
    }
    else {
        $.get("/cart/delfullsend?pmId=" + pmId + "&pos=" + pos, function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartSnap").html(data);
                $("#cartSnapProudctCount").html($("#csProudctCount").html());
            }
        })
    }
}

//清空购物车
function clearCart(pos) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/cart/clear?pos=" + pos, function (data) {
        try {
            alert(eval("(" + data + ")").content);
        }
        catch (ex) {
            if (pos == 0) {
                $("#cartBody").html(data);
            }
            else {
                $("#cartSnap").html(data);
                $("#cartSnapProudctCount").html("0");
            }
        }
    })
}

//改变商品数量
function changePruductCount(pid, buyCount) {
    if (!isInt(buyCount)) {
        alert('请输入数字');
    }
    else if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else {
        var key = "0_" + pid;
        $("#cartBody input[type=checkbox][value=" + key + "]").each(function () {
            $(this).prop("checked", true);
            return false;
        })
        $.get("/cart/changeproductcount?pid=" + pid + "&buyCount=" + buyCount + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
    }
}

//改变套装数量
function changeSuitCount(pmId, buyCount) {
    if (!isInt(buyCount)) {
        alert('请输入数字');
    }
    else if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else {
        var key = "1_" + pmId;
        $("#cartBody input[type=checkbox][value=" + key + "]").each(function () {
            $(this).prop("checked", true);
            return false;
        })
        $.get("/cart/changesuitcount?pmId=" + pmId + "&buyCount=" + buyCount + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
    }
}

//获取满赠商品
function getFullSend(pmId) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/cart/getfullsend?pmId=" + pmId, function (data) {
        getFullSendResponse(data, pmId);
    })
}

//处理获取满赠商品的反馈信息
var selectedFullSendPid = 0;
function getFullSendResponse(data, pmId) {
    var result = eval("(" + data + ")");
    if (result.state != "success") {
        alert(result.content);
    }
    else {
        if (result.content.length < 1) {
            alert("满赠商品不存在");
            return;
        }
        var html = "<table width='100%' border='0' cellpadding='0' cellspacing='0'>";
        for (var i = 0; i < result.content.length; i++) {
            html += "<tr><td width='30' align='center'><input type='radio' name='fullSendProduct' value='" + result.content[i].pid + "' onclick='selectedFullSendPid=this.value'/></td><td width='70'><img src='/upload/product/show/thumb60_60/" + result.content[i].showImg + "' width='50' height='50' /></td><td valign='top'><a href='" + result.content[i].url + "'>" + result.content[i].name + "</a><em>¥" + result.content[i].shopPrice + "</em></td></tr>";
        }
        html += "</table>";
        selectedFullSendPid = 0;
        $("#fullSendProductList" + pmId).html(html);
        $("#fullSendBlock" + pmId).show();
    }
}

//关闭满赠层
function closeFullSendBlock(pmId) {
    selectedFullSendPid = 0;
    $("#fullSendProductList" + pmId).html("");
    $("#fullSendBlock" + pmId).hide();
}

//添加满赠商品
function addFullSend(pmId) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else if (selectedFullSendPid < 1) {
        alert("请先选择商品");
    }
    else {
        $.get("/cart/addfullsend?pmId=" + pmId + "&pid=" + selectedFullSendPid + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
        closeFullSendBlock(pmId);
    }
}

//取消或选中购物车项
function cancelOrSelectCartItem() {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/cart/cancelorselectcartitem?selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
        try {
            alert(eval("(" + data + ")").content);
        }
        catch (ex) {
            $("#cartBody").html(data);
            setSelectAllCartItemCheckbox();
        }
    })
}

//取消或选中全部购物车项
function cancelOrSelectAllCartItem(obj) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    if (obj.checked) {
        $.get("/cart/selectallcartitem", function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
            }
        })
    }
    else {
        $("#cartBody input[type=checkbox]").each(function () {
            $(this).prop("checked", false);
        })
        $("#totalCount").html("0");
        $("#productAmount").html("0.00");
        $("#fullCut").html("0");
        $("#orderAmount").html("0.00");
    }
}

//前往确认订单
function goConfirmOrder() {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    var valueList = new Array();
    $("#cartBody input[type=checkbox][name=cartItemCheckbox]:checked").each(function () {
        valueList.push($(this).val());
    })

    if (valueList.length < 1) {
        alert("请先选择购物车商品");
    }
    else {
        $("#selectedCartItemKeyList").val(valueList.join(','));
        document.forms[0].submit();
    }
}

//获取优惠劵
function getCoupon(uid, couponTypeId) {
    if (uid < 1) {
        alert("请先登录");
    }
    else if (couponTypeId < 1) {
        alert("请选择优惠劵");
    }
    else {
        $.get("/coupon/getcoupon?couponTypeId=" + couponTypeId, getCouponResponse)
    }
}

//处理获取优惠劵的反馈信息
function getCouponResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        alert("领取成功");
    }
    else {
        alert(result.content);
    }
}