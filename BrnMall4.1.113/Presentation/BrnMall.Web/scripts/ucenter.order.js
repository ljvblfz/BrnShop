//取消订单
function cancelOrder(oid, cancelReason) {
    $.post("/ucenter/cancelorder", { 'oid': oid, 'cancelReason': cancelReason }, cancelOrderResponse);
}

//处理取消订单的反馈信息
function cancelOrderResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        $("#orderState" + result.content).html("取消");
        $("#orderOperator" + result.content).html("<a href='/ucenter/orderinfo?oid=" + result.content + "'>查看</a>");
        alert("取消成功");
    }
    else {
        alert(result.content);
    }
}

//收货
function receiveOrder(oid) {
    $.get("/ucenter/receiveorder?oid=" + oid, receiveOrderResponse);
}

//处理收货的反馈信息
function receiveOrderResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        $("#orderState" + result.content).html("已收货");
        $("#orderOperator" + result.content).html("<a href='/ucenter/orderinfo?oid=" + result.content + "'>查看</a> <a href='/ucenter/revieworder?oid=" + result.content + "'>评价</a> <a href='/ucenter/orderafterservice?oid=" + result.content + "'>售后服务</a>");
        alert("操作成功");
    }
    else {
        alert(result.content);
    }
}

//打开评价商品层
function openReviewProductBlock(recordId) {
    var reviewProductFrom = document.forms["reviewProductFrom"];
    reviewProductFrom.elements["recordId"].value = recordId;
    $("#reviewProductBlock").show();
}

//评价商品
function reviewProduct() {
    var reviewProductFrom = document.forms["reviewProductFrom"];

    var oid = reviewProductFrom.elements["oid"].value;
    var recordId = reviewProductFrom.elements["recordId"].value;
    var star = $("#reviewProductFrom input[name=star]:checked").val();
    var message = reviewProductFrom.elements["message"].value;

    if (!verifyReviewProduct(recordId, star, message)) {
        return;
    }
    $.post("/ucenter/reviewproduct?oid=" + oid + "&recordId=" + recordId, { 'star': star, 'message': message }, reviewProductResponse);
}

//验证评价商品
function verifyReviewProduct(recordId, star, message) {
    if (recordId < 1) {
        alert("请选择商品");
        return false;
    }
    if (star < 1 || star > 5) {
        alert("请选择正确的星星");
        return false;
    }
    if (message.length == 0) {
        alert("请输入评价内容");
        return false;
    }
    if (message.length > 100) {
        alert("评价内容最多输入100个字");
        return false;
    }
    return true;
}

//处理评价商品的反馈信息
function reviewProductResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        var reviewProductFrom = document.forms["reviewProductFrom"];
        reviewProductFrom.elements["recordId"].value = 0;
        reviewProductFrom.elements["message"].value = "";

        $("#reviewProductBlock").hide();

        $("#reviewState" + result.content).html("已评价");
        $("#reviewOperate" + result.content).html("");

        alert("评价成功");
    }
    else {
        alert(result.content);
    }
}

//评价店铺
function reviewStore() {
    var reviewStoreFrom = document.forms["reviewStoreFrom"];

    var oid = reviewStoreFrom.elements["oid"].value;

    var descriptionStar = $("#reviewStoreFrom input[name=descriptionStar]:checked").val();
    var serviceStar = $("#reviewStoreFrom input[name=serviceStar]:checked").val();
    var shipStar = $("#reviewStoreFrom input[name=shipStar]:checked").val();

    $.post("/ucenter/reviewstore?oid=" + oid, { 'descriptionStar': descriptionStar, 'serviceStar': serviceStar, 'shipStar': shipStar }, reviewStoreResponse);
}

//处理评价店铺的反馈信息
function reviewStoreResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        $("#reviewStoreBut").remove();
        alert("评价成功");
    }
    else {
        alert(result.content);
    }
}

//申请订单售后服务
function applyOrderAfterService(oid, recordId) {
    var applyOrderAfterServiceForm = document.forms["applyOrderAfterServiceForm"];

    var type = $(applyOrderAfterServiceForm.elements["type"]).val();
    var count = applyOrderAfterServiceForm.elements["count"].value;
    var applyReason = applyOrderAfterServiceForm.elements["applyReason"].value;

    if (!verifyApplyOrderAfterService(count, applyReason)) {
        return;
    }

    var parms = new Object();
    parms["type"] = type;
    parms["count"] = count;
    parms["applyReason"] = applyReason;
    $.post("/ucenter/applyorderafterservice?oid=" + oid + "&recordId=" + recordId, parms, applyOrderAfterServiceResponse)
}

//验证申请订单售后服务
function verifyApplyOrderAfterService(count, applyReason) {
    if (count < 1) {
        alert("数量必须大于0");
        return false;
    }
    if (applyReason.length == 0) {
        alert("请输入申请原因");
        return false;
    }
    if (applyReason.length > 150) {
        alert("申请原因最多输入150个字");
        return false;
    }
    return true;
}

//处理申请订单售后服务的反馈信息
function applyOrderAfterServiceResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        alert("申请成功");
        window.location.href = "/ucenter/orderafterservice?oid=" + result.content;
    }
    else {
        alert(result.content);
    }
}

//邮寄给商城
function sendOrderAfterService(asId) {
    var sendOrderAfterServiceForm = document.forms["sendOrderAfterServiceForm"];

    var shipCoId = $(sendOrderAfterServiceForm.elements["shipCoId"]).find("option:selected").val();
    var shipSN = sendOrderAfterServiceForm.elements["shipSN"].value;
    var consignee = sendOrderAfterServiceForm.elements["consignee"].value;
    var mobile = sendOrderAfterServiceForm.elements["mobile"].value;
    var regionId = $(sendOrderAfterServiceForm.elements["regionId"]).find("option:selected").val();
    var address = sendOrderAfterServiceForm.elements["address"].value;

    if (!verifySendOrderAfterService(shipCoId, shipSN, consignee, mobile, regionId, address)) {
        return;
    }

    var parms = new Object();
    parms["shipCoId"] = shipCoId;
    parms["shipSN"] = shipSN;
    parms["consignee"] = consignee;
    parms["mobile"] = mobile;
    parms["regionId"] = regionId;
    parms["address"] = address;
    $.post("/ucenter/sendorderafterservice?asId=" + asId, parms, sendOrderAfterServiceResponse)
}

//验证邮寄给商城
function verifySendOrderAfterService(shipCoId, shipSN, consignee, mobile, regionId, address) {
    if (shipCoId < 1) {
        alert("请选择配送公司");
        return false;
    }
    if (shipSN.length == 0) {
        alert("请输入配送单号");
        return false;
    }
    if (shipSN.length > 30) {
        alert("配送单号最多输入30个字符");
        return false;
    }
    if (consignee.length == 0) {
        alert("请输入收货人");
        return false;
    }
    if (consignee.length > 10) {
        alert("收货人名称不能超过10个字");
        return false;
    }
    if (mobile.length == 0) {
        alert("请输入手机号");
        return false;
    }
    if (mobile.length != 11) {
        alert("请输入正确的手机号");
        return false;
    }
    if (regionId < 1) {
        alert("请选择配送区域");
        return false;
    }
    if (address.length == 0) {
        alert("请输入详细地址");
        return false;
    }
    if (address.length > 75) {
        alert("详细地址最多输入75个字");
        return false;
    }
    return true;
}

//处理邮寄给商城的反馈信息
function sendOrderAfterServiceResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        alert("操作成功");
        window.location.href = "/ucenter/orderafterservice?oid=" + result.content;
    }
    else {
        alert(result.content);
    }
}