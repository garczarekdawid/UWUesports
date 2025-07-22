$(document).ready(function () {
    $('#playerId').select2();
    $('#teamId').select2();
});


$(document).ready(function () {
    // inicjalizacja Select2 dla wielokrotnego wyboru graczy
    $('select[name="playerIds"]').select2({
        placeholder: "Wybierz graczy...",
        allowClear: true,
        width: '100%'
    });
});