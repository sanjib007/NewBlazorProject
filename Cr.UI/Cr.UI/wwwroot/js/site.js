
function modalClose() {
    document.getElementById('modal').classList.add('hidden');
}
function modalOpen() {
    document.getElementById('modal').classList.remove('hidden');
}

function singleModalClose() {
    document.getElementById('singleModal').classList.add('hidden');
}
function singleModalOpen() {
    document.getElementById('singleModal').classList.remove('hidden');
}

window.onclick = function (event) {
    var isChild = event.target.classList.contains("testClass");
    if (!isChild && btn != "") {
        removedClassList(btn);
        notiryClose(btn.id);
        btn = "";
    }
    if (isShow) {
        ChangeProfileBtnClass();
        isShow = false;
    }
}


var btn = "";
function showNotificationBtn() {
    if (btn == "") {
        btn = document.getElementById('showNotify');
        addClassList(btn);
        notifyOpen(btn.id);
    }  
}

var isShow = false;
function ChangeProfileBtnClass() {
    isShow = document.getElementById("showHide");
    if (isShow) {
        var getChildClass = isShow.classList.contains("hidden");
        if (getChildClass) {
            notifyOpen(isShow.id)
        } else {
            notiryClose(isShow.id)
            isShow = false;
        }
    }
}

function notiryClose(element) {
    document.getElementById(element).classList.remove('block');
    document.getElementById(element).classList.add('hidden');
}
function notifyOpen(element) {
    document.getElementById(element).classList.add('block');
    document.getElementById(element).classList.remove('hidden');
}

const addClassList = (element) => {
    Object.values(element.children).forEach((e) => {
        e.classList.add('testClass');
        if (e.children.length > 0) addClassList(e);
    });
};

const removedClassList = (element) => {
    Object.values(element.children).forEach((e) => {
        e.classList.remove('testClass');
        if (e.children.length > 0) removedClassList(e);
    });
};



function initializeInactivityTimer(dotnetHelper) {
    var timer;
    //the timer will be reset whenever the user clicks the mouse or presses the keyboard
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 18000000); //600,000 milliseconds = 10 minuts
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("LogoutFromJS");
    }

}




// side-dashboard-menu-collapse-on-off

//document.querySelectorAll('.cus_collapse').forEach(element => {
//    element.addEventListener('click', () => {
//        document.querySelector('.cus_drawer').classList.toggle('w-[40px]')
//        document.querySelector('.cus_drawer').classList.toggle('md:w-[40px]')
//        document.querySelector('.cus_drawer').classList.toggle('lg:w-[48px]')
//        document.getElementsByClassName('cus_collapse')[1].classList.toggle('w-[40px]')
//        document.getElementsByClassName('cus_collapse')[1].classList.toggle('md:w-[40px]')
//        document.getElementsByClassName('cus_collapse')[1].classList.toggle('lg:w-auto')
//        document.querySelectorAll('.aside_extra').forEach(item => {
//            item.classList.toggle('hidden');
//        })
//    })
//})


//// submenu open-close

//document.getElementById('sub_menu').addEventListener('click', () => {
//    document.querySelector('.sub_extra').classList.toggle('lg:h-[17.72rem]');
//    document.querySelector('.sub_extra').classList.toggle('h-[15.22rem]');
//    document.querySelector('.sub_extra').classList.toggle('h-0');
//    // hidden
//})

//document.getElementById('dash_click').addEventListener('click', () => {
//    document.querySelector('.sub_extra').classList.remove('lg:h-[17.72rem]');
//    document.querySelector('.sub_extra').classList.remove('h-[15.22rem]');
//    document.querySelector('.sub_extra').classList.add('h-0');
//    // hidden
//})


// active submenu color

// document.querySelectorAll(".sub_list_item").forEach((item) => {
//     item.addEventListener("click", () => {
//         activeSubList(item);
//     });
// });

// function activeSubList(item) {

//     document.querySelector(".bg-active_bg")?.classList.remove("bg-active_bg");
//     document.querySelector(".text-my_blue")?.classList.remove("text-my_blue");
//     document.querySelector(".active-side-sub-menu")?.classList.remove("active-side-sub-menu");

//     item.classList.add("bg-active_bg");
//     item.classList.add("text-my_blue");
//     item.classList.add("active-side-sub-menu");

// }


// active main-side-menu color

// document.querySelectorAll(".list_item").forEach((item) => {
//     item.addEventListener("click", () => {
//         activeList(item);
//     });
// });

// function activeList(item) {

//     document.querySelector(".bg-active_bg")?.classList.remove("bg-active_bg");
//     document.querySelector(".text-my_blue")?.classList.remove("text-my_blue");
//     document.querySelector(".active-side-menu")?.classList.remove("active-side-menu");

//     item.classList.add("bg-active_bg");
//     item.classList.add("text-my_blue");
//     item.classList.add("active-side-menu");
// }


