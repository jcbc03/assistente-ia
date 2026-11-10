# Assistente IA

Base de um aplicativo Android original de companhia 3D editavel. A primeira personagem e Lucia; modelos, roupas, cenarios, vozes e animacoes devem ser proprios ou ter licenca compativel com a distribuicao do app.

## Visao do produto

- Aplicativo Android feito em Unity 6 e URP, com personagem VRM 1.0.
- Editor de personagem cujas configuracoes ficam em JSON versionado e validado.
- Conversa opcional com modelo local (Ollama) e voz TTS gratuita em pt-BR.
- Importacao local de VRM/GLB, texturas e fundos com validacao e limites de tamanho.
- Nenhum login, backend, assinatura ou API paga obrigatorios para o MVP.

## Estrutura

- [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md): decisoes, limites e fluxo dos dados.
- [`docs/ROADMAP.md`](docs/ROADMAP.md): plano de entrega com dependencias e criterios de aceite.
- [`docs/PROJECT_MAP.md`](docs/PROJECT_MAP.md): mapa Mermaid de componentes e estrutura.
- [`shared/schemas/character.schema.json`](shared/schemas/character.schema.json): contrato JSON Schema v2 da personagem.
- [`shared/examples/char_lucia_01.json`](shared/examples/char_lucia_01.json): preset inicial original de Lucia.
- [`unity/`](unity/): estrutura-base para o cliente Unity.

## Comecar

1. Instale Unity Hub e a versao Unity `6000.3.18f1` com suporte a Android Build Support.
2. Abra a pasta `unity` no Unity Hub. O projeto esta preparado como scaffold; o editor deve gerar os arquivos locais de projeto ao abrir.
3. Configure URP, Cinemachine e UniVRM pelas instrucoes de [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md), verificando compatibilidade e licencas antes de fixar dependencias.
4. Leia o roadmap antes de implementar funcionalidades. O primeiro marco e carregar um avatar VRM proprio e salvar/recarregar seu JSON.

## Limites importantes

Este repositorio nao inclui modelos, texturas, sons, prompts ou codigo extraidos de outros aplicativos. A referencia fornecida serve apenas como descricao de funcionalidades; os assets do produto devem ser originais ou licenciados. O preset Lucia e um ponto de partida editavel, nao um pacote de assets.

## Estado

Scaffold e contratos iniciais criados. O runtime Unity, a interface, a conversa, a TTS e qualquer backend ainda precisam ser implementados conforme [`docs/ROADMAP.md`](docs/ROADMAP.md).
