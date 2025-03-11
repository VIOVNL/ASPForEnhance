# ASP For Enhance

ASP For Enhance is a Windows desktop application designed for managing ASP.NET Core websites hosted on Linux servers. It provides a user-friendly interface for server management, website deployment, and service monitoring.

## Features

### Server Management
- Connect to Linux servers via SSH
- Store and manage multiple server configurations
- Securely handle server credentials

### Website Management
- Discover existing ASP.NET Core websites on the server
- Create new websites with automated service configuration
- View website details including hostname, IP, port, and service status

### Service Operations
- Start, stop, and restart website services
- Enable/disable services on system startup
- View real-time service logs
- Monitor resource usage (CPU, memory)

## Integration with Enhance

[Enhance](https://enhance.com/) is a powerful multi-server hosting control panel that simplifies the management of ASP.NET Core websites on Linux servers. It offers a unified admin and customer interface, containerized websites, and built-in incremental backups, making server management, website deployment, and service monitoring more efficient.

By integrating ASP For Enhance with Enhance, users can leverage features like automatic Let's Encrypt SSL certificate provisioning, role-based user access, and support for various web servers (NGINX, Apache, LiteSpeed, and OpenLiteSpeed). This integration streamlines the deployment and management of ASP.NET Core applications, providing a robust and user-friendly environment for developers and administrators.

For more details on Enhance and its capabilities, visit their official website: [enhance.com](https://enhance.com/)

## System Requirements

- Windows operating system
- .NET 9.0 Runtime
- Administrative privileges (for some features)

## Installation

1. Download the latest release from the releases page
2. Extract the ZIP file to your preferred location
3. Run `ASPForEnhance.exe` to start the application

## Getting Started

### Connecting to a Server

1. Click "Add Server" to add a new server configuration
2. Enter the server details:
   - **Name**: A friendly name for your server
   - **Hostname**: IP address or domain name
   - **Username**: SSH username (typically root or a user with sudo privileges)
   - **Password**: SSH password
   - **Port**: SSH port (default: 22)
3. Click "Save" to add the server
4. Select the server from the dropdown and click "Connect"

### Managing Websites

Once connected to a server, you can:

- View all discovered websites in the table
- Click "Create Website" to deploy a new website
- Use the action buttons to:
  - View website details
  - Restart a service
  - Start/stop a service
  - Enable/disable a service

### Website Details

Click the settings icon for a website to view detailed information:

- General website information
- Service status and resource usage
- Service logs
- Action buttons for service management

## Creating a New Website

1. Click the "Create Website" button
2. Enter the domain name
3. Provide the full path to your ASP.NET Core application DLL on the server
   - Format: `/var/www/[UUID]/[folder]/[dll-file]`
4. Check "Enable Blazor/SignalR" if you need WebSocket support
5. Click "Save" to create the website

## Technical Notes

- The application creates systemd service configurations for managed websites
- Apache vhost configurations are created and managed automatically
- The app monitors resource usage through systemd service status

## Screenshots
[![3qK2Q5P.md.png](https://iili.io/3qK2Q5P.md.png)](https://freeimage.host/i/3qK2Q5P)
[![3qK2tmF.md.png](https://iili.io/3qK2tmF.md.png)](https://freeimage.host/i/3qK2tmF)
[![3qK2LdB.md.png](https://iili.io/3qK2LdB.md.png)](https://freeimage.host/i/3qK2LdB)
[![3qK2Ze1.md.png](https://iili.io/3qK2Ze1.md.png)](https://freeimage.host/i/3qK2Ze1)
[![3qK2pLJ.md.png](https://iili.io/3qK2pLJ.md.png)](https://freeimage.host/i/3qK2pLJ)

## Credits

Developed by Viov software development

### Libraries Used
- AcrylicUI
- Microsoft ASP.NET Core Components WebView
- Radzen.Blazor
- SSH.NET

## License

© 2024 Viov software development. All rights reserved.
