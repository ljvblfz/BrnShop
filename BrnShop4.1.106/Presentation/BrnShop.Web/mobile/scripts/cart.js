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
        $("#selectAllBut_bottom").prop("checked", true);
    }
    else {
        $("#selectAllBut_bottom").prop("checked", false);
    }
}

//删除购物车中商品
function delCartProduct(pid) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/mob/cart/delproduct?pid=" + pid + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
        try {
            alert(val("(" + data + ")").content);
        }
        catch (ex) {
            $("#cartBody").html(data);
            setSelectAllCartItemCheckbox();
        }
    })
}

//删除购物车中套装
function delCartSuit(pmId) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/mob/cart/delsuit?pmId=" + pmId + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
        try {
            alert(eval("(" + data + ")").content);
        }
        catch (ex) {
            $("#cartBody").html(data);
            setSelectAllCartItemCheckbox();
        }
    })
}

//删除购物车中满赠
function delCartFullSend(pmId) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/mob/cart/delfullsend?pmId=" + pmId + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
        try {
            alert(eval("(" + data + ")").content);
        }
        catch (ex) {
            $("#cartBody").html(data);
            setSelectAllCartItemCheckbox();
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
        $.get("/mob/cart/changeproductcount?pid=" + pid + "&buyCount=" + buyCount + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
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
        $.get("/mob/cart/changesuitcount?pmId=" + pmId + "&buyCount=" + buyCount + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
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

//商品购买数量输入框事件
var tempProductNumber = 0;
function productNumberFocus(obj) {
    tempProductNumber = obj.value;
    obj.value = "";
}
function productNumberBlur(obj, itemId, itemType) {
    var value = obj.value;
    if (value == "") {
        obj.value = tempProductNumber;
    }
    else {
        if (!isInt(value)) {
            alert("只能输入数字!");
            obj.value = tempProductNumber;
        }
        else {
            if (itemType == 0) {
                changePruductCount(itemId, value);
            }
            else {
                changeSuitCount(itemId, value);
            }
        }
    }
}

//获取满赠商品
var selectedFullSendPmId = 0;
function getFullSend(pmId) {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/mob/cart/getfullsend?pmId=" + pmId, function (data) {
        selectedFullSendPmId = pmId;
        getFullSendResponse(data);
    })
}

//处理获取满赠商品的反馈信息
var selectedFullSendPid = 0;
function getFullSendResponse(data) {
    var result = eval("(" + data + ")");
    if (result.state != "success") {
        alert(result.content);
    }
    else {
        if (result.content.length < 1) {
            alert("满赠商品不存在");
            return;
        }

        var html = "";
        for (var i = 0; i < result.content.length; i++) {
            html += "<div class='proInfo'>";
            html += "<div class='price'>￥" + result.content[i].shopPrice + "</div>";
            html += "<div class='proInfo1'><input type='radio' name='fullSendProduct' class='checkbox' value='" + result.content[i].pid + "' onclick='selectedFullSendPid=this.value'/></div>";
            html += "<div class='proInfo2 change'>";
            html += "<a href='" + result.content[i].url + "' class='proImg'><img src='/upload/product/show/thumb60_60/" + result.content[i].showImg + "' width='59' height='59' /></a>";
            html += "<div class='text'>";
            html += "<a href='" + result.content[i].url + "'>" + result.content[i].name + "</a>";
            html += "<div class='nb'>x1</div></div></div></div>";
        }
        $("#fullSendProductList").html(html);
        $("#fullSendBlock").show();
        $("#fullSendMask").show();
    }
}

//关闭满赠层
function closeFullSendBlock() {
    selectedFullSendPmId = 0;
    selectedFullSendPid = 0;
    $("#fullSendProductList").html("");
    $("#fullSendBlock").hide();
    $("#fullSendMask").hide();
}

//添加满赠商品
function addFullSend() {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
    }
    else if (selectedFullSendPmId < 1) {
        alert("请先选择促销活动");
    }
    else if (selectedFullSendPid < 1) {
        alert("请先选择商品");
    }
    else {
        $.get("/mob/cart/addfullsend?pmId=" + selectedFullSendPmId + "&pid=" + selectedFullSendPid + "&selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
            try {
                alert(eval("(" + data + ")").content);
            }
            catch (ex) {
                $("#cartBody").html(data);
                setSelectAllCartItemCheckbox();
            }
        })
        closeFullSendBlock();
    }
}

//取消或选中购物车项
function cancelOrSelectCartItem() {
    if (isGuestSC == 0 && uid < 1) {
        alert("请先登录");
        return;
    }
    $.get("/mob/cart/cancelorselectcartitem?selectedCartItemKeyList=" + getSelectedCartItemKeyList(), function (data) {
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
        $.get("/mob/cart/selectallcartitem", function (data) {
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