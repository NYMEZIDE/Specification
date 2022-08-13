![Logo](https://github.com/NYMEZIDE/Specification/blob/master/.github/images/specification.png?raw=true)

[![Coverage Status](https://coveralls.io/repos/github/NYMEZIDE/Specification/badge.svg?branch=master&kill_cache=1)](https://coveralls.io/github/NYMEZIDE/Specification?branch=master)
![Build Status](https://github.com/NYMEZIDE/Specification/workflows/master/badge.svg)
[![Nuget version](https://img.shields.io/nuget/v/Nymezide.Specification?label=NuGet)](https://www.nuget.org/packages/Nymezide.Specification)
[![Nuget stats](https://img.shields.io/nuget/dt/Nymezide.Specification.svg)](https://www.nuget.org/packages/Nymezide.Specification)
 
# Паттерн Спецификация на .NET

## Определение
«Спецификация» в программировании  — это шаблон проектирования, посредством которого представление правил бизнес логики может быть преобразовано в виде цепочки объектов, связанных операциями булевой логики.

## Область применения
1. **Валидация** объекта **в памяти** на соответствие требованиям 
2. **Поиск** объектов **в базе данных**, соответствующих требованиям
3. ~~Создание экземпляра объекта по требованиям~~ - это невозможно, т.к. спецификация инкапсулирует требования внутри себя. случайный перебор всех вариантов - это плохой подход.

## Возможности этой библиотеки

1. [Cтрого-типизированные спецификации](#cтрого-типизированные-спецификации)
2. [Динамические спецификации](#динамические-спецификации)
    - однострочные спецификации прямо в коде
3. [Запросы в Базу Данных, а также коллекции в памяти, через Linq](#запросы-в-базу-данных-а-также-коллекции-в-памяти-через-linq)
4. [Fluent-интерфейс](#fluent-интерфейс)
    - ```.And()```
    - ```.Or()``` 
    - ```.Not()```
5. [Поддержка операторов](#поддержка-операторов)
    - ```&```
    - ```|```
    - ```!```
    - ```==```
    - ```!=```
6. [Множество методов расширений для коллекций](#множество-методов-расширений-для-коллекций)
    - ```.Is()```
    - ```.IsAny()```
    - ```.IsAll()```
    - ```.AnyIs()```
    - ```.AllIs()```
7. [Реактивные экшен-методы для выполнения действия, при False результате](#реактивные-экшен-методы-для-выполнения-действия-при-false-результате)
    - например можно подсчитать сколько объектов из коллекции не соответствуют спецификации
    - или выполнить какой-либо метод для объектов из коллекции, которые не соответствуют специцикации

### Cтрого-типизированные спецификации

*Базовый абстрактный класс, который необходимо наследовать и реализовать*
``` csharp
public abstract class AbstractSpec<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> Expression { get; }
    public bool IsSatisfiedBy(T candidate)
    {
        // implementation
    }
}
```

*Пример:*
``` csharp
// Скучный фильм
public class BoringMovieSpec : AbstractSpec<Movie>
{
    private readonly TimeSpan _durationGreaterThat;
    private readonly double _ratingLessOrEqualThat;

    public BoringMovieSpec(TimeSpan? durationGreaterThat = null, double ratingLessOrEqualThat = 4)
    {
        _durationGreaterThat = durationGreaterThat ?? TimeSpan.FromHours(2.5);
        _ratingLessOrEqualThat = ratingLessOrEqualThat;
    }
    
    public override Expression<Func<Movie, bool>> Expression
        => movie => movie.Duration > _durationGreaterThat &&
                    movie.Rating <= _ratingLessOrEqualThat;
}
```


### Динамические спецификации

``` csharp
AbstractSpec<Movie> spec = new Spec<Movie>(m => m.Rating > 5 && m.MpaaRating == MpaaRating.PG13);
```


### Запросы в Базу Данных, а также коллекции в памяти, через Linq

``` csharp
using var db = new OrdersDbContext();

var richProductsSpec = new Spec<Order>(x => x.Products.Any(p => p.Price >= 500));

var query = db.Orders
    .Include(o => o.Products)
    .Where(richProductsSpec);

var orders = query.ToList();
```

### Fluent-интерфейс

``` csharp
AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
AbstractSpec<Movie> ratingLess = new Spec<Movie>(m => m.Rating <= 3);

var combineSpecs1 = longest.And(ratingLess);
var combineSpecs2 = longest.Or(ratingLess); 
```

### Поддержка операторов
``` csharp
AbstractSpec<Movie> longest = new Spec<Movie>(m => m.Duration > TimeSpan.FromHours(2.5));
AbstractSpec<Movie> ratingLess = new Spec<Movie>(m => m.Rating <= 3);

var combineSpecs1 = longest & ratingLess;
var combineSpecs2 = longest | ratingLess;
```

### Множество методов расширений для коллекций

``` csharp
Movie movie = new Movie()
{
    Rating = 3,
};

var spec1 = new Spec<Movie>(m => m.Rating == 2);
var spec2 = new Spec<Movie>(m => m.Rating == 3);
var spec3 = new Spec<Movie>(m => m.Rating == 4);

var result = movie.IsAny(spec1, spec2, spec3);

```

``` csharp
ICollection<Movie> movies = new List<Movie>
{
    new Movie { Rating = 2 },
    new Movie { Rating = 3 },
    new Movie { Rating = 4 },
    new Movie { Rating = 5 },
    new Movie { Rating = 6 },
};

var spec = new Spec<Movie>(m => m.Rating > 4);

var result = movies.AnyIs(spec);
```

### Реактивные экшен-методы для выполнения действия, при False результате 

```OnFalseAction``` выполняется только при выполнении метода ```IsSatisfiedBy()```, а значит только при вызове операторов:
    ```.Is()```, ```.IsAny()```, ```.IsAll()```, ```.AnyIs()```, ```.AllIs()```

*простой пример на проверку одного объекта:*
``` csharp
 Movie movie = new Movie()
{
    Rating = 4,
    MpaaRating = MpaaRating.G
};

var spec = new Spec<Movie>(m => m.Rating > 5 && m.MpaaRating == MpaaRating.R);
var actionValue = false;
spec.OnFalseAction = (s, c) => actionValue = true;

var result = movie.Is(spec); // actionValue станет true, т.к. спецификация вернет False
```

*сложный пример, проверка коллекции через методы расширения:*
``` csharp
IEnumerable<Movie> movies = new List<Movie>
{
    new Movie { Rating = 2 },
    new Movie { Rating = 3 },
    new Movie { Rating = 4 },
    new Movie { Rating = 5 },
    new Movie { Rating = 6 },
};
var counter = 0;
var spec = new Spec<Movie>(m => m.Rating > 4)
{
    OnFalseAction = (spec, candidate) => counter++
};

var result1 = movies.AllIs(spec); // сработает для всех кто вернул false, в данном случае counter будет равен 3, т.к. всего 3 объекта не соответсвуют 
var result2 = movies.AnyIs(spec); // сработает для всех, если никто не вернул true, в данном случае counter будет равен 0 - т.к. в коллекции есть хотябы один объект попадающий под спецификацию
```

```OnFalseAction``` не выполняется при запросах в БД или запросах в коллекции через Linq 

## Подключение библиотеки через Nuget

```
Install-Package Nymezide.Specification
```

### Поддержка фрейморков
- .NET Standard 2.1
- .NET 5.0
- .NET 6.0


