# .NET Full CI Demo with SonarQube

## Features
- Sample .NET Web API
- Unit Tests
- GitHub Actions CI Pipeline
- SonarQube Scan + Quality Gate

## Setup
1. Add GitHub Secrets:
   - SONAR_TOKEN
   - SONAR_HOST_URL

2. Push to main branch → pipeline runs

## Run Locally
dotnet restore
dotnet build
dotnet test
