//获得配送地址列表
function getShipAddressList() {
    $.get("/mob/ucenter/ajaxshipaddresslist", getShipAddressListResponse);
}

//处理获得配送地址列表的反馈信息
function getShipAddressListResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        var shipAddressList = "";
        for (var i = 0; i < result.content.count; i++) {
            shipAddressList += "<div class='bgBlock'></div><div class='adressI'><p>" + result.content.list[i].user + "</p><p class='f14'>" + result.content.list[i].address + "</p><div class='chooseAD'><input type='checkbox' class='radio' name='shipAddressItem' value='" + result.content.list[i].saId + "' onclick='selectShipAddress(" + result.content.list[i].saId + ")'/>送到这里去</div></div>";
        }
        shipAddressList += "<a href='javascript:openAddShipAddressBlock()' class='addAddress'>+添加收货地址</a>";
        $("#mainBlock").hide();
        $("#shipAddressListBlock").show();
        $("#shipAddressListBlock").html(shipAddressList);
    }
    else {
        alert(result.content);
    }
}

//选择配送地址
function selectShipAddress(saId) {
    $("#saId").val(saId);
    document.getElementById("confirmOrderForm").submit();
}

//打开添加配送地址块
function openAddShipAddressBlock() {
    $("#addShipAddressBlock").show();
}

//添加配送地址
function addShipAddress() {
    var addShipAddressForm = document.forms["addShipAddressForm"];

    var consignee = addShipAddressForm.elements["consignee"].value;
    var mobile = addShipAddressForm.elements["mobile"].value;
    var regionId = $(addShipAddressForm.elements["regionId"]).find("option:selected").val();
    var address = addShipAddressForm.elements["address"].value;

    if (!verifyAddShipAddress(consignee, mobile, regionId, address)) {
        return;
    }

    $.post("/mob/ucenter/addshipaddress",
            { 'consignee': consignee, 'mobile': mobile, 'regionId': regionId, 'address': address, 'isDefault': 1 },
            addShipAddressResponse)
}

//验证添加的收货地址
function verifyAddShipAddress(consignee, mobile, regionId, address) {
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

//处理添加配送地址的反馈信息
function addShipAddressResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        $("#saId").val(result.content);
        document.getElementById("confirmOrderForm").submit();
    }
    else {
        var msg = "";
        for (var i = 0; i < result.content.length; i++) {
            msg += result.content[i].msg + "\n";
        }
        alert(msg)
    }
}

//显示支付方式列表
function showPayModeList() {
    $("#mainBlock").hide();
    $("#payModeListBlock").show();
}

//选择支付方式
function selectPayMode(mode) {
    $("#payMode").val(mode);
    if (mode == 0) {
        $("#payModeShowBlock").html("<span>货到付款</span>");
    }
    else {
        $("#payModeShowBlock").html("<span>在线支付</span>");
    }
    $("#mainBlock").show();
    $("#payModeListBlock").hide();
}

//获得有效的优惠劵列表
function getValidCouponList() {
    $.get("/mob/order/getvalidcouponlist", getValidCouponListResponse);
}

//处理获得有效的优惠劵列表的反馈信息
function getValidCouponListResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state == "success") {
        if (result.content.length < 1) {
            document.getElementById("validCouponList").innerHTML = "<div class=\"allCell\">此订单暂无可用的优惠券</div>";
        }
        else {
            var itemList = "";
            for (var i = 0; i < result.content.length; i++) {
                itemList += "<div class=\"allCell\"><span class=\"radio\" checked='false' value='" + result.content[i].couponId + "' useMode='" + result.content[i].useMode + "' onclick='checkCouponUseMode(this)'></span>" + result.content[i].name + "</div>";
            }
            $("#validCouponList").html(itemList);
        }
        $("#mainBlock").hide();
        $("#validCouponListBlcok").show();
    }
    else {
        alert(result.content);
    }
}

//检查优惠劵的使用模式
function checkCouponUseMode(obj) {
    if (obj.getAttribute("checked") == "true") {
        obj.setAttribute("checked", "false");
        obj.className = "radio";
    }
    else {
        var useMode = obj.getAttribute("useMode");
        if (useMode == "1") {
            var checkboxList = document.getElementById("validCouponList").getElementsByTagName("span");
            for (var i = 0; i < checkboxList.length; i++) {
                checkboxList[i].setAttribute("checked", "false");
                checkboxList[i].className = "radio";
            }
        }
        obj.setAttribute("checked", "true");
        obj.className = "radio checked";
    }
}

//确认选择的优惠劵
function confirmSelectedCoupon() {
    var couponList = "";
    var couponIdCheckboxList = document.getElementById("validCouponList").getElementsByTagName("span");
    for (var i = 0; i < couponIdCheckboxList.length; i++) {
        if (couponIdCheckboxList[i].getAttribute("checked") == "true") {
            couponList += "<div class='sell'><i>惠</i>" + couponIdCheckboxList[i].getAttribute("text") + "</div>";
        }
    }
    $("#selectCouponList").html(couponList);
    $("#mainBlock").show();
    $("#validCouponListBlcok").hide();
}

//提交订单
function submitOrder() {
    var selectedCartItemKeyList = $("#selectedCartItemKeyList").val()
    var saId = $("#saId").val();
    var payMode = $("#payMode").val();
    var allFullCut = $("#allFullCut").val();

    var payCreditCount = $("#payCreditCount").prop("checked") ? $("#payCreditCount").val() : 0;

    var couponIdList = "";
    $("#validCouponList span[checked=true]").each(function () {
        couponIdList += $(this).attr("value") + ",";
    })
    if (couponIdList != "")
        couponIdList = couponIdList.substring(0, couponIdList.length - 1);


    if (!verifySubmitOrder(saId)) {
        return;
    }

    $.post("/mob/order/submitorder",
            { 'selectedCartItemKeyList': selectedCartItemKeyList, 'saId': saId, 'payMode': payMode, 'payCreditCount': payCreditCount, 'couponIdList': couponIdList, 'fullCut': allFullCut },
            submitOrderResponse)
}

//验证提交订单
function verifySubmitOrder(saId) {
    if (saId < 1) {
        alert("请填写收货人信息");
        return false;
    }
    return true;
}

//处理提交订单的反馈信息
function submitOrderResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state != "success") {
        alert(result.content);
    }
    else {
        window.location.href = result.content;
    }
}