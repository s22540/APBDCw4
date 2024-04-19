using VeterinaryClinicApi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Przyk³adowe rekordy zwierz¹t
var animals = new List<Animal>
{
    new Animal { Id = 1, Name = "Burek", Category = "Pies", Weight = 22.5, FurColor = "Br¹zowy" },
    new Animal { Id = 2, Name = "Miau", Category = "Kot", Weight = 5.2, FurColor = "Szary" }
};

//Przyk³adowe rekordy wizyt
var visits = new List<Visit>
{
    new Visit { Id = 1, Date = DateTime.Now.AddDays(-1), Animal = animals[0], Description = "Szczepienie roczne", Price = 150 },
    new Visit { Id = 2, Date = DateTime.Now.AddDays(-2), Animal = animals[1], Description = "Kontrolne badanie krwi", Price = 120 }
};

//Endpoint zwierz¹t
app.MapGet("/animals", () => animals);
app.MapGet("/animals/{id}", (int id) => animals.FirstOrDefault(a => a.Id == id));
app.MapPost("/animals", (Animal animal) => {
    animal.Id = animals.Max(a => a.Id) + 1;
    animals.Add(animal);
    return Results.Created($"/animals/{animal.Id}", animal);
});
app.MapPut("/animals/{id}", (int id, Animal updatedAnimal) => {
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal == null) return Results.NotFound();
    animal.Name = updatedAnimal.Name;
    animal.Category = updatedAnimal.Category;
    animal.Weight = updatedAnimal.Weight;
    animal.FurColor = updatedAnimal.FurColor;
    return Results.NoContent();
});
app.MapDelete("/animals/{id}", (int id) => {
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal == null) return Results.NotFound();
    animals.Remove(animal);
    return Results.Ok();
});

// Endpoint wizyt
app.MapGet("/visits", () => visits);
app.MapGet("/visits/{id}", (int id) => visits.FirstOrDefault(v => v.Id == id));
app.MapGet("/animalVisits/{animalId}", (int animalId) => visits.Where(v => v.Animal.Id == animalId).ToList());
app.MapPost("/visits", (Visit visit) => {
    visit.Id = visits.Max(v => v.Id) + 1;
    visits.Add(visit);
    return Results.Created($"/visits/{visit.Id}", visit);
});
app.Run();