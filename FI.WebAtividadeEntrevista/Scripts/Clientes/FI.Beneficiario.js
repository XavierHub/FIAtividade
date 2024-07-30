$(document).ready(function () {    
    var editIndex = -1; // Índice do beneficiário que está sendo editado

    // Função para carregar beneficiários da API
    function carregarBeneficiarios() {
        $.ajax({
            url: urlBeneficiarios, // URL da action Listar
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                $('#beneficiariosTableBody').empty(); // Limpar a tabela antes de carregar os dados
                beneficiarios = data; // Atualizar o array de beneficiários

                // Adicionar os beneficiários na tabela
                data.forEach(function (beneficiario, index) {
                    var newRow = `
                        <tr data-id="${beneficiario.Id}">
                            <td class="d-none">${beneficiario.Id}</td> <!-- Campo escondido para o Id -->
                            <td>${beneficiario.CPF}</td>
                            <td>${beneficiario.Nome}</td>
                            <td>
                                <button class="btn btn-primary btn-edit">Alterar</button>
                                <button class="btn btn-primary btn-delete">Excluir</button>
                            </td>
                        </tr>
                    `;
                    $('#beneficiariosTableBody').append(newRow);
                });

                // Definir o foco no campo CPFBeneficiario
                $('#CPFBeneficiario').focus();
            },
            error: function (xhr, status, error) {
                console.error("Erro ao carregar beneficiários: ", xhr, status, error);
            }
        });
    }

    $('#btnBeneficiarios').on('click', function () {
        var myModal = new bootstrap.Modal(document.getElementById('beneficiariosModal'));
        myModal.show();
        // Definir o foco no campo CPFBeneficiario
        $('#CPFBeneficiario').focus();
    });

    $('#formBeneficiario').submit(function (event) {
        event.preventDefault(); // Prevenir a submissão do formulário

        if (!$(this).valid()) return;

        // Capturar os valores dos campos
        var cpf = $('#CPFBeneficiario').val().trim();
        var nome = $('#NomeBeneficiario').val().trim();

        // Validar os campos (simples)
        if (cpf === "" || nome === "") {
            alert("Por favor, preencha todos os campos.");
            return;
        }

        // Verificar se o CPF já existe na tabela
        const cpfExists = beneficiarios.some(b => b.cpf === cpf);

        // Não permitir incluir 2 beneficiários com o mesmo CPF
        if (editIndex === -1 && cpfExists) {
            alert(`O CPF ${cpf} já está na tabela`);
            return;
        }

        // Não permitir alterar o CPF para manter 2 beneficiários com o mesmo CPF na tabela
        if (editIndex > -1) {
            const cpfIndex = beneficiarios.findIndex(b => b.cpf === cpf);
            if (cpfExists && cpfIndex !== editIndex) {
                alert(`O CPF ${cpf} já está na tabela`);
                return;
            }
        }

        // Verificar se estamos editando um beneficiário existente
        if (editIndex > -1) {
            // Atualizar o beneficiário existente
            var id = $('#beneficiariosTableBody tr:eq(' + editIndex + ')').find('td:eq(0)').text();
            beneficiarios[editIndex] = { id, cpf, nome };
            $(`#beneficiariosTableBody tr:eq(${editIndex}) td:eq(1)`).text(cpf);
            $(`#beneficiariosTableBody tr:eq(${editIndex}) td:eq(2)`).text(nome);
            $('#btnBeneficiarioIncluir').text('Incluir');
            editIndex = -1; // Resetar o índice de edição
        } else {
            // Criar uma nova linha da tabela
            var newRow = `
                <tr data-id="">
                    <td class="d-none"></td> <!-- Campo escondido para o Id -->
                    <td>${cpf}</td>
                    <td>${nome}</td>
                    <td>
                        <button class="btn btn-primary btn-edit">Alterar</button>
                        <button class="btn btn-primary btn-delete">Excluir</button>
                    </td>
                </tr>
                `;

            // Adicionar a nova linha na tabela
            $('#beneficiariosTableBody').append(newRow);
            beneficiarios.push({ id: 0, cpf, nome });
        }

        // Limpar os campos do formulário
        $('#CPFBeneficiario').val('');
        $('#NomeBeneficiario').val('');

        // Definir o foco no campo CPFBeneficiario
        $('#CPFBeneficiario').focus();
    });

    // Evento para o botão "Alterar"
    $('#beneficiariosTableBody').on('click', '.btn-edit', function () {
        var $row = $(this).closest('tr');
        var cpf = $row.find('td:eq(1)').text();
        var nome = $row.find('td:eq(2)').text();
        var id = $row.find('td:eq(0)').text(); // Recuperar o Id da célula escondida

        // Preencher os campos do formulário com os dados do beneficiário
        $('#CPFBeneficiario').val(cpf);
        $('#NomeBeneficiario').val(nome);
        $('#btnBeneficiarioIncluir').text('Alterar');

        // Atualizar o índice de edição
        editIndex = $row.index();

        // Definir o foco no campo CPFBeneficiario
        $('#CPFBeneficiario').focus();
    });

    // Evento para o botão "Excluir"
    $('#beneficiariosTableBody').on('click', '.btn-delete', function () {
        var $row = $(this).closest('tr');
        var index = $row.index();

        // Remover o beneficiário do array
        beneficiarios.splice(index, 1);

        // Remover a linha da tabela
        $row.remove();
    });

    // Carregar beneficiários quando a tela carregar
    carregarBeneficiarios();

    // Definir o foco no campo CPFBeneficiario
    $('#CPFBeneficiario').focus();
});
