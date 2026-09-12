-- =========================================================
-- PetCareAI - CLYVO VET
-- Script de criação e carga do banco relacional (Azure SQL)
-- Tabelas do CORE: Pets e Consultas (relacionadas entre si)
-- =========================================================

CREATE TABLE Pets (
    Id INT IDENTITY(1,1) PRIMARY KEY,          -- chave primária
    Nome NVARCHAR(100) NOT NULL,               -- nome do pet
    Especie NVARCHAR(50) NOT NULL,             -- ex: Cachorro, Gato
    Raca NVARCHAR(80) NOT NULL,                -- raça do pet
    Idade INT NOT NULL,                        -- idade em anos
    Peso FLOAT NOT NULL,                       -- peso em kg
    UsuarioId INT NOT NULL                     -- referência ao tutor (fora do escopo desta entrega)
);

CREATE TABLE Consultas (
    Id INT IDENTITY(1,1) PRIMARY KEY,          -- chave primária
    PetId INT NOT NULL,                        -- chave estrangeira para Pets
    DataConsulta DATETIME2 NOT NULL,           -- data/hora da consulta
    Veterinario NVARCHAR(120) NOT NULL,        -- nome do veterinário responsável
    Motivo NVARCHAR(200) NOT NULL,             -- motivo da consulta
    Diagnostico NVARCHAR(400) NOT NULL,        -- diagnóstico/observações
    CONSTRAINT FK_Consultas_Pets FOREIGN KEY (PetId)
        REFERENCES Pets(Id)
        ON DELETE CASCADE
);

-- =========================================================
-- Carga de dados significativos (mínimo 2 registros por tabela)
-- =========================================================

INSERT INTO Pets (Nome, Especie, Raca, Idade, Peso, UsuarioId) VALUES
('Thor', 'Cachorro', 'Golden Retriever', 3, 28.5, 1),
('Mimi', 'Gato', 'Siamês', 2, 4.2, 2);

INSERT INTO Consultas (PetId, DataConsulta, Veterinario, Motivo, Diagnostico) VALUES
(1, '2026-08-20T09:30:00', 'Dra. Carla Souza', 'Consulta de rotina', 'Animal saudável, vacinação em dia'),
(2, '2026-09-01T14:00:00', 'Dr. Marcos Lima', 'Vômito e apatia', 'Gastroenterite leve, prescrita dieta e observação');
