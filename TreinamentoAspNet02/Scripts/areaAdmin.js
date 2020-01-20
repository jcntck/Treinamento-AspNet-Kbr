$(document).ready(function () {
    $('#consultores, #atendimentos').DataTable({
        "order": [],
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.10.20/i18n/Portuguese-Brasil.json"
        },
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]]
    });
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
})