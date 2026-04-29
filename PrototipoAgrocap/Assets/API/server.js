// Importação das bibliotecas necessárias
const express = require("express");
const mysql = require("mysql2/promise");
const cors = require("cors");

// Inicialização da aplicação
const app = express();

// Habilita requisições externas (Unity para API)
app.use(cors());
// Permite receber dados em formato JSON
app.use(express.json());

// Configuração do banco de dados
const db = mysql.createPool({
    host: "localhost",
    user: "root",
    password: "",
    database: "pinball_game"
});

// Rota de teste da API
app.get("/", (req, res) => {
    res.send("API AgroCAP funcionando!");
});

// Rota para salvar pontuação no ranking
app.post("/ranking", async (req, res) => {
    try {

        // Dados recebidos da Unity
        const {
            nome,
            pontos,

            boloEspecial,
            boloChocolate,
            boloMorango,
            boloSimples,

            trigoRestante,
            ovoRestante,
            leiteRestante,
            chocolateRestante,
            morangoRestante

        } = req.body;

        // Inserção dos dados no banco
        await db.execute(`
            INSERT INTO ranking (
                nome,
                pontos,

                bolo_especial,
                bolo_chocolate,
                bolo_morango,
                bolo_simples,

                trigo_restante,
                ovo_restante,
                leite_restante,
                chocolate_restante,
                morango_restante
            )
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        `, [
            nome,
            pontos,

            boloEspecial,
            boloChocolate,
            boloMorango,
            boloSimples,

            trigoRestante,
            ovoRestante,
            leiteRestante,
            chocolateRestante,
            morangoRestante
        ]);

        // Retorno de sucesso
        res.json({
            sucesso: true,
            mensagem: "Ranking salvo com sucesso!"
        });

    } catch (erro) {
        // Log de erro no servidor
        console.error(erro);
        // Retorno de erro
        res.status(500).json({
            sucesso: false,
            mensagem: "Erro ao salvar ranking"
        });
    }
});

// Rota para buscar ranking
app.get("/ranking", async (req, res) => {
    try {
        const [rows] = await db.execute(`
            SELECT nome, pontos
            FROM ranking
            ORDER BY pontos DESC
            LIMIT 6
        `);

        res.json(rows);

    } catch (erro) {
        console.error(erro);

        res.status(500).json({
            sucesso: false,
            mensagem: "Erro ao buscar ranking"
        });
    }
});

// Inicialização do servidor
app.listen(3000, () => {
    console.log("API rodando em http://localhost:3000");
});