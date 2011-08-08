$(document).ready(function () {

    $('#theform').submit(function () {

        var searchString = "";
        if ($('form input[name=name]').val().trim() != "") {
            searchString += " FullName:" + $('form input[name=name]').val().trim();
        }

        if ($('form input[name=company]').val().trim() != "") {
            searchString += " CompanyName:" + $('form input[name=company]').val().trim();
        }

        if ($('form input[name=skills]').val().trim() != "") {
            searchString += " SkillName:" + $('form input[name=skills]').val().trim();
        }


        $.get('/indextest/advancedsearch',
            "q=" + searchString,
            function (response) {
                $('#results').html("<p>" + response.length + " results</p>");
                $.each(response, function (key, result) {
                    var resultHtml = $('#results').append('<div class="result"></div>');
                    $(resultHtml).append("<strong>" + result.Candidate.FullName + "</strong><br/>");
                    $.each(result.Candidate.Skills, function (key, skill) {
                        $(resultHtml).append(skill.SkillName + ", ");
                    });
                    $.each(result.Candidate.WorkHistory, function (key, placement) {
                        $(resultHtml).append(placement.CompanyName + " (" + placement.CompanySectorName + ")" + "<br/>");
                    });

                });
            });

        return false;

    });

});