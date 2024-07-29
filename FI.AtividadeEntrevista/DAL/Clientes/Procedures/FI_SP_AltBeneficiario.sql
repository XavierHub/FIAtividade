create or alter PROC FI_SP_AltBeneficiario
    @Id            BIGINT,
	@NOME          VARCHAR (255),    
	@CPF           VARCHAR (14),
	@IDCLIENTE     BIGINT
AS
BEGIN
	UPDATE BENEFICIARIOS 
	SET 
		NOME = @NOME,
		CPF = CPF,
		IDCLIENTE = @IDCLIENTE
	WHERE Id = @Id
END