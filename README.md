# Machine Finder

A full-stack web application that helps users locate vending machines around the world, with a focus on condom vending machines to make sexual health products more accessible.

## Overview

Machine Finder aims to make it easier for people to find nearby vending machines and access essential products when they need them. The first release version focuses on mapping condom vending machines worldwide, helping users discover available locations more conveniently.

Built with Vue.js, .NET, and PostgreSQL, the project combines an interactive web interface with a backend API and database to support location-based discovery.

The project explores full-stack development, API design, database integration, and cloud technologies through a practical, real-world application.

## Tech Stack

- **Frontend:** Vue.js
- **Backend:** C# / .NET
- **Database:** PostgreSQL
- **Cloud:** Microsoft Azure

## Architecture

The application follows a frontend–backend architecture:

```text
User
  |
  v
Vue.js Frontend
  |
  v
.NET Backend API
  |
  v
PostgreSQL Database
```

- **Frontend:** Provides the user interface for interacting with the application.
- **Backend:** Handles application logic and communication between the frontend and database.
- **Database:** Stores/Retrieves the application's data.
- **Cloud:** Microsoft Azure is used for the project's cloud environment.

## Getting Started

### Prerequisites

Install the tools required for the relevant parts of the project:

- Node.js and npm
- .NET SDK compatible with the backend project
- PostgreSQL
- Git

### Clone the Repository

```bash
git clone https://github.com/Quang-huyy/machine-finder.git
cd machine-finder
```

Switch to the develop branch to access the latest version of the project or to create a new branch from it:

```bash
git checkout develop
```

### Run the Application

The exact commands depend on the repository's frontend and backend structure.

1. Configure the local PostgreSQL connection string.
2. Restore the .NET dependencies and start the backend API.
3. Install the frontend dependencies using npm.
4. Start the Vue.js development server.
5. Configure the frontend to communicate with the backend API.

Refer to the project configuration files for the exact commands, environment variables, and database setup.

## Author

**Quang Huy Dang**

[GitHub Profile](https://github.com/Quang-huyy)
