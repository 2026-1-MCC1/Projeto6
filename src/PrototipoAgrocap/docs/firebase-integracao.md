# Integracao Firebase no AgroCAP

## Resumo

O projeto agora usa Firebase Firestore como origem principal do ranking online.
O fluxo antigo baseado em `StartAPI` e `RankingAPI` saiu das cenas principais.

Fluxo atual:

1. `MenuController` salva o nome do jogador em memoria e em `PlayerPrefs`.
2. `GameManager` calcula os resultados finais e persiste tudo em `GameResults`.
3. `FirebaseRanking` envia a partida pendente para a colecao `ranking`.
4. `RankingManager` busca o top 6 e preenche a UI da cena `Scoreboard`.
5. Se um jogador repetir o mesmo nome, o documento dele e atualizado em vez de criar duplicata.

## O que foi alterado

### Scripts

- `Assets/Scripts/FirebaseBootstrap.cs`
  Inicializa o Firebase, verifica dependencias e mantem a instancia viva entre cenas.

- `Assets/Scripts/MenuController.cs`
  Agora persiste o nome do jogador em `PlayerPrefs` e preenche o campo de nome com o ultimo valor salvo.

- `Assets/Scripts/GameResults.cs`
  Agora salva nome do jogador, data da partida, status de ranking pendente, pontuacao, bolos e ingredientes.

- `Assets/Scripts/GameManager.cs`
  Agora grava em `GameResults` tanto os ingredientes coletados quanto os ingredientes restantes.

- `Assets/Scripts/FirebaseRanking.cs`
  Faz o envio assinado por callbacks para o Firestore e busca o ranking ordenado por `pontos`.

- `Assets/Scripts/Rank/RankingManager.cs`
  Passou a depender de `FirebaseRanking` em vez do backend local.

- `Assets/Scripts/Rank/ScoreboardUI.cs`
  A classe voltou a combinar com o nome do arquivo e do componente usado na cena.

### Cenas

- `Assets/Scenes/Menu.unity`
  O objeto vazio antigo `StartAPI` foi reaproveitado e agora chama `FirebaseBootstrap`.

- `Assets/Scenes/Scoreboard.unity`
  O objeto vazio antigo `RankingAPI` foi reaproveitado e agora chama `FirebaseRanking`.

- `Assets/Scenes/Scoreboard.unity`
  O `RankingManager` agora referencia `firebaseRanking`.

## Checklist do que voce precisa configurar

### Firebase Console

1. Confirmar que o projeto Firebase correto esta em uso.
2. Confirmar que o Firestore Database esta criado em `Native mode`.
3. Confirmar que a colecao `ranking` pode receber documentos.
4. Durante desenvolvimento, usar regras liberadas ou equivalentes.

Exemplo simples para desenvolvimento:

```txt
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {
    match /ranking/{document=**} {
      allow read, write: if true;
    }
  }
}
```

Para publicacao, troque isso por regras mais seguras.

### Unity / arquivos locais

1. Manter `Assets/FireBase/google-services.json` apontando para o projeto Firebase correto.
2. Manter `Assets/StreamingAssets/google-services-desktop.json` para testes no Editor/Desktop.
3. Se trocar de projeto Firebase, substituir os dois arquivos acima.
4. No Unity Editor, rodar:
   `Assets > External Dependency Manager > Android Resolver > Force Resolve`
5. Confirmar em `ProjectSettings/ProjectSettings.asset` que o identificador Android continua:
   `com.agrocap.game`

## Campos enviados ao Firestore

Cada documento da colecao `ranking` agora envia:

- `nome`
- `pontos`
- `nomeNormalizado`
- `boloEspecial`
- `boloChocolate`
- `boloMorango`
- `boloSimples`
- `trigo`
- `ovo`
- `leite`
- `chocolate`
- `morango`
- `trigoRestante`
- `ovoRestante`
- `leiteRestante`
- `chocolateRestante`
- `morangoRestante`
- `data`

## Observacoes importantes

- O envio do ranking acontece apenas se existir partida pendente.
- Se o envio falhar, a partida continua marcada como pendente em `PlayerPrefs`.
- O botao de ranking ainda mostra o top 6 mesmo quando a ultima tentativa de envio falha.
- O script legado `Assets/Scripts/Rank/RankingAPI.cs` continua no projeto, mas nao esta mais ligado nas cenas principais.
- O arquivo `Assembly-CSharp.csproj` foi atualizado para a checagem local reconhecer `FirebaseBootstrap.cs`.
  O Unity pode regenerar esse arquivo automaticamente depois.

## Verificacao feita

Foi executado:

```powershell
dotnet build Assembly-CSharp.csproj
```

Resultado:

- Compilacao concluida com sucesso.
- Restaram apenas warnings de arquitetura dos assemblies do Firebase no build MSBuild local.
- Restou tambem um warning antigo em `Assets/Scripts/Plunger.cs` sobre `minPower` sem uso.
