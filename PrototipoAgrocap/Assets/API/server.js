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

        // Garante que o banco receba o nome digitado ou o nome padrao
        const nomeJogador = typeof nome === "string" && nome.trim() !== "" ? nome.trim() : "Jogador";

        // Garante que a pontuacao seja tratada como numero
        const pontosJogador = Number(pontos) || 0;

        // Busca a maior pontuacao ja salva para esse nome
        const [jogadorExistente] = await db.execute(`
            SELECT MAX(pontos) AS maiorPontuacao
            FROM ranking
            WHERE nome = ?
        `, [
            nomeJogador
        ]);

        // Verifica se o nome ja existe no ranking
        const nomeJaExiste = jogadorExistente[0].maiorPontuacao !== null;

        // Guarda a pontuacao atual desse nome
        const maiorPontuacao = nomeJaExiste ? Number(jogadorExistente[0].maiorPontuacao) : 0;

        // Se a nova pontuacao for menor ou igual, ignora o envio
        if (nomeJaExiste && pontosJogador <= maiorPontuacao) {
            return res.json({
                sucesso: true,
                atualizado: false,
                mensagem: "Pontuação menor ou igual. Ranking mantido."
            });
        }

        // Se o nome ja existe e a pontuacao e maior, remove o registro antigo
        if (nomeJaExiste) {
            await db.execute(`
                DELETE FROM ranking
                WHERE nome = ?
            `, [
                nomeJogador
            ]);
        }

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
            nomeJogador,
            pontosJogador,

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
            atualizado: true,
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
            SELECT nome, MAX(pontos) AS pontos
            FROM ranking
            GROUP BY nome
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
