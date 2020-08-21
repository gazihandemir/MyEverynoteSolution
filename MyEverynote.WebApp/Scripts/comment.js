
var modalCommentBodyId = "#modal_comment_body";
var noteid = -1;
$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");
        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    });
});

function doComment(btn, e, commentid, spanid) {
    var button = $(btn);
    var mode = button.data("edit-mode");
    if (e == 'edit_clicked') {
        if (!mode) {
            button.data("edit-mode", true);
            // button.removeClass("btn-warning");
            // button.addClass("btn-success");
            var btnSpan = button.find("i");
            btnSpan.removeClass("fa-edit");
            btnSpan.addClass("fa-check");
            btnSpan.removeClass("text-warning");
            btnSpan.addClass("text-success");

            $(spanid).focus();
            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
        } else {
            button.data("edit-mode", false);
            //button.removeClass("btn-success");
           // button.addClass("btn-warning");
            var btnSpan = button.find("i");
            btnSpan.removeClass("fa-check");
            btnSpan.addClass("fa-edit");
            btnSpan.removeClass("text-success");
            btnSpan.addClass("text-warning");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);
            var txt = $(spanid).text();
            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {
                if (data.result) {
                    // yorumlar partial tekrar yüklenir..
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
                } else {
                    alert("Yorum Güncellenemedi.");
                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }
    }
    else if (e == "delete_clicked") {
        var dialog_res = confirm("Yorum silinsin mi ?");
        if (!dialog_res) return false;
        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid,
        }).done(function (data) {
            if (data.result) {
                // yorumlar partial tekrar yüklenir.
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("yorum silinemedi.");
            }
        }).fail(function () {
            alert("sunucu ile bağlantı kurulamadı");
        });
    }
    else if (e == 'new_clicked') {
        var txt = $("#new_comment_text").val();
        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { text: txt, "noteid": noteid }
        }).done(function (data) {
            if (data.result) {
                // yorumlar partial tekrar yüklenir.
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("yorum eklenemedi.");
            }
        }).fail(function () {
            alert("sunucu ile bağlantı kurulamadı");
        });
    }
}