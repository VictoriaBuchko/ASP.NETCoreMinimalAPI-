using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var counter = 0;

//лічильника переглядів
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/counter")
    {
        counter++;
        context.Response.ContentType = "text/html; charset=utf-8";
        
        var html = $@"
            <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Лічільник переглядів</title>
                </head>
                <body>
                    <h1>Лічільник переглядів: {counter}</h1>
                    <p>Сторінка переглянута {counter} разів</p>
                </body>
            </html>";
            
        await context.Response.WriteAsync(html);
        return;
    }
    await next();
});

//поточна дата та час
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/datetime")
    {
        var now = DateTime.Now;
        context.Response.ContentType = "text/html; charset=utf-8";
        
        var html = $@"
            <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Поточна дата та час</title>
                </head>
                <body>
                    <h1>Поточна дата та час</h1>
                    <p><strong>Дата:</strong> {now:dd.MM.yyyy}</p>
                    <p><strong>Час:</strong> {now:HH:mm:ss}</p>
                </body>
            </html>";
            
        await context.Response.WriteAsync(html);
        return;
    }
    await next();
});

//відображення об'єктів
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/objects")
    {
        var objects = new[]
        {
            new { Name = "Комп'ютер", Price = 25000, Brand = "Lenovo", Color = "Чорний" },
            new { Name = "Телефон", Price = 15000, Brand = "Samsung", Color = "Білий" },
            new { Name = "Планшет", Price = 12000, Brand = "Apple", Color = "Сірий" },
            new { Name = "Клавіатура", Price = 2000, Brand = "Logitech", Color = "Чорний" },
            new { Name = "Миша", Price = 1500, Brand = "Razer", Color = "Зелений" },
            new { Name = "Монітор", Price = 8000, Brand = "LG", Color = "Чорний" },
            new { Name = "Навушники", Price = 3000, Brand = "Sony", Color = "Синій" },
            new { Name = "Камера", Price = 20000, Brand = "Canon", Color = "Чорний" },
            new { Name = "Принтер", Price = 5000, Brand = "HP", Color = "Білий" },
            new { Name = "Роутер", Price = 2500, Brand = "TP-Link", Color = "Чорний" }
        };

        var objectCards = string.Join("", objects.Select(obj => $@"
            <div class='object-card'>
                <div class='object-name'>{obj.Name}</div>
                <p><strong>Ціна:</strong> <span class='object-price'>{obj.Price} грн</span></p>
                <p><strong>Бренд:</strong> {obj.Brand}</p>
                <p><strong>Колір:</strong> {obj.Color}</p>
            </div>"));

        var html = $@"
            <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Список об'єктів</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 20px; }}
                        .object-card {{ 
                            border: 1px solid #ccc; 
                            padding: 15px; 
                            margin: 10px 0; 
                            border-radius: 5px;
                            background-color: #f9f9f9;
                        }}
                        .object-name {{ color:rgb(0, 128, 255); font-weight: bold; }}
                        .object-price {{ color: #e74c3c; font-weight: bold; }}
                    </style>
                </head>
                <body>
                    {objectCards}
                </body>
            </html>";

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(html);
        return;
    }
    await next();
});

//headers
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/headers")
    {
        var headerss = string.Join("", context.Request.Headers.Select(header => $@"
            <div class='header'>
                <span class='header-name'>{header.Key}:</span>
                <span class='header-value'>{string.Join(", ", header.Value)}</span>
            </div>"));

        var html = $@"
            <html>
                <head>
                    <meta charset='utf-8'>
                    <title>HTTP Headers</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; margin: 20px; }}
                        .header {{ 
                            background-color: #f0f8ff; 
                            padding: 10px; 
                            margin: 5px 0; 
                            border-left: 4px solid #4CAF50;
                        }}
                        .header-name {{ font-weight: bold; color: #2c3e50; }}
                        .header-value {{ color: #7f8c8d; margin-left: 10px; }}
                    </style>
                </head>
                <body>
                    <h1>HTTP Headers</h1>
                    <h3>Метод: {context.Request.Method}</h3>
                    <h3>URL: {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}</h3>
                    <hr>
                    {headerss}
                </body>
            </html>";

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(html);
        return;
    }
    await next();
});

//профіль
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/profile")
    {
        var query = context.Request.Query;
        
        var name = query["name"].FirstOrDefault() ?? "...";
        var surname = query["surname"].FirstOrDefault() ?? "...";
        var age = query["age"].FirstOrDefault() ?? "...";
        var city = query["city"].FirstOrDefault() ?? "...";
        var profession = query["profession"].FirstOrDefault() ?? "...";
        var hobby = query["hobby"].FirstOrDefault() ?? "...";
        var email = query["email"].FirstOrDefault() ?? "...";

        var html = $@"
            <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Профіль користувача</title>
                    <style>
                        body {{ 
                            font-family: Arial, sans-serif;
                            background-color: #f8f9fa;
                            margin: 0;
                            padding: 20px;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background: white;
                            padding: 30px;
                            border-radius: 10px;
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
                        }}
                        .back-btn {{ 
                            display: inline-block; 
                            padding: 10px 20px; 
                            background-color: #007bff; 
                            color: white; 
                            text-decoration: none; 
                            border-radius: 5px; 
                            margin-bottom: 20px;
                        }}
                        .back-btn:hover {{ 
                            background-color: #0056b3; 
                        }}
                        h1 {{
                            color: #2c3e50;
                            text-align: center;
                            margin-bottom: 10px;
                        }}
                        h2 {{
                            color: #6c757d;
                            text-align: center;
                            margin-bottom: 30px;
                        }}
                        .info {{
                            margin: 15px 0;
                            padding: 15px;
                            background-color: #f8f9fa;
                            border-left: 3px solid #007bff;
                            border-radius: 5px;
                        }}
                        .label {{
                            font-weight: bold;
                            color: #495057;
                            margin-bottom: 5px;
                        }}
                        .value {{
                            color: #212529;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <a href='/' class='back-btn'>← Головне меню</a>
                        <h1>Профіль користувача</h1>
                        <h2>{name} {surname}</h2>
                        
                        <div class='info'>
                            <div class='label'>Ім'я:</div>
                            <div class='value'>{name}</div>
                        </div>
                        
                        <div class='info'>
                            <div class='label'>Прізвище:</div>
                            <div class='value'>{surname}</div>
                        </div>
                        
                        <div class='info'>
                            <div class='label'>Вік:</div>
                            <div class='value'>{age}</div>
                        </div>
                        
                        <div class='info'>
                            <div class='label'>Місто:</div>
                            <div class='value'>{city}</div>
                        </div>
                        
                        <div class='info'>
                            <div class='label'>Професія:</div>
                            <div class='value'>{profession}</div>
                        </div>
                        
                        <div class='info'>
                            <div class='label'>Хобі:</div>
                            <div class='value'>{hobby}</div>
                        </div>
                        
                        <div class='info'>
                            <div class='label'>Email:</div>
                            <div class='value'>{email}</div>
                        </div>
                    </div>
                </body>
            </html>";

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(html);
        return;
    }
    await next();
});

//головна сторінка 
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" && context.Request.Method == "GET")
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        
        var html = @"
            <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Minimal API - головна</title>
                    <style>
                        body { 
                            font-family: Arial, sans-serif; 
                            margin: 0; 
                            padding: 20px; 
                            background-color: #f5f5f5; 
                        }
                        .container { 
                            max-width: 800px; 
                            margin: 0 auto; 
                            background: white; 
                            padding: 30px; 
                            border-radius: 10px; 
                            box-shadow: 0 2px 10px rgba(0,0,0,0.1); 
                        }
                        h1 { color: #2c3e50; }
                        .nav-links { 
                            list-style: none; 
                            padding: 0; 
                        }
                        .nav-links li { 
                            margin: 15px 0; 
                            padding: 15px; 
                            background: #3498db; 
                            border-radius: 5px; 
                        }
                        .nav-links a { 
                            color: white; 
                            text-decoration: none; 
                            font-weight: bold; 
                            display: block; 
                        }
                        .nav-links li:hover { 
                            background: #2980b9; 
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Minimal API</h1>
                        <ul class='nav-links'>
                            <li><a href='/counter'>Лічільник переглядів</a></li>
                            <li><a href='/datetime'>Поточна дата та час</a></li>
                            <li><a href='/objects'>Список об'єктів</a></li>
                            <li><a href='/headers'>HTTP Headers</a></li>
                            <li><a href='/profile?name=Вікторія&surname=Бучко&age=20&city=Одеса&profession=Студент&hobby=Програмування&email=vika@example.com'>Профіль користувача</a></li>
                        </ul>
                    </div>
                </body>
            </html>";
            
        await context.Response.WriteAsync(html);
        return;
    }
    await next();
});

app.Run();