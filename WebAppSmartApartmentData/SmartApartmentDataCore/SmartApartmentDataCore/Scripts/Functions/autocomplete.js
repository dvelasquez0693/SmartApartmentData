$(document).ready(function () {
    $("#search").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "GET",
                url: '/Properties',                
                dataType: "json",
                data: { queryParameter: request.term, filter: $('#markets-dropdown').val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.name, value: item.name };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
})