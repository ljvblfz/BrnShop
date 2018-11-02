var provinceId = -1; //省id
var cityId = -1; //市id
var countyId = -1; //县或区id

//绑定省列表
function bindProvinceList(provinceSelectObj, selectProvinceId) {
    $.get("/tool/provincelist", function (data) {
        var provinceList = eval("(" + data + ")");
        if (provinceList.content.length > 0) {
            var html = "";
            if (selectProvinceId == -1) {
                html += "<option selected=\"selected\" value=\"-1\">" + "请选择" + "</option>";
            }
            else {
                html += "<option value=\"-1\">" + "请选择" + "</option>";
            }
            for (var i = 0; i < provinceList.content.length; i++) {
                if (selectProvinceId == provinceList.content[i].id) {
                    html += "<option selected=\"selected\" value=\"" + provinceList.content[i].id + "\">" + provinceList.content[i].name + "</option>";
                }
                else {
                    html += "<option value=\"" + provinceList.content[i].id + "\">" + provinceList.content[i].name + "</option>";
                }
            }
            $(provinceSelectObj).html(html);
        }
        else {
            alert("加载省列表时出错！")
        }
    })
}

//绑定市列表
function bindCityList(provinceId, citySelectObj, selectCityId) {
    $.get("/tool/citylist?provinceId=" + provinceId, function (data) {
        var cityList = eval("(" + data + ")");
        if (cityList.content.length > 0) {
            var html = "";
            if (selectCityId == -1) {
                html += "<option selected=\"selected\" value=\"-1\">" + "请选择" + "</option>";
            }
            else {
                html += "<option value=\"-1\">" + "请选择" + "</option>";
            }
            for (var i = 0; i < cityList.content.length; i++) {
                if (selectCityId == cityList.content[i].id) {
                    html += "<option selected=\"selected\" value=\"" + cityList.content[i].id + "\">" + cityList.content[i].name + "</option>";
                }
                else {
                    html += "<option value=\"" + cityList.content[i].id + "\">" + cityList.content[i].name + "</option>";
                }
            }
            $(citySelectObj).html(html);
        }
        else {
            alert("加载市列表时出错！")
        }
    })
}

//绑定县或区列表
function bindCountyList(cityId, countySelectObj, selectCountyId) {
    $.get("/tool/countylist?cityId=" + cityId, function (data) {
        var countyList = eval("(" + data + ")");
        if (countyList.content.length > 0) {
            var html = "";
            if (selectCountyId == -1) {
                html += "<option selected=\"selected\" value=\"-1\">" + "请选择" + "</option>";
            }
            else {
                html += "<option value=\"-1\">" + "请选择" + "</option>";
            }
            for (var i = 0; i < countyList.content.length; i++) {
                if (selectCountyId == countyList.content[i].id) {
                    html += "<option selected=\"selected\" value=\"" + countyList.content[i].id + "\">" + countyList.content[i].name + "</option>";
                }
                else {
                    html += "<option value=\"" + countyList.content[i].id + "\">" + countyList.content[i].name + "</option>";
                }
            }
            $(countySelectObj).html(html);
        }
        else {
            alert("加载县或区列表时出错！")
        }
    })
}
