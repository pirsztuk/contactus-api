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

2. **Apply migrations (create SQLite database):**

```bash
dotnet ef database update
```

3. **Run the project:**

```bash
dotnet run
```

The application will be available at `http://localhost:5000`.

---

### 🐳 Using Docker:

1. **Apply migrations locally before building Docker image:**

```bash
dotnet ef database update
```

> This creates the `app.db` SQLite file, which will be copied into the Docker image.

2. **Build Docker image:**

```bash
docker build -t contactus-api .
```

3. **Run Docker container:**

```bash
docker run -d -p 8080:8080 --name contactus-app contactus-api
```

The application will be accessible at: `http://localhost:8080`

---

### 📌 Deploying with Docker as a service on Ubuntu (systemd + volume):

1. **Create host directory and ensure `app.db` exists:**

```bash
mkdir -p /var/contactus-data
cp app.db /var/contactus-data/app.db
```

2. **Build Docker image:**

```bash
docker build -t contactus-api .
```

3. **Run container with mounted volume and auto-restart:**

```bash
docker run -d \
  --restart always \
  -p 8080:8080 \
  --name contactus-app \
  -v /var/contactus-data/app.db:/app/app.db \
  contactus-api
```

> This mounts only the `app.db` file to persist data outside of the container.

4. (Optional) Create a systemd service:

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

---

## Notes

- Ensure the SQLite file `app.db` exists before running the container with a volume.
- Mounting the DB file (`app.db`) instead of the whole directory avoids overwriting the application files inside the container.
- Future improvements may include running migrations automatically inside Docker.

