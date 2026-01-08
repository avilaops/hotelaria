// Gerenciamento de Sessão com LocalStorage
// Persistência de autenticação entre recargas

window.SessionManager = {
    // Salvar token de sessão
    setSession: function(token, expiresInMinutes = 60) {
        const expiry = new Date();
        expiry.setMinutes(expiry.getMinutes() + expiresInMinutes);
        
        const session = {
            token: token,
            expiry: expiry.toISOString()
        };
        
        localStorage.setItem('hotelaria_session', JSON.stringify(session));
        console.log('Sessão salva com sucesso');
    },
    
    // Obter token de sessão
    getSession: function() {
        const sessionJson = localStorage.getItem('hotelaria_session');
        if (!sessionJson) {
            return null;
        }
        
        try {
            const session = JSON.parse(sessionJson);
            const expiry = new Date(session.expiry);
            
            // Verificar se expirou
            if (expiry < new Date()) {
                console.log('Sessão expirada');
                this.clearSession();
                return null;
            }
            
            return session.token;
        } catch (e) {
            console.error('Erro ao ler sessão:', e);
            this.clearSession();
            return null;
        }
    },
    
    // Limpar sessão
    clearSession: function() {
        localStorage.removeItem('hotelaria_session');
        console.log('Sessão limpa');
    },
    
    // Renovar sessão (extender tempo)
    renewSession: function(expiresInMinutes = 60) {
        const token = this.getSession();
        if (token) {
            this.setSession(token, expiresInMinutes);
            console.log('Sessão renovada');
        }
    },
    
    // Verificar se sessão está ativa
    isSessionActive: function() {
        return this.getSession() !== null;
    }
};

// Auto-renovar sessão a cada 5 minutos se página estiver ativa
setInterval(function() {
    if (document.visibilityState === 'visible') {
        window.SessionManager.renewSession();
    }
}, 5 * 60 * 1000); // 5 minutos
