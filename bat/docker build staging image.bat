cd /d "%~dp0"
cd ../src
docker build . --no-cache --progress=plain -t rm-local-stage -f Demo.DbRefreshManager.WebApi/Dockerfile
pause