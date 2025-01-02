using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapGet("/items", async (ToDoDbContext db) =>
     await db.Items.ToListAsync());


app.MapPost("/items", async (string name, ToDoDbContext db) =>
{

    var newItem = new Item
    {
        Taskname = name,
        Iscomplate = false
    };


    db.Items.Add(newItem);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{newItem.Iditem}", newItem);
});

app.MapPut("/items/id", async (int id, bool inputItem, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);

    if (item is null) return Results.NotFound();

    item.Iscomplate = inputItem;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);

    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok(item);
});
app.UseCors("AllowAll");

app.Run();


//using Microsoft.EntityFrameworkCore;



// API Endpoints
// app.MapGet("/items", async (ToDoDbContext db) =>
//     await db.Items.ToListAsync());

// app.MapGet("/items/{id}", async (int id, ToDoDbContext db) =>
//     await db.Items.FindAsync(id) is Item item
//         ? Results.Ok(item)
//         : Results.NotFound());

// app.MapPost("/items", async (Item item, ToDoDbContext db) =>
// {
//     db.Items.Add(item);
//     await db.SaveChangesAsync();
//     return Results.Created($"/items/{item.Id}", item);
// });

// app.MapPut("/items/{id}", async (int id, Item inputItem, ToDoDbContext db) =>
// {
//     var item = await db.Items.FindAsync(id);

//     if (item is null) return Results.NotFound();

//     item.Name = inputItem.Name;
//     item.IsCompleted = inputItem.IsCompleted;

//     await db.SaveChangesAsync();
//     return Results.NoContent();
// });

// app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
// {
//     var item = await db.Items.FindAsync(id);

//     if (item is null) return Results.NotFound();

//     db.Items.Remove(item);
//     await db.SaveChangesAsync();
//     return Results.Ok(item);
// });

//app.Run();

///
///////
//dotnet ef dbcontext scaffold Name=DefaultConnection Pomelo.EntityFrameworkCore.MySql  -f -c ToDoDbContext
