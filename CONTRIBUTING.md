# Guia de Contribuição

Obrigado por considerar contribuir com o projeto Hotelaria! 🎉

## Como Contribuir

### Reportar Bugs

Se você encontrou um bug, por favor:

1. Verifique se o bug já não foi reportado nas [Issues](https://github.com/avilaops/hotelaria/issues)
2. Caso não tenha sido reportado, abra uma nova issue incluindo:
   - Descrição clara do problema
   - Passos para reproduzir
   - Comportamento esperado vs atual
   - Screenshots (se aplicável)
   - Versão do .NET e navegador

### Sugerir Melhorias

Sugestões de melhorias são sempre bem-vindas! Para sugerir:

1. Abra uma issue com a tag `enhancement`
2. Descreva claramente a melhoria proposta
3. Explique por que ela seria útil para o projeto

### Pull Requests

1. **Fork** o projeto
2. **Clone** seu fork:
   ```bash
   git clone https://github.com/seu-usuario/hotelaria.git
   ```

3. Crie uma **branch** para sua feature:
   ```bash
   git checkout -b feature/minha-nova-feature
   ```

4. Faça suas alterações seguindo os padrões do projeto

5. **Commit** suas mudanças:
   ```bash
   git commit -m 'feat: adiciona nova funcionalidade X'
   ```

6. **Push** para sua branch:
   ```bash
   git push origin feature/minha-nova-feature
   ```

7. Abra um **Pull Request**

## Padrões de Código

### Convenções de Nomenclatura

- **Classes**: PascalCase (ex: `ReservaService`)
- **Métodos**: PascalCase (ex: `ObterTodas()`)
- **Variáveis**: camelCase (ex: `receitaTotal`)
- **Propriedades**: PascalCase (ex: `ValorTotal`)
- **Constantes**: PascalCase (ex: `MaximoHospedes`)

### Estrutura de Arquivos

- Modelos em `Models/`
- Serviços em `Services/`
- Páginas em `Pages/`
- Componentes compartilhados em `Shared/`

### Comentários

- Adicione comentários em código complexo
- Use XML documentation para métodos públicos
- Escreva comentários em português

### Commits

Siga o padrão [Conventional Commits](https://www.conventionalcommits.org/):

- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Documentação
- `style`: Formatação
- `refactor`: Refatoração
- `test`: Testes
- `chore`: Tarefas gerais

Exemplos:
```
feat: adiciona filtro por período no relatório financeiro
fix: corrige cálculo de comissão em reservas
docs: atualiza README com instruções de deploy
```

## Testes

Antes de enviar um PR:

1. Compile o projeto: `dotnet build`
2. Execute a aplicação: `dotnet run`
3. Teste manualmente todas as funcionalidades afetadas
4. Verifique se não há erros no console do navegador

## Código de Conduta

- Seja respeitoso e inclusivo
- Aceite críticas construtivas
- Foque no que é melhor para a comunidade
- Mostre empatia com outros membros

## Dúvidas?

Se tiver dúvidas, abra uma issue ou entre em contato através do GitHub.

Obrigado por contribuir! 🙏
