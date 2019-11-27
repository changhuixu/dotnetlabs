// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function() {
    var mousePressed = false;
    var lastX, lastY;
    var ctx = document.getElementById("canvas").getContext("2d");

    $("#canvas").mousedown(function(e) {
        mousePressed = true;
        draw(e.pageX - $(this).offset().left, e.pageY - $(this).offset().top, false);
    });

    $("#canvas").mousemove(function(e) {
        if (mousePressed) {
            draw(e.pageX - $(this).offset().left, e.pageY - $(this).offset().top, true);
        }
    });

    $("#canvas").mouseup(function() {
        mousePressed = false;
    });
    $("#canvas").mouseleave(function() {
        mousePressed = false;
    });

    $("#clearArea").click(function() {
        clearArea();
    });

    $("#check").click(function() {
        check();
    });

    function draw(x, y, isDown) {
        if (isDown) {
            ctx.beginPath();
            ctx.strokeStyle = "#FF0000";
            ctx.lineWidth = "32";
            ctx.lineJoin = "round";
            ctx.moveTo(lastX, lastY);
            ctx.lineTo(x, y);
            ctx.closePath();
            ctx.stroke();
        }
        lastX = x; lastY = y;
    }

    function clearArea() {
        ctx.setTransform(1, 0, 0, 1, 0, 0);
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
        $("#prediction").text("?");
        $("#scores").text("");
    }

    function check() {
        var canvas = document.getElementById("canvas");
        $("#prediction").text("?");
        $.ajax({
            type: "POST",
            url: "home/upload",
            data: {
                imgBase64: canvas.toDataURL()
            }
        }).done(function(msg) {
            console.log(msg.pixelValues);
            $("#prediction").text(msg.prediction);
            $("#scores").text(msg.scores);
        });
    }
}());
