git rm --cached .env
git add .gitignore Hotelaria.csproj .github/workflows/main_hotelaria-app.yml
git commit -m "fix: Update to .NET 9.0 and ignore .env"
git push origin main
