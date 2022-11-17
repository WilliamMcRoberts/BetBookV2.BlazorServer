
var prevScrollPos = window.pageYOffset;

window.onscroll = () => {
   checkBoxes()
    adjustHeader()
    drawleft()
    drawright()
}

//window.addEventListener('scroll', drawleft, false);

//window.addEventListener('scroll', drawright, false);

function drawleft() {
    var svg = document.getElementById("svg-div-left");
    svg.style.display = 'block';

    var footballLeft = document.getElementById("path-left");
    var length = footballLeft.getTotalLength();

    footballLeft.style.stroke = '#fff';

    footballLeft.style.strokeDasharray = length;

    footballLeft.style.strokeDashoffset = length;

    var scrollpercent = (document.body.scrollTop + document.documentElement.scrollTop) / (document.documentElement.scrollHeight - document.documentElement.clientHeight);

    var draw = length * scrollpercent;

    footballLeft.style.strokeDashoffset = length - draw;
}

function drawright() {
    var svg = document.getElementById("svg-div-right");
    svg.style.display = 'block';

    var footballRight = document.getElementById("path-right");
    var length = footballRight.getTotalLength();

    footballRight.style.stroke = '#fff';

    footballRight.style.strokeDasharray = length;

    footballRight.style.strokeDashoffset = length;

    var scrollpercent = (document.body.scrollTop + document.documentElement.scrollTop) / (document.documentElement.scrollHeight - document.documentElement.clientHeight);

    var draw = length * scrollpercent;

    footballRight.style.strokeDashoffset = length - draw;
}

function adjustHeader() {
    var currentScrollPos = window.pageYOffset;
    if (prevScrollPos <= currentScrollPos) {
        document.getElementById("header").style.top = "-100px";
        prevScrollPos = currentScrollPos;
        return;
    }
    document.getElementById("header").style.top = "0";
    prevScrollPos = currentScrollPos;
}

function checkBoxes() {

    const boxes = document.querySelectorAll('.game-container')
    
    const triggerBottom = window.innerHeight / 5 * 4

    boxes.forEach(box => {
        
        const boxTop = box.getBoundingClientRect().top

        if (boxTop < triggerBottom) {
            box.classList.add('show')
            return
        }
        box.classList.remove('show')

    })
}