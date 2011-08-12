$(document).ready(function () {
    setUpEvents();
});

function setUpEvents()
{
$('.edit').click(function (event) {  switchToEdit(event.target); });
}

function switchToEdit(element) {
    $('.edit').unbind('click');

    $.each($('.edit'),
        function (index, element) {
            var input = $('<input type="text"/>');
            $(input).attr('name', $(element).attr('id'));
            $(input).val($(element).html());
            $(element).html(input);
        });

        var container = $(element).parents('.edit-container');
        if (container.length) {
            container = container[0];

            var parent = $(container).parent();
            var form = $('<form></form');
            $(form).attr('action', $(container).attr('id'));
            $(container).detach();
            $(form).append(container);
            $(form).append('<input type="submit" value="Update" />');
            $(parent).append(form);

            $(form).find('.edit input')[0].select();
            $(form).submit(function () {

                var jsonString = "{";
                $.each($(form).find('.edit input'), function (index, element) {
                    var name = $(element).attr('name');
                    var value = $(element).val();
                    jsonString += "\"" + name + "\": \"" + value + "\","
                });
                jsonString = jsonString.substring(0, jsonString.length - 1);
                jsonString += "}";
                var data = JSON.parse(jsonString);

                $.post(this.action, data, switchToDisplay);

                return false;
            });
        }
}

function switchToDisplay()
{
    $('#basicInformation').load('/profile/basicinformationedit', setUpEvents);
}