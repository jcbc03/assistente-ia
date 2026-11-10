# Arquitetura

## Decisao de produto

O produto e um aplicativo Android original, offline-first, para configurar uma personagem 3D, conversar com ela e personalizar sua aparencia e cenario. Unity e o runtime de renderizacao e interacao 3D. A versao inicial roda como app Unity Android; uma casca Kotlin/Compose so sera adicionada se houver necessidade comprovada de UX nativa que os plugins Unity nao atendam.

## Componentes

### Cliente Unity (Android)

- Unity `6000.3.18f1`, URP e Cinemachine.
- UniVRM/UniGLTF para VRM 1.0 e GLB humanoide; VRM e a base, nao o formato interno de todas as pecas.
- C# organizado em `Assets/Scripts/{Application,Characters,Customization,Conversation,Import,Persistence,Scene,UI,Voice}`.
- `CharacterConfig` e o contrato de dados; `CharacterCustomizationApplier` aplica o estado em etapas e reporta erros por campo.
- `CharacterCatalog` resolve IDs do app para assets proprios/permitidos. `Addressables` remotos ficam fora do MVP.
- Persistencia local em JSON, com escrita atomica, validacao de `schemaVersion`, backup e migracoes explicitas.
- Importacao por seletor de arquivos Android; os arquivos permanecem locais e sao validados antes de carregar.

### Conversa e voz

- Interface de conversa isolada do provedor de modelo.
- Adaptador local Ollama e modo de demonstracao sem modelo configurado.
- TTS local gratuita em pt-BR (Piper ou Kokoro), selecionada apos validar licenca, tamanho e desempenho no Android.
- O audio da TTS alimenta o lipsync; animacao corporal, visemas e expressoes nao devem escrever simultaneamente no mesmo canal de blendshape.
- `systemPrompt`, likes, dislikes e limites sao dados editaveis pelo usuario. Nao enviar esses dados a um servico remoto sem uma decisao explicita de produto e consentimento informado.

### Servicos futuros (opcionais)

- API Python/FastAPI para sincronizacao entre dispositivos, se solicitada.
- PostgreSQL apenas para contas/sincronizacao ativadas pelo usuario; nunca e dependencia do modo local.
- Provedor de LLM remoto atras de uma interface substituivel, sem chave de API no cliente.
- Conteudo remoto via Addressables e CDN proprio somente apos haver catalogo, versionamento, assinatura de assets e estrategia de cache.

## Contratos e fluxo

1. A UI produz uma alteracao tipada sobre a configuracao atual.
2. A camada de dominio valida limites, IDs e compatibilidade da alteracao.
3. O applier aplica a mudanca na cena; falha parcial e apresentada sem substituir o ultimo estado valido.
4. O repositorio local salva JSON validado de forma atomica e atualiza `updatedAt`.
5. Conversa usa os campos de persona/memoria autorizados e retorna texto, emocao opcional e audio.
6. Importadores tratam cada arquivo como entrada nao confiavel, limitam tamanho e validam formato antes de iniciar o parser 3D.

O formato canonico esta em `shared/schemas/character.schema.json`; o preset inicial esta em `shared/examples/char_lucia_01.json`. Mudancas incompatíveis exigem incremento de `schemaVersion` e migracao testada.

## Organizacao do Unity

- `Assets/Characters/Lucia/`: avatar base e materiais originais.
- `Assets/Wardrobe/{Hair,Clothes,Accessories}/`: pecas modulares e manifests.
- `Assets/Scenes/{Backgrounds,Lighting}/`: ambientes e presets de luz.
- `Assets/Animations/{Idle,Talk,Emote,Poke,Dance}/`: clipes autorizados.
- `Assets/Shaders/`: shaders URP originais.
- `Assets/Scripts/`: runtime dividido por responsabilidade.
- `Assets/Tests/{EditMode,PlayMode}/`: validacao do schema, migracoes e cenas.

IDs de pecas em manifests devem ser estaveis e iguais aos IDs gravados no JSON. Nunca gravar caminhos absolutos do dispositivo no documento compartilhavel; arquivos importados recebem identificadores locais e metadados.

## Dependencias e licencas

Adicionar URP, Cinemachine, UniVRM, TTS, modelos e animacoes somente depois de verificar compatibilidade com Unity 6, IL2CPP/Android, tamanho, manutencao e termos de redistribuicao. O MVP deve iniciar com SpringBone do VRM antes de considerar plugins de fisica pagos. Registrar licencas e atribuicoes junto aos assets.

## Privacidade e robustez

- O modo local nao exige conta e nao faz upload de persona, memoria, audio ou arquivos.
- Segredos de provedores nao podem ser embutidos no APK.
- Memoria tem retencao configuravel, edicao e exclusao.
- Importacao de VRM/GLB/textura e limitada por tamanho, formato e tempo; arquivos invalidos nao derrubam a sessao.
- Erros de carregamento/aplicacao sao visiveis e registrados sem incluir prompt ou conteudo pessoal.
- O usuario pode exportar e apagar seus dados locais.

## Fora do MVP

Sincronizacao de conta, CDN de assets, editor de malhas 3D, captura/clonagem de voz, plugins de fisica pagos, multiplayer e distribuicao de conteudo de terceiros.
