var uid = -1; //用户id
var isGuestSC = 0; //是否允许游客使用购物车(0代表不可以，1代表可以)

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

//返回
function pageBack() {
    var a = window.location.href;
    if (/#top/.test(a)) {
        window.history.go(-2);
        window.location.load(window.location.href)
    } else {
        window.history.back();
        window.location.load(window.location.href)
    }
}

//导航栏显示和隐藏
function navSH() {
    $("#nav").toggle();
}

//搜索
function search(word) {
    if (word == undefined || word == null || word.length < 1) {
        alert("请输入关键词");
    }
    else {
        window.location.href = "/mob/catalog/search?word=" + encodeURIComponent(word);
    }
}

//获得分类商品列表
var cpListNextPageNumber = 2;
function getCategoryProductList(cateId, brandId, filterPrice, filterAttr, onlyStock, sortColumn, sortDirection, page) {
    $("#loadBut").hide();
    $("#loadPrompt").show();
    $.get("/mob/catalog/ajaxcategory?cateId=" + cateId + "&brandId=" + brandId + "&filterPrice=" + filterPrice + "&filterAttr=" + filterAttr + "&onlyStock=" + onlyStock + "&sortColumn=" + sortColumn + "&sortDirection=" + sortDirection + "&page=" + page, function (data) {
        getCategoryProductListResponse(data);
    })
}

//处理获得分类商品列表的反馈信息
function getCategoryProductListResponse(data) {
    try {
        var result = eval("(" + data + ")");
        var element = document.createElement("div");
        element.className = "proItme";
        var innerHTML = "";
        for (var i = 0; i < result.ProductList.length; i++) {
            var reviewLayer = 100;
            var goodStars = result.ProductList[i].Star1 + result.ProductList[i].Star2 + result.ProductList[i].Star3;
            var allStars = goodStars + result.ProductList[i].Star4 + result.ProductList[i].Star5;
            if (allStars != 0) {
                reviewLayer = goodStars * 100 / allStars;
            }

            innerHTML += "<a href='/mob/" + result.ProductList[i].Pid + ".html'>";
            innerHTML += "<img src='/upload/product/show/thumb100_100/" + result.ProductList[i].ShowImg + "' width='100' height='100' class='img' />";
            innerHTML += "<span class='proDt'>";
            innerHTML += "<strong class='proDD DD1'>" + result.ProductList[i].Name + "</strong>";
            innerHTML += "<b class='proDD DD3'>￥" + result.ProductList[i].ShopPrice + "</b>";
            innerHTML += "<p class='proDD DD4'>" + result.ProductList[i].ReviewCount + " 人评价，" + reviewLayer + "%好评</p>";
            innerHTML += "</span></a>";
        }
        element.innerHTML = innerHTML;
        document.getElementById("categoryProductListBlock").appendChild(element);
        if (result.PageModel.HasNextPage) {
            $("#loadBut").show();
            $("#loadPrompt").hide();
            cpListNextPageNumber += 1;
        }
        else {
            $("#loadBut").hide();
            $("#loadPrompt").hide();
            $("#lastPagePrompt").show();
        }
    }
    catch (ex) {
        alert("加载错误");
    }
}

//获得搜索商品列表
var spListNextPageNumber = 2;
function getSearchProductList(word, cateId, brandId, filterPrice, filterAttr, onlyStock, sortColumn, sortDirection, page) {
    document.getElementById("loadBut").style.display = "none";
    document.getElementById("loadPrompt").style.display = "block";
    $.get("/mob/catalog/ajaxsearch?word=" + encodeURIComponent(word) + "&cateId=" + cateId + "&brandId=" + brandId + "&filterPrice=" + filterPrice + "&filterAttr=" + filterAttr + "&onlyStock=" + onlyStock + "&sortColumn=" + sortColumn + "&sortDirection=" + sortDirection + "&page=" + page, function (data) {
        getSearchProductListResponse(data);
    })
}

//处理获得搜索商品列表的反馈信息
function getSearchProductListResponse(data) {
    try {
        var result = eval("(" + data + ")");
        var element = document.createElement("div");
        element.className = "proItme";
        var innerHTML = "";
        for (var i = 0; i < result.ProductList.length; i++) {
            var reviewLayer = 100;
            var goodStars = result.ProductList[i].Star1 + result.ProductList[i].Star2 + result.ProductList[i].Star3;
            var allStars = goodStars + result.ProductList[i].Star4 + result.ProductList[i].Star5;
            if (allStars != 0) {
                reviewLayer = goodStars * 100 / allStars;
            }

            innerHTML += "<a href='/mob/" + result.ProductList[i].Pid + ".html'>";
            innerHTML += "<img src='/upload/product/show/thumb100_100/" + result.ProductList[i].ShowImg + "' width='100' height='100' class='img' />";
            innerHTML += "<span class='proDt'>";
            innerHTML += "<strong class='proDD DD1'>" + result.ProductList[i].Name + "</strong>";
            innerHTML += "<b class='proDD DD3'>￥" + result.ProductList[i].ShopPrice + "</b>";
            innerHTML += "<p class='proDD DD4'>" + result.ProductList[i].ReviewCount + " 人评价，" + reviewLayer + "%好评</p>";
            innerHTML += "</span></a>";
        }
        element.innerHTML = innerHTML;
        document.getElementById("searchProductListBlock").appendChild(element);
        if (result.PageModel.HasNextPage) {
            $("#loadBut").show();
            $("#loadPrompt").hide();
            spListNextPageNumber += 1;
        }
        else {
            $("#loadBut").hide();
            $("#loadPrompt").hide();
            $("#lastPagePrompt").show();
        }
    }
    catch (ex) {
        alert("加载错误");
    }
}