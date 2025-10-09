using GameCommerce.Aplicacao;
using GameCommerce.Aplicacao.Interfaces;
using GameCommerce.Persistencia;
using GameCommerce.Persistencia.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

ConfigureDbContext(builder);
ConfigureServices(builder);

var app = builder.Build();
ConfigureAplication(app);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void ConfigureDbContext(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<AppDbContext>(context =>
    {
        context.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        context.EnableSensitiveDataLogging();
    });
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers()
                    .AddJsonOptions(x =>
                    {
                        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AcessoTotal",
            policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
    });

    builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AllowNullCollections = true;
        }, AppDomain.CurrentDomain.GetAssemblies()
    );

    // Registrar Persistências
    builder.Services.AddScoped<ICategoriaPersist, CategoriaPersist>();
    builder.Services.AddScoped<IProdutoPersist, ProdutoPersist>();
    builder.Services.AddScoped<ICupomPersist, CupomPersist>();
    builder.Services.AddScoped<IPedidoPersist, PedidoPersist>();
    builder.Services.AddScoped<ISiteInfoPersist, SiteInfoPersist>();
    builder.Services.AddScoped<ITransacaoPagamentoPersist, TransacaoPagamentoPersist>();

    // Registrar o HttpClient para GatewayService
    builder.Services.AddHttpClient<IGatewayService, GatewayService>();

    // Registrar Services (Camada de Aplicação)
    builder.Services.AddScoped<ICategoriaService, CategoriaService>();
    builder.Services.AddScoped<IProdutoService, ProdutoService>();
    builder.Services.AddScoped<ICupomService, CupomService>();
    builder.Services.AddScoped<IGatewayService, GatewayService>();
    builder.Services.AddScoped<IPedidoService, PedidoService>();
    builder.Services.AddScoped<ISiteInfoService, SiteInfoService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "GameCommerce API - Vendas",
            Version = "v1",
            Description = "API para página de vendas"
        });

        options.SwaggerDoc("v2", new OpenApiInfo
        {
            Title = "GameCommerce API - Admin",
            Version = "v2",
            Description = "API para área administrativa"
        });

        // Método SIMPLES e FUNCIONAL para separar as versões
        options.DocInclusionPredicate((docName, apiDesc) =>
        {
            // Pega o caminho do endpoint
            var routeTemplate = apiDesc.RelativePath;

            if (routeTemplate == null) return false;

            // V1: endpoints que começam com /api/v1/
            // V2: endpoints que começam com /api/v2/
            if (docName == "v1")
            {
                return routeTemplate.StartsWith("api/v1/");
            }
            else if (docName == "v2")
            {
                return routeTemplate.StartsWith("api/v2/");
            }

            return false;
        });
    });
}

static void ConfigureAplication(WebApplication app)
{
    // Criar pastas de upload
    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
    var produtosPath = Path.Combine(uploadsPath, "produtos");
    var categoriasPath = Path.Combine(uploadsPath, "categorias");

    if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);
    if (!Directory.Exists(produtosPath)) Directory.CreateDirectory(produtosPath);
    if (!Directory.Exists(categoriasPath)) Directory.CreateDirectory(categoriasPath);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 - Vendas");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 - Admin");

                // Opcional: definir qual versão abrir por padrão
                options.ConfigObject.DefaultModelsExpandDepth = -1;
                options.ConfigObject.DisplayRequestDuration = true;
            }
        );
    }

    app.UseCors("AcessoTotal");

    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(uploadsPath),
        RequestPath = new PathString("/uploads")
    });
}