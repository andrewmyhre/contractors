$(document).ready(function () {
    loadHistory();

});

function hookHistoryEditLinks() {
    $('a.edit').click(function (event) {
        var container = $(event.target).parent().parent();

        $(container).load('/workhistory/editplacement/' + $(event.target).attr('id'), function () {
            $('.update-placement').submit(function () {

                var dataString = '{';
                dataString += '"companyName":"' + $(container).find('input[name=companyName]').val() + '",'
                dataString += '"sector":"' + $(container).find('input[name=sector]').val() + '",'
                dataString += '"started":"' + $(container).find('input[name=started]').val() + '",'
                dataString += '"stillThere":"' + $(container).find('input[name=stillThere]').val() + '",'
                dataString += '"finished":"' + $(container).find('input[name=finished]').val() + '",'
                dataString += '"placementType":"' + $(container).find('select[name=placementType]').val() + '",'
                dataString += '"skillSet":"' + $(container).find('input[name=skillSet]').val() + '"'
                dataString += '}';
                var data = JSON.parse(dataString);

                $.post(this.action, data, loadHistory);

                return false;
            });

            $('a.delete-placement').click(function (event) {
                if (confirm("Are you sure you want to delete this placement from your work history?")) {
                    $.ajax({ 
                            type: "DELETE",
                            url: $(event.target).attr('href'),
                            success: loadHistory
                        });
                }
                return false;
            });

        });

        return false;
    });
}

function loadHistory()
{
    $('#history').load('/workhistory/gethistory', hookHistoryEditLinks);
}