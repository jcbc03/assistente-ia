# Roadmap de implementacao

Plano executavel em marcos verticais. Cada marco deve terminar com uma build que ainda abre e com os criterios de aceite abaixo passando. Estimativas dependem de assets e do hardware Android e devem ser ajustadas apos o primeiro prototipo.

## M0 — Fundacao e decisao tecnica

**Entregas**
- Abrir `unity/` em Unity `6000.3.18f1` e configurar build Android/URP.
- Fixar versoes compativeis de URP, Cinemachine e UniVRM; registrar licencas.
- Adicionar assemblies C# e testes EditMode para `CharacterConfig`, IDs e limites.
- Fazer leitura/escrita atomica do JSON v2, import/export e migracao versionada.

**Aceite**
- Build Android vazia instala e inicia em aparelho de referencia.
- JSON de Lucia valida; JSON malformado, versao desconhecida e valores fora de faixa geram erro recuperavel.
- Salvar, fechar e reabrir preserva configuracao sem corromper o arquivo em interrupcao simulada.

## M1 — Vertical slice 3D

**Entregas**
- Importar um VRM original/licenciado, carregar humanoide e apresentar idle.
- Camera bust/full, look-at, fundo simples e luz URP.
- Um preset de expressao e um emote com animacao autorizada.

**Aceite**
- Avatar carrega em aparelho Android sem erro de shader/material e mantem pes no chao.
- Troca de camera, idle/emote e expressao funcionam sem reiniciar.
- Falha ao abrir um avatar invalido preserva a cena anterior e explica o erro.

## M2 — Customizacao persistente

**Entregas**
- Tela de editor com abas Corpo, Rosto, Olhos, Cabelo, Roupas, Acessorios e Materiais.
- Sliders numericos, cores/hex, reset por campo, undo/redo e salvar look.
- Aplicador incremental para morfologia, materiais e visibilidade dos slots.

**Aceite**
- Cada controle implementado atualiza a cena e o JSON; recarregar restaura o mesmo estado.
- Slot vazio, troca de peca e undo/redo sao testados.
- Campos ainda sem suporte aparecem como indisponiveis; nao sao descartados ao salvar.

## M3 — Guarda-roupa e importacao

**Entregas**
- Catalogo local de cabelo, roupa e acessorios originais com manifest/IDs e licencas.
- Slots em camadas, compatibilidade de esqueleto e regras de onesie.
- Importacao limitada de VRM/GLB, textura PNG e imagem/HDRI; aviso de licenca e origem.

**Aceite**
- Importacao validada nao substitui dados anteriores ate concluir com sucesso.
- Arquivos muito grandes, tipos incorretos, malhas sem humanoide e texturas invalidas sao rejeitados com mensagem.
- Textura/material sao instanciados sem alterar o asset-fonte; slots podem ser limpos.

## M4 — Cenario e atuacao

**Entregas**
- Presets de fundo, horario, luz, camera, clima e pos-processamento.
- Expressoes configuraveis sobre shapes disponiveis, visemes e sincronizacao com fala.
- Animacoes de idle, talk, listen, emote e danca; SpringBone do VRM como fisica inicial.

**Aceite**
- Alteracoes de ambiente e camera persistem no JSON e sao aplicadas ao reabrir.
- Pesos de expressao estao limitados e interpolados; animacao corporal nao sobrescreve os visemes.
- Perfil grafico reduzido mantem interacao estavel no aparelho de referencia.

## M5 — Conversa e TTS offline

**Entregas**
- Tela de conversa e interface `IChatProvider`; adaptador Ollama e modo sem modelo.
- TTS pt-BR local escolhida apos teste/licenca; controles de pitch, velocidade e volume suportados.
- Retorno opcional de emocao do dialogo, envio de audio ao lipsync e controles de memoria.

**Aceite**
- Sem Ollama, TTS ou rede, a configuracao e a cena continuam utilizaveis e o estado ausente e explicado.
- Ollama desligado/timeout pode ser tentado novamente sem perder a conversa salva localmente.
- Usuario edita/apaga memoria; nenhum prompt/audio sai do aparelho no modo offline.
- TTS gera audio pt-BR local e o avatar anima visemas sem conflito com preset emocional.

## M6 — Qualidade e publicacao Android

**Entregas**
- Testes PlayMode e aparelho para importacao, aplicacao, persistencia, conversa e retomada.
- Medicao de tempo de inicializacao, memoria, tamanho do APK/AAB, FPS e bateria.
- Acessibilidade basica, onboarding, exportacao/apagamento de dados, crash reporting sem dados pessoais.
- Checklist de licencas, politica de privacidade, assinatura e release.

**Aceite**
- Cenarios criticos passam em aparelho de referencia e na matriz minima Android definida antes do beta.
- Nenhum segredo de servico esta no cliente; modo local nao exige login/rede.
- Todos os assets distribuidos tem origem/licenca registrada; build release e reproduzivel.

## M7 — Servicos opcionais (somente apos validar demanda)

**Entregas**
- Especificacao de contas e sincronizacao; API FastAPI e PostgreSQL isolados do cliente.
- Consentimento granular, exportacao/exclusao, criptografia em transito e politica de retencao.
- Provedor LLM remoto opcional e CDN proprio de conteudo versionado.

**Aceite**
- Sem backend, todos os fluxos offline continuam funcionando.
- Falha/revogacao de sync nao apaga dados locais; conflitos tem resolucao explicita.
- Testes de autorizacao isolam dados por conta e verificam exclusao/revogacao.

## Ordem critica

`M0 -> M1 -> M2 -> M3/M4 -> M5 -> M6`; M3 e M4 podem evoluir em paralelo depois do contrato de configuracao. M7 e opcional e depende de avaliacao de privacidade, seguranca, custos e demanda. Nao iniciar producao de catalogo amplo antes de validar M1 e M2 em um dispositivo real.
