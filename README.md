# ContactUs API (.NET 8)

A simple ASP.NET Core minimal API to handle contact form submissions and store data in a SQLite database using Entity Framework Core.

## Requirements

- .NET 8 SDK
- SQLite (built-in)
- Docker (optional)

## How to Run the Application

### 🚀 Simple Run (without Docker):

1. **Restore dependencies and build:**

```bash
dotnet restore
dotnet build
```

2. **Run the project:**

```bash
dotnet run
```

The application will be available at `http://localhost:5000`.

---

### 🐳 Using Docker:

1. **Build Docker image:**

```bash
docker build -t contactus-api .
```

2. **Run Docker container:**

```bash
docker run -d -p 8080:8080 --name contactus-app contactus-api
```

The application will be accessible at: `http://localhost:8080`

---

### 📌 Deploying with Docker as a service on Ubuntu (systemd):

1. **Build Docker image:**

```bash
docker build -t contactus-api .
```

2. **Run container with restart:**

```bash
docker run -d --restart always -p 8080:8080 --name contactus-app contactus-api
```

3. (Optional) Create a systemd service:

Create file `/etc/systemd/system/contactus-api.service`:

```ini
[Unit]
Description=ContactUs API Docker Container
After=docker.service
Requires=docker.service

[Service]
Restart=always
ExecStart=/usr/bin/docker start -a contactus-app
ExecStop=/usr/bin/docker stop -t 2 contactus-app

[Install]
WantedBy=multi-user.target
```

Enable and start the service:

```bash
sudo systemctl daemon-reload
sudo systemctl start contactus-api
sudo systemctl enable contactus-api
```

Your API will now automatically restart and run with your Ubuntu server.

