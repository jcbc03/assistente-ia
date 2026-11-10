# Mapa do projeto

## Fluxo principal

```mermaid
flowchart LR
    User[Usuario]
    UI[Unity UI / Editor]
    Domain[Dominio e validacao]
    Config[(CharacterConfig JSON v2)]
    Applier[CharacterCustomizationApplier]
    Runtime[Unity URP / VRM Runtime]
    Catalog[Catalogo local de assets licenciados]
    Import[Importadores VRM GLB PNG HDRI]
    Chat[Interface de conversa]
    Ollama[Ollama local opcional]
    TTS[TTS pt-BR local opcional]
    Audio[Audio + lipsync + emocao]
    Store[(JSON e arquivos locais)]
    API[API futura opcional]
    DB[(PostgreSQL futuro opcional)]

    User --> UI --> Domain
    Domain <--> Config
    Domain --> Applier --> Runtime
    Catalog --> Applier
    Import --> Domain
    Import --> Store
    Domain --> Store
    User --> Chat
    Chat --> Ollama
    Ollama --> Chat
    Chat --> TTS --> Audio --> Runtime
    Config -. consentimento futuro .-> API --> DB
```

## Estrutura de pastas

```text
.
|-- docs/
|   |-- ARCHITECTURE.md
|   |-- PROJECT_MAP.md
|   `-- ROADMAP.md
|-- shared/
|   |-- examples/char_lucia_01.json
|   `-- schemas/character.schema.json
|-- unity/
|   |-- Assets/
|   |   |-- Animations/{Dance,Emote,Idle,Poke,Talk}/
|   |   |-- Characters/Lucia/
|   |   |-- Scenes/{Backgrounds,Lighting}/
|   |   |-- Scripts/{Application,Characters,Conversation,Customization,Import,Persistence,Scene,UI,Voice}/
|   |   |-- Shaders/
|   |   |-- Tests/{EditMode,PlayMode}/
|   |   `-- Wardrobe/{Accessories,Clothes,Hair}/
|   |-- Packages/
|   `-- ProjectSettings/
`-- backend/ (opcional, apenas no marco M7)
```

## Dependencias entre modulos

```mermaid
flowchart TD
    Schema[Contrato JSON compartilhado] --> Domain[Dominio / validacao / migracoes]
    Domain --> Persistence[Persistencia local]
    Domain --> Editor[Editor de personagem]
    Domain --> Applier[Applier Unity]
    Catalog[Manifests de assets] --> Applier
    Import[Importacao validada] --> Domain
    Runtime[VRM / URP / Cinemachine] --> Applier
    Chat[Interface de conversa] --> Provider[Provedor Ollama local]
    Chat --> Voice[Adaptador Piper ou Kokoro]
    Voice --> LipSync[uLipSync / visemes]
    LipSync --> Runtime
    Sync[Sync remota futura] -. opcional .-> Domain
    Sync -. opcional .-> DB[(PostgreSQL)]
```

As setas pontilhadas representam servicos opcionais; nao sao pre-requisitos do app local.
