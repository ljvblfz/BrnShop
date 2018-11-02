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

//咨询商品
function consultProduct(uid, pid) {
    var consultProductFrom = document.forms["consultProductFrom"];

    var consultTypeId = 0;
    $(consultProductFrom.elements["consultTypeId"]).each(function () {
        if ($(this).prop("checked")) {
            consultTypeId = $(this).val();
            return false;
        }
    })
    var consultMessage = consultProductFrom.elements["consultMessage"].value;
    var verifyCode = consultProductFrom.elements["verifyCode"] ? consultProductFrom.elements["verifyCode"].value : undefined;

    if (!verifyConsultProduct(uid, pid, consultTypeId, consultMessage, verifyCode)) {
        return;
    }
    $.post("/catalog/consultproduct", { 'pid': pid, 'consultTypeId': consultTypeId, 'consultMessage': consultMessage, 'verifyCode': verifyCode }, consultProductResponse)
}

//验证咨询商品
function verifyConsultProduct(uid, pid, consultTypeId, consultMessage, verifyCode) {
    if (uid < 1) {
        alert("请登录");
        return false;
    }
    if (pid < 1) {
        alert("请选择商品");
        return false;
    }
    if (consultTypeId < 1) {
        alert("请选择咨询类型");
        return false;
    }
    if (consultMessage.lenth < 1) {
        alert("请填写咨询内容");
        return false;
    }
    if (consultMessage.length > 100) {
        alert("咨询内容内容太长");
        return false;
    }
    if (verifyCode != undefined && verifyCode.length == 0) {
        alert("请输入验证码");
        return false;
    }
    return true;
}

//处理咨询商品的反馈信息
function consultProductResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        alert("您的咨询我们已经收到，我们会尽快给您回复");
        window.location.href = result.content;
    }
    else {
        alert(result.content);
    }
}

//获得商品评价列表
function getProductReviewList(pid, reviewType, page) {
    $.get("/catalog/ajaxproductreviewlist?pid=" + pid + "&reviewType=" + reviewType + "&page=" + page, getProductReviewListResponse)
}

//处理获得商品评价的反馈信息
function getProductReviewListResponse(data) {
    $("#productReviewList").html(data);
    $("#prCount").html($("#productReviewCount").html());
}

//获得商品咨询列表
function getProductConsultList(pid, consultTypeId, consultMessage, page) {
    $.get("/catalog/ajaxproductconsultlist?pid=" + pid + "&consultTypeId=" + consultTypeId + "&consultMessage=" + encodeURIComponent(consultMessage) + "&page=" + page, getProductConsultListResponse)
}

//处理获得商品咨询的反馈信息
function getProductConsultListResponse(data) {
    $("#productConsultList").html(data);
}