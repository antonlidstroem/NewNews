NewNews is a cross-platform .NET MAUI application that retrieves news from a public API, stores data locally using SQLite, and employs caching for improved performance and offline usage. The app demonstrates clean architecture and modern practices for data persistence, offline support, and responsive UI across multiple platforms.

---

## Overview

NewNews is designed to:

- Fetch news content from a public API in real-time
- Cache API responses to improve performance
- Store news data locally using SQLite
- Provide a responsive UI across iOS, Android, Windows, and other supported .NET MAUI platforms
- Offer offline access to previously loaded news data

The project is structured to separate concerns between the app UI and data access logic, with a dedicated data layer for persistence and caching.

---

## Tech Stack

The application uses:

- .NET MAUI (Multi-platform App UI)
- C#
- SQLite for local data storage
- MVVM pattern for UI architecture
- REST API consumption

---

## Project Structure

NewNews/ ├── NewNews.DAL/        # Data access layer (SQLite entities, services) ├── NewNews.MAUI/       # .NET MAUI project (UI, Views, ViewModels) ├── NewNews.slnx        # Solution file ├── splash.png          # App splash image ├── .gitignore └── README.md

### NewNews.DAL

Contains:

- Database models
- SQLite context and configuration
- Repository or service logic for data persistence

### NewNews.MAUI

Contains:

- MAUI UI projects
- Views (XAML)
- ViewModels
- Platform-specific resources and configuration

---

## Getting Started

### Prerequisites

To build and run NewNews locally, you will need:

- Visual Studio 2022 (or later) with .NET MAUI workloads installed
- .NET 7 (or later) SDK
- A device/emulator for your chosen platform (Android, iOS, Windows)

### Installation

1. Clone the repository:

```bash
git clone https://github.com/antonlidstroem/NewNews.git
cd NewNews

2. Open the solution in Visual Studio:



Open NewNews.slnx with Visual Studio.

3. Restore NuGet packages:



Visual Studio will automatically restore required packages on load.

4. Configure API endpoints and settings:



If applicable, update API URLs or keys in your configuration files or project settings.

5. Build and run:



Select the platform you want to run (Android, iOS, Windows) and start the application.


---

Features

Cross-platform support via .NET MAUI

Local data caching to reduce API calls

Offline support using SQLite

Clean separation of UI and data access

Example implementation of REST API consumption



---

Architecture

NewNews is built with scalability and maintainability in mind:

Uses MVVM (Model-View-ViewModel) for UI architecture

Data access layer segregated into NewNews.DAL

UI logic contained in NewNews.MAUI

Uses local caching to minimize network usage and optimize performance



---

Future Improvements

Ideas for enhancements include:

Implementing pull-to-refresh behavior in the UI

Adding support for more news sources or APIs

Improving offline sync logic and cache invalidation

Adding unit and integration tests

Implementing settings for user customization



---

Contributing

Contributions are welcome. You can contribute by:

Reporting bugs via issues

Submitting pull requests for new features or fixes

Improving documentation


Please ensure your contributions align with the project architecture and coding standards.


---

License

This project is provided for educational and demonstration purposes. Add a specific license file if you intend to share or distribute the project publicly.

If you’d like, I can also **generate badges**, add **screenshots**, or expand the **Usage / API details** section.0