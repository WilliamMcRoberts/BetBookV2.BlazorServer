
/* When the user scrolls down, hide the navbar. When the user scrolls up, show the navbar */
var prevScrollPos = window.pageYOffset;



window.onscroll = () => {
    var currentScrollPos = window.pageYOffset;
    if (prevScrollPos <= currentScrollPos) {
        document.getElementById("header").style.top = "-100px";
        prevScrollPos = currentScrollPos;
        return;
    }
    document.getElementById("header").style.top = "0";
    prevScrollPos = currentScrollPos;
}

//window.addEventListener('scroll', setProp, false);

//function setProp() {
//    document.body.style.setProperty(
//        '--scroll', window.pageYOffset / (document.body.offsetHeight - window.innerHeight)
//    );
//}

window.addEventListener('scroll', drawleft, false);

function drawleft() {

    var footballLeft = document.getElementById("path-left");
    var length = footballLeft.getTotalLength();

    footballLeft.style.stroke = '#fff';

    footballLeft.style.strokeDasharray = length;

    footballLeft.style.strokeDashoffset = length;

    var scrollpercent = (document.body.scrollTop + document.documentElement.scrollTop) / (document.documentElement.scrollHeight - document.documentElement.clientHeight);

    var draw = length * scrollpercent;

    footballLeft.style.strokeDashoffset = length - draw;
}

window.addEventListener('scroll', drawright, false);

function drawright() {

    var footballRight = document.getElementById("path-right");
    var length = footballRight.getTotalLength();

    footballRight.style.stroke = '#fff';

    footballRight.style.strokeDasharray = length;

    footballRight.style.strokeDashoffset = length;

    var scrollpercent = (document.body.scrollTop + document.documentElement.scrollTop) / (document.documentElement.scrollHeight - document.documentElement.clientHeight);

    var draw = length * scrollpercent;

    footballRight.style.strokeDashoffset = length - draw;
}