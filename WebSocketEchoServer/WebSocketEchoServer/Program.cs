using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using OpenAIProxyService.Controllers;
using OpenAIProxyService.Extension;
using OpenAIProxyService.Websocket;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using XPlan.Utility;
using XPlan.WebSockets;

var builder = WebApplication.CreateBuilder(args);

/********************************************
 * 加上 Filter 
 * ******************************************/
builder.Services.AddExceptionHandling<ProxyServiceErrorFilter>();

/********************************************
 * 註冊AutoMapper
 * ******************************************/
builder.Services.AddAutoMapperProfiles(LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // 或其他你需要的設定
}));

/***********************************************************************************************
* 從環境變數或 appsettings.json 取 API Key 與 Realtime 參數
* 環境變數：OPENAI_API_KEY、OPENAI_REALTIME_MODEL、OPENAI_REALTIME_VOICE、OPENAI_RT_INSTRUCTIONS
************************************************************************************************/
var apiKey          = builder.Configuration["OpenAI:OPENAI_API_KEY"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
var model           = builder.Configuration["OpenAI:OPENAI_REALTIME_MODEL"] ?? "gpt-4o-mini-realtime-preview";
var voice           = builder.Configuration["OpenAI:OPENAI_REALTIME_VOICE"] ?? "alloy";
var instructions    = builder.Configuration["OpenAI:OPENAI_RT_INSTRUCTIONS"] ?? "You are a helpful, concise voice assistant.";
var autoCreateRes   = bool.TryParse(builder.Configuration["OpenAI:OPENAI_RT_AUTO_CREATE"], out var b) && b;
var cts             = new CancellationTokenSource();

builder.Services.AddSingleton(cts);                 // 目的為了 可以被其他物件使用
builder.Services.AddSingleton(new OpenAIProxyOptions
{
    ApiKey              = apiKey,
    Model               = model,
    Voice               = voice,
    BasicInstructions   = instructions,
    AutoCreate          = autoCreateRes,
    ct                  = cts.Token
});
builder.Services.AddSingleton<OpenAIProxyWsHub>();  // 你的 WsHub（繼承 WebsocketBase）

/********************************************
 * 加上記憶體快取
 * ******************************************/
builder.Services.AddCacheSettings(builder.Configuration);

/********************************************
 * 註冊 JWT 認證
 * ******************************************/
builder.Services.AddJWTSecurity();
builder.Services.AddJwtAuthentication(new JwtOptions()
{
    ValidateIssuer              = true,
    ValidateAudience            = false,
    ValidateLifetime            = false,
    ValidateIssuerSigningKey    = true,
    Issuer                      = builder.Configuration["Jwt:Issuer"]!,
    Audience                    = builder.Configuration["Jwt:Audience"]!,
    Secret                      = builder.Configuration["Jwt:Secret"]!
});

/********************************************
 * 加上Data Access
 * ******************************************/
builder.Services.AddDataAccesses();

/********************************************
 * 加上Repository
 * ******************************************/
builder.Services.AddRepositorys();

/********************************************
 * 加上Service
 * ******************************************/
builder.Services.AddServices();

/********************************************
 * 加上Controller
 * ******************************************/
builder.Services.AddControllers();

/********************************************
 * 加上Swagger 設定
 * ******************************************/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.GenerateSwaggerDoc(builder.Environment);

    c.OperationFilter<ControllerAddSummaryFilter>();    // 使用 Operation Filter 來給API加上註解
    c.DocumentFilter<ApiHiddenFilter>();                // 使用 Operation Filter 來給API加上開關
    c.OperationFilter<AddAuthorizeCheckFilter>();       // 使用 Operation Filter 來給API加上認證
});

var app = builder.Build();

/***********************************************************************************************
* 啟用 WebSockets，設置保活間隔與收包緩衝
************************************************************************************************/
var wsOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(20),
    ReceiveBufferSize = 64 * 1024
};

app.UseWebSockets(wsOptions);

// WebSocket 端點：/ws
app.Map<OpenAIProxyWsHub>("/ws");
app.MapGet("/", () => "WS backend running").ExcludeFromDescription();

/***********************************************************************************************
* 啟用 Swagger
************************************************************************************************/
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //c.SwaggerEndpoints();
    });
}

/***********************************************************************************************
* 其他
************************************************************************************************/
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();              // 先認證身分
app.UseAuthorization();               // 再授權權限

app.MapControllers();

await app.RunAsync();


