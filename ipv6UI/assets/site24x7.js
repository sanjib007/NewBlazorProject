//ignorei18n_start
if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}
function isNotEmpty(varName) {
    var response = true;
    if (varName == "" || varName == undefined) {
        response = false;
    }
    return response;
}

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

var countryEu = ['RS', 'VA', 'GB', 'UA', 'CH', 'SE', 'ES', 'SI', 'SK', 'SM', 'RU', 'RO', 'PT', 'PL', 'NO', 'NL', 'ME', 'MC', 'MD', 'MT', 'MK', //No i18N
    'LU', 'LT', 'LI', 'LV', 'IT', 'IM', 'IE', 'IS', 'HU', 'GR', 'GI', 'DE', 'FR', 'FI', 'FO', 'EE', 'DK', 'CZ', 'CY', 'HR', 'BG', 'BA', 'BE', //No i18N
    'BY', 'AT', 'AD', 'AL', 'AX', 'GG', 'JE', 'XK', 'SJ', 'CS']; //No i18N

var headerHeight = $('.header.uheader').height() + $('#mini-panel-product_menu').height();

function triggerGeoData(data) {
    const urlParams = new URLSearchParams(window.location.search);
    let autoCC = urlParams.get('autoCC');
    if (autoCC && autoCC.length === 2) {
        data = {
            'COUNTRY_CODE': autoCC,
            'COUNTRY_NAME': autoCC
        };
    }
    $(document).trigger("geo-located", data);
}

function fetchGeoData() {
    let geoData = null;
    try {
        geoData = JSON.parse(localStorage.getItem("geoData"));
    } catch (e) { }
    if (geoData == null) {
        $.ajax({
            url: '/tools/api/iplocation', //No i18N
            success: function (data) {
                localStorage.setItem("geoData", JSON.stringify(data));
                triggerGeoData(data);
            }
        });
    } else {
        triggerGeoData(geoData);
    }
}

$(function () {
    const homeRegex = /^\/$/;
    const toolsPagesRegex = /\/tools(\/|\.html)|^\/check-website-availability.html|^\/link-explorer.html|^\/web-speed-report.html|^\/lynx-view.html|^\/text-ratio.html|^\/server-header.html|^\/html-validator.html|^\/link-checker.html|^\/code-cleaner.html|^\/dns-lookup.html|^\/port-test.html|^\/check-heartbleed-vulnerability.html|^\/web-page-analyzer.html|^\/find-ip-address-of-web-site.html|^\/trace-route.html|^\/ping-test.html|^\/find-website-location.html|^\/ssl-certificate.html/;
    const subProductRegex = /^\/(cloudspend|statusiq)/;
    const multilingualRegex = /^\/(id|it|ko|nl|pl|sv|th|vi|zh-Hant)\//;
    const communityRegex = /^\/(community|blog)\//;
    const helpRegex = /^\/(help)\//;
    const miscRegex = /^\/(sysadminday|learn)\//;
    const privacyRegex = /^\/privacypolicy\.html/;
    const communityPluginsRegex = /^\/community\/filter\/plugins\//;
    let displayBanner = false;
    let bannerText = "";
    let bannerLink = "";
    let linkText = "";
    let pathname = location.pathname;

    // Top Banner
    $(document).on('geo-located', function (event, data) {
        let countryCode = data.COUNTRY_CODE;
        let city = data.CITY;
        let preferredRegion = false;
        if (countryCode === "US") {
            bannerText = `<a href="https://events.site24x7.com/site24x7-seminar-newyork-2023#/?affl=WAB">
Join us for the Site24x7 seminar, Beyond Monitoring: Leverage AIOps for observability | 6 Oct. in New York | Register Now</a>`;
            showBanner(bannerText);
        }
        if (countryCode === "DE") {
            bannerText = `<a href="https://events.site24x7.com/site24x7-seminar-germany-2023#/?affl=WAB">
Join us for the Site24x7 seminar, Beyond Monitoring: Leverage AIOps for observability | 17 Oct. in Germany | Register Now</a>`;
            showBanner(bannerText);
        }
        if (countryCode === "NL") {
            bannerText = `<a href="https://events.site24x7.com/site24x7-seminar-netherlands-2023#/?affl=WAB">
Join us for the Site24x7 seminar, Beyond Monitoring: Leverage AIOps for observability | 19 Oct. in Netherlands | Register Now</a>`;
            showBanner(bannerText);
        }
    });

    fetchGeoData();

    // Top Banner
    bannerText = "Participate in exciting contests this SysAdmin Day for a chance to win a $50 gift card.";
    linkText = "Join the fun! "
    bannerLink = "/sysadminday/2023/?src=tpbn";
    let bannerCookie2 = readCookie('tmp-announce');
    const ancBannerRuleMatch = ![multilingualRegex, helpRegex, /^\/(sysadminday)\//, privacyRegex, communityPluginsRegex].some(rx => rx.test(pathname));
    /*if (ancBannerRuleMatch && bannerCookie2 != '1') {
        showBanner(bannerText, bannerLink, linkText);
    }*/

    // Floating side banner
    let floatingBannerRegex = /^\/aws-monitoring\.html$/;
    if (floatingBannerRegex.test(pathname)) {
        let cookieName = "banner_floating"; //No i18N
        let bannerCookie = readCookie(cookieName);
        if (bannerCookie !== "1") {
            showFloatingBanner(function (banner) {
                banner.hide();
                createCookie(cookieName, "1", 7);
            });
        }
    }

    // Floating request demo and price quote - not needed in following pages.
    const floatingBtnRuleMatch = ![toolsPagesRegex, subProductRegex, multilingualRegex,
        communityRegex, helpRegex, miscRegex, privacyRegex].some(rx => rx.test(pathname));
    if (floatingBtnRuleMatch) {
        renderFloatingButtons();
    }


    updateLanguageOnLoad()

});

function showBanner(bannerText, bannerLink, linkText, close) {
    let $header = $('.nav-sticky');
    let bannerStyle = `
    <style>
    .tmp-announcement div {
      position: relative;
      line-height: 19px;
    }

    .tmp-announcement div span a {
        background-color: #000;
        color: #fff;
        height: 100%;
        padding: 5px 10px;
        border-radius: 3px;
        font-family: 'ZohoPuviSemBd', sans-serif;
        transition: .2s all ease-in-out;
    }
    
    .tmp-announcement div span a:hover {
        cursor: pointer;
        background-color: #333;
    }
    
    .tmp-announcement button {
      background: none;
      outline: none;
      border: none;
      float: right;
      opacity: .4;
      transition: 0.2s all ease-in-out;
    }
    
    .tmp-announcement button:hover {
      cursor: pointer;
      opacity: .8;
    }
    
    .tmp-announcement a.primary {
        line-height: normal;
        background-color: #000;
        color: #fff;
        padding: 6px 15px;
        margin: 0 30px 0 0;
        text-decoration: none;
        text-transform: uppercase;
        display: inline-block;
        font-size: 15px;
        font-weight: 600;
        border-radius: 5px;
        box-shadow: 0 0 1px #1381b0;
    }
    
    @media screen and (max-width: 768px) {
        .tmp-announcement button {
          position: absolute;
          right: 15px;
          bottom: 0;
          float:none;
        }
    
        .ab-body {
            flex-direction: column;
    }
        
        .register-btn {
            align-self: center;
     } 
     }

     @media screen and (max-width: 990px) {
        .confetti-canvas {
            top: 32px !important;
            height: 36px !important;
        }
     }

     @media screen and (max-width: 670px) {
        .confetti-canvas {
            display: none;
            top: 32px !important;
            height: 61px !important;
        }

        #confetti-canvas-1, #confetti-canvas-2 {
            width: 50% !important;
          }
     }

     #confetti-canvas-1, #confetti-canvas-2 {
        position: absolute;
        top: 0;
        width: 30%;
        z-index: 50;
        pointer-events: none;
      }

      #confetti-canvas-1 {
        left: 0;
      }

      #confetti-canvas-2 {
        right: 0;
      }
  
      #temp-body {
        z-index: 100;
      }
  
      .confetti-canvas {
        position: absolute;
        z-index: 50;
        width: 100%;
        height: 45px;
        overflow: hidden;
        top: 80px;
        pointer-events: none;
      }
    </style>
    `;
    let $banner = $(`
        <div class="tmp-announcement p-2" style="background-color: #ffce26; position: sticky;">
        <div class="mx-auto text-center">
            <span class="mx-auto text-center" style="color:#000; font-size: 14px;">
            <img class="d-none d-lg-inline-block mr-2" style="margin-top: -6px; transform: scaleX(-1);" width="26" 
            src="https://img.site24x7static.com/images/celebration-cone.svg" />
            ${bannerText}
            <a class="d-block d-md-inline-block mt-2 mx-auto m-md-0 ml-md-2" style="font-size: 14px; max-width: max-content;" href="${bannerLink}">${linkText}</a>
            <button class="d-inline-block close mx-auto ml-lg-3 mt-lg-1">&#215;</button>
            </span>
        </div>
        </div>
        <div class="confetti-canvas">
        </div>
    `);


    $(bannerStyle).appendTo('head');
    $banner.insertAfter($header);
    startConfetti();
    let $closeBth = $('.tmp-announcement .close');

    $closeBth.on('click', function (e) { stopConfetti(); $('.tmp-announcement').remove(); createCookie('tmp-announce', "1", 7); });
}

function showFloatingBanner(close) {
    let $header = $('.nav-sticky');
    let bannerStyle = `
        <style>

.floating-promo-container {
    width: 100%;
    max-width: 200px;
    right: 0px;
    top: 45%;
    z-index: 10;
    will-change: transform;
    transform: translateX(500px);
    transition: transform 0.5s ease-in-out;
    }

    .floating-promo-container.show {
      transform: translateX(0px);
    }

.promo-trigger {
  right: 0px;
  top: 0px;
  width: 32px;
  height: 32px;
  z-index: 10;
  cursor: pointer;
}

.trigger-open {
  top: 45%;

  background-color: #000;
  will-change: transform;
  transition: transform 0.5s ease-in-out;
}

.trigger-open.hide {
  transform: translateX(500px);
}

.trigger::after, .trigger::before {
  position: absolute;
  content: '';
  width: 12px;
  height: 3px;
  background: #fff;
  right: 10px;
}
.trigger-open::before {
  top: 10px;
  transform: rotate(-45deg);
}
.trigger-open::after {
  top: 17px;
  transform: rotate(-135deg);
}


.trigger-close::before {
  top: 12px;
  transform: rotate(45deg);
}

.trigger-close::after {
  top: 19px;
  transform: rotate(135deg);
}
.bg-black {
  background-color: #000;
}
        </style>
    `;

    let imgStaticDomain = $('link[rel="SHORTCUT ICON"]').attr('href').split('/images/')[0] + "/";
    imgStaticDomain = imgStaticDomain.indexOf("static") > 0 ? imgStaticDomain : "/"

    var $banner = $(`
        <aside>
        <div>
            <span class="promo-trigger trigger trigger-open position-fixed"></span>
            <div class="floating-promo-container position-fixed bg-black shadow px-3 py-2 text-center rounded-left">
                <span class="promo-trigger trigger trigger-close position-absolute"></span>
                <img class="mb-1" src="${imgStaticDomain}images/register-icon.svg" alt="" height="40" width="40">
                <p class="mb-0 text-white">Things to look forward in 2023 from CloudSpend.</p>
                <a class="d-block mt-1" style="color: #83b633" href="https://blogs.manageengine.com/cloudspend/2023/02/15/6-new-features-coming-to-manageengine-cloudspend-in-2023.html" target="_blank" rel="noopener">Know more</a>
        </div>
      </div>
        </aside>
    `);

    $(bannerStyle).appendTo('head');
    $banner.insertBefore($header);
    var new_script = document.createElement("script");
    var inlineScript = document.createTextNode(`var triggerOpen = document.querySelector('.trigger-open')//No i18N
            var triggerClose = document.querySelector('.trigger-close')//No i18N
            var floatingPromo = document.querySelector('.floating-promo-container')//No i18N
            triggerOpen.addEventListener('click', onViewChange);
            triggerClose.addEventListener('click', onViewChange)
            function onViewChange(evt) {
              floatingPromo.classList.toggle('show');//No i18N
              triggerOpen.classList.toggle('hide')//No i18N
            }
            onViewChange()
            setTimeout(onViewChange,3000);`);
    new_script.appendChild(inlineScript)
    document.body.appendChild(new_script)
    $('.premoclose').on('click', function () {
        createCookie(cookieName, 0);
        $banner.hide();
    });
}

function renderFloatingButtons() {
    const requestDemoText = i18n.requestDemo ?? "Request Demo";
    const getQuoteText = i18n.getQuote ?? "Get Quote";
    let imgStaticDomain = $('link[rel="SHORTCUT ICON"]').attr('href').split('/images/')[0] + "/";
    imgStaticDomain = imgStaticDomain.indexOf("static") > 0 ? imgStaticDomain : "/";

    const lang = $('html').attr('lang');
    const langHome = lang === 'en' ? '/' : `/${lang}/`;

    let buttonHtml = `<div class="floating-buttons shadow d-none">
                <div class="row mx-md-auto">
                    <a href="${langHome}schedule-demo.html?src=flbtn"
                        class="text-center col-6 col-sm-12 mx-auto p-0 pt-2 m-0 request-demo d-none border-sm-0">
                        <img src="${imgStaticDomain}images/request-demo-b1.svg" alt="Request Demo" width="40" height="26">
                        <li class="pt-1 mb-2">${requestDemoText}</li>
                    </a>
                    <a href="${langHome}custom-pricing.html?src=flbtn"
                        class="text-center col-6 col-sm-12 mx-auto p-0 pt-2 m-0 request-quote d-none">
                        <img src="${imgStaticDomain}images/request-quote-b1.svg" alt="Get Quote" width="40" height="33">
                        <li class="pt-1">${getQuoteText}</li>
                    </a>
                </div>
            </div>`;

    $('body').append(buttonHtml);
    // Floating button handler
    let floatingBtn = document.querySelector(".floating-buttons")
    let requestDemoBtn = document.querySelector(".floating-buttons .request-demo");
    let requestQuoteBtn = document.querySelector(".floating-buttons .request-quote");

    window.addEventListener('scroll', function (e) {
        if (window.pageYOffset >= 50) {
            requestDemoBtn.classList.remove("d-none")
            requestQuoteBtn.classList.remove("d-none")
            floatingBtn.classList.remove("d-none")
        } else {
            requestDemoBtn.classList.add("d-none")
            requestQuoteBtn.classList.add("d-none")
            floatingBtn.classList.add("d-none")


        }

    })

}

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

(function ($) {
    /*$('.mobile-menu-icon1').click(function () {
        $(this).toggleClass('active');
        $('.zhamburger').toggleClass('active');
        $('.zmobile-menu-new').toggleClass('active');
        $('.zmobile-menu-new').toggle();
        $('.zmobile-menu-new-inner').toggle();
        $('.zmobile-menu-new-inner').toggleClass('zshow');
    });*/

    // var toggleMyDropdown = function(){
    //     $(this).find('.dropdown-toggle').dropdown('toggle');
    // };
    // $('#myDropdown, #myDropdown-2').hover(toggleMyDropdown, toggleMyDropdown);
    // $('#myDropdown, #myDropdown-2').on('shown.bs.dropdown', function () {
    //     $('.backdrop').show();
    //     // $('body').css("overflow-y","hidden");
    // });
    // $('#myDropdown, #myDropdown-2').on('hide.bs.dropdown', function () {
    //     $('.backdrop').hide();
    //     // $('body').css("overflow-y","auto");
    // });

    // showBanner();

    $('#footer a').on('click', function () {
        var ea = $(this).attr('href');
        ea = ea.replace(/\.\.\//g, '');
        var el = $(this).closest("ul").attr('data-ft');
        try { $zoho.salesiq.visitor.customaction("{'eventCategory':'Footer LinkClick','eventAction':'" + ea + "','eventLabel':'" + el + "','customID':'-'}"); } catch (exp) { }
    });
    $('.navbar a').on('click', function () {
        var anchorLink = $(this).attr('href');
        if ((anchorLink !== "#") && (typeof (anchorLink) !== "undefined")) {
            anchorLink = anchorLink.replace(/\.\.\//g, '')
            try { $zoho.salesiq.visitor.customaction("{'eventCategory':'Global Navigation','eventAction':'" + anchorLink + "','customID':'-'}"); } catch (exp) { }
        }
    });

    // Log if ztm is blocked
    setTimeout(function () {
        let ztmBlocked = localStorage.getItem("ztmBlocked");
        if (ztmBlocked === null && (typeof ZTMData === "undefined")) {
            localStorage.setItem("ztmBlocked", "1");
            $.get("/tools/marketing/_ln?method=ztmBlocked", function (data, status) {
                // do nothing
            });
        }
    }, 2000);

})(jQuery);

$(".search-btn").on('click', function () {
    $(".search-wrap").addClass("search-visible");
    $('#search-input').attr("tabindex", "0").focus();
});

$(".close-btn").on('click', function () {
    $(".search-wrap").removeClass("search-visible");
    $('#search-input').attr("tabindex", "-1")
});

$("#mobile-search").on('click', function (e) {
    e.stopPropagation();
    if ($("#navbarNavAltMarkup").hasClass("show")) {
        $("#navbarNavAltMarkup").collapse('hide')
    }
    $(".s247-search-container").addClass("search-active");
    $(".overlay").addClass("overlay-active");
    $("#search-query").focus();

});

$(document).on('keydown', function (event) {
    if (event.keyCode == 27) {
        $('.s247-search-container').removeClass("search-active");
    }
});

$(document).on('click', function (e) {
    $target = $(e.target);
    if ($target != null && !$target.closest('.global-search-icon').length && !$target.closest('.s247-search-container').length &&
        $('.s247-search-container').is(":visible")) {
        $('.s247-search-container').removeClass("search-active");
        $(".overlay").removeClass("overlay-active");
    }
});

$('#menuDropdown').on('shown.bs.dropdown', function () {
    $screensize = $(window).width();
    if ($screensize < 768) {
        $('#navbarNavAltMarkup').addClass('max-height overflow-auto');
    }
})
$('#menuDropdown').on('hidden.bs.dropdown', function () {
    $screensize = $(window).width();
    if ($screensize < 768) {
        $('#navbarNavAltMarkup').removeClass('max-height overflow-auto');
    }
})
$('#navbarNavAltMarkup').on('show.bs.collapse', function () {
    $('.nav-toggler-menu').addClass('open');
});
$('#navbarNavAltMarkup').on('hide.bs.collapse', function () {
    $('.nav-toggler-menu').removeClass('open');
});


function toggleDropdown(e) {
    const _d = $(e.target).closest('.dropdown'),
        _m = $('.dropdown-menu', _d);
    setTimeout(function () {
        const shouldOpen = e.type !== 'click' && _d.is(':hover');//No i18N
        _m.toggleClass('show', shouldOpen);//No i18N
        _d.toggleClass('show', shouldOpen);//No i18N
        $('[data-toggle="dropdown"]', _d).attr('aria-expanded', shouldOpen);//No i18N
    }, e.type === 'mouseleave' ? 100 : 0);
}

$('body').on('mouseenter mouseleave', '.dropdown', toggleDropdown)



var footer_promo =
{
    "heading": "How to overcome challenges in distributed environments with tracing",//No i18N
    "description": "Discover how Site24x7's distributed tracing feature helps you navigate and overcome various challenges in your distributed architecture and harness more powerful observability data.",//No i18N
    "date": "October 11",//No i18N
    "zone1": {//No i18N
        "time": "10:00 am EDT",//No i18N
        "url": "https://meet.zoho.com/pe237EiX9D"//No i18N
    },
    "zone2": {//No i18N
        "time": "06:00 am BST",//No i18N
        "url": "https://meet.zoho.com/JUOS38VlKA"//No i18N
    }
}

var promoCards = document.querySelectorAll(".footer-promo-card")//No i18N
promoCards.forEach(function myFunction(item, index) {
    item.classList.add('p-2', 'bg-white', 'rounded');//No i18N
    item.querySelector(".webinar-title").innerHTML = footer_promo.heading;//No i18N
    item.querySelector(".webinar-desc").innerHTML = footer_promo.description;//No i18N
    item.querySelector(".webinar-date").innerHTML = footer_promo.date;//No i18N
    item.querySelector(".webinar-reg-link-1").innerHTML = footer_promo.zone1.time;//No i18N
    item.querySelector(".webinar-reg-link-1").href = footer_promo.zone1.url;//No i18N
    item.querySelector(".webinar-reg-link-2").innerHTML = footer_promo.zone2.time;//No i18N
    item.querySelector(".webinar-reg-link-2").href = footer_promo.zone2.url;//No i18N
    //item.querySelector(".webinar-reg-link-3").innerHTML = footer_promo.zone3.time;//No i18N
    //item.querySelector(".webinar-reg-link-3").href = footer_promo.zone3.url;//No i18N

});

$(".footer-toggle").on('click', function () {
    $screensize = $(window).width();
    if ($screensize < 768) {
        $(this).toggleClass("active");
        $(this).parent().find("ul").slideToggle('medium');//No i18N
    }
});


$(".scroll-animate").click(function (event) {
    $('html,body').animate({
        scrollTop: $(event.currentTarget.getAttribute('data-href')).offset().top - 60
    }, //Added "-60" to minus the height  of the fixed navbar
        'slow'); //No i18N
});

function updateLanguageOnLoad() {
    let languagesOptions = document.querySelectorAll('#select-language option')
    let languages = [];

    for (let i = 1; i < languagesOptions.length; i++) {
        languages.push(languagesOptions[i].getAttribute('value'))
    }

    languages.some((lang, index) => {
        if (window.location.href.includes(lang)) {
            languagesOptions[index + 1].setAttribute('selected', '') // index + 1 because we are ignoring the default value
            return true;
        }
    })
}

//ignorei18n_end
