var date = new Date();
var dialog = null;

$(document).ready(function () {
    $('#prev').click(function () {
        date = addMonths(date, -1);
        $('#cal').load('/candidates/cal?month=' + (date.getMonth() + 1) + '&year=' + date.getFullYear());
        showMonth();
        return false;
    });
    $('#next').click(function () {
        date = addMonths(date, 1);
        $('#cal').load('/candidates/cal?month=' + (date.getMonth() + 1) + '&year=' + date.getFullYear());
        showMonth();
        return false;
    });

    dialog = $('<div></div>').dialog({ autoOpen: false, width: 700, resizable:false });

    showMonth();
});

function showMonth() {
    $('#cal').load('/candidates/cal',
        function () {
            $('.candidate-name').mouseenter(function (event) {
                var url = '/candidates/Details/' + $(event.target).attr('id') + '?view=tiny';
                var pos = $(event.target).offset();
                var width = $(event.target).width();

                dialog.dialog({ title:$(event.target).html(),  position: [pos.left + width, pos.top]});
                dialog.load(url);
                dialog.dialog('open');
            });
        });

    var prevMonth = addMonths(date, -1);
    var nextMonth = addMonths(date, 1);

    $.get('/candidates/candidatecount?month=' + (prevMonth.getMonth() + 1) + '&year=' + prevMonth.getFullYear(),
        function (response) {
            $('#prev').html(prevMonth.format('mmmm') + ' (' + response + ')');
        });

    $.get('/candidates/candidatecount?month=' + (nextMonth.getMonth() + 1) + '&year=' + nextMonth.getFullYear(),
        function (response) {
            $('#next').html(nextMonth.format('mmmm') + ' (' + response + ')');
        });
}

function addMonths(date, monthsToAdd) {
    var month = date.getMonth();
    var year = date.getFullYear();

    if (monthsToAdd > 0) {
        while (monthsToAdd > 12) {
            year++;
            monthsToAdd -= 12;
        }
    } else if (monthsToAdd < 0) {
        while (monthsToAdd < -12) {
            year--;
            monthsToAdd += 12;
        }
    }

    month += monthsToAdd;

    return new Date(year, month, date.getDay(), 0, 0, 0, 0);
}
