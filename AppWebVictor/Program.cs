using AppWebVictor.Components;
using AppWebVictor.Configs;
using AppWebVictor.DAO;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Adiciona os serviços do Blazor
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Registra a conexão com o banco de dados
        builder.Services.AddScoped<Conexao>();

        // Registra o DAO de Processo
        builder.Services.AddScoped<ProcessoDAO>();

        var app = builder.Build();

        // Configura o pipeline HTTP
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(
                "/Error",
                createScopeForErrors: true
            );

            // HSTS - segurança para HTTPS
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute(
            "/not-found",
            createScopeForStatusCodePages: true
        );

        // Redireciona HTTP para HTTPS
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        // Arquivos estáticos
        app.MapStaticAssets();

        // Configuração dos componentes Razor
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}