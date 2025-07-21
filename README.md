# TeknoKentYemekhaneApp

Bu proje, Blazor ve Dotnet Aspire kullanarak modern bir Yemekhane uygulaması geliştirmek amacıyla oluşturulmuştur. Projenin temel amacı, .NET dünyasında **Blazor**, **Dotnet Aspire**, **Docker** ve **docker-compose** teknolojilerini deneyimlemek ve uygulamada kullanmaktır. Kimlik doğrulama işlemleri için **JWT** tabanlı authentication mekanizması kullanılmıştır. Uygulama mimarisi olarak **CQRS** (Command Query Responsibility Segregation) ile birlikte **Onion Architecture** tercih edilmiştir.

## Kullanılan Teknolojiler ve Araçlar

- **Blazor**: Kullanıcı arayüzü için modern, hızlı ve interaktif web uygulamaları geliştirmeye olanak sağlar.
- **Dotnet Aspire**: Mikroservis mimarisini kolayca inşa etmek ve yönetmek için kullanılır.
- **Docker & docker-compose**: Uygulamanın kolayca başlatılması, taşınabilir ve ölçeklenebilir olması için konteynerleştirme sağlar.
- **JWT (JSON Web Token)**: Güvenli kimlik doğrulama ve yetkilendirme mekanizması.
- **CQRS**: Komut ve sorguları birbirinden ayırarak ölçeklenebilir ve bakımı kolay bir yapı sunar.
- **Onion Architecture**: Katmanlı ve kolay test edilebilir bir kod yapısı oluşturmak için kullanılır.

## Proje Yapısı

```
TeknoKentYemekhaneApp/
│
├── src/
│   ├── Application/        # CQRS ve iş mantığı
│   ├── Domain/             # Temel domain modelleri ve arayüzler
│   ├── Infrastructure/     # Veri erişimi ve dış servis entegrasyonları
│   ├── Web/                # Blazor tabanlı UI
│   └── Api/                # JWT ile korunan API uç noktaları
│
├── docker-compose.yml      # Tüm servisleri ayağa kaldıran Docker Compose dosyası
├── README.md
└── ...
```

## Başlangıç

1. **Gereksinimler**
   - [.NET SDK 8+](https://dotnet.microsoft.com/download)
   - [Docker](https://www.docker.com/get-started)
   - [Node.js](https://nodejs.org/) (eğer arayüzde ek paketler gerekiyorsa)

2. **Uygulamayı Docker ile çalıştırmak**
   ```bash
   docker-compose up --build
   ```

3. **Geliştirme ortamında çalıştırmak**
   ```bash
   dotnet build
   dotnet run --project src/Web
   ```

## Özellikler

- **JWT Authentication**: Kullanıcılar güvenli bir şekilde giriş yapabilir ve yetkilendirilebilir.
- **Blazor UI**: Dinamik ve hızlı kullanıcı arayüzü.
- **CQRS & Onion Architecture**: Sürdürülebilir, bakımı kolay ve test edilebilir bir kod yapısı.
- **Dockerize**: Tüm sistem bileşenleri Docker ile kolayca ayağa kaldırılır.
- **Dotnet Aspire**: Mikroservis ya da dağıtık mimariyi kolaylaştırır.

## Katkıda Bulunma

Pull request'ler ve sorun bildirimleri (issue) açıktır. Her türlü katkınızı bekliyoruz!

## Lisans

Bu proje MIT lisansı ile lisanslanmıştır.

---

**Amaç**: Bu proje, Blazor, Dotnet Aspire, Docker, docker-compose, JWT, CQRS ve Onion Architecture gibi modern yazılım geliştirme pratiklerini uygulama ve öğrenme amacı taşımaktadır.
