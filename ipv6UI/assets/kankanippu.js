//ignorei18n_start
var Kankanippu = Kankanippu || function () { };

Kankanippu.prototype = {
    categories: {
        server: 0,
        apm: 2,
        networkMonitor: 1,
        website: 5,
        rum: 3,
        tools: 4,
        statusPage: 7,
        aws: 6,
        mobileApm: 8,
        msp: 9,
        appLog: 10,
        vmware: 11,
        home: 12
    },
    setProduct: function (productCode) {
        if (productCode === undefined) {
            var category = $('meta[name=s247-category]').attr('content');
            if (category !== undefined) {
                productCode = this.categories[category];
            }
        }

        if (productCode === undefined) {
            return;
        }

        var visitedProducts = readCookie('VPT');

        if (visitedProducts != null) {
            var productList = visitedProducts.split(",");

            if ($.inArray(productCode.toString(), productList) == -1) {
                visitedProducts = visitedProducts + "," + productCode;
            }
        } else {
            visitedProducts = productCode;
        }

        createCookie('VPT', visitedProducts);
    }
};

var tracker = new Kankanippu();

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') { c = c.substring(1, c.length) };
        if (c.indexOf(nameEQ) == 0) { return decodeURIComponent(c.substring(nameEQ.length, c.length)) };
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

function getCookie(name) {
    var cookiename = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        if (c.indexOf(cookiename) > 0) {
            return c.substring(cookiename.length + 1, c.length);
        }
    }
    return null;
}

var cookieExpiry = 90;

function persistCrossDCCookies() {
    var cookieNames = ["utm_source", "utm_medium", "VPT", "httpReferrer1", "httpReferrer2", "landingPageUrl", "ad_src", "ad_grp"]; //No i18N
    var params = [];
    for (var i = 0; i < cookieNames.length; i++) {
        var cookieName = cookieNames[i];
        var cookieValue = readCookie(cookieName);
        if (cookieValue) {
            params.push(cookieName + "=" + encodeURIComponent(cookieValue));
        }
    }
    var uri = "/lp/mar-com.html?" + params.join('&'); //No i18N
    $('body').append('<iframe style="display:none" src="https://www.site24x7.in' + uri + '"></iframe>');
    $('body').append('<iframe style="display:none" src="https://www.site24x7.eu' + uri + '"></iframe>');
    $('body').append('<iframe style="display:none" src="https://www.site24x7.net.au' + uri + '"></iframe>');
}

$(function (){
    $.QueryString = (function (paramsArray) {
        var params = {};

        for (var i = 0; i < paramsArray.length; ++i) {
            var param = paramsArray[i].split('=', 2);

            if (param.length !== 2) {
                continue;
            }

            params[param[0]] = decodeURIComponent(param[1].replace(/\+/g, " "));
        }

        return params;
    })(window.location.search.substr(1).split('&'));

    function isNotEmpty(varName) {
        var response = true;
        if (varName == "" || varName == undefined) {
            response = false;
        }
        return response;
    }

    function sendSIQEvent(action) {
        try {
            $zoho.salesiq.visitor.customaction("{'eventCategory':'Help','eventAction':'" + action + "','customID':'-'}");
        } catch (exp) { }
    }

    var httpReferrer1 = readCookie("httpReferrer1"); //No i18N

    if (!isNotEmpty(httpReferrer1)) {
        var referrer = document.referrer && document.referrer.length > 0 ? document.referrer : "-";
        createCookie("httpReferrer1", referrer, cookieExpiry); //No i18N
    }

    var landingPageUrl = readCookie("landingPageUrl"); //No i18N
    if (!isNotEmpty(landingPageUrl)) {
        createCookie("landingPageUrl", document.location.href, cookieExpiry); //No i18N
    }

    createCookie("ad_src", $.QueryString.ad_src, cookieExpiry); //No i18N
    createCookie("ad_grp", $.QueryString.ad_grp, cookieExpiry); //No i18N

    createCookie("utm_source", $.QueryString.utm_source, cookieExpiry); //No i18N
    createCookie("utm_medium", $.QueryString.utm_medium, cookieExpiry); //No i18N
    createCookie("utm_term", $.QueryString.utm_term, cookieExpiry); //No i18N
    createCookie("utm_content", $.QueryString.utm_content, cookieExpiry); //No i18N
    createCookie("utm_campaign", $.QueryString.utm_campaign, cookieExpiry); //No i18N
    createCookie("utm_expid", $.QueryString.utm_expid, cookieExpiry); //No i18N
    createCookie("utm_referrer", $.QueryString.utm_referrer, cookieExpiry); //No i18N

    var url = [location.protocol, '//', location.host, location.pathname].join('');
    if (/(signup|website-monitoring2)\.html$/.test(url) || /\/ads\//.test(url)) {
        persistCrossDCCookies();
    }

    if (/\/help\//.test(url)) {
        fetch('/tools/marketing/_ln?method=subscriptionDetails', {
            method: 'GET'
        }).then(function (response) {
            if (response.status === 200) {
                response.json().then(function (subscription) {
                    if (subscription.isEvalUser) {
                        sendSIQEvent('EvalUser');
                    } else if (subscription.isPaidUser) {
                        sendSIQEvent('PaidUser');
                    }
                });
            }
        });
    }

    tracker.setProduct();
});
//ignorei18n_end