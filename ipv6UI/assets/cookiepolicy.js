//ignorei18n_start
var euCountries = ['RS', 'VA', 'GB', 'UA', 'CH', 'SE', 'ES', 'SI', 'SK', 'SM', 'RU', 'RO', 'PT', 'PL', 'NO', 'NL', 'ME', 'MC', 'MD', 'MT', 'MK', //No i18N
    'LU', 'LT', 'LI', 'LV', 'IT', 'IM', 'IE', 'IS', 'HU', 'GR', 'GI', 'DE', 'FR', 'FI', 'FO', 'EE', 'DK', 'CZ', 'CY', 'HR', 'BG', 'BA', 'BE', //No i18N
    'BY', 'AT', 'AD', 'AL', 'AX', 'GG', 'JE', 'XK', 'SJ', 'CS']; //No i18N

let analyticsCookieName = ["landingPageUrl", "VPT", "httpReferrer1", "httpReferrer2", "ad_src", "ad_grp", "utm_source", "utm_medium", "utm_term", "utm_content", "utm_campaign", "utm_expid", "utm_referrer"];
let functionalCookieName = [];
let regexAnalyticCookieName = [];
let regexFuctionalCookieName = [];
let pathname = location.pathname;
let pathPrefix = /\/v2\//.test(pathname) ? "/v2" : "";
let ctogArray = [];
let geoData = JSON.parse(localStorage.getItem("geoData")) || (typeof ipLocationDetailsJson !== 'undefined' ? JSON.parse(ipLocationDetailsJson) : null);
const jsStaticPrefix = new URL(document.currentScript.src).origin;

function isCalifornia() {
    if (typeof CountryCode != "undefined" && typeof RegionName != "undefined") {
        if (CountryCode == "US" && regionNameLocal.toLowerCase() == "california") {
            return true
        } else {
            return false
        }
    } else {
        return false
    }
}

function setDefaults() {
    let CountryCode = geoData.COUNTRY_CODE;
    if (euCountries.indexOf(CountryCode) > -1 || CountryCode == 'BR' || isCalifornia() ) {
        if (localStorage.getItem("zglobal_Acookie_optOut") == "true") {
            localStorage.setItem("zglobal_Acookie_optOut", 2)
        } else {
            if (localStorage.getItem("zglobal_Acookie_optOut") == "false") {
                localStorage.setItem("zglobal_Acookie_optOut", 0)
            } else {
                if (localStorage.getItem("zglobal_Acookie_optOut") == null) {
                    if (isCalifornia() || CountryCode == 'BR' || CountryCode == 'JP') {
                        localStorage.setItem("zglobal_Acookie_optOut", -2)
                    } else {
                        localStorage.setItem("zglobal_Acookie_optOut", -1)
                    }
                }
            }
        }
    }
    else{
        if(localStorage.getItem('zglobal_Acookie_optOut') == null){
            if(isCalifornia()){
                localStorage.setItem('zglobal_Acookie_optOut',-2);
            }
        } else if (localStorage.getItem('zglobal_Acookie_optOut') === "-1" && !(euCountries.indexOf(CountryCode) > -1 || CountryCode == 'BR')) {
            // zglobal_Acookie_optOut incorrectly set so remove it.
            localStorage.removeItem('zglobal_Acookie_optOut');
        }
    }

    var j = localStorage.getItem("zglobal_Acookie_optOut");
    if (j === null || j === "-2") {
        insertTracker();
    }
}

function initCookiePolicy() {
    let timestamp = new Date().getTime();
    setDefaults();
    $("<div>").load(`${pathPrefix}/cookiepolicybanner.html?${timestamp} .zbottom-cookie-container-outer`, onCookieBannerLoad);
    document.dispatchEvent(new Event('cp-init'));
}

if (geoData == null) {
    $(document).on('geo-located', function (event, data) {
        geoData = data;
        initCookiePolicy();
    });
} else {
    initCookiePolicy();
}

$(window).on("resize", function () {
    setCookiePopup()
});

function createCookie(name, value, days) {
    var expires = "";
    if (isNotEmpty(value)) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + encodeURIComponent(value) + expires + "; path=/; SameSite=None; Secure";
    }
}

function insertTracker() {
    let trackerScriptUrl = `${jsStaticPrefix}/js/kankanippu.js?v1.2`;
    if (document.getElementById('tracker-script') === null) { // insert only if not already added
        let trackerScript = document.createElement('script');
        trackerScript.setAttribute('src', trackerScriptUrl);
        trackerScript.setAttribute('id', 'tracker-script');
        document.body.appendChild(trackerScript);
    }
}

function setCookiePopup() {
    zbcMainHt = $(window).height() - ($(".zbottom-cookie-container").outerHeight(true) + 50);
    $(".zbc-main").css("height", zbcMainHt)
}

function onCookieBannerLoad(cookieContainerContent, textStatus, req) {
    if (textStatus === "error") {
        return "";
    }
    $('body').append(cookieContainerContent);
    setCookiePopup();
    if (geoData.COUNTRY_CODE == "BR") {
        $(".zbottom-cookie-container").addClass("ignore-cta-added")
    }
    $(".zbottom-cookie-container").hide();
    $(".zc-overlay, .zc-container-close").on("click", function () {
        $("body").css("overflow", "auto");
        $(".zc-overlay").fadeOut(300);
        $(".zbc-main").fadeOut(200).removeClass("active");
        $.each(ctogArray, function (b, c) {
            $(".ctog-container.ctog-active[optout=" + c + "]").toggleClass("ctog-no")
        });
        ctogArray.length = 0
    });
    $(document).on("keydown", function (b) {
        if (b.key == "Escape") {
            $("body").css("overflow", "auto");
            $(".zc-overlay").fadeOut(300);
            $(".zbc-main").fadeOut(200).removeClass("active");
            $.each(ctogArray, function (c, d) {
                $(".ctog-container.ctog-active[optout=" + d + "]").toggleClass("ctog-no")
            });
            ctogArray.length = 0
        }
    });
    $(".managecookie, .zbc-cta-settings .zbc-cta").on("click", function () {
        if (!$(".zbc-main").hasClass("active")) {
            $("body").css("overflow", "hidden");
            $(".zc-overlay").fadeIn(300);
            $(".zbc-main").fadeIn(200).addClass("active")
        }
    });
    $(".ctog-container.ctog-active").on("click", function () {
        $(this).toggleClass("ctog-no");
        ctogArray.push($(this).attr("optout"))
    });
    $(".zbc-heading h5").on("click", function () {
        $(this).parent().parent().toggleClass("active");
        $(this).parent().siblings(".zbc-desc").slideToggle()
    });
    $(".zbc-popup-inner .zc-type-container:nth-child(2) h5").trigger("click");

    function sendCookieEvent(b) {
        try {
            $zoho.salesiq.visitor.customaction('{"eventCategory":"cookieOptOut","eventAction":"' + b + '","eventLabel":"privacy"}')
        } catch (b) {
        }
    }

    $(document).on("click", ".zbc-cta-se", function () {
        ctogArray.length = 0;
        cookieSetCheck = false;
        $("body").css("overflow", "auto");
        $(".zbc-main,.zbottom-cookie-container,.zc-overlay").fadeOut(200).removeClass("active");
        $(".zc-cookiewidget").css({
            opacity: "1",
            display: "block"
        });
        $(".managecookie").show();
        if ($(".ctog-container.ctog-active").hasClass("ctog-no")) {
            $(".ctog-no").each(function () {
                cookieArrayName = $(this).attr("arrayname");
                regexArrayName = $(this).attr("regexarrayname");
                optOutValue = $(this).attr("optout");
                $.each(window[cookieArrayName], function (i, h) {
                    document.cookie = h + "=; path=/; expires=" + new Date(0).toUTCString();
                    document.cookie = h + "=; path=/; expires=" + new Date(0).toUTCString()
                });
                var b = document.cookie.split(";");
                for (var d = 0; d < b.length; d++) {
                    var e = b[d].indexOf("=");
                    var c = e > -1 ? b[d].substr(0, e) : b[d];
                    $.each(window[regexArrayName], function (f, g) {
                        if (g.test(c)) {
                            document.cookie = c + "=; path=/; expires=" + new Date(0).toUTCString();
                            document.cookie = c + "=; path=/; expires=" + new Date(0).toUTCString()
                        }
                    })
                }
                if (!cookieSetCheck) {
                    if ($(".ctog-no").length == 2) {
                        localStorage.setItem("zglobal_Acookie_optOut", 3);
                        cookieSetCheck = true
                    } else {
                        localStorage.setItem("zglobal_Acookie_optOut", optOutValue)
                    }
                }
            });
            sendCookieEvent("OptOut")
        } else {
            localStorage.setItem("zglobal_Acookie_optOut", 0);
            sendCookieEvent("OptIn");
        }
    });

    $(document).on("click", ".zbc-cta-accept", function () {
        $("body").css("overflow", "auto");
        $(this).fadeOut(300);
        $(".managecookie").show();
        $(".zbc-main,.zbottom-cookie-container,.zc-overlay").fadeOut(200).removeClass("active");
        $(".zc-cookiewidget").css({
            opacity: "1",
            display: "block"
        });
        localStorage.setItem("zglobal_Acookie_optOut", 0);
        sendCookieEvent("OptIn");
        $(".ctog-container.ctog-active").removeClass("ctog-no");
        insertTracker()
        document.dispatchEvent(new Event('cp-optIn'));
    });

    let CountryCode = geoData.COUNTRY_CODE;
    let RegionName = geoData.REGION;
    let regionNameLocal = geoData.CITY;

    var j = localStorage.getItem("zglobal_Acookie_optOut");
    var h = [];
    var d = [];
    if (j == "-1" || j == "-2") {
        if ($.inArray(CountryCode, euCountries) !== -1 || CountryCode == "BR") {
            $(".ctog-container.ctog-active").addClass("ctog-no");
            $(".zbottom-cookie-container").show();
            $(".managecookie").hide();
            h = analyticsCookieName.concat(functionalCookieName);
            d = regexAnalyticCookieName.concat(regexFuctionalCookieName);
        } else {
            if (isCalifornia()) {
                $(".zbottom-cookie-container").show();
                $(".managecookie").hide();
                h = [];
                d = []
            } else {
                $(".zbottom-cookie-container").hide();
                $(".managecookie").show();
                h = [];
                d = [];
                insertTracker();
            }
        }
    } else {
        $(".zbottom-cookie-container").hide();
        if ($.inArray(CountryCode, euCountries) !== -1 || CountryCode == "BR" || isCalifornia()) {
            $(".zc-cookiewidget").css({
                opacity: "1",
                display: "block"
            })
        }
        if (j == "1") {
            $('.ctog-container.ctog-active[optout="1"]').addClass("ctog-no");
            h = functionalCookieName;
            d = regexFuctionalCookieName
        } else {
            if (j == "2") {
                $('.ctog-container.ctog-active[optout="2"]').addClass("ctog-no");
                h = analyticsCookieName;
                d = regexAnalyticCookieName
            } else {
                if (j == "3") {
                    $(".ctog-container.ctog-active").addClass("ctog-no");
                    h = analyticsCookieName.concat(functionalCookieName);
                    d = regexAnalyticCookieName.concat(regexFuctionalCookieName)
                } else {
                    $(".ctog-container.ctog-active").removeClass("ctog-no");
                    h = [];
                    d = []
                }
            }
        }
    }
    if (j != null && j !== "-2") {
        $.each(h, function (k, i) {
            document.cookie = i + "=; path=/; expires=" + new Date(0).toUTCString();
            document.cookie = i + "=; path=/; expires=" + new Date(0).toUTCString()
        });
        var c = document.cookie.split(";");
        for (var f = 0; f < c.length; f++) {
            var g = c[f].indexOf("=");
            var e = g > -1 ? c[f].substr(0, g) : c[f];
            $.each(d, function (i, k) {
                if (k.test(e)) {
                    document.cookie = e + "=; path=/; expires=" + new Date(0).toUTCString();
                    document.cookie = e + "=; path=/; expires=" + new Date(0).toUTCString()
                }
            })
        }
    }
}

$("body").on("click", ".zc-cookiewidget", function () {
    if (!$(".zbc-main").hasClass("active")) {
        $("body").css("overflow", "hidden");
        $(".zc-overlay").fadeIn(300);
        $(".zbc-main").fadeIn(200).addClass("active").addClass("widget-active")
    }
});


//ignorei18n_end