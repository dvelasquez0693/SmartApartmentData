let dropdown = $('#markets');
dropdown.empty();
dropdown.append('<option selected="true" disabled>Choose Market</option>');
dropdown.prop('selectedIndex', 0);

const url = window.location.origin + '/StaticFiles/Json/Markets.json';

// Populate dropdown with list of markets
$.getJSON(url, function (data) {

    $.each(data, function () {
        $("#markets-dropdown").append($("<option />").val(this.market).text(this.market));
    });

});
