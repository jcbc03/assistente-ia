# Backend opcional

O MVP nao depende deste diretorio nem de conexao com rede. Um backend so sera iniciado no marco M7, apos validar demanda por sincronizacao entre dispositivos e definir consentimento, autenticacao, retencao e exclusao.

Decisao prevista: Python/FastAPI para API e PostgreSQL para dados de conta/sincronizacao. Ollama local continua sendo o caminho offline. Nunca colocar chaves de provedores no app Android.
