sudo systemctl stop pos-system
dotnet publish -o /var/www/pos
sudo systemctl start pos-system
sudo systemctl status pos-system