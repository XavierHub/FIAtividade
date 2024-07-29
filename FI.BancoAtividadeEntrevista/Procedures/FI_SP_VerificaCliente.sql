CREATE PROCEDURE FI_SP_VerificaCliente
    @CPF VARCHAR(14)  -- Parâmetro de entrada para o CPF a ser verificado
AS
BEGIN
    -- Verifica se o CPF já existe na tabela CLIENTES
    IF EXISTS (
        SELECT 1
        FROM dbo.CLIENTES
        WHERE CPF = @CPF
    )
    BEGIN
        -- Se existir, retorna 1
        SELECT 1;
    END
    ELSE
    BEGIN
        -- Se não existir, retorna 0
        SELECT 0;
    END
END
