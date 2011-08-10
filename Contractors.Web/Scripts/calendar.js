var date = new Date();
var dialog = null;

$(document).ready(function () {
    parseDateFromQueryString(window.location.hash);

    dialog = $('<div></div>').dialog({ autoOpen: false, width: 700, resizable: false });

    showMonth(date.getMonth() + 1, date.getFullYear());

    setUpYearOptionEditor();

    $('#months li a').click(function (event) {
        $('#months li a').removeClass('selected');
        var month =
            $(event.target).parentsUntil('ul', 'li').index() - 1; // first LI is the year selector
        showMonth(month + 1, date.getFullYear());
        $(event.target).addClass('selected');
        return false;
    });
});

function setUpYearOptionEditor() {
    $('#yearoption').click(function () {
        var yearForm = $('<form method="get"></form>');
        var editor = $('<input type="text" name="yearEdit" value="' + $('#yearoption').html() + '" />');
        yearForm.html(editor);
        $('#yearoption').html(yearForm);
        $(editor).select();

        yearForm.submit(function () {
            var year = $(editor).val();
            showMonth(date.getMonth() + 1, year);
            $('#yearoption').html(year);
            return false;
        });
    });
}

function getCountsForYear(year) {
    $.get('/candidates/CandidateCountForYear?year='+year,
        function (response) {
            var months = $('#months li a .count');
            for (var i = 0; i < 12; i++)
                $(months[i]).html('(' + response[i] + ')');
        });
}

function parseDateFromQueryString(queryString) {
    if (queryString[0] == '?' || queryString[0] == '#')
        queryString = queryString.substring(1);
    var vars = queryString.split('&');
    var month = date.getMonth()+1;
    var year = date.getFullYear();
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (pair[0] == 'month')
            month = parseInt(pair[1]);
        else if (pair[0] == 'year')
            year = parseInt(pair[1]);
    }

    date = new Date(year, month-1, 1);
}

function showMonth(month, year) {
    date = new Date(year, month - 1, 1);
    dialog.dialog('close');
    $($('#months li a')[date.getMonth()]).addClass('selected');
    $('#yearoption').html(year);
    $('#cal').load('/candidates/cal?month='+month+'&year='+year,
        function () {
            $('.candidate-name').mouseenter(function (event) {
                var url = '/candidates/Details/' + $(event.target).attr('id') + '?view=tiny';
                var pos = $(event.target).offset();
                var width = $(event.target).width();

                dialog.dialog({ title: $(event.target).html(), position: [pos.left + width, pos.top] });
                dialog.load(url);
                dialog.dialog('open');
            });

            getCountsForYear(year);
        });

    var prevMonth = addMonths(date, -1);
    var nextMonth = addMonths(date, 1);

    window.location.hash = 'month=' + month + '&year=' + year;
    $('#month-heading').html(date.format('mmmm'));
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
