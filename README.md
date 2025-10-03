# STGM_Gallery

# Showcase Gallery

A full-stack gallery application for showcasing creative work with ratings, comments, and media storage.

## Tech Stack

**Backend:**
- ASP.NET Core 9.0
- PostgreSQL 15
- MinIO (S3-compatible storage)
- JWT Authentication

**Frontend:**
- React 18
- Vite
- React Router

## Prerequisites

- Docker & Docker Compose
- (Optional) .NET 9 SDK for local development
- (Optional) Node.js 20+ for local development

## Quick Start

1. **Copy environment file:**
   ```bash
   cp .env.example .env
   ```
   (Or use the provided `.env` file)

2. **Start all services:**
   ```bash
   docker-compose up -d
   ```

3. **Verify services are running:**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000/health
   - MinIO Console: http://localhost:9001 (minioadmin / ShowcaseMinio123!)
   - PostgreSQL: localhost:5432

## Service URLs

| Service | URL | Credentials |
|---------|-----|-------------|
| Frontend | http://localhost:3000 | - |
| Backend API | http://localhost:5000 | - |
| API Docs | http://localhost:5000/openapi/v1.json | - |
| MinIO Console | http://localhost:9001 | minioadmin / ShowcaseMinio123! |
| PostgreSQL | localhost:5432 | showcase_user / ShowcaseDb123! |

## Docker Commands

```bash
# Start services
docker-compose up -d

# View logs
docker-compose logs -f

# View specific service logs
docker-compose logs -f api
docker-compose logs -f frontend

# Stop services
docker-compose down

# Rebuild after code changes
docker-compose up -d --build

# Reset everything (WARNING: deletes data)
docker-compose down -v
```

## Development Workflow

The project uses Docker volumes for hot-reloading:
- Backend changes in `backend/src/` automatically restart the API
- Frontend changes in `frontend/src/` trigger HMR (Hot Module Replacement)

## Project Structure

```
.
├── backend/
│   ├── src/ShowcaseGallery.Api/
│   │   ├── Program.cs
│   │   └── ShowcaseGallery.Api.csproj
│   └── Dockerfile
├── frontend/
│   ├── src/
│   │   ├── App.jsx
│   │   └── main.jsx
│   ├── package.json
│   └── Dockerfile
├── database/
│   └── init/
│       └── 01-schema.sql
├── docker-compose.yml
└── .env

```

## Database Schema

The PostgreSQL database is automatically initialized with:
- Users table with authentication
- Showcases table for gallery items
- Ratings and Comments tables
- Notifications system
- JWT refresh tokens

## Next Steps: Phase 1 Development

- [ ] Implement JWT authentication endpoints
- [ ] Add user registration/login backend logic
- [ ] Create login/register UI components
- [ ] Add protected routes
- [ ] Implement token refresh mechanism

## Troubleshooting

**API won't start:**
- Check logs: `docker-compose logs api`
- Verify PostgreSQL is healthy: `docker-compose ps`

**Frontend can't connect to API:**
- Verify CORS settings in Program.cs
- Check API is responding: `curl http://localhost:5000/health`

**Database connection issues:**
- Ensure PostgreSQL is fully started before API
- Check connection string in `.env`

**MinIO buckets not created:**
- Check minio-setup logs: `docker-compose logs minio-setup`
- Manually create buckets via MinIO console

## License

Private project