$(document).ready(function () {
    $('.cpf').mask('000.000.000-00', { reverse: true });
    $(document).ajaxStart(function () {
        $("#loading").show();
    }).ajaxStop(function () {
        $("#loading").hide();
    });
})



function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var modalHtml = `
        <div id="${random}" class="modal fade" tabindex="-1" aria-labelledby="${random}Label" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="${random}Label">${titulo}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>${texto}</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                    </div>
                </div>
            </div>
        </div>`;

    $('body').append(modalHtml);
    var modalElement = document.getElementById(random);
    var modalInstance = new bootstrap.Modal(modalElement);
    modalInstance.show();
}