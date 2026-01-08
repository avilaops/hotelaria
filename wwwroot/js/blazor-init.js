// Prevenir múltiplas inicializações do Blazor
// Garantir que Blazor.start() só seja chamado uma vez

(function() {
    'use strict';
    
    // Flag para controlar se Blazor já foi iniciado
    if (window.blazorStarted) {
        console.warn('[BLAZOR] Tentativa de iniciar Blazor novamente - ignorando');
        return;
    }
    
    // Marcar como iniciado
    window.blazorStarted = true;
    
    console.log('[BLAZOR] Aguardando carregamento do Blazor...');
    
    // Aguardar o script blazor.server.js carregar
    function waitForBlazor() {
        if (typeof Blazor !== 'undefined') {
            console.log('[BLAZOR] Blazor carregado, verificando estado...');
            
            // Verificar se já foi iniciado
            if (Blazor.started) {
                console.warn('[BLAZOR] Blazor já foi iniciado anteriormente');
                return;
            }
            
            // Configurar reconexão
            Blazor.defaultReconnectionHandler._reconnectCallback = function() {
                console.log('[BLAZOR] Tentando reconectar...');
            };
            
            console.log('[BLAZOR] Iniciando Blazor...');
        } else {
            // Tentar novamente após 100ms
            setTimeout(waitForBlazor, 100);
        }
    }
    
    // Iniciar verificação
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', waitForBlazor);
    } else {
        waitForBlazor();
    }
    
    // Prevenir múltiplas chamadas ao Blazor.start()
    if (typeof Blazor !== 'undefined') {
        const originalStart = Blazor.start;
        let startCalled = false;
        
        Blazor.start = function() {
            if (startCalled) {
                console.warn('[BLAZOR] Blazor.start() já foi chamado - ignorando');
                return Promise.resolve();
            }
            startCalled = true;
            console.log('[BLAZOR] Blazor.start() chamado');
            return originalStart.apply(this, arguments);
        };
    }
})();
